using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject
{
    class BankSimulator
    {

        private CancellationTokenSource cancelTokenSource;
        private TransactionGenerator transactionGenerator;

        private int bankVaultAmount;
        private int numberCustomers;
        private int numberTellers;
        private int initCustomersAmount;
        private int customersGoal;
        private int maxTransactionAmount;
        private UIHelper uiHelper;

        private Task simulatorTask;

        private Bank bank;
        
        
        //TODO need to have this added to the config?
        private int timeOutThrottle = 100;

        public BankSimulator(UIHelper uiHelper, int bankVaultAmount, int numberCustomers, int numberTellers, int initCustomersAmount, int customersGoal, int maxTransactionAmount)
        {
            this.bankVaultAmount = bankVaultAmount;
            this.numberCustomers = numberCustomers;
            this.numberTellers = numberTellers;
            this.initCustomersAmount = initCustomersAmount;
            this.customersGoal = customersGoal;
            this.maxTransactionAmount = maxTransactionAmount;
            this.uiHelper = uiHelper;

            //starts
            simulatorTask = new Task(Simulate);
            simulatorTask.Start();

            cancelTokenSource = new CancellationTokenSource();
            
        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
        }

        private void Simulate()
        {

            //uiHelper.AddListBoxItems("Adding messages", new[] {"Text test1", "Text test2" });
            uiHelper.AddListBoxItem("BankSimulator.Simulate Started...");

            bank = new Bank(uiHelper, cancelTokenSource.Token, numberTellers, numberCustomers, bankVaultAmount, maxTransactionAmount);

            transactionGenerator = new TransactionGenerator(uiHelper, cancelTokenSource.Token, bank.BankQueue(), bank.Customers(), maxTransactionAmount, timeOutThrottle, bank.Tellers(), bank);
        }
        

    }
}
