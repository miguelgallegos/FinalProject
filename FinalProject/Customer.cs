using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Customer
    {
        List<Transaction> customerHistory;
        private string name;
        private decimal balance;

        public string Name
        {
            get
            {
               
                return name;

            }
        }

        public decimal Balance
        {

            set { balance = value; }

            get
            {
                return balance;

            }
        }

        public Customer(string name, decimal balance)
        {
            this.name = name;
            this.balance = balance;
            customerHistory = new List<Transaction>();
        }

        public void AddTransaction(Transaction transaction)
        {
            customerHistory.Add(transaction);
        }

    }
}
