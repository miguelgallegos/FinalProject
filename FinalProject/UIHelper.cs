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

        public void AddListBoxItem(string message) {
            form.AddListBoxItem(message);
        }

        public void AddListBoxItems(string message, string[] messages ){
            form.AddListBoxItems(message, messages);
        }
        public void AddTellerStartedMessage(string message){
            form.AddListBoxItem(message);
        }
        public void AddTellerStoppedMessage(string message){
            form.AddListBoxItem(message);
        }
        public void AddThreadIdMessage(string str, string str1){}
        public void  AddTransactionGeneratorStartedMessage(){}
        public void  AddTransactionGeneratorStoppedMessage(){}
        public void  StartButton(bool state){
            form.StartButtonState(state);
        }
        public void  StopButton(bool state){
            form.StopButtonState(state);
        }

    }
}
