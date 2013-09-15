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
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;



namespace FinalProject
{
    public partial class BankSimulatorForm : Form
    {

        BankSimulator bankSimulator;
        SynchronizationContext syncContext;
        UIHelper uiHelper;
        public delegate void Enabler();
        public Enabler Enable;

        private int custInitialAmount;
        private int bankVaultAmount;
        private int numberCustomers;
        private int numberTellers;
        private int custGoal;
        private int maxTransactionAmount;
        private SerializableDataStructure serializableDataStructure;
        private bool myTextBoxChanging = false;


        private ErrorProvider errorProvider;

        public const string SerializedFileName = "SavedInitialSettings.xml";
        public FileStream stream;
        public SoapFormatter formatter;
        
        [Serializable] public class SerializableDataStructure : ISerializable
        {
            public int serCustInitialAmount;
            public int serBankVaultAmount;
            public int serNumberCustomers;
            public int serNumberTellers;
            public int serCustGoal;
            public int serMaxTransactionAmount;

            public SerializableDataStructure()  { }

            //Deserialization
            protected SerializableDataStructure(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                    throw new System.ArgumentNullException("info");
                serCustInitialAmount = (int)info.GetValue("custInitialAmount", typeof(int));
                serBankVaultAmount = (int)info.GetValue("bankVaultAmount", typeof(int));
                serNumberCustomers = (int)info.GetValue("numberCustomers", typeof(int));
                serNumberTellers = (int)info.GetValue("numberTellers", typeof(int));
                serCustGoal = (int)info.GetValue("custGoal", typeof(int));
                serMaxTransactionAmount = (int)info.GetValue("maxTransactionAmount", typeof(int));
            }

            // Serialization
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                    throw new System.ArgumentNullException("info");

                info.AddValue("custInitialAmount", serCustInitialAmount);
                info.AddValue("bankVaultAmount", serBankVaultAmount);
                info.AddValue("numberCustomers", serNumberCustomers);
                info.AddValue("numberTellers", serNumberTellers);
                info.AddValue("custGoal", serCustGoal);
                info.AddValue("maxTransactionAmount", serMaxTransactionAmount);
            }
        }


        public BankSimulatorForm()
        {
            InitializeComponent();

            LoadConfig();

            syncContext =  WindowsFormsSynchronizationContext.Current;

            serializableDataStructure = new SerializableDataStructure();

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

            foreach(Control control in this.Controls)
            {
                foreach (Control ctl in control.Controls)
                {
                    if (ctl is TextBox)
                    {
                        TextBox myTB = (TextBox)ctl;
                        myTB.TextChanged += new EventHandler(this.tbox_TextChanged);
                    }
                }
            }

            
            uiHelper = new UIHelper(this);
            Enable = new Enabler(Stop);
            
        }

        public void Stop()
        {
            bankSimulator.Stop();
            if (this.startBtn.InvokeRequired)
            {
                this.Invoke(Enable);
            }
            else
            {
                StartButtonState(true);
                StopButtonState(false);
            }
        }


