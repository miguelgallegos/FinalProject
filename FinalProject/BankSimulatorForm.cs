using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace FinalProject
{
    public partial class BankSimulatorForm : Form
    {

        BankSimulator bankSimulator;
        SynchronizationContext syncContext;
        UIHelper uiHelper;

        public BankSimulatorForm()
        {
            InitializeComponent();

            LoadConfig();

        }


        private void LoadConfig() {

            tboxBankInitialVaultAmount.Text = ConfigurationManager.AppSettings["InitialBankVaultAmount"];
            tboxBankNumberOfCustomers.Text = ConfigurationManager.AppSettings["NumberOfCustomers"];
            tboxBankNumberOfTellers.Text = ConfigurationManager.AppSettings["NumberOfBankers"];
            tboxBankNumberOfCustomers.Text = ConfigurationManager.AppSettings["NumberOfCustomers"];
            tboxCustGoalAmount.Text = ConfigurationManager.AppSettings["FinancialGoalPerCustomer"];
            tboxCustInitialAmount.Text = ConfigurationManager.AppSettings["CustomersInitialAmount"];
            tboxTransactionMaxAmount.Text = ConfigurationManager.AppSettings["MaximumAmountPerTransaction"];
        
        }
        /*
        public int GetConfigurationInt32(string key, string val, int val1) { 
        
        }
        */

        public void AddListBoxItem(string message){
            
        }

        public void AddListBoxItems(string message, string[] messages)
        {

        }

        public void StartButtonState(bool on)
        {
        }

        public void StopButtonState(bool on) 
        { 
        
        }
        protected override void OnClosing(CancelEventArgs e)
        {
        }

        private void startBtn_Click(object sender, EventArgs e)
        {

        }

        private void stopBtn_Click(object sender, EventArgs e)
        {

        }

        private void clearBtn_Click(object sender, EventArgs e)
        {

        }

        public void UpdateCustomerInitialAmount(string amount, string val) { 
        
        }



    }
}

///stop threads or put to wait using wait handle? - waitAll waitAny?