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

        // GET Https://wilds.mhdb.io/en/monsters/{id} - not implemented yet as of 3/18/2025, the data is not yet updated
        public async Task<Monster?> GetMonsterAsync() {
            using var client = _factory.CreateClient("wildsapi");

            var testMonster = await client.GetFromJsonAsync<Monster>("en/monsters/20");

            return testMonster;
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
                // it could be an error that we cant parse, or we tried to parse our armor but failed
                _logger.LogError(ex, ex.Message);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}