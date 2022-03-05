using MongoDB.Driver;

namespace CurrencyExchange.Core
{
    public interface IDbClient
    {
        IMongoCollection<ExchangeRate> GetExchangeRateCollection();
    }
}
