using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WildsApi {
    public class WildsDocService {
        private ILogger<WildsDocService> _logger;
        private IHttpClientFactory _factory;

        public WildsDocService(ILogger<WildsDocService> logger, IHttpClientFactory client) {
            _logger = logger;
            _factory = client;
        }

        /// <summary>
        /// Gets the monster information from the API at https://wilds.mhdb.io/en/monsters. With a query by name, it will also return
        /// the other versions of the monster, for example Rey Dau will also return Tempered Ray Dau. So this method will also search the list for
        /// the single monster instead.
        /// </summary>
        /// <param name="name">Name of the monster to search for</param>
        /// <returns>A single monster that matches the searched name.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Monster?> GetMonsterAsync(string name) {
            using var client = _factory.CreateClient("wildsapi");
            var requestString = $"en/monsters?q{{\"name\":\"{name}\"}}";

            try {
                var response = await client.GetAsync(requestString);

                if (response.IsSuccessStatusCode) {
                    // get the content before deserializing it to make sure its not an error from the server being returned
                    var data = await response.Content.ReadAsStringAsync();

                    if (data.Contains("error")){
                        var errorResponse = JsonConvert.DeserializeObject<ApiError>(data);
                        var errorMessage = $"Api call to {requestString} failed with code: { errorResponse?.code } and message: {errorResponse?.message}";
                        _logger.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    } else {
                        // hey it might actually be our data, lets try to parse it!
                        var monsterData = JsonConvert.DeserializeObject<List<Monster?>?>(data)?.Find(x => x?.name == name);
                        return monsterData;
                    }
                } else {
                    // response is not a successful status code
                    _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
            } catch(Exception ex) {
                // it could be an error that we cant parse, or we tried to parse our monster but failed
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Gets requested armor set information as a list of armor
        /// </summary>
        /// <returns>Retruns the armor requested, otherwise throws an error.</returns>
        /// <exception cref="Exception">If it gets an error, it throws so be sure to try/catch this method when using it</exception>
        public async Task<List<ArmorSet?>?> GetArmorSetAsync(string armorName) {
            using var client = _factory.CreateClient("wildsapi");
            var requestString = $"en/armor/sets?q={{\"name\":\"{armorName}\"}}";

            try {
                var response = await client.GetAsync(requestString);

                if (response.IsSuccessStatusCode) {
                    // get the content before deserializing it to make sure its not an error from the server being returned
                    var data = await response.Content.ReadAsStringAsync();

                    if (data.Contains("error")){
                        var errorResponse = JsonConvert.DeserializeObject<ApiError>(data);
                        var errorMessage = $"Api call to {requestString} failed with code: { errorResponse?.code } and message: {errorResponse?.message}";
                        _logger.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    } else {
                        // hey it might actually be our data, lets try to parse it!
                        var armorData = JsonConvert.DeserializeObject<List<ArmorSet?>?>(data);
                        return armorData;
                    }
                } else {
                    // response is not a successful status code
                    _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
            } catch(Exception ex) {
                // it could be an error that we cant parse, or we tried to parse our armor but failed
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Lookup a weapon based on the given name. Technically the query could be a list of weapons that all match that name, so be careful 
        // (mostly it will be the first item)
        /// </summary>
        /// <param name="weaponName">String name of the weapon, it can handle the alpha, beta, gamma symbols</param>
        /// <returns>Returns a list of parsed weapon(s), otherwise throws an error.</returns>
        /// <exception cref="Exception">If it gets an error, it throws so be sure to try/catch this method when using it</exception>
        public async Task<List<Weapon?>?> GetWeaponAsync(string weaponName) {
            using var client = _factory.CreateClient("wildsapi");
            var requestString = $"en/weapons?q={{\"name\":\"{weaponName}\"}}";

            try {
                var response = await client.GetAsync(requestString);

                if (response.IsSuccessStatusCode) {
                    // get the content before deserializing it to make sure its not an error from the server being returned
                    var data = await response.Content.ReadAsStringAsync();

                    if (data.Contains("error")){
                        var errorResponse = JsonConvert.DeserializeObject<ApiError>(data);
                        var errorMessage = $"Api call to {requestString} failed with code: { errorResponse?.code } and message: {errorResponse?.message}";
                        _logger.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    } else {
                        // hey it might actually be our data, lets try to parse it!
                        var weaponData = await client.GetFromJsonAsync<List<Weapon?>?>(requestString);
                        return weaponData;
                    }
                } else {
                    // response is not a successful status code
                    _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
            } catch(Exception ex) {
                // it could be an error that we cant parse, or we tried to parse our weapon but failed
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Lookup a skill, primarily because weapons only have skillrank lists and dont have the name but just the description of the skill. So use the ID here to get the skill name.
        /// </summary>
        /// <param name="skillId">Id of the particular skill</param>
        /// <returns>A list of skills that match that ID (usually 1)</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Skill?>?> GetSkillAsync(int skillId) {
            using var client = _factory.CreateClient("wildsapi");
            var requestString = $"en/skills?q={{\"id\":\"{skillId}\"}}";

            try {
                var response = await client.GetAsync(requestString);

                if (response.IsSuccessStatusCode) {
                    // get the content before deserializing it to make sure its not an error from the server being returned
                    var data = await response.Content.ReadAsStringAsync();

                    if (data.Contains("error")){
                        var errorResponse = JsonConvert.DeserializeObject<ApiError>(data);
                        var errorMessage = $"Api call to {requestString} failed with code: { errorResponse?.code } and message: {errorResponse?.message}";
                        _logger.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    } else {
                        // hey it might actually be our data, lets try to parse it!
                        var skillData = await client.GetFromJsonAsync<List<Skill?>>(requestString);
                        return skillData;
                    }
                } else {
                    // response is not a successful status code
                    _logger.LogError($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                
            } catch(Exception ex) {
                // it could be an error that we cant parse, or we tried to parse our skill but failed
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}