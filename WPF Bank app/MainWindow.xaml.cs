using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static WPF_Bank_app.Loginscreen;
using System.Security.Cryptography;
using System.Configuration;
using System.Drawing;
using System.Windows.Input;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Collections;
using FixerSharp;

namespace WPF_Bank_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// note: remove loginban from Mongo Database asap
    /// 
    /// 
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Languages> langageList = new List<Languages>();
        public IMongoQueryable<DataClass> query = Loginscreen.query;
        public static DataClass database;
        static string operatorValue;
        public bool startup = true;
        public string currentUse = null;
        public int current = 1;
        public  ArrayList accountList = new ArrayList();
        public int transferVal;
        public string curentCurrency = "DKK";
        public string newCurrency = "USD";
        private double exchangeResult;
        private bool isNumeric;
        public bool checkbox;
        public IMongoQueryable<DataClass> accountTransfer;
        public string newUser;
        public bool selectionCheck;
        public bool transactionConfirmed;
        public string globalLanguage = "English";
        public string currentBtn;

        public MainWindow()
        {
            InitializeComponent();
            FirstTimeInitiate();
        }

        public void FirstTimeInitiate()
        {
            try
            {
                // Key used for exchange/valuta API
                Fixer.SetApiKey("f2aac993c3e6c4e71a022f012cf5cd74");
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show($"Error Fixershar key / access to the internet:{e}");
            }
            // used for language combobox, for showing the pictures / information
            langageList.Add(new Languages { ID = 1, Photo = @"C:\Users\hakan\OneDrive\Billeder/englishFlag.ico", Name = "English" });
            langageList.Add(new Languages { ID = 2, Photo = @"C:\Users\hakan\OneDrive\Billeder/danishFlag.ico", Name = "Danish" });
            comboBoxLang.ItemsSource = langageList;
            current = 1;

            InitiateDatabase();
        }

        public void InitiateDatabase()
        {                    
            // General setup of database connection
            accountQuery = from c in collection.AsQueryable<DataClass>()
                           select c;
            foreach (var item in accountQuery)
            {
                accountList.Add(item.account_number);
            }

            foreach (DataClass databaseLogin in query)
            {
                database = databaseLogin;
            }
            BtnSetup();

            lbNameFinish.Content = $"{database.account_number}";
            lbUsernameFinish.Content = database.cpr;

            if (startup == true)
            {
                SectionSetup();
            }
        }

        #region Language setup section

        public class Languages
        {
            public int ID { get; set; }
            public string Photo { get; set; }
            public string Name { get; set; }
        }
        public void BtnSetup()
        {
            btnAccount.Content = languageSection("Btn1",0);
            btnDeposit.Content = languageSection("Btn2",0);
            btnWithdraw.Content = languageSection("Btn3",0);
            btntransfer.Content = languageSection("Btn4",0);
            btnValuta.Content = languageSection("Btn5",0);
        }
        public string languageSection(string info, int currentToUse)
        {            
      /*     * 1 = 1 usage
             * 0 = Multiple uses */
            
            //mulig ændring: excel ark fromat
            int max = 10000;
            if (current == 4) { max = 100000; }

            string result = "";
            if (globalLanguage == "English")
            {
                if (currentToUse == 1)
                {
                    switch (info)
                    {
                        case "CustomerNr":
                            result = "Customer Nr:";
                            break;                    
                        case "Age":
                            result = "Age";
                            break;
                        case "Year":
                            result = "Birth Year";
                            break;
                        case "Month":
                            result = "Birth Month";
                            break;
                        case "Day":
                            result = "Birthday";
                            break;
                        case "Confirm":
                            result = "Confirm";
                            break;
                        case "AccountNr":
                            result = "Account Nr:";
                            break;
                        case "From":
                            result = "From";
                            break;
                        case "To":
                            result = "To";
                            break;
                    }
                }

                else if (currentToUse == 0)
                {
                    switch (info)
                    {
                        case "Name":
                            result = "Account Nr";
                            break;
                        case "UserID":
                            result = "Username";
                            break;
                        case "Rent":
                            result = "Rent rate:";
                            break;
                        case "Btn1":
                            result = "Account Info";
                            break;
                        case "Btn2":
                            result = "Deposit";
                            break;
                        case "Btn3":
                            result = "Withdraw";
                            break;
                        case "Btn4":
                            result = "Transfer";
                            break;
                        case "Btn5":
                            result = "Exchange";
                            break;
                        case "confirmation":
                            result = $"Error! Cannot {languageSection(currentBtn, 0)} more than {max}!";
                            break;
                    }
                }             
            }
            else if (globalLanguage == "Danish")
            {
                if (currentToUse == 1)
                {
                    switch (info)
                    {
                        case "CustomerNr":
                            result = "Kunde Nr:";
                            break;
                        case "Age":
                            result = "Alder:";
                            break;
                        case "Year":
                            result = "Fødselsår:";
                            break;
                        case "Month":
                            result = "Fødselsmåned:";
                            break;
                        case "Day":
                            result = "Fødselsdag:";
                            break;
                        case "Confirm":
                            result = "Bekræft";
                            break;
                        case "AccountNr":
                            result = "Konto Nr:";
                            break;
                        case "From":
                            result = "Fra";
                            break;
                        case "To":
                            result = "Til";
                            break;
                    }
                }
                else if (currentToUse == 0)
                {
                    switch (info)
                    {
                        case "Name":
                            result = "Kunde Nr";
                            break;
                        case "UserID":
                            result = "Brugernavn";
                            break;
                        case "Rent":
                            result = "Rente:";
                            break;
                        case "Btn1":
                            result = "Saldo";
                            break;
                        case "Btn2":
                            result = "Deposit";
                            break;
                        case "Btn3":
                            result = "Hævning";
                            break;
                        case "Btn3Val2":
                            result = "Hæve";
                            break;
                        case "Btn4":
                            result = "Overførsel";
                            break;
                        case "Btn4Val2":
                            result = "Overføre";
                            break;
                        case "Btn5":
                            result = "Konvertering";
                            break;
                        case "Btn5Val2":
                            result = "Konvertere";
                            break;
                        case "confirmation":
                            result = $"Fejl! Kan ikke {languageSection(currentBtn,0)} mere end {max}";
                            break;
                    }
                }                
            }
            return result;
        }

        #endregion

        private void ComboBoxLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // language combobox setup
            var currentUsage = current;
            current = 0;
            switch (comboBoxLang.SelectedIndex)
            {
                // English
                case 0:
                    globalLanguage = langageList[0].Name;
                    break;
                // Danish
                case 1:
                    globalLanguage = langageList[1].Name;
                    break;
            }
            BtnSetup();
            current = currentUsage;
            if (current == 5)
            {
                ValutaInitiate();
            }
            else
            {
                SectionSetup();
            }
        }
        public void Clear()
        {
            try
            {
                //Section for clearing to get ready for section change
                txtDeposit.Visibility = Visibility.Hidden;
                txtDeposit.Text = "";
                txtDeposit.Margin = ThicknessSection(txtDeposit.Margin, 196);
                txtDeposit.Width = 84;

                btnConfirm.Visibility = Visibility.Hidden;

                LbField2.Visibility = Visibility.Hidden;
                LbField2.Content = "";
                LbfieldOutput2.Content = "";
                LbfieldOutput2.Visibility = Visibility.Visible;

                comboBoxMoney.Margin = ThicknessSection(comboBoxMoney.Margin, 147);
          
                LbField3.Content = "";
                LbfieldOutput3.Content = "";

                separatorValuta.Visibility = Visibility.Hidden;

                LbField4.Content = "";
                LbfieldOutput4.Content = "";

                LbField5.Content = "";
                LbfieldOutput5.Content = "";

                LbField6.Content = "";
                LbfieldOutput6.Content = "";

                LbField7.Content = "";
                LbfieldOutput7.Content = "";
                txtDeposit.Visibility = Visibility.Hidden;

                LbField7.Margin = ThicknessSection(LbField7.Margin, 308);
               
                ComboboxTransfer.Visibility = Visibility.Hidden;

                ComboBoxEnd.Visibility = Visibility.Hidden;

                checkBoxAccept.Visibility = Visibility.Hidden;


                checkBoxAccept.IsChecked = false;
                comboBoxMoney.SelectedIndex = 1;
                comboBoxMoney.SelectedIndex = 0;
            }

            catch (System.IO.IOException exception)
            {
                MessageBox.Show($"Error at Clear section: {exception}");
            }
        }
        
        private void BtnDeposit_click(object sender, RoutedEventArgs e)
        {
            if (current != 2)
            {
                // operatorValue is if the value is gonna be added or subtracked later on
                current = 2;
                operatorValue = "+";
                currentUse = languageSection("Btn2",0);
                Clear();
                SectionSetup();
            }
            else
            {
                MessageBox.Show($"Error! Already displaying {btnDeposit.Content}!");
            }
        }

        public void SectionSetup()
        {
            try
            {     
                // Used for general information
                LbName.Content = languageSection("Name", 0);
                LbUserId.Content = languageSection("UserID", 0);
                if (current == 1)
                {
                    #region // Section 1 Initiate                  
                    // Starting off as index 0, for Danish currency
                    comboBoxMoney.SelectedIndex = 0;
                    LbField2.Content = languageSection("CustomerNr", 1);
                    LbfieldOutput2.Content = database.customer_number;

                    LbField2.Visibility = Visibility.Visible;
                    LbField3.Content = languageSection("Rent", 0);


                    LbField4.Content = languageSection("Age", 1);
                    LbfieldOutput4.Content = database.age;

                    LbField5.Content = languageSection("Year", 1);
                    LbfieldOutput5.Content = database.birth_year;

                    LbField6.Content = languageSection("Month", 1);
                    LbfieldOutput6.Content = database.birth_month;

                    LbField7.Content = languageSection("Day", 1);
                    LbfieldOutput7.Content = database.birth_day;
                    if (startup == true)
                    {
                        // Sets current language to English
                        comboBoxLang.SelectedIndex = 0;
                    }
                    startup = false;
                    if (database.DKK > 0)
                    {
                        LbfieldOutput3.Content = database.rent_rate;
                    }
                    else
                    {
                        LbfieldOutput3.Content = database.rent_rate_negative;
                    }
                    return;
                    #endregion
                }

                #region //CurrentBtn changes
                if (globalLanguage == "English")
                {
                    switch (current)
                    {
                        case 1:
                            currentBtn = "Btn1";
                            break;
                        case 2:
                            currentBtn = "Btn2";
                            break;
                        case 3:
                            currentBtn = "Btn3";
                            break;
                        case 4:
                            currentBtn = "Btn4";
                            break;
                        case 5:
                            currentBtn = "Btn5";
                            break;
                    }
                }
                else
                {
                    switch (current)
                    {
                        case 1:
                            currentBtn = "Btn1";
                            break;
                        case 2:
                            currentBtn = "Btn2";
                            break;
                        case 3:
                            currentBtn = "Btn3Val2";
                            break;
                        case 4:
                            currentBtn = "Btn4Val2";
                            break;
                        case 5:
                            currentBtn = "Btn5Val2";
                            break;
                    }
                }
                #endregion
                // Deposit, used for the general form of deposit, withdraw, exchange and transfer. Used for all transactions               
                checkBoxAccept.Visibility = Visibility.Visible;

                txtDeposit.Visibility = Visibility.Visible;
                btnConfirm.Visibility = Visibility.Visible;

                if ( current == 2 || current == 3 || current == 4)
                {

                    if (database.DKK > 0)
                    {
                        LbfieldOutput2.Content = database.rent_rate;
                    }
                    else
                    {
                        LbfieldOutput2.Content = database.rent_rate_negative;
                    }
                    LbField2.Content = languageSection("Rent", 0);
                    LbField2.Visibility = Visibility.Visible;

                    LbField3.Content = ($"{languageSection("Confirm",1)}:");
                    if (current == 4)
                    {
                        selectionCheck = true;
                        ComboboxTransfer.SelectedItem = null;
                        ComboboxTransfer.SelectedValue = "--Select--";
                        selectionCheck = false;
                        transfer();
                    }
                }                               

                btnConfirm.Content = $"{languageSection("Confirm",1)} {currentUse}";
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void txtDeposit_click(object sender, EventArgs e)
        {
            txtDeposit.Text = string.Empty; // Double click on the textbox to clear it
        }


        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            if (current != 1)
            {
                current = 1;
                currentUse = languageSection("Btn1",0);
                Clear();
                SectionSetup();
            }
            else
            {
                MessageBox.Show($"Error! Already displaying {btnAccount.Content}!");
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            // Logout section to enter back to the logout function to get on another user
            var result = MessageBox.Show("Are you sure you want to logout?", "Application Logout", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes )
            {
                Loginscreen loginscreen = new Loginscreen();
                loginscreen.Show();
                this.Close();                
            }
        }
        
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // exits the application
            try
            {
                var result = MessageBox.Show("Are you sure you want to exit the apllication?", "Application Exit", MessageBoxButton.YesNo);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        System.Environment.Exit(1);
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }

            catch (System.IO.IOException exception)
            {
                MessageBox.Show($"Error Exit section: {exception}");
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // used to all transactions / changes. Is the confirmation button for all functions
                if (checkbox == true) // isNumeric is a function to confirm that the value of the written item is a number
                {
                    if (current == 4 && ComboboxTransfer.SelectedItem == null)
                    {
                        MessageBox.Show("Error, please select an account to transfer to!");
                    }
                    else if (confirmation() == false)
                    {
                        MessageBox.Show(languageSection("confirmation", 0));
                        return;
                    }
                    else
                    {
                        transactionConfirmed = true;
                        var result = MessageBox.Show($"Are you sure you want to {languageSection(currentBtn,0)} {txtDeposit.Text}?", $"{currentUse} Confirm", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Yes)
                        {
                            Transactions();
                            if (transactionConfirmed == true)
                            {
                                MessageBox.Show($"{currentUse} confirmed! \nReturning to main menu!");
                            }

                            else
                            {
                                MessageBox.Show("Error! Insuffient funds!");
                            }
                            checkbox = false;
                            checkBoxAccept.IsChecked = false; // turning off the checkbox so the user cant make any mistakes
                            txtDeposit.Text = string.Empty; // clears the box for use again immediately right after the confirmation
                        }
                        else
                        {
                            checkBoxAccept.IsChecked = false;
                            return;
                        }                        
                    }
                }
                else
                {
                    MessageBox.Show("Error! Please accept ToS");
                }
            }

            catch (System.IO.IOException exception)
            {
                MessageBox.Show($"Error at btnConfirm_Click Button: {exception}");
            }
        }
        
        public bool confirmation()
        {
            bool result = true;       
            if (current == 2 || current == 3 || current == 5)
            {
                if (Convert.ToDouble(txtDeposit.Text) >= 10000) { result = false; }
            }
            else
            {
                if (Convert.ToDouble(txtDeposit.Text) >= 100000) { result = false; }
            }
            return result;
        }

        public void Transactions()
        {
            try
            {
                double transferTotalValue = 0;
                double totalValue = 0;

                if (operatorValue == "-")
                {
                    #region CurrentCurrency Negative val
                    switch (curentCurrency)
                    {
                        case Symbols.DKK:
                            totalValue = database.DKK - Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.USD:
                            totalValue = database.USD - Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.EUR:
                            totalValue = database.EUR - Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.CNY:
                            totalValue = database.CNY - Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.BTC:
                            totalValue = database.BTC - Convert.ToDouble(txtDeposit.Text);
                            break;
                    }
                }
                #endregion
                else
                {
                    #region currentCurrency Positive val
                    switch (curentCurrency)
                    {
                        case Symbols.DKK:
                            totalValue = database.DKK + Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.USD:
                            totalValue = database.USD + Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.EUR:
                            totalValue = database.EUR + Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.CNY:
                            totalValue = database.CNY + Convert.ToDouble(txtDeposit.Text);
                            break;

                        case Symbols.BTC:
                            totalValue = database.BTC + Convert.ToDouble(txtDeposit.Text);
                            break;
                    }
                    #endregion
                }
                // depending on the amount needs to be deposited / subtracked from the account
                if (current == 3 && database.DKK + 9999 < Convert.ToDouble(txtDeposit.Text))
                {
                    transactionConfirmed = false;
                    return;
                }

                // setting up for the valuta values 

                else if (current == 5)
                {
                    #region newCurrency
                    switch (newCurrency)
                    {
                        case "USD":
                            transferTotalValue = database.USD + exchangeResult;
                            break;

                        case "DKK":
                            transferTotalValue = database.DKK + exchangeResult;
                            break;

                        case "EUR":
                            transferTotalValue = database.EUR + exchangeResult;
                            break;

                        case "CNY":
                            transferTotalValue = database.CNY + exchangeResult;
                            break;

                        case "BTC":
                            transferTotalValue = database.BTC + exchangeResult;
                            break;
                    }
                    #endregion
                    BsonSection(newCurrency, string.Empty, transferTotalValue, 0, $"+{exchangeResult} {newCurrency}", currentUse);
                    BsonSection(curentCurrency, string.Empty, totalValue, 0, $"{operatorValue}{txtDeposit.Text} {curentCurrency}", currentUse);
                }
                else
                {
                    BsonSection(comboBoxMoney.Text, string.Empty, totalValue, 0, $"{operatorValue}{txtDeposit.Text} {curentCurrency}", currentUse);
                }
                if (current == 4)
                {
                    // code for transfer
                    accountTransfer = from c in collection.AsQueryable<DataClass>()
                                          where c.account_number == accountList[transferVal]
                                          select c;
                    #region TransferTotalValue
                    foreach (DataClass output in accountTransfer)
                    {
                        switch (curentCurrency)
                        {
                            case "USD":
                                transferTotalValue = output.USD + Convert.ToDouble(txtDeposit.Text);
                                break;

                            case "DKK":
                                transferTotalValue = output.DKK + Convert.ToDouble(txtDeposit.Text);
                                break;

                            case "EUR":
                                transferTotalValue = output.EUR + Convert.ToDouble(txtDeposit.Text);
                                break;

                            case "CNY":
                                transferTotalValue = output.CNY + Convert.ToDouble(txtDeposit.Text);
                                break;

                            case "BTC":
                                transferTotalValue = output.BTC + Convert.ToDouble(txtDeposit.Text);
                                break;
                        }
                        #endregion
                        newUser = Convert.ToString(accountList[transferVal]);
                        BsonSection(curentCurrency, string.Empty, transferTotalValue, output.customer_number, $"+{txtDeposit.Text} {Symbols.DKK}", currentUse);                        
                    }
                }
                InitiateDatabase();
                SectionSetup();
                var currentCombo = comboBoxMoney.SelectedIndex; 
                comboBoxMoney.SelectedIndex = 0;
                comboBoxMoney.SelectedIndex = 1;
                comboBoxMoney.SelectedIndex = currentCombo;

                txtDeposit.Text = " -";
                isNumeric = false;
            }
            catch (System.IO.IOException exception)
            {
                MessageBox.Show($"Error Transactions: {currentUse} - {exception}");
            }
        }

        private void BtnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            if (current != 3)
            {
                operatorValue = "-";
                current = 3;
                currentUse = languageSection("Btn3",0);
                
                Clear();
                SectionSetup();
            }
            else
            {
                MessageBox.Show($"Error! Already displaying {btnWithdraw.Content}!");
            }
        }

        private void btntransfer_Click(object sender, RoutedEventArgs e)
        {
            if (current != 4)
            {
                operatorValue = "-";
                currentUse = languageSection("Btn4Val2",0);
                current = 4;
                Clear();
                exchangeResult = 0;
                SectionSetup();
            }
            else
            {
                MessageBox.Show($"Error! Already displaying {btntransfer.Content}!");
            }
        }

        public void transfer()
        {
            try
            {
                LbField2.Content = languageSection("AccountNr",1);
                ComboboxTransfer.Visibility = Visibility.Visible;
                txtDeposit.Visibility = Visibility.Visible;
                btnConfirm.Visibility = Visibility.Visible;
                btnConfirm.Content = "Confirm Transaction";
                txtDeposit.Width = 130;

                accountList.Remove(database.account_number); // current user's account
                // list of accounts to select from in the ComboList
                Item_0.Text = Convert.ToString(accountList[0]);
                Item_1.Text = Convert.ToString(accountList[1]);
                Item_2.Text = Convert.ToString(accountList[2]);
                Item_3.Text = Convert.ToString(accountList[3]);
                Item_4.Text = Convert.ToString(accountList[4]);
                Item_5.Text = Convert.ToString(accountList[5]);
                Item_6.Text = Convert.ToString(accountList[6]);
                Item_7.Text = Convert.ToString(accountList[7]);
                Item_8.Text = Convert.ToString(accountList[8]);
            }

            catch (System.IO.IOException exception)
            {
                MessageBox.Show($"Error at Transfer(): {exception}");
            }
        }

        private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectionCheck != true)
            {
                string val = ComboboxTransfer.SelectedIndex.ToString();
                #region switch for transferVal, using val
                switch (val)
                {
                    // transferval = Selected account to transfer money to
                    case "0":
                        transferVal = 0;

                        break;

                    case "1":
                        transferVal = 1;
                        break;

                    case "2":
                        transferVal = 2;
                        break;

                    case "3":
                        transferVal = 3;
                        break;

                    case "4":
                        transferVal = 4;
                        break;

                    case "5":
                        transferVal = 5;
                        break;

                    case "6":
                        transferVal = 6;
                        break;

                    case "7":
                        transferVal = 7;
                        break;

                    case "8":
                        transferVal = 8;
                        break;

                    default:
                        MessageBox.Show("Error at Switch statement - ComboBox_SelectionChanged");
                        break;
                }
                #endregion
            }
        }

        private void BtnValuta_Click(object sender, RoutedEventArgs e)
        {
            if (current != 5)
            {
                operatorValue = "-";
                current = 5;
                currentUse = languageSection("Btn5Val2",0);
                
                Clear();
                ValutaInitiate();
            }
            else
            {
                MessageBox.Show($"Error! Already displaying {btnValuta.Content}!");
            }            
        }

        public Thickness ThicknessSection(Thickness Input, double value)
        {
            Input.Top = value;
            return Input;
        }

        public void ValutaInitiate()
        {
            comboBoxMoney.Margin = ThicknessSection(comboBoxMoney.Margin, 170);
          
            LbfieldOutput2.Content = "-";
            separatorValuta.Visibility = Visibility.Visible;
            LbfieldOutput2.Visibility = Visibility.Hidden;
           
            LbField7.Content = languageSection("From",1);
            LbfieldOutput1.Content = languageSection("To",1);
            LbField7.Margin = ThicknessSection(LbField7.Margin, 147);
                       
            ComboBoxEnd.Visibility = Visibility.Visible;
            LbField4.Visibility = Visibility.Visible;

            LbField4.Content = languageSection("Btn5",0);

            txtDeposit.Visibility = Visibility.Visible;
            btnConfirm.Visibility = Visibility.Visible;
            btnConfirm.Content = $"{languageSection("Confirm",1)} {languageSection("Btn5",0)}";
            checkBoxAccept.Visibility = Visibility.Visible;

            ComboBoxEnd.Margin = LbfieldOutput2.Margin;

            LbfieldOutput6.Visibility = Visibility.Visible;
            LbfieldOutput6.Content = " ";

            ValutaItemEnd1.Content = Symbols.DKK;
            ValutaItemEnd2.Content = Symbols.USD;
            ValutaItemEnd3.Content = Symbols.EUR;
            ValutaItemEnd4.Content = Symbols.CNY;
            ValutaItemEnd5.Content = Symbols.BTC;

            ComboBoxEnd.SelectedIndex = 1;
            SectionSetup();
        }
        
        public string valuta()
        {
            if (isNumeric == true)
            {
                exchangeResult = Fixer.Convert(curentCurrency, newCurrency, Convert.ToDouble(txtDeposit.Text));
            }
            return $"{Math.Round(exchangeResult, 4)} {newCurrency}";
        }

        private void txtDeposit_TextChanged(object sender, TextChangedEventArgs e)
        {
            isNumeric = double.TryParse(txtDeposit.Text, out double n);
            if (isNumeric == true && current == 5)
            {
                LbfieldOutput4.Content = valuta();
            }
            else if (txtDeposit.Text == "" || txtDeposit.Text == " -")
            {
                LbfieldOutput6.Content = string.Empty;
            }
            else if (isNumeric != true)
            {
                LbfieldOutput6.Content = string.Empty;
                txtDeposit.Text = string.Empty;
                MessageBox.Show("Error! Please only enter valid numbers!\nPlease try again!");
            }
        }

        private void CheckBoxAccept_Click(object sender, System.EventArgs e)
        {
            switch (checkBoxAccept.IsChecked)
            {
                case true:
                    checkbox = true;
                    break;
                case false:
                    checkbox = false;
                    break;
            }
        }


        public void ComboBoxEnd_SelectionChanged(object sender, System.EventArgs e)
        {
            string end = ComboBoxEnd.SelectedIndex.ToString();
            switch (end)
            {
                case "0":
                    newCurrency = Symbols.DKK;
                    break;
                case "1":
                    newCurrency = Symbols.USD;
                    break;
                case "2":
                    newCurrency = Symbols.EUR;
                    break;
                case "3":
                    newCurrency = Symbols.CNY;
                    break;
                case "4":
                    newCurrency = Symbols.BTC;
                    break;
                default:
                    MessageBox.Show("Error at Switch - valuta");
                    break;
            }
            LbfieldOutput4.Content = valuta();
        }

        private void BtnInformation_Click(object sender, RoutedEventArgs e)
        {
            // helpbox
            MessageBox.Show(Information());
        }

        public string Information()
        {
            var value = "";
            #region Language output in english - value output
            if (globalLanguage == "English")
            {
                switch (current)
                {
                    case 1:
                        value = $"{btnAccount.Content}:\nIn this section you're able to see all of your account details, such as current balance, customer- and accountnumber, and so on.";
                        break;
                    case 2:
                        value = $"{btnDeposit.Content}:\nIn this section you're able to make deposits from the outside and into your account.";
                        break;
                    case 3:
                        value = $"{btnWithdraw.Content}:\nThis section is solely used for withdrawing of your money from your current balance and into the outside.";
                        break;
                    case 4:
                        value = $"{btntransfer.Content}:\nThis sction is solely used for transactions / transfering some of the money from your account to the other accounts in the bank, through the use of accountnumbers.";
                        break;
                    case 5:
                        value = $"{btnValuta.Content}:\nThis section is used for currency converting, from and to followings: DKK, USD, EUR, DNY and BTC. After the currency convertion is confirmation, the converted about will be added to your account in the form of prefered currency, while the transfered about will be removed.";
                        break;
                }
            }
            #endregion - value output

            #region Language output in danish - Value output
            else if (globalLanguage == "Danish")
            {
                switch (current)
                {
                    case 1:
                        value = $"{btnAccount.Content}:\nVed denne sektion er du i stand til at kunne se alle dine konto informationer, såsom nuværende Saldo, Kunde Nr, Kontonummer, Etc";
                        break;
                    case 2:
                        value = $"{btnDeposit.Content}:\nI denne sektion er du i stand til at kunne indsætte penge til din egen bankkonto / bruger.";
                        break;
                    case 3:
                        value = $"{btnWithdraw.Content}:\nI modsætningen til Depositium sektionen, kan du i denne sektion så derved hæve dine penge i bankkontoen som du så havde indsat på et tidligere tidspunkt.";
                        break;
                    case 4:
                        value = $"{btntransfer.Content}:\nDenne sektion bliver brugt til transaktioner / overførelser af fra din konto's penge til andre kontoer ved brug af deres kontonumre.";
                        break;
                    case 5:
                        value = $"{btnValuta.Content}:\nDenne sektion er til for valuta konverteringer, hvor du så derved konverterer dine egne penge, om til andre valuta former, såsom Euro. Følgende valutaer er tilgængelig: DKK, EUR, USD, CNY, BTC.";
                        break;
                }
            }
            #endregion
            return value;
        }


        public void ComboboxMoney_SelectionChanged(object sender, EventArgs e)
        {
            var usage = LbfieldOutput1;
            if (current == 5)
            {
                usage = LbField3;
            }
            switch (comboBoxMoney.SelectedIndex)
            {
                case 0:
                    usage.Content = Math.Round( database.DKK, 8);
                    curentCurrency = Symbols.DKK;
                    break;
                case 1:
                    usage.Content = Math.Round(database.EUR, 8);
                    curentCurrency = Symbols.EUR;
                    break;
                case 2:
                    usage.Content = Math.Round(database.USD, 8);
                    curentCurrency = Symbols.USD;
                    break;
                case 3:
                    usage.Content = Math.Round(database.CNY, 8);
                    curentCurrency = Symbols.CNY;
                    break;
                case 4:
                    usage.Content = Math.Round(database.BTC, 8);
                    curentCurrency = Symbols.BTC;
                    break;
            }
        }     

    }
}

