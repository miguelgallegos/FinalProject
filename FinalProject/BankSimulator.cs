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
        private object transactionGenerator;

        private int bankVaultAmount;
        private int numberCustomers;
        private int numberTellers;
        private int initCustomersAmount;
        private int customersGoal;
        private int maxTransactionAmount;
        private UIHelper uiHelper;

        

        public BankSimulator(UIHelper uiHelper, int bankVaultAmount, int numberCustomers, int numberTellers, int initCustomersAmount, int customersGoal, int maxTransactionAmount)
        {
            this.bankVaultAmount = bankVaultAmount;
            this.numberCustomers = numberCustomers;
            this.numberTellers = numberTellers;
            this.initCustomersAmount = initCustomersAmount;
            this.customersGoal = customersGoal;
            this.maxTransactionAmount = maxTransactionAmount;
            this.uiHelper = uiHelper;

            

        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
        }

        private void Simulate()
        {

        }
        

    }
}
