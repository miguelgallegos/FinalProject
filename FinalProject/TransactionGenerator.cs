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
        Task  task;
        int timeOutThrottle;
        UIHelper uiHelper;
        int currentTranAmount;
        Random rand;
        enum TransactionType { Deposit=0, Withdrawal=1 };
        BankQueue bankQueue;
        List<Teller> tellers;
        BlockingCollection<Teller> availTellerQueue;
        BlockingCollection<Teller> unAvailTellerQueue;
        public delegate void MakeAvailable(Teller tel);
        public MakeAvailable ma;

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

            ma = new MakeAvailable(MakeTellerAvailable);
            availTellerQueue = new BlockingCollection<Teller>();
            unAvailTellerQueue = new BlockingCollection<Teller>();

           foreach(Teller tel in tellers)
           {
               availTellerQueue.Add(tel);
           }

           CreateTransaction();

        }



        public TaskStatus Stop()
        {

            return task.Status;
           
        }


        private void CreateTransaction()
           
        {
            try
            {

                while (!cancelToken.IsCancellationRequested)
                {
                    if (currentTranAmount < maxTransAmount)
                    {
                        currentTranAmount++;
                        task = Task.Factory.StartNew(() =>
                        {
                            Customer cust = customerList.GetRandomCustomer(cancelToken);
                            if (cust != null)
                            {
                                Transaction tran = new Transaction(cust, (decimal)rand.Next(1, 20), (int)RandomTransactionType());
                                Teller tel = GetAvailableTeller();
                                tel.ProcessTransaction(tran, this);
                                cancelToken.ThrowIfCancellationRequested();
                            }
                           
                            Thread.Sleep(100);
                        });
                    }
                }
            }
            catch (OperationCanceledException oce)
            {
            }
                finally{

                uiHelper.AddTellerStoppedMessage(string.Format("TransactionGenerator {0} Stopped!", task.Id));
            }
        }


        private TransactionType RandomTransactionType()
        {
            Array values = Enum.GetValues(typeof(TransactionType));
            Random random = new Random();
            TransactionType randomTransaction = (TransactionType)values.GetValue(random.Next(values.Length));

            return randomTransaction;
        }

        private Teller GetAvailableTeller()
        {
               Teller tel;

               availTellerQueue.TryTake(out tel, timeOutThrottle, cancelToken);

            if (tel != null)
            {
                unAvailTellerQueue.Add(tel);
            }
            return tel;

        }

        public void MakeTellerAvailable(Teller tel)
            
        {
            unAvailTellerQueue.TryTake(out tel, timeOutThrottle, this.cancelToken);
            availTellerQueue.Add(tel);
        }
        

    }
}
