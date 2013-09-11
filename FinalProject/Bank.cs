﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject
{
    class Bank
    {
        private UIHelper uiHelper;
        private CancellationToken cancelToken;
        private BankQueue bankQueue;
        private BankVault bankVault;

        private List<Teller> tellers;
        private CustomerList custList;

        private decimal bankVaultAmount;
        private int numberCustomers;
        private int numberTellers;
        private decimal maxTransactionAmount;


        public Bank(UIHelper uiHelper, CancellationToken ct, int numberTellers, int numberCustomers, decimal bankVaultAmount, decimal maxTransactionAmount)
        {
            this.uiHelper = uiHelper;
            this.cancelToken = ct;

            this.custList = new CustomerList();
            this.numberCustomers = numberCustomers;
            this.numberTellers = numberTellers;
            this.bankVaultAmount = bankVaultAmount;
            this.maxTransactionAmount = maxTransactionAmount;


            tellers = new List<Teller>();
            bankQueue = new BankQueue(cancelToken);

            InitBank();

        }


        public BankQueue BankQueue() {
            return bankQueue;
        }

        public CustomerList Customers() {
            return custList;
        }

        public List<Teller> Tellers() {
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
                custList.SetCustomer(new Customer("customer " + i.ToString(), (decimal)rand.Next(1, 20)));
            }

       
        }


    }
}
