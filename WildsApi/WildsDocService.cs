using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

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

        public async Task<Armor?> GetArmorAsync() {
            using var client = _factory.CreateClient("wildsapi");

            try {
                var testArmor = await client.GetFromJsonAsync<Armor>("en/armor/1");
                return testArmor;
            } catch(Exception ex) {
                _logger.LogError(ex, ex.Message);
                return null;
            }

            

            
        }
    }
}