using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
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
        enum TransactionType { Deposit=0, Withdrawal=1 };
        BankQueue bankQueue;
        List<Teller> tellers;
        BlockingCollection<Teller> availTellerQueue;
        BlockingCollection<Teller> unAvailTellerQueue;

        //TODO Add delegate for teller to call to add them to the available queue again when done

       public  TransactionGenerator(UIHelper uiHelper, CancellationToken cancelToken, BankQueue bankQueue, CustomerList customerList, int maxTransAmount, int timeOutThrottle, List<Teller> tellers)
        {
            this.cancelToken = cancelToken;
            this.customerList = customerList;
            this.maxTransAmount = maxTransAmount;
            this.uiHelper = uiHelper;
            this.timeOutThrottle = timeOutThrottle;
            this.bankQueue = bankQueue;
            currentTranAmount = 0;
            rand = new Random();
            this.tellers = tellers;

           foreach(Teller tel in tellers)
           {
               availTellerQueue.Add(tel);
           }

        }



        public TaskStatus Stop()
        {

            return task.Status;
           
        }


        public void CreateTransaction()

        {
            while (!cancelToken.IsCancellationRequested)
            {
                if (currentTranAmount < maxTransAmount)
                {
                    currentTranAmount++;
                    task = new Task(ThreadProc);
                    task.Start();
                    Thread.Sleep(100);
                }
            }
        }

        private void ThreadProc()
        {

            Transaction tran = new Transaction(customerList.GetRandomCustomer(cancelToken), (decimal)rand.Next(1, 20), (int)RandomTransactionType());
            GetAvailableTeller(cancelToken).ProcessTransaction(tran);

        }


        private TransactionType RandomTransactionType()
        {
            Array values = Enum.GetValues(typeof(TransactionType));
            Random random = new Random();
            TransactionType randomTransaction = (TransactionType)values.GetValue(random.Next(values.Length));

            return randomTransaction;
        }

        private Teller GetAvailableTeller(CancellationToken cToken)
        {
               Teller tel;
            
            availTellerQueue.TryTake(out tel,100, cToken);

            if (tel != null)
            {
                unAvailTellerQueue.Add(tel);
            }
            return tel;

        }


        

    }
}
