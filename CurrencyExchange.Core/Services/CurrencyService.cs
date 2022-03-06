using MongoDB.Driver;

namespace CurrencyExchange.Core
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IMongoCollection<ExchangeRate> _exchangeRates;
        public CurrencyService(IDbClient dbClient)
        {
            _exchangeRates = dbClient.GetExchangeRateCollection();
        }

        public List<ExchangeRate> GetRates()
        {
           return _exchangeRates.Find(exchangeRate => true).ToList();
        }
        public ExchangeRate GetRate(string key)
        {
            var filter = Builders<ExchangeRate>.Filter.Eq("Keyword", key);
            return _exchangeRates.Find(filter).FirstOrDefault();
        }
        public ExchangeRate GetRate(int id)
        {
            var filter = Builders<ExchangeRate>.Filter.Eq("Id", id);
            return _exchangeRates.Find(filter).FirstOrDefault();
        }
        public int GetLatestId()
        {
            var rate = _exchangeRates.Find(exchangeRate => true).SortByDescending(x=>x.Id).FirstOrDefault();
            if(rate == null)
                return 1;
            return rate.Id + 1;
        }

        public void AddExchangeRate(ExchangeRate rate)
        {
            _exchangeRates.InsertOne(rate);
        }

        public void DeleteExchangeRate(int id)
        {
            var filter = Builders<ExchangeRate>.Filter.Eq("Id", id);
            _exchangeRates.DeleteOne(filter);
        }

        public void UpdateExchangeRate(ExchangeRate rate)
        {
            var filter = Builders<ExchangeRate>.Filter.Eq("Id", rate.Id);
            var update = Builders<ExchangeRate>.Update.Set("Keyword", rate.Keyword).Set("Value",rate.Value);
            _exchangeRates.UpdateOne(filter, update);
        }
    }
}
