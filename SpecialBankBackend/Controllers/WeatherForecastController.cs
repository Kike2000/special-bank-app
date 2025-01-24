using Microsoft.AspNetCore.Mvc;
using SpecialBankAPI.Data;
using System.Text;

namespace SpecialBankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SpecialBankDbContext _specialBankDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, SpecialBankDbContext specialBankDbContext)
        {
            _logger = logger;
            _specialBankDbContext = specialBankDbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            byte[] pinHash, pinSalt;
            CreatePin("23232", out pinHash, out pinSalt);
            var test = _specialBankDbContext.Account.AddAsync(new Models.Account
            {
                FirstName = "Pedro",
                LastName="Carrillo",
                Email = "pedro@gmail.com",
                PinHash = pinHash,
                PinSalt = pinSalt,
                PhoneNumber = "1234567890",
                CreatedDate = DateTime.UtcNow,
            });
            await _specialBankDbContext.SaveChangesAsync();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static void CreatePin(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac= new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }
    }
}
