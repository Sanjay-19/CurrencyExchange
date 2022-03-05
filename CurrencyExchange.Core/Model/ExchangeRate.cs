using MongoDB.Bson.Serialization.Attributes;

namespace CurrencyExchange.Core
{
    public class ExchangeRate
    {
        [BsonId]
        public int Id { get; set; }
        public string BaseCurrency { get; set; }
        public string Keyword { get; set; }
        public double Value { get; set; }
    }
}
