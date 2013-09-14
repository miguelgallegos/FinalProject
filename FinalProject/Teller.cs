using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    class Teller
    {
        //shared data
        Bank bank;
        CancellationToken cancelToken;
        UIHelper uiHelper;
        BankVault bankVault;
        private Task tellerTask;
        decimal bankBalance;

        public Teller(UIHelper uiHelper, CancellationToken cancelToken, Bank bank) {
            this.cancelToken = cancelToken;
            this.uiHelper = uiHelper;
            this.bank = bank;
                tellerTask = new Task(DoWork);
                tellerTask.Start();

                uiHelper.AddTellerStartedMessage(string.Format(" -Teller Started {0}", tellerTask.Id));

        }

        public override string ToString() {
            return "Teller-" + tellerTask.Id;
        }

        //TaskStatus
        public TaskStatus Stop() {
            return tellerTask.Status;
        }

        public void ProcessTransaction(Transaction transaction, TransactionGenerator tranGen) {

            //TODO
            //Process the transaction.

       
            
        }
        public void ProcessTransaction(Transaction transaction)
        {
            Customer aCustomer = transaction.Customer();
            if(transaction.Type() == TransactionGenerator.TransactionType.Deposit ){
           
                aCustomer.Balance += transaction.Amount();

               bankBalance= bank.BankVault().Deposit(transaction.Amount());
                OutPutTran(transaction);
                if (aCustomer.Balance >= bank.CustomerGoal)
                {
                    uiHelper.AddGoalReachedCustomerTransaction(transaction, this);
                }

            }else if(transaction.Type() == TransactionGenerator.TransactionType.Withdrawal){

                decimal diff;
                bool bankEmpty;
                //Checks customer balance first
                if (aCustomer.Balance >= transaction.Amount())
                {
                    if (bank.BankVault().Withdraw(transaction.Amount(), out diff, out bankEmpty))
                    {
                        //Normal Transaction
                        if (!bankEmpty)
                        {
                            aCustomer.Balance -= transaction.Amount();
                            OutPutTran(transaction);
                        }
                        //Get the remainder of bank funds if withdrawal amount >bank balance
                        if(bankEmpty &&diff>0)
                        {
                            aCustomer.Balance -= (transaction.Amount()-diff);
                            uiHelper.AddBankOutOfFundsCustomerTransaction(transaction, this);
                           
                        }
                        
                    }
                    else
                    {
                        OutPutTran(transaction);
                        uiHelper.AddBankOutOfFundsCustomerTransaction(transaction, this);

                    }
                    //Customer doesnt have the funds
                }
                else
                {
                    uiHelper.AddCustomerOutOfFundsCustomerTransaction(transaction, this);

                }

            }

            bank.Customers().MakeCustomerAvailable(aCustomer, cancelToken);


          

            //decimal customerBalance = transaction.

        }
        private void DoWork()
        {
            
            try
            {
                while (!cancelToken.IsCancellationRequested)
                {
                    

                    Transaction transactionToProcess;
                    transactionToProcess = bank.BankQueue().Dequeue();

                    // uiHelper.AddListBoxItem("    --->TELLER.DoWork. VERIFY_TRANSACTION: " + transactionToProcess);
                    if (transactionToProcess != null)
                        ProcessTransaction(transactionToProcess);
      
                    cancelToken.ThrowIfCancellationRequested();

                    Thread.Sleep(100);

                }
            }
            catch (OperationCanceledException oce)
            {

            }
           
                finally
                {
                    uiHelper.AddTellerStoppedMessage(string.Format("  --Teller Stopped {0}", tellerTask.Id));
 
                }
        }


        private void OutPutTran(Transaction transaction)
        {
            uiHelper.AddCustomerTransaction(transaction, this, bank.BankVault().Balance());
            uiHelper.BankAmountFinalUpdate(bank.BankVault().Balance());
        }

        }

}
