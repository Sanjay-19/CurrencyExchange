namespace CurrencyExchange.Core
{
    public interface ICurrencyService
    {
        List<ExchangeRate> GetRates();
        ExchangeRate GetRate(string key); 
        ExchangeRate GetRate(int id);
        int GetLatestId();
        void AddExchangeRate(ExchangeRate rate);
        void DeleteExchangeRate(int id);
        void UpdateExchangeRate(ExchangeRate rate);
    }
}