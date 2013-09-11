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

        public void AddBankOutOfFundsCustomerTransaction(Transaction transaction, decimal funds)
        {

        }

        public void AddCustomerTransaction(Transaction transaction, Teller teller)//, decimal val, bool val1
        {
            form.AddListBoxItem(string.Format("  #{0} of {1} from {2}, assisted by {3}, balance ${4}", transaction.Type() == TransactionGenerator.TransactionType.Deposit?"Deposit":"Withdrawal",
                transaction.Amount(), transaction.Customer().Name, teller, transaction.Customer().Balance));
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
