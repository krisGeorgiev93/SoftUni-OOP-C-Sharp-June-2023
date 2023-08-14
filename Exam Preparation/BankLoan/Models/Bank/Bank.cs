using BankLoan.Models.Contracts;
using BankLoan.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoan.Models.Bank
{
    public abstract class Bank : IBank
    {
        private string name;
        private int capacity;
        private ICollection<ILoan> loans;
        private ICollection<IClient> clients;

        public Bank(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            this.loans = new List<ILoan>();
            this.clients = new List<IClient>();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.BankNameNullOrWhiteSpace);
                }
                name = value;
            }
        }

        public int Capacity { get; private set; }

        public IReadOnlyCollection<ILoan> Loans => this.loans.ToList().AsReadOnly();

        public IReadOnlyCollection<IClient> Clients => this.clients.ToList().AsReadOnly();

        public void AddClient(IClient Client)
        {
            if (clients.Count >= this.Capacity)
            {
                throw new ArgumentException(ExceptionMessages.NotEnoughCapacity);
            }
            clients.Add(Client);
        }

        public void AddLoan(ILoan loan)
        {
            loans.Add(loan);
        }

        public string GetStatistics()
        {
           StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Name: {this.Name}, Type: {this.GetType().Name}");
            if (clients.Count > 0)
            {
                stringBuilder.AppendLine($"Clients: {string.Join(", ", clients.Select(x=> x.Name))}");
            }
            else
            {
                stringBuilder.AppendLine("Clients: none");
            }
            stringBuilder.AppendLine($"Loans: {loans.Count}, Sum of Rates: {SumRates()}");
            return stringBuilder.ToString().TrimEnd();
        }

        public void RemoveClient(IClient Client)
        {
            clients.Remove(Client);
        }

        public double SumRates() => loans.Sum(x => x.InterestRate);
       
    }
}
