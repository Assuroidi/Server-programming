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

        public ItemTypes MyType { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class NewItem
    {
        public string Name { get; set; }
        
        [Range(1, 99)]
        public int Level { get; set; }
        public ItemTypes MyType { get; set; }
        
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
            forwarded.Level = item.Level;
            forwarded.ItemId = Guid.NewGuid();
            forwarded.MyType = item.MyType;
            return _repository.CreateItem(playerId, forwarded);
        }

        public Task<Item> UpdateItem(Guid playerId, ModifiedItem item)
        {
            // var temp = Task.FromResult(GetItem(playerId, item.ItemId));
            var temp2 = GetItem(playerId, item.ItemId).Result;
            temp2.Level = item.Level;
            return _repository.UpdateItem(playerId, temp2);
        }

        public Task<Item> Delete(Guid playerId, Guid itemId)
        {
            var temp2 = GetItem(playerId, itemId).Result;

            return _repository.DeleteItem(playerId, temp2);
        }

    }

[ApiController]
[Route("api/players/{playerId}/items")]
    public class ItemsController
    {
        ItemsProcessor _processor;
        
        public ItemsController(ItemsProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet("{itemId}")]
        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return _processor.GetItem(playerId, itemId);
        }

        [Route("")]
        [HttpGet]
        public Task<Item[]> GetAll(Guid playerId)
        {
            return _processor.GetAll(playerId);
        }
        [Route("")]
        [HttpPost]
        public Task<Item> Create(Guid playerId, [FromBody]NewItem item)
        {
            return _processor.Create(playerId, item);
        }
        [HttpPut]
        public Task<Item> Modify(Guid playerId, ModifiedItem item)
        {
            return _processor.UpdateItem(playerId, item);
        }

        [HttpDelete("{itemId}")]
        public Task<Item> Delete(Guid playerId, Guid itemId)
        {
            return _processor.Delete(playerId, itemId);
        }



    }


}