        private void LoadConfig() {

            bool deserializeResult = false;
            DialogResult dialogResult = DialogResult.No;  //Default to "start fresh" if SerializedFileName doesn't exist

            if (File.Exists(SerializedFileName))
            {
                //MessageBox.Show("Environment.CurrentDirectory:  " + Environment.CurrentDirectory);
                dialogResult = MessageBox.Show("Do you want to revert to the previous serialized settings?\n" +
                    "Press 'Yes' to user serialized values, or 'No' to use default values.",
                    "Revert to Serialized State?",
                    MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //MessageBox.Show("Logic to DeSerialize goes here");
                    deserializeResult = Deserialize();
                    if (deserializeResult == true)
                    {
                        tboxBankInitialVaultAmount.Text = serializableDataStructure.serBankVaultAmount.ToString();
                        tboxBankNumberOfCustomers.Text = serializableDataStructure.serNumberCustomers.ToString();
                        tboxBankNumberOfTellers.Text = serializableDataStructure.serNumberTellers.ToString();
                        //tboxBankNumberOfCustomers.Text = serializableDataStructure.serNumberCustomers.ToString();
                        tboxCustGoalAmount.Text = serializableDataStructure.serCustGoal.ToString();

                        custInitialAmount = serializableDataStructure.serCustInitialAmount;
                        tboxCustInitialAmount.Text = custInitialAmount.ToString();

                        tboxTransactionMaxAmount.Text = serializableDataStructure.serMaxTransactionAmount.ToString();
                    }
                }
            }
                
            if (dialogResult != DialogResult.Yes || deserializeResult == false)
            {
                //MessageBox.Show("Logic to start fresh from the config file settings.");
                //File.Delete(SerializedFileName);
                tboxBankInitialVaultAmount.Text = ConfigurationManager.AppSettings["InitialBankVaultAmount"];
                tboxBankNumberOfCustomers.Text = ConfigurationManager.AppSettings["NumberOfCustomers"];
                tboxBankNumberOfTellers.Text = ConfigurationManager.AppSettings["NumberOfBankers"];
                //tboxBankNumberOfCustomers.Text = ConfigurationManager.AppSettings["NumberOfCustomers"];
                tboxCustGoalAmount.Text = ConfigurationManager.AppSettings["FinancialGoalPerCustomer"];

                custInitialAmount = int.Parse(tboxBankInitialVaultAmount.Text) / int.Parse(tboxBankNumberOfCustomers.Text);
                tboxCustInitialAmount.Text = custInitialAmount.ToString();

                tboxTransactionMaxAmount.Text = ConfigurationManager.AppSettings["MaximumAmountPerTransaction"];
            }
        }

        //public int GetConfigurationInt32(string key, string val, int val1) { }

        
        public void AddListBoxItem(string message)
        {
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

        public void BankAmountFinalUpdate(decimal amount)
        {
            syncContext.Post(x => { x = bankVaultFinal.Text = string.Format("{0:C}", amount); Application.DoEvents(); }, null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
           StopButtonState(true);
           StartButtonState(false);

           bankSimulator = new BankSimulator(uiHelper, bankVaultAmount, numberCustomers, numberTellers, custInitialAmount, custGoal, maxTransactionAmount);
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            StopButtonState(false);
            bankSimulator.StopSimulation();

            try
            {
                formatter = new SoapFormatter();
                using (stream = new FileStream(SerializedFileName, FileMode.Create)) 
                {
                    //Write values to SerializedFileName
                    formatter.Serialize(stream, serializableDataStructure);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error Serializing to {0}. Message = {1}", SerializedFileName, ex.Message));
            }

            StartButtonState(true);
        }

        private bool Deserialize()
        {
            formatter = new SoapFormatter();
            using (stream = new FileStream(SerializedFileName, FileMode.OpenOrCreate))
            {
                try
                {
                    //Pull values from SerializedFileName
                    serializableDataStructure = (SerializableDataStructure)formatter.Deserialize(stream);
                    if (serializableDataStructure.serCustGoal > 0 && serializableDataStructure.serMaxTransactionAmount > 0 &&
                        serializableDataStructure.serNumberCustomers > 0 && serializableDataStructure.serNumberTellers > 0 )
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Invalid values found in file {0}, so default values will be used.", SerializedFileName));
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error Deserializing from {0}. Message = {1}", SerializedFileName, ex.Message));
                }
            }
            return false;
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
                if (int.Parse(tboxBankNumberOfCustomers.Text) != 0)
                {
                    custInitialAmount = int.Parse(tboxBankInitialVaultAmount.Text) / int.Parse(tboxBankNumberOfCustomers.Text);
                    tboxCustInitialAmount.Text = custInitialAmount.ToString();
                    //serializableDataStructure.serCustInitialAmount = custInitialAmount;
                }
            }
            
        }

        private void tboxBankInitialVaultAmount_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxValidationNonEmpty(new List<TextBox>() { tboxBankInitialVaultAmount, tboxBankNumberOfCustomers }))
            {
                if (int.Parse(tboxBankNumberOfCustomers.Text) != 0)
                {
                    custInitialAmount = int.Parse(tboxBankInitialVaultAmount.Text) / int.Parse(tboxBankNumberOfCustomers.Text);
                    tboxCustInitialAmount.Text = custInitialAmount.ToString();
                    //serializableDataStructure.serCustInitialAmount = custInitialAmount;
                }
            }
        }

