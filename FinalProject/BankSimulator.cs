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
        public delegate void StopBankSimulation();
        public StopBankSimulation StopSimulation;
        
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
            this.StopSimulation = Stop;
            //starts
            cancelTokenSource = new CancellationTokenSource();

            simulatorTask = new Task(Simulate);
           
            simulatorTask.Start();

           
        }

        public void Stop()
        {
            if (!cancelTokenSource.IsCancellationRequested)
            {
                cancelTokenSource.Cancel();
                uiHelper.AddListBoxItem("BankSimulator.Simulate Ended...");
            }
        }

        private void Simulate()
        {



            bank = new Bank(uiHelper, cancelTokenSource.Token, numberTellers, numberCustomers, bankVaultAmount, maxTransactionAmount, customersGoal);
            //uiHelper.AddListBoxItems("Adding messages", new[] {"Text test1", "Text test2" });
            uiHelper.AddListBoxItem("BankSimulator.Simulate Started...");
           
                transactionGenerator = new TransactionGenerator(uiHelper, cancelTokenSource.Token, bank.BankQueue(), bank.Customers(), maxTransactionAmount, timeOutThrottle, bank.Tellers(), bank);
            
        
         
        }
        

    }
}
