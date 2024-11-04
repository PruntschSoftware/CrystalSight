using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using CrystalSight.Web.DataContract;
using CrystalSight.Web.Model;
using RestSharp;

namespace CrystalSight.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TickerController : ControllerBase
    {
        private readonly ILogger<TickerController> _logger;

        public TickerController(ILogger<TickerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("FetchSymbol")]
        public StockTicker FetchSymbol()
        {
            return new StockTicker() { Symbol = "AMSC" };
        }


        [HttpPost]
        [Route("ChangeTickerSymbol")]
        public async Task ChangeTickerSymbol([FromBody] TickerModel tickerModel)
        {
            using (var context = new SqLiteContext())
            {
                var normalizedUser = tickerModel.UserName.ToUpper();
                var usr = context.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUser);
                if (usr != null)
                {
                    var tickr = context.Devices.FirstOrDefault(x => x.User.Id == usr.Id && x.MacAdress == tickerModel.DeviceId);
                    if (tickr != null)
                    {
                        tickr.Symbol = tickerModel.Symbol;
                        context.SaveChanges();
                    }
                }
            }
        }


        [HttpPost]
        [Route("AddDevice")]
        public async Task AddDevice([FromBody] TickerModel tickerModel)
        {
            using (var context = new SqLiteContext())
            {
                var normalizedUser = tickerModel.UserName.ToUpper();
                var usr = context.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUser);
                if (usr != null)
                {
                    context.Devices.Add(new Device()
                    {
                        Id = Guid.NewGuid(),
                        Symbol = tickerModel.Symbol,
                        MacAdress = tickerModel.DeviceId,
                        User = usr
                    });
                    context.SaveChanges();
                }
            }
        }


        [HttpPost]
        [Route("GetValue")]
        public async Task<StockData> GetValue(TickerModel getValueModel)
        {

            using (var context = new SqLiteContext())
            {
                var normalizedUser = getValueModel.UserName.ToUpper();
                var usr = context.Users.FirstOrDefault(x => x.NormalizedUserName == normalizedUser);
                if (usr != null)
                {
                    var device = context.Devices.FirstOrDefault(x => x.User == usr && x.MacAdress == getValueModel.DeviceId);
                    if (device != null)
                    {
                        //TODO: Check if stored value exists and if yes - return the stored value; if not - get from api and store

                        string apiKey = "c0026347-1eaf-4dae-8acb-807dd3f66c66";
                        string url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";
                        string symbol = device.Symbol;
                        string currency = "USD";

                        var client = new RestClient(url);
                        var request = new RestRequest();

                        request.AddParameter("symbol", symbol);
                        request.AddParameter("convert", currency);
                        request.AddHeader("X-CMC_PRO_API_KEY", apiKey);

                        try
                        {
                            var response = await client.ExecuteAsync(request);
                            if (response.IsSuccessful)
                            {
                                Console.WriteLine(response.Content); // Print the response content

                                JObject json = JObject.Parse(response.Content);

                                decimal price = (decimal)json["data"][symbol]["quote"][currency]["price"];
                                return new StockData() { Currency = currency, Symbol = symbol, Value = price };
                            }
                            else
                            {
                                return null;
                            }
                        }
                        catch (Exception ex)
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }
    }
}
