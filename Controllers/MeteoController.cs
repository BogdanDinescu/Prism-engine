using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Prism.Controllers
{
    [Authorize]
    [ApiController]
    [Route("meteo")]
    public class MeteoController: ControllerBase
    {
        
        static readonly HttpClient client = new HttpClient();
        
        [HttpGet]
        [Route("city")]
        public async Task<IActionResult> GetMeteoCity([FromQuery] int cityId = 0)
        {
            if (cityId == 0)
            {
                return NotFound("CityId empty");
            }

            try	
            {
                var response = await client.GetStringAsync("https://www.metaweather.com/api/location/" + cityId);
                return Ok(response);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!" + e.ToString());	
                return NotFound("CityId incorrect");
            }
        }

        [HttpGet]
        [Route("location")]
        public async Task<IActionResult> GetMeteoLatLng([FromQuery] float lat = 0, float lng = 0)
        {
            if (lat == 0 || lng == 0)
            {
                return NotFound("Incorrect coordinates");
            }
            try	
            {
                var cities = await client.GetStringAsync("https://www.metaweather.com/api/location/search/?lattlong="+lat+","+lng);
                JArray json = JArray.Parse(cities);
                var response = await client.GetStringAsync("https://www.metaweather.com/api/location/" + json[0]["woeid"]);
                return Ok(response);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!" + e.ToString());	
                return NotFound("Incorrect coordinates");
            }
            
        }
    }
}