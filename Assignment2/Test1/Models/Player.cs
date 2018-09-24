using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Test1.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreationTime { get; set; }
        public List<Item> itemList = new List<Item>();

    }

    public class NewPlayer
    {
        public string Name { get; set; }
    }

    public class ModifiedPlayer
    {
        public int Score { get; set; }
    }

    public class PlayerProcessor
    {
        // IRepository<Player, ModifiedPlayer> myRepository;
        IRepository myRepository;

        // public PlayerProcessor(IRepository<Player, ModifiedPlayer> repository)
        public PlayerProcessor(IRepository repository)

        {
            myRepository = repository;
        }

        public Task<Player> Get(Guid id)
        {
            return myRepository.GetPlayer(id);
        }
        public Task<Player[]> GetAll()
        {
            return myRepository.GetAllPlayers();
        }
        public Task<Player> Create(NewPlayer player)
        {
            Player forwarded = new Player();
            forwarded.Name = player.Name;
            forwarded.CreationTime = DateTime.Now;
            forwarded.IsBanned = false;
            forwarded.Score = 0;
            forwarded.Id = Guid.NewGuid();
            return myRepository.CreatePlayer(forwarded);
        }
        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return myRepository.UpdatePlayer(id, player);
        }

        public Task<Player> Delete(Guid id)
        {
            return myRepository.DeletePlayer(id);
        }
    }
//Postman configs
//url -> https://localhost:5001/api/players
//postman -> POST -> body -> raw -> json format -> {"name" : "test"}



[Route("api/players")]
    public class PlayersController : Controller
    {
        


        PlayerProcessor myProcessor;
	    public PlayersController(PlayerProcessor processor)
        {
            myProcessor = processor;

        }

        [HttpGet("{id}")]
        public Task<Player> Get(Guid id)
        {
            return myProcessor.Get(id);
        }

        [HttpGet]
        public Task<Player[]> GetAll()  //Toimii!
        {
            return myProcessor.GetAll();
        }

        [HttpPost]
        public Task<Player> Create([FromBody] NewPlayer player)
        {
            // if (player == null)
            // {
            //     player = new NewPlayer();
            //     player.Name = name;
            // }
            return myProcessor.Create(player);
        }

        [HttpPut("{id}")]
        public Task<Player> Modify(Guid id, [FromBody] ModifiedPlayer player)
        {
            return myProcessor.Modify(id, player);
        }

        [HttpDelete("{id}")]
        public Task<Player> Delete(Guid id)
        {
            return myProcessor.Delete(id);
        }
    }

}