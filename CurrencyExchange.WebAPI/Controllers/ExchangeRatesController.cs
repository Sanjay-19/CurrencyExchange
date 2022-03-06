using CurrencyExchange.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CurrencyExchange.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private const string baseCurrency = "USD";
        private readonly ICurrencyService _currencyService;
        public ExchangeRatesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [Route("GetAll")]
        [HttpGet]
        [Description("Gets all of the currency exchange rates")]
        public IActionResult GetAll()
        {
            return new JsonResult(_currencyService.GetRates());
        }

        [Route("GetWithKeyword")]
        [HttpGet]
        [Description("Gets currency exchange rate for a specific currency using it's keyword")]
        public IActionResult GetExchangeRateWithKeyword([Required]string keyword)
        {
            var exchangeRate = _currencyService.GetRate(keyword.ToUpper());
            if (exchangeRate == null)
            {
                throw new AppException($"Exchange rate for {keyword} doesn't exist");
            }
            return new JsonResult(exchangeRate);
        }

        [Route("GetWithId")]
        [HttpGet]
        [Description("Gets currency exchange rate for a specific currency using it's id")]
        public IActionResult GetExchangeRateWithId([Required]int id)
        {
            var exchangeRate = _currencyService.GetRate(id);
            if (exchangeRate == null)
            {
                throw new AppException($"Exchange rate for Id: {id} doesn't exist");
            }
            return new JsonResult(exchangeRate);
        }

        [HttpPost]
        public IActionResult Post(CurrencyModel model)
        {
            var exchangeRate = _currencyService.GetRate(model.Keyword.ToUpper());
            if (exchangeRate == null)
            {
                ExchangeRate rate = new ExchangeRate()
                {
                    Id = _currencyService.GetLatestId(),
                    BaseCurrency = baseCurrency,
                    Keyword = model.Keyword,
                    Value = model.Value
                };
                _currencyService.AddExchangeRate(rate);

                return Ok($"Exchange Rate for {model.Keyword} has been added successfully");
            }
            throw new AppException($"Exchange rate for {model.Keyword} already exists.");
        }

        [HttpPut]
        [Description("Updates keyword & value of exchange rate for a specific currency using it's id")]
        public IActionResult Put(CurrencyModel model)
        {
            var exchangeRate = _currencyService.GetRate(model.Id);
            if (exchangeRate == null)
            {
                throw new AppException($"Exchange rate for Id: {model.Id} doesn't exist.");
            }
            exchangeRate.Id = model.Id;
            exchangeRate.Keyword = model.Keyword.ToUpper();
            exchangeRate.Value = model.Value;

            _currencyService.UpdateExchangeRate(exchangeRate);
            return Ok($"Exchange rate for Id: {model.Id} has been updated successfully.");
        }

        [HttpDelete]
        public IActionResult Delete([Required] int id)
        {
            var exchangeRate = _currencyService.GetRate(id);
            if (exchangeRate == null)
            {
                throw new AppException($"Exchange rate for Id: {id} doesn't exist.");
            }
            _currencyService.DeleteExchangeRate(exchangeRate.Id);
            return Ok($"Exchange rate for Id:{id} has been deleted successfully.");
        }
    }
}