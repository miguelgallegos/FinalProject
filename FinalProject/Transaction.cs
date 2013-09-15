using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable] class Transaction
    {

        private Customer customer;
        private decimal amount;
        TransactionGenerator.TransactionType transactionType;
        [NonSerialized]
        TransactionGenerator transGen;

        private string transactionId;

        public Transaction(Customer customer, decimal amount, TransactionGenerator.TransactionType transactionType)
        {
            this.customer = customer;
            this.amount = amount;
            this.transactionType = transactionType;
            customer.AddTransaction(this);
    

        }


        public TransactionGenerator TransactionGenerator
        {
            set { transGen = value; }
            get { return transGen; }

        }

        public decimal Amount()
        {
            return this.amount;
        }

        public Customer Customer()
        {
            return this.customer;
        }

        public TransactionGenerator.TransactionType Type()
        {
            return this.transactionType;
        }

        public override string ToString() {

            transactionId = string.Format("{0}-Type{1}-{2:yyyy-MM-dd_hh-mm-ss-tt}", customer.Name.Replace(' ', '_'), this.transactionType == TransactionGenerator.TransactionType.Deposit ? "Deposit" : "Widraw", DateTime.Now);

            return transactionId;
            
        }


    }
}
