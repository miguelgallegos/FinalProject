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

        public void AddBankOutOfFundsCustomerTransaction(Transaction transaction, Teller teller)
        {
            form.AddListBoxItem(string.Format("  <<<<<{0} of ${1} for {2}, assisted by {3} failed because the BANK OUT OF MONEY", transaction.Type() == TransactionGenerator.TransactionType.Deposit ? "Deposit" : "Withdrawal",
               transaction.Amount(), transaction.Customer().Name, teller));
           form.Invoke(form.Enable);
        }

        public void AddCustomerOutOfFundsCustomerTransaction(Transaction transaction,Teller teller)
        {
            form.AddListBoxItem(string.Format("  <<<{0} of ${1} for {2}, assisted by {3} failed because the CUSTOMER BALANCE of ${4} is LOWER than the withdrawal amount", transaction.Type() == TransactionGenerator.TransactionType.Deposit ? "Deposit" : "Withdrawal",
                transaction.Amount(), transaction.Customer().Name, teller, transaction.Customer().Balance));
           
        }

        public void AddCustomerTransaction(Transaction transaction, Teller teller, decimal bankBalance)//, decimal val, bool val1
        {
            string type = transaction.Type() == TransactionGenerator.TransactionType.Deposit ? "Deposit" : "Withdrawal";

            form.AddListBoxItem(string.Format("  {6}{0} of {1} from {2}, assisted by {3}, balance ${4},       [Bank Balance: ${5}]", type,
                transaction.Amount(), transaction.Customer().Name, teller, transaction.Customer().Balance, bankBalance, type == "Deposit"?">>>" : "<<<" ));
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

        public void AddGoalReachedCustomerTransaction(Transaction transaction, Teller teller)
        {
            form.AddListBoxItem(string.Format("  >>>>>{0} of ${1} for {2}, assisted by {3} resulted in Customer Goal Reached", transaction.Type() == TransactionGenerator.TransactionType.Deposit ? "Deposit" : "Withdrawal",
                          transaction.Amount(), transaction.Customer().Name, teller));
           form.Invoke(form.Enable);

        }

        public void BankAmountFinalUpdate(decimal amount){
            form.BankAmountFinalUpdate(amount);
        }

    }
}
