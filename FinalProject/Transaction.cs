using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Transaction
    {

        private Customer customer;
        private decimal amount;
        private int TransactionType;  



        public Transaction(Customer customer, decimal amount, int TransactionType)
        {
            this.customer = customer;
            this.amount = amount;
            this.TransactionType=TransactionType;


        }
         


    }
}
