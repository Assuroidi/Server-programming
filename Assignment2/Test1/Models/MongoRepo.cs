using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test1.Models
{
    public class MongoRepo : IRepository
    {
        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<Player> playerCollection;
        IMongoCollection<LogEntry> logCollection;
        
        public MongoRepo()
        {
            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("game");
            playerCollection = database.GetCollection<Player>("players");
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            await playerCollection.InsertOneAsync(player);
            return player;  //Miksi palauttaa sama player? Miksei esim bool onnistuiko vai ei
        }

        public async Task<Player> GetPlayer(Guid playerId)
        {
            var filter = Builders<Player>.Filter.Eq("id", playerId);
            var temp = playerCollection.Find(filter) /*FirstAsync()*/;
            var count = await temp.CountDocumentsAsync();
            if (count > 0 )
            {
                return await temp.FirstAsync();
            }
            return null;
        }

        public async Task<Player[]> GetAllPlayers()
        {
            List<Player> players = await playerCollection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> UpdatePlayer(Guid id, Player player)
        {
            // Player replacePlayer = GetPlayer(id).Result;
            // replacePlayer.Score = player.Score;
            // var filter = Builders<Player>.Filter.Eq("id", id);
            // await playerCollection.ReplaceOneAsync(filter, replacePlayer);
            // return replacePlayer;


            var filter = Builders<Player>.Filter.Eq("id", player.Id);
            await playerCollection.ReplaceOneAsync(filter, player);
            return player;
        }

        public async Task<Player> DeletePlayer(Guid playerId)
        {
            var filter = Builders<Player>.Filter.Eq("id", playerId);
            playerCollection.DeleteOne(filter);
            return null;
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            var temp = GetPlayer(playerId).Result;
            temp.itemList.Add(item);

            var filter = Builders<Player>.Filter.Eq("id", playerId);
            await playerCollection.ReplaceOneAsync(filter, temp);
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
                    await UpdatePlayer(playerId, temp.Result);
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
                    var filter = Builders<Player>.Filter.Eq("id", playerId);
                    await playerCollection.ReplaceOneAsync(filter, temp.Result);
                    return itemvar;
                }

            }
            return null;

        }

        public Task<Player[]> GetAllPlayersMinScore(int minScore)
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.Gte("Score", minScore);
            List<Player> _players = playerCollection.Find(filter).ToListAsync().Result;
            return Task.FromResult(_players.ToArray());
        }

        public Task<Player[]> GetAllPlayersWithItem(ItemTypes itemType)
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.ElemMatch(e=> e.itemList, i=> i.MyType == itemType);
            List<Player> _players = playerCollection.Find(filter).ToListAsync().Result;
            return Task.FromResult(_players.ToArray());
        }

        public Task<Player[]> GetAllWithAthLeastItemAmount(int minItems)
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.Size("itemList", minItems);
            List<Player> _players = playerCollection.Find(filter).ToListAsync().Result;
            return Task.FromResult(_players.ToArray());
        }

        public async Task<int> GetMostCommonLevel()
        {
            var temp = playerCollection.Aggregate()
                    .Project(f=> new {Level = f.Level})
                    .Group(f=>f.Level, f => new {Level = f.Key, Count = f.Sum(u=>1)})
                    .SortByDescending(f=>f.Count)
                    .Limit(3);
            var lista = await temp.ToListAsync();
            return lista[0].Level;
        }


        public async Task WriteLog(LogEntry logEntry)
        {
            await logCollection.InsertOneAsync(logEntry);
        }

        public async Task<LogEntry[]> GetLogs()
        {
            var filter = Builders<LogEntry>.Filter.Empty;
            List<LogEntry> logs = await logCollection.Find(filter).ToListAsync();
            return logs.ToArray();
        }


    }
}


