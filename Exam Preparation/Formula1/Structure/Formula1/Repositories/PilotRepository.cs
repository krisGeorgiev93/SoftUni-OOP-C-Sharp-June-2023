using Formula1.Models.Contracts;
using Formula1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula1.Repositories
{
    public class PilotRepository : IRepository<IPilot>
    {
        private List<IPilot> pilots;
        public PilotRepository()
        {
            this.pilots = new List<IPilot>();
        }
        public IReadOnlyCollection<IPilot> Models => this.pilots.AsReadOnly();

        public void Add(IPilot pilot)
        {
            pilots.Add(pilot);
        }

        public IPilot FindByName(string name)
        {
           return pilots.FirstOrDefault(x=> x.FullName == name); // Returns the first pilot with the given fullName. Otherwise, returns null.
        }

        public bool Remove(IPilot model)
        {
            return pilots.Remove(model);
        }
    }
}
