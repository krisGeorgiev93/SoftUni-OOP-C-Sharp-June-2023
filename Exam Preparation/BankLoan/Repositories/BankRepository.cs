using BankLoan.Models.Contracts;
using BankLoan.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoan.Repositories
{
    public class BankRepository : IRepository<IBank>
    {
        private ICollection<IBank> banks;
        public BankRepository()
        { 
        this.banks = new List<IBank>();  
        }
        public IReadOnlyCollection<IBank> Models => this.banks.ToList().AsReadOnly();

        public void AddModel(IBank model)
        {
            banks.Add(model);
        }

        public IBank FirstModel(string name)
        {
            return banks.FirstOrDefault(x => x.Name == name);
        }

        public bool RemoveModel(IBank model)
        {
            return banks.Remove(model);
        }
    }
}
