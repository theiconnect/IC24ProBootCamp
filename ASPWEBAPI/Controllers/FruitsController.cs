using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Cache;

namespace ASPWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
        
        public List<Heroes> Hero = new List<Heroes>
        {
            new Heroes{movie = "Kick", Hero = "RaviTeja",Age = 45},
            new Heroes{movie = "Kalki", Hero = "Prabhas",Age = 50},
            new Heroes{movie = "Pushpa", Hero = "AlluArjun",Age = 35}
        };


        [HttpGet("Heroes")]
        public List<Heroes> GetHeroes()
        {
            return (Hero);
        }

        [HttpPost("AddHeroes")]

        public IActionResult AddHeroes(Heroes newHeroes)
        {
            if(newHeroes == null || string.IsNullOrEmpty(newHeroes.Hero)){ 
                return BadRequest("Invalid Hero Details Entered");
            }
            Hero.Add(newHeroes); // Add the new hero to the list
            return Ok(newHeroes); // Return the added hero
        }
        [HttpPut("UpdateHeroes")]
        public IActionResult UpdateHeroes([FromBody]Heroes updatedHero)
        {
            var existingHero = Hero.FirstOrDefault(h => h.Hero == updatedHero.Hero);
            if (existingHero == null)
            {
                return NotFound("Hero Not Found");
            }
            existingHero.Hero = updatedHero.Hero;
            existingHero.Age = updatedHero.Age;
            existingHero.movie = updatedHero.movie; 
            return Ok(existingHero);



        }



    }


    public class Heroes
    {
        public int Age { get; set; }
        public string Hero { get; set; }
        public string movie { get; set; }
    }

}
