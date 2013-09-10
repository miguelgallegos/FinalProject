using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Transaction
    {

        private Customer customer { private set; public get; }
        private decimal amount { private set; get; }
        private TransactionType type { private set; get; }


        public Transaction(Customer customer, decimal amount, TransactionType type)
        {
            this.customer = customer;
            this.amount = amount;
            this.type=type;
        }
         


    }
}
