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
        private List<Teller> tellers;
        private object bank;
        private BankQueue bankQueue;
        private CustomerList custList;
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

            this.custList = new CustomerList();
            //starts
            simulatorTask = new Task(Simulate);
            simulatorTask.Start();

            tellers = new List<Teller>();
            bank = new object();
            bankQueue = new BankQueue();

            cancelTokenSource = new CancellationTokenSource();
        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
        }

        private void Simulate()
        {
          

            uiHelper.AddListBoxItems("Adding messages", new[] {"Text test1", "Text test2" });

            for (int i = 0; i < numberTellers; i++)
            {
                uiHelper.AddListBoxItem(string.Format(" +BankSimulator.Simulate adding teller {0}", i));
                tellers.Add(new Teller(uiHelper, cancelTokenSource.Token, bank));

            }

            for (int i = 0; i < numberCustomers; i++)
            {
                 Random rand = new Random();
                uiHelper.AddListBoxItem(string.Format(" +BankSimulator.Simulate adding customer {0}", i));
               custList.SetCustomer(new Customer( "customer "+i.ToString() , (decimal)rand.Next(1, 20)));

            }

            transactionGenerator = new TransactionGenerator(uiHelper, cancelTokenSource.Token, bankQueue, custList, maxTransactionAmount, timeOutThrottle, tellers);
        }
        

    }
}
