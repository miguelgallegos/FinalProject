using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    class UIHelper
    {

        BankSimulatorForm form;



        public UIHelper(BankSimulatorForm form)
        {
            this.form = form;
        }

        public void AddBankOutOfFundsCustomerTransaction(/*Transaction*/ object transaction, decimal funds)
        {

        }

        public void AddCustomerTransaction(/*Transaction*/ object transaction, decimal val, bool val1)
        {

        }
        public void AddListBoxItems(string message, string[] strs ){
        
        }
        public void AddTellerStartedMessage(){}
        public void AddTellerStoppedMessage(){}
        public void AddThreadIdMessage(string str, string str1){}
        public void  AddTransactionGeneratorStartedMessage(){}
        public void  AddTransactionGeneratorStoppedMessage(){}
        public void  StartButton(bool on){}
        public void  StopButton(bool on){}

        public void AddString(string message)
        {
        }

    }
}
