using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula1.Repositories
{
    public class RaceRepository : IRepository<IRace>
    {
        private List<IRace> races;

        public RaceRepository()
        {
            this.races = new List<IRace>();
        }

        public IReadOnlyCollection<IRace> Models => this.races.AsReadOnly();

        public void Add(IRace race)
        {
            races.Add(race);
        }

        public IRace FindByName(string raceName)
        {
           return races.FirstOrDefault(x=> x.RaceName == raceName);
        }

        public bool Remove(IRace model)
        {
            return races.Remove(model);
        }
    }
}
