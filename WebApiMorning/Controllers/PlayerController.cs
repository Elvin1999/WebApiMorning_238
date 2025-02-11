﻿using Microsoft.AspNetCore.Mvc;
using WebApiMorning.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiMorning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private static List<Player> players = new List<Player>
        {
            new Player { Id = 1, City ="New York",PlayerName="John Doe",Score=1500},
            new Player { Id = 2, City ="Los Angelos",PlayerName="Jane Smith",Score=2000},
        };

        // GET: api/<PlayerController>
        [HttpGet]
        public ActionResult<IEnumerable<Player>> Get()
        {
            return Ok(players);
        }

        // GET api/<PlayerController>/5
        [HttpGet("{id}")]
        public ActionResult<Player> Get(int id)
        {
            var player = players.FirstOrDefault(x => x.Id == id);
            if (player == null)
            {
                return NotFound($"player does not exist with this id {id}");
            }
            return Ok(player);
        }

        // POST api/<PlayerController>
        [HttpPost]
        public ActionResult<Player> Post([FromBody] Player newPlayer)
        {
            newPlayer.Id = players.Any() ? players.Max(p => p.Id) + 1 : 1;
            players.Add(newPlayer);
            return CreatedAtAction(nameof(Get), new { id = newPlayer.Id }, newPlayer);
        }

        // PUT api/<PlayerController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Player updatedPlayer)
        {
            var player = players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            player.City = updatedPlayer.City;
            player.PlayerName = updatedPlayer.PlayerName;
            player.Score = updatedPlayer.Score;
            return NoContent();
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var player=players.FirstOrDefault(x => x.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            players.Remove(player);
            return NoContent(); 
        }
    }
}
