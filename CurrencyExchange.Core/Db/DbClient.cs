using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CurrencyExchange.Core
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<ExchangeRate> _exchangeRates;
        public DbClient(IOptions<AppDbConfig> currencyExchangeDbConfig)
        {
            var client = new MongoClient(currencyExchangeDbConfig.Value.Connection_String);
            var database = client.GetDatabase(currencyExchangeDbConfig.Value.Database_Name);
            _exchangeRates = database.GetCollection<ExchangeRate>(currencyExchangeDbConfig.Value.EXCHANGE_RATE_COLLECTION_NAME);
        }
        public IMongoCollection<ExchangeRate> GetExchangeRateCollection() => _exchangeRates;
    }
}