        private void tbox_TextChanged(object sender, EventArgs e){

            TextBox myTB = sender as TextBox;

            if (myTB.Text.Length <= 0) {
                return;
            }

            ValidateText(myTB);
            int testInt;
            if (! int.TryParse(myTB.Text, out testInt))
            {
                MessageBox.Show("Please enter only integers");
            }


            if (myTB.Name.ToLower().Contains("tellers"))
            {
                if (! int.TryParse(myTB.Text, out testInt))  { myTB.Text = numberTellers.ToString(); }
                else { numberTellers = int.Parse(myTB.Text); }
            }
            if (myTB.Name.ToLower().Contains("customers"))
            {
                if (!int.TryParse(myTB.Text, out testInt)) { myTB.Text = numberCustomers.ToString(); }
                else { numberCustomers = int.Parse(myTB.Text); }
            }
            if (myTB.Name.ToLower().Contains("vault"))
            {
                if (!int.TryParse(myTB.Text, out testInt)) { myTB.Text = bankVaultAmount.ToString(); }
                else { bankVaultAmount = int.Parse(myTB.Text); }
            }
            if (myTB.Name.ToLower().Contains("max"))
            {
                if (!int.TryParse(myTB.Text, out testInt)) { myTB.Text = maxTransactionAmount.ToString(); }
                else { maxTransactionAmount = int.Parse(myTB.Text); }
            }
            if (myTB.Name.ToLower().Contains("goal"))
            {
                if (!int.TryParse(myTB.Text, out testInt)) { myTB.Text = custGoal.ToString(); }
                else { custGoal = int.Parse(myTB.Text); }
            }

            UpdateSerializableDataStructure();
        }


        private void ValidateText(TextBox box)
        {
            // stop multiple changes;
            if (myTextBoxChanging)
                return;
            myTextBoxChanging = true;

            string text = box.Text;
            if (text == "")
                return;
            string validText = "";
            bool hasPeriod = false;
            int pos = box.SelectionStart;
            for (int i = 0; i < text.Length; i++)
            {
                bool badChar = false;
                char s = text[i];
                if (s == '.')
                {
                    if (hasPeriod)
                        badChar = true;
                    else
                        hasPeriod = true;
                }
                else if (s < '0' || s > '9')
                    badChar = true;

                if (!badChar)
                    validText += s;
                else
                {
                    if (i <= pos)
                        pos--;
                }
            }

            // trim starting 00s
            while (validText.Length >= 2 && validText[0] == '0')
            {
                if (validText[1] != '.')
                {
                    validText = validText.Substring(1);
                    if (pos < 2)
                        pos--;
                }
                else
                    break;
            }

            if (pos > validText.Length)
                pos = validText.Length;
            box.Text = validText;
            box.SelectionStart = pos;
            myTextBoxChanging = false;
        }

        private void UpdateSerializableDataStructure()
        {
            serializableDataStructure.serCustInitialAmount = custInitialAmount;
            serializableDataStructure.serBankVaultAmount = bankVaultAmount;
            serializableDataStructure.serNumberCustomers = numberCustomers;
            serializableDataStructure.serNumberTellers = numberTellers;
            serializableDataStructure.serCustGoal = custGoal;
            serializableDataStructure.serMaxTransactionAmount = maxTransactionAmount;
        }

        private void GetValuesFromSerializableDataStructure()
        {
            custInitialAmount = serializableDataStructure.serCustInitialAmount;
            bankVaultAmount = serializableDataStructure.serBankVaultAmount;
            numberCustomers = serializableDataStructure.serNumberCustomers;
            numberTellers = serializableDataStructure.serNumberTellers;
            custGoal = serializableDataStructure.serCustGoal;
            maxTransactionAmount = serializableDataStructure.serMaxTransactionAmount;

            this.tboxCustInitialAmount.Text = custInitialAmount.ToString();
            this.tboxBankInitialVaultAmount.Text = bankVaultAmount.ToString();
            this.tboxBankNumberOfCustomers.Text = numberCustomers.ToString();
            this.tboxBankNumberOfTellers.Text = numberTellers.ToString();
            this.tboxCustGoalAmount.Text = custGoal.ToString();
            this.tboxTransactionMaxAmount.Text = maxTransactionAmount.ToString();
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

        private void label8_Click(object sender, EventArgs e)
        {
        }
        
    }
}