using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prism.Controllers
{
    [Authorize]
    [ApiController]
    [Route("meteo")]
    public class MeteoController: ControllerBase
    {
        
        private static readonly HttpClient Client = new HttpClient();
        private const string url = "https://api.weatherbit.io/v2.0/forecast/daily?key=b5ffe9c81de244f9aef3fc7cf5d998b7";
        [HttpGet]
        [Route("city")]
        public async Task<IActionResult> GetMeteoCity([FromQuery] string cityName)
        {
            
            if (cityName == null || cityName.Length.Equals(0))
            {
                return NotFound("City name empty");
            }

            try	
            {
                var response = await Client.GetStringAsync(url + "&city=" + cityName);
                return Ok(response);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!" + e.ToString());	
                return NotFound("City incorrect");
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
                var response = await Client.GetStringAsync(url + "&lat=" + lat + "&lon=" + lng);
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