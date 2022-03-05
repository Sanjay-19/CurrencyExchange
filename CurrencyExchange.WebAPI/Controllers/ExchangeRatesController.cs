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
            var exchangeRate = _currencyService.GetRate(keyword);
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
        public IActionResult Post([Required]string keyword, [Required] double value)
        {
            var exchangeRate = _currencyService.GetRate(keyword);
            if (exchangeRate == null)
            {
                ExchangeRate rate = new ExchangeRate()
                {
                    Id = _currencyService.GetLatestId(),
                    BaseCurrency = baseCurrency,
                    Keyword = keyword,
                    Value = value
                };
                _currencyService.AddExchangeRate(rate);

                return Ok($"Exchange Rate for {keyword} has been added successfully");
            }
            throw new AppException($"Exchange rate for {keyword} already exists.");
        }

        [HttpPut]
        [Description("Updates keyword & value of exchange rate for a specific currency using it's id")]
        public IActionResult Put([Required] int id, [Required]string keyword, [Required]double value)
        {
            var exchangeRate = _currencyService.GetRate(id);
            if (exchangeRate == null)
            {
                throw new AppException($"Exchange rate for Id: {id} doesn't exist.");
            }
            _currencyService.UpdateExchangeRate(id, keyword, value);
            return Ok($"Exchange rate for Id: {id} has been updated successfully.");
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