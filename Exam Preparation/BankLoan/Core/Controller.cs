using BankLoan.Core.Contracts;
using BankLoan.Models.Bank;
using BankLoan.Models.Client;
using BankLoan.Models.Contracts;
using BankLoan.Models.Loan;
using BankLoan.Repositories;
using BankLoan.Repositories.Contracts;
using BankLoan.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoan.Core
{
    public class Controller : IController
    {
        private IRepository<ILoan> loans;
        private IRepository<IBank> banks;

        public Controller()
        {
            this.loans = new LoanRepository();
            this.banks = new BankRepository();

        }
        public string AddBank(string bankTypeName, string name)
        {
            IBank bank;
            if (bankTypeName == nameof(BranchBank))
            {
                bank = new BranchBank(name);
            }
            else if (bankTypeName == nameof(CentralBank))
            {
                bank = new CentralBank(name);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.BankTypeInvalid);
            }
            banks.AddModel(bank);
            return string.Format(OutputMessages.BankSuccessfullyAdded, bankTypeName);


        }

        public string AddClient(string bankName, string clientTypeName, string clientName, string id, double income)
        {
            IClient client;
            IBank bank;

            if (clientTypeName != nameof(Student) && clientTypeName != nameof(Adult))
            {
                throw new ArgumentException(ExceptionMessages.ClientTypeInvalid);
            }
            bank = banks.FirstModel(bankName);
            if (clientTypeName == nameof(Student) && bank.GetType().Name == nameof(CentralBank))
            {
                return string.Format(OutputMessages.UnsuitableBank);
            }
            else if (clientTypeName == nameof(Adult) && bank.GetType().Name == nameof(BranchBank))
            {
                return string.Format(OutputMessages.UnsuitableBank);
            }

            if (clientTypeName == nameof(Adult))
            {
                client = new Adult(clientName, id, income);
            }
            else
            {
                client = new Student(clientName, id, income);
            }
            bank.AddClient(client);
            return string.Format(OutputMessages.ClientAddedSuccessfully, clientTypeName, bankName);
        }

        public string AddLoan(string loanTypeName)
        {
            ILoan loan;
            if (loanTypeName != nameof(StudentLoan) && loanTypeName != nameof(MortgageLoan))
            {
                throw new ArgumentException(ExceptionMessages.LoanTypeInvalid);
            }
            if (loanTypeName == nameof(StudentLoan))
            {
                loan = new StudentLoan();
            }
            else
            {
                loan = new MortgageLoan();
            }
            loans.AddModel(loan);
            return string.Format(OutputMessages.LoanSuccessfullyAdded, loanTypeName);
        }

        public string FinalCalculation(string bankName)
        {
            IBank bank = banks.FirstModel(bankName);
            double funds = 0;
            double totalIncome = (bank.Clients.Sum(x => x.Income));
            double totalLoans = bank.Loans.Sum(x => x.Amount);
            funds += totalIncome;
            funds += totalLoans;
            return string.Format(OutputMessages.BankFundsCalculated, bankName, $"{funds:f2}");
        }

        public string ReturnLoan(string bankName, string loanTypeName)
        {
            ILoan loan = loans.FirstModel(loanTypeName);
            IBank bank = banks.FirstModel(bankName);

            if (loan == null)
            {
                return string.Format(ExceptionMessages.MissingLoanFromType, loanTypeName);
            }

            bank.AddLoan(loan);
            loans.RemoveModel(loan);
            return string.Format(OutputMessages.LoanReturnedSuccessfully, loanTypeName, bankName);
        }

        public string Statistics()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var bank in banks.Models)
            {
                sb.AppendLine(bank.GetStatistics());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
