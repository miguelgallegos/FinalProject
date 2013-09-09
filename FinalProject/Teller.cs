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

        object bank;
        CancellationToken cancelToken;
        UIHelper uiHelper;

        public Teller(UIHelper uiHelper, CancellationToken cancelToken, object bank) {
            this.cancelToken = cancelToken;
            this.uiHelper = uiHelper;
            this.bank = bank;
        }

        //TaskStatus
        public object Stop() {
            return new object();
        }

        public void ProcessTransaction(object transaction) { 
            
        }

        private void ThreadProc()
        {

        }



    }
}
