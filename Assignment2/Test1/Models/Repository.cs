using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test1.Models
{

    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);
    }

    public class InMemoryRepository : IRepository
    {

        public List<Player> playerList = new List<Player>();
        public async Task<Player> Get(Guid id)  //Tarvitaanko await?
        {
            foreach(var playervar in playerList)
            {
                if (playervar.Id == id)
                {
                    // Task<Player> derp = await playervar;
                    return playervar;
                    // return Task.FromResult<Player>(playervar);
                }
            }
            return null;
            // return Task.FromResult<Player>(null);
        }

        public async Task<Player[]> GetAll()
        {
            return playerList.ToArray();
            // return Task.FromResult<Player[]>(playerList);
        }

        public async Task<Player> Create(Player player)
        {
            playerList.Add(player);
            return player;  //Miksi palauttaa sama player? Miksei esim bool onnistuiko vai ei
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
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

        public async Task<Player> Delete(Guid id)
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


    }
}