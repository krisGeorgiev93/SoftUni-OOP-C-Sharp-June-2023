using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        private List<IPlayer> models;

        public PlayerRepository()
        {
            models = new List<IPlayer>();
        }

        public IReadOnlyCollection<IPlayer> Models => models.AsReadOnly();

        public void AddModel(IPlayer model) => models.Add(model);

        public bool ExistsModel(string name)
        {
            var currentPlayer = models.Where(p => p.Name == name).FirstOrDefault();
            if (currentPlayer is not null)
            {
                return true;
            }
            return false;
        }

        public IPlayer GetModel(string name)
        {
            var currentPlayer = models.Where(p => p.Name == name).FirstOrDefault();
            return currentPlayer;
        }

        public bool RemoveModel(string name)
        {
            var currentPlayer = models.Where(p => p.Name == name).FirstOrDefault();
            return models.Remove(currentPlayer);
        }
    }
}
