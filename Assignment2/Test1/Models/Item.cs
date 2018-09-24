using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Test1.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        // [Required]  //non-nullable -> inherently required
        [Range(1, 99)]
        public int Level { get; set; }

        public ItemType.Types Type { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class NewItem
    {
        public string Name { get; set; }
        
        [Range(1, 99)]
        public int Level { get; set; }
        public ItemType.Types Type { get; set; }
        
    }

    public class ModifiedItem
    {
        public Guid ItemId { get; set; }
        [Range(1, 99)]
        public int Level { get; set; }
    }

    public class ItemsProcessor
    {
        IRepository _repository;
        public ItemsProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return _repository.GetItem(playerId, itemId);
        }

        public Task<Item[]> GetAll(Guid playerId)
        {
            return _repository.GetAllItems(playerId);
        }

        public Task<Item> Create(Guid playerId, NewItem item)
        {
            Item forwarded = new Item();
            forwarded.Name = item.Name;
            forwarded.CreationDate = DateTime.Now;
            forwarded.Level = 0;
            forwarded.ItemId = Guid.NewGuid();
            forwarded.Type = item.Type;
            return _repository.CreateItem(playerId, forwarded);
        }

        public Task<Item> Modify(Guid playerId, ModifiedItem item)
        {
            // var temp = Task.FromResult(GetItem(playerId, item.ItemId));
            var temp2 = GetItem(playerId, item.ItemId).Result;
            temp2.Level = item.Level;
            return _repository.UpdateItem(playerId, temp2);
        }

        public Task<Item> Delete(Guid playerId, Item item)
        {
            return _repository.DeleteItem(playerId, item);
        }

    }

[Route("api/players/{playerId}/items")]
    public class ItemsController
    {
        ItemsProcessor _processor;
        
        Guid _playerId = ;  //miten tallentaa tämä routesta?

        public ItemsController(ItemsProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet("{id}")]
        public Task<Item> GetItem(Guid itemId)
        {
            return _processor.GetItem(_playerId, itemId);
        }

        [HttpGet]
        public Task<Item[]> GetAll()
        {
            return _processor.GetAll(_playerId);
        }
        [HttpPost]
        public Task<Item> Create([FromBody] NewItem item)
        {
            return _processor.Create(_playerId, item);
        }
        [HttpPut]
        public Task<Item> Modify([FromBody] ModifiedItem item)
        {
            return _processor.Modify(_playerId, item);
        }

        [HttpDelete("{id}")]
        public Task<Item> Delete(Item item)
        {
            return _processor.Delete(_playerId, item);
        }



    }


}