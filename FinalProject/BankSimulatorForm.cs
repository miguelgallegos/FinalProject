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

        //
        private int custInitialAmount;
        private int bankVaultAmount;
        private int numberCustomers;
        private int numberTellers;
        private int custGoal;
        private int maxTransactionAmount;

        private ErrorProvider errorProvider;
        

        public BankSimulatorForm()
        {
            InitializeComponent();

            LoadConfig();

            syncContext =  WindowsFormsSynchronizationContext.Current;

            bankVaultAmount = int.Parse(tboxBankInitialVaultAmount.Text);
            numberCustomers = int.Parse(tboxBankNumberOfCustomers.Text);
            numberTellers = int.Parse(tboxBankNumberOfTellers.Text);
            custGoal = int.Parse(tboxCustGoalAmount.Text);
            maxTransactionAmount = int.Parse(tboxTransactionMaxAmount.Text);

            errorProvider = new ErrorProvider();
            
            tboxBankInitialVaultAmount.Validated += new EventHandler(this.tboxes_Validated);
            tboxBankNumberOfCustomers.Validated += new EventHandler(this.tboxes_Validated);
            tboxBankNumberOfTellers.Validated += new EventHandler(this.tboxes_Validated);
            tboxCustGoalAmount.Validated += new EventHandler(this.tboxes_Validated);
            tboxTransactionMaxAmount.Validated += new EventHandler(this.tboxes_Validated);

            uiHelper = new UIHelper(this);

            
        }


        private void LoadConfig() {

            tboxBankInitialVaultAmount.Text = ConfigurationManager.AppSettings["InitialBankVaultAmount"];
            tboxBankNumberOfCustomers.Text = ConfigurationManager.AppSettings["NumberOfCustomers"];
            tboxBankNumberOfTellers.Text = ConfigurationManager.AppSettings["NumberOfBankers"];
            tboxBankNumberOfCustomers.Text = ConfigurationManager.AppSettings["NumberOfCustomers"];
            tboxCustGoalAmount.Text = ConfigurationManager.AppSettings["FinancialGoalPerCustomer"];

            custInitialAmount = int.Parse(tboxBankInitialVaultAmount.Text) / int.Parse(tboxBankNumberOfCustomers.Text);
            tboxCustInitialAmount.Text = custInitialAmount.ToString();
            
            tboxTransactionMaxAmount.Text = ConfigurationManager.AppSettings["MaximumAmountPerTransaction"];
        
        }
        /*
        public int GetConfigurationInt32(string key, string val, int val1) { 
        
        }
        */

        public void AddListBoxItem(string message){


            syncContext.Post(x => { x = listBox1.Items.Add(message); listBox1.SelectedIndex = listBox1.Items.Count - 1;  Application.DoEvents(); }, null);

        }

        public void AddListBoxItems(string message, string[] messages)
        {
            //????
            if (!string.IsNullOrEmpty(message))
            {
                syncContext.Post(x => { x = listBox1.Items.Add(message); listBox1.SelectedIndex = listBox1.Items.Count - 1; Application.DoEvents(); }, null);
            }
            foreach (string m in messages)
            {
                syncContext.Post(x => { x = listBox1.Items.Add(m); listBox1.SelectedIndex = listBox1.Items.Count - 1; Application.DoEvents(); }, null);
            }
        }

        public void StartButtonState(bool state)
        {
            startBtn.Enabled = state;
        }

        public void StopButtonState(bool state) 
        { 
            stopBtn.Enabled = state;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            bankSimulator = new BankSimulator(uiHelper, bankVaultAmount, numberCustomers, numberTellers, custInitialAmount, custGoal, maxTransactionAmount);

        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            bankSimulator.Stop();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        public void UpdateCustomerInitialAmount(string amount, string val) { 
            //uiHelper.Ad
        }


        private void tboxBankNumberOfCustomers_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxValidationNonEmpty(new List<TextBox>() { tboxBankInitialVaultAmount, tboxBankNumberOfCustomers }))
            {
                custInitialAmount = int.Parse(tboxBankInitialVaultAmount.Text) / int.Parse(tboxBankNumberOfCustomers.Text);
                tboxCustInitialAmount.Text = custInitialAmount.ToString();
            }
            
        }

        private void tboxBankInitialVaultAmount_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxValidationNonEmpty(new List<TextBox>() { tboxBankInitialVaultAmount, tboxBankNumberOfCustomers }))
            {
                custInitialAmount = int.Parse(tboxBankInitialVaultAmount.Text) / int.Parse(tboxBankNumberOfCustomers.Text);
                tboxCustInitialAmount.Text = custInitialAmount.ToString();
            }
        }


        private bool TextBoxValidationNonEmpty(List<TextBox> tboxes)
        {
            int t;
             return tboxes.Count(b => b.Text.Length <= 0 || !int.TryParse(b.Text, out t)) <= 0;
        }



        

        private void tboxes_Validated(object sender, EventArgs e) {
            
            TextBox myTextBox = (TextBox)sender;
            int t;
            if (!int.TryParse(myTextBox.Text, out t))
            {
                errorProvider.SetError(myTextBox, "Please enter a numeric value");
                myTextBox.Focus();

                //TODO: not allowed to start
                uiHelper.StartButton(false);

            }
            else
            {
                
                errorProvider.SetError(myTextBox, string.Empty);
               
               uiHelper.StartButton(true);
                
            }
           
        }


    }
}

///stop threads or put to wait using wait handle? - waitAll waitAny?