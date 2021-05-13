using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NetDemo.Controllers
{
    public class Author
    {
        public int id { get; set; }
        public string username { get; set; }
        public string about { get; set; }
        public int submitted { get; set; }
        public DateTime updated_at { get; set; }
        public int submission_count { get; set; }
        public int comment_count { get; set; }
        public int created_at { get; set; }
    }



    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var client = new HttpClient();
            var baseAddr = "https://jsonmock.hackerrank.com/api/article_users";
            var page = 1;
            var threshold = 1;
            var response = client.GetAsync($"{baseAddr}/?page={page}").Result;
            if (response.IsSuccessStatusCode)
            {
                var payload = response.Content.ReadAsStringAsync().Result;
                var jsonObject = JObject.Parse(payload);
          
                var authors = jsonObject.SelectToken("data")?.ToObject<IEnumerable<Author>>();
                var overQuota = from a in authors
                    where a.submission_count > threshold
                    select a.username;

                var zz = overQuota.ToList();
            }


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
