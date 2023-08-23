using Handball.Models.Contracts;
using Handball.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handball.Repositories
{
    public class TeamRepository : IRepository<ITeam>
    {
        private List<ITeam> models;

        public TeamRepository()
        {
            models = new List<ITeam>();
        }

        public IReadOnlyCollection<ITeam> Models => models.AsReadOnly();

        public void AddModel(ITeam model) => models.Add(model);

        public bool ExistsModel(string name)
        {
            var currentTeam = models.Where(t => t.Name == name).FirstOrDefault();
            if (currentTeam is not null)
            {
                return true;
            }
            return false;
        }

        public ITeam GetModel(string name)
        {
            var currentTeam = models.Where(t => t.Name == name).FirstOrDefault();
            return currentTeam;
        }

        public bool RemoveModel(string name)
        {
            var currentTeam = models.Where(t => t.Name == name).FirstOrDefault();
            return models.Remove(currentTeam);
        }
    }
}
