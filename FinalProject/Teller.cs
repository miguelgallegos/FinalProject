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

        //TaskStatus
        public TaskStatus Stop() {
            return tellerTask.Status;
        }

        public void ProcessTransaction(Transaction transaction, TransactionGenerator tranGen) {

            //TODO
            //Process the transaction.

            tranGen.ma(this);
            
        }

        private void DoWork()
        {
            //TODO: make update async
            uiHelper.AddTellerStartedMessage(string.Format("  -Teller Started {0}", tellerTask.Id));


                try
                {
                    while (true)
                    {

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
