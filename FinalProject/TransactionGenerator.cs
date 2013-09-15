using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace FinalProject
{
    class TransactionGenerator
    {
        [NonSerialized] CancellationToken cancelToken;
        CustomerList customerList;
        int maxTransAmount;
        Task  task;
        int timeOutThrottle;
        UIHelper uiHelper;
        int currentTranAmount;
        Random rand;
        public enum TransactionType { Deposit=0, Withdrawal=1 };
        BankQueue bankQueue;
        //List<Teller> tellers;
        BlockingCollection<Teller> tellers;
        BlockingCollection<Teller> availTellerQueue;
        BlockingCollection<Teller> unAvailTellerQueue;
        
        private Bank bank;


        public const string SerializedFileName = "Bank.xml";
        public FileStream stream;
        public SoapFormatter formatter;

       public  TransactionGenerator(UIHelper uiHelper, CancellationToken cancelToken, BankQueue bankQueue, CustomerList customerList, int maxTransAmount, int timeOutThrottle,
            /*List<Teller> tellers,*/  BlockingCollection<Teller> tellers, Bank bank)
        {
            this.cancelToken = cancelToken;
            this.customerList = customerList;
            this.maxTransAmount = maxTransAmount;
            this.uiHelper = uiHelper;
            this.timeOutThrottle = timeOutThrottle;
            this.bankQueue = bankQueue;
            this.bank = bank;

            currentTranAmount = 0;
            rand = new Random();
            this.tellers = tellers;
            availTellerQueue = new BlockingCollection<Teller>();
            unAvailTellerQueue = new BlockingCollection<Teller>();

           foreach(Teller tel in tellers)
           {
               availTellerQueue.Add(tel);
           }


           task = new Task(Generate);
           task.Start();
        }



        public TaskStatus Stop()
        {

            return task.Status;
           
        }


    
        private void Generate()
        {
            try
            {

                while (!cancelToken.IsCancellationRequested)
                {

                    {
                        currentTranAmount++;

                        Customer cust = customerList.GetRandomCustomer(cancelToken);
                        if (cust != null)
                        {
                            Transaction tran = new Transaction(cust, (decimal)rand.Next(1, maxTransAmount), RandomTransactionType());
                            tran.TransactionGenerator = this;
                            bank.BankQueue().Enqueue(tran);
                            cancelToken.ThrowIfCancellationRequested();
                        }

                        Thread.Sleep(100);
                    }
                }
            }
            catch (OperationCanceledException )
            {
            
            }
        
            finally{

                uiHelper.AddTellerStoppedMessage(string.Format("TransactionGenerator {0} Stopped!", task.Id));


                //Tried but was unable to serialize current state to include bank, customers, history, etc..  
                // Worked through a bunch of "not serializable" errors,
                //  and marked the appropriate fields/properties as [NonSerialized]; 
                //but couldn't track them all down.

                //try
                //{
                //    formatter = new SoapFormatter();
                //    using (stream = new FileStream(SerializedFileName, FileMode.Create))
                //    {
                //        //Write values to SerializedFileName
                //        formatter.Serialize(stream, bank);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(string.Format("Error Serializing to {0}. Message = {1}", SerializedFileName, ex.Message));
                //}
            }
        }


        private TransactionType RandomTransactionType()
        {
            Array values = Enum.GetValues(typeof(TransactionType));
            Random random = new Random();
            TransactionType randomTransaction = (TransactionType)values.GetValue(random.Next(values.Length));

            return randomTransaction;
        }

      
    }
}
