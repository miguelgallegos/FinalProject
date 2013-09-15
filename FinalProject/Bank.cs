using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Collections.Concurrent;

namespace FinalProject
{
    [Serializable] class Bank
    {
        private UIHelper uiHelper;
        [NonSerialized] private CancellationToken cancelToken;
        private BankQueue bankQueue;
        private BankVault bankVault;

        //private List<Teller> tellers;
        private BlockingCollection<Teller> tellers;
        private CustomerList custList;

        private decimal bankVaultAmount;
        private int numberCustomers;
        private int numberTellers;
        private decimal maxTransactionAmount;
        private decimal customerGoal;
        private decimal customersInitAmount;
        public decimal CustomerGoal
        {
            get
            {

                return customerGoal;

            }
        }


        public Bank(UIHelper uiHelper, CancellationToken ct, int numberTellers, int numberCustomers, decimal bankVaultAmount, decimal maxTransactionAmount, decimal customersGoal, decimal customersInitAmount)
        {
            this.uiHelper = uiHelper;
            this.cancelToken = ct;

            this.custList = new CustomerList();
            this.numberCustomers = numberCustomers;
            this.numberTellers = numberTellers;
            this.bankVaultAmount = bankVaultAmount;
            this.maxTransactionAmount = maxTransactionAmount;
            this.customerGoal = customersGoal;
            this.customersInitAmount = customersInitAmount;

            tellers = new BlockingCollection<Teller>();  // List<Teller>();
            bankQueue = new BankQueue(cancelToken);

            InitBank();

        }


        public BankQueue BankQueue() {
            return bankQueue;
        }

        public CustomerList Customers() {
            return custList;
        }

        public BlockingCollection<Teller> Tellers() {
        //public List<Teller> Tellers() {
            return tellers;
        }

        public BankVault BankVault() {
            return bankVault;
        }

        private void InitBank() {

            bankVault = new BankVault(bankVaultAmount);

            for (int i = 0; i < numberTellers; i++)
            {
                uiHelper.AddListBoxItem(string.Format(" +Bank.InitBank adding teller {0}", i));
                tellers.Add(new Teller(uiHelper, cancelToken, this));
            }

            for (int i = 0; i < numberCustomers; i++)
            {
                Random rand = new Random();
                uiHelper.AddListBoxItem(string.Format(" +Bank.InitBank adding customer {0}", i));
                
                custList.SetCustomer(new Customer("customer " + i.ToString(), customersInitAmount));
            }

       
        }


    }
}
