using BankLoan.Models.Contracts;
using BankLoan.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoan.Repositories
{
    public class LoanRepository : IRepository<ILoan>
    {
        private ICollection<ILoan> loans;
        public LoanRepository() 
        {
        this.loans = new List<ILoan>(); 
        }

        public IReadOnlyCollection<ILoan> Models => this.loans.ToList().AsReadOnly();

        public void AddModel(ILoan model)
        {
            loans.Add(model);
        }

        public ILoan FirstModel(string name)
        {
            return loans.FirstOrDefault(x=> x.GetType().Name == name);
        }

        public bool RemoveModel(ILoan model)
        {
            return loans.Remove(model);
        }
    }
}
