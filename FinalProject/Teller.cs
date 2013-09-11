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

        private Task tellerTask;

        public Teller(UIHelper uiHelper, CancellationToken cancelToken, Bank bank) {
            this.cancelToken = cancelToken;
            this.uiHelper = uiHelper;
            this.bank = bank;

            tellerTask = new Task(DoWork);
            tellerTask.Start();

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

            tranGen.ma(this);
            
        }
        public void ProcessTransaction(Transaction transaction)
        {

            //TODO
            //Process the transaction.

            //tranGen.ma(this);

            Customer aCustomer = transaction.Customer();
            decimal newBankVaultAmount = 0;
            if(transaction.Type() == TransactionGenerator.TransactionType.Deposit){
                newBankVaultAmount = bank.BankVault().Deposit(transaction.Amount());

                aCustomer.Balance += transaction.Amount();


            }else if(transaction.Type() == TransactionGenerator.TransactionType.Withdrawal){

                if(bank.BankVault().Withdraw(transaction.Amount(), decimal.Parse("0"))){
                    aCustomer.Balance -= transaction.Amount();

                    newBankVaultAmount = bank.BankVault().Balance();

                }else{
                    //cancelToken.
                    
                }

            }

            bank.Customers().MakeCustomerAvailable(aCustomer, cancelToken);

            uiHelper.AddCustomerTransaction(transaction, this);
            uiHelper.AddListBoxItem(string.Format("         BankBalance: ${0}", newBankVaultAmount));

            //transaction.TransactionGenerator.ma(this);
            //decimal customerBalance = transaction.

        }
        private void DoWork()
        {
            //TODO: make update async
            uiHelper.AddTellerStartedMessage(string.Format("  -Teller Started {0}", tellerTask.Id));

            Transaction transactionToProcess;

                try
                {
                    while (true)
                    {

                        transactionToProcess = bank.BankQueue().Dequeue();

                       // uiHelper.AddListBoxItem("    --->TELLER.DoWork. VERIFY_TRANSACTION: " + transactionToProcess);

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
                    uiHelper.AddTellerStoppedMessage(string.Format("    --Teller Stopped {0}!", tellerTask.Id));
                }


            


        }



    }
}
