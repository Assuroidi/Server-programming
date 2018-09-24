using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test1.Models
{
    //originally T1 = player, T2 = modifedplayer
    // public interface IRepository<T1, T2>
    // {
    //     Task<T1> Get(Guid id);
    //     Task<T1[]> GetAll();
    //     Task<T1> Create(T1 playeroritem);
    //     Task<T1> Modify(Guid id, T2 player);
    //     Task<T1> Delete(Guid id);
    // }

    public interface IRepository
{
    Task<Player> CreatePlayer(Player player);
    Task<Player> GetPlayer(Guid playerId);
    Task<Player[]> GetAllPlayers();
    Task<Player> UpdatePlayer(Guid id, ModifiedPlayer player);
    Task<Player> DeletePlayer(Guid playerId);

    Task<Item> CreateItem(Guid playerId, Item item);
    Task<Item> GetItem(Guid playerId, Guid itemId);
    Task<Item[]> GetAllItems(Guid playerId);
    Task<Item> UpdateItem(Guid playerId, Item item);
    Task<Item> DeleteItem(Guid playerId, Item item);
}

    public class InMemoryRepository : IRepository
    {
         public List<Player> playerList = new List<Player>();

        public async Task<Player> CreatePlayer(Player player)
        {
            playerList.Add(player);
            return player;  //Miksi palauttaa sama player? Miksei esim bool onnistuiko vai ei
        }

        public async Task<Player> GetPlayer(Guid playerId)  //Tarvitaanko await?
        {
            foreach(var playervar in playerList)
            {
                if (playervar.Id == playerId)
                {
                    // Task<Player> derp = await playervar;
                    return playervar;
                    // return Task.FromResult<Player>(playervar);
                }
            }
            return null;
            // return Task.FromResult<Player>(null);
        }

        public async Task<Player[]> GetAllPlayers()
        {
            return playerList.ToArray();
            // return Task.FromResult<Player[]>(playerList);
        }



        public async Task<Player> UpdatePlayer(Guid id, ModifiedPlayer player)
        {
            foreach(var playervar in playerList)
            {
                if (playervar.Id == id)
                {
                    playervar.Score = player.Score;
                    return playervar;
                }


            }
            return null;

        }

        public async Task<Player> DeletePlayer(Guid id)
        {
            foreach(var playervar in playerList)
            {
                if (playervar.Id == id)
                {
                    playerList.Remove(playervar);
                    return playervar;
                }

            }
            return null;

        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId);
            temp.Result.itemList.Add(item);
            return item;
        }
        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            var temp = GetPlayer(playerId);

            foreach(var itemvar in temp.Result.itemList)
            {
                if(itemvar.ItemId  == itemId)
                {
                    return itemvar;
                }
            }
            return null;
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            return GetPlayer(playerId).Result.itemList.ToArray();
        }
        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId);

            foreach(var itemvar in temp.Result.itemList)
            {
                if (itemvar.ItemId == item.ItemId)
                {
                    temp.Result.itemList.Remove(itemvar);
                    temp.Result.itemList.Add(item);
                    return item;
                }


            }
            return null;
        }
        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId);

            foreach(var itemvar in temp.Result.itemList)
            {
                if (itemvar.ItemId == item.ItemId)
                {
                    temp.Result.itemList.Remove(itemvar);
                    return itemvar;
                }

            }
            return null;

        }

        
    }
}