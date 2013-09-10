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
        private string name { private set; public get; }
        private decimal balance { private set; public get; }


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
