using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace FinalProject
{
    class TransactionGenerator
    {
        CancellationToken cancelToken;
        CustomerList customerList;
        int maxTransAmount;
        Task task;
        int timeOutThrottle;
        UIHelper uiHelper;
        int currentTranAmount;
        Random rand;
        TransactionType type;
        BankQueue bankQueue;

        TransactionGenerator(UIHelper uiHelper, CancellationToken cancelToken, BankQueue bankQueue, CustomerList customerList, int maxTransAmount, int timeOutThrottle)
        {
            this.cancelToken = cancelToken;
            this.customerList = customerList;
            this.maxTransAmount = maxTransAmount;
            this.uiHelper = uiHelper;
            this.timeOutThrottle = timeOutThrottle;
            this.bankQueue = bankQueue;
            currentTranAmount = 0;
            rand = new Random();
        }



        public TaskStatus Stop()
        {

            return task.Status;

        }


        public void CreateTransaction()

        {
            if(type.type=="Withdrawal")
            {
                type.type="Deposit";
            }
            else
            {
                    type.type="Withdrawal";

            }

            if(currentTranAmount<maxTransAmount)
            {
                currentTranAmount++;
                task = new Task(ThreadProc);
                task.Start();
                Thread.Sleep(100);
            }
             
        }

        private void ThreadProc()
        {

            Transaction tran = new Transaction(customerList.GetRandomCustomer(cancelToken), (decimal)rand.Next(1, 20), type);

        }

       
    }
}
