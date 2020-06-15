using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WPF_Bank_app.Loginscreen;
using MongoDB.Driver;
using System.Collections;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using MessageBox = System.Windows.MessageBox;
using System.Security.Cryptography;
using MongoDB.Bson;


namespace WPF_Bank_app
{
    public partial class Adminside : Window
    {
        public IQueryable<DataClass> newQuery;
        public List<string> list = new List<string>();
        public static DataClass userDatabase;
        public Adminside()
        {
            InitializeComponent();
            Initiate();

        }
        public int currentUse;
        public void Initiate()
        {
            newQuery = from c in collection.AsQueryable<DataClass>()
                       select c;
            foreach (DataClass item in newQuery)
            {
                list.Add(item.account_number);
                this.DataContext = this;
                TestItems = new List<string>();
                TestItems.AddRange(list);
                comboBoxUserList.Items.Add(item.account_number);
                txtUserList.Text = string.Empty;
            }
        }

        private void ComboBoxUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtUserList.Text = Convert.ToString(comboBoxUserList.SelectedItem);
            CollectionSetup();
        }

        private void ComboBoxUserList_TextInput(object sender, TextCompositionEventArgs e)
        {
            comboBoxUserList.IsDropDownOpen = true;
            comboBoxUserList.ItemsSource = newQuery.Where(p => p.account_number.Contains(e.Text)).ToList();
        }

        public List<string> TestItems { get; set; }

        private void TxtUserList_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isNumeric = double.TryParse(txtUserList.Text, out double n);

            if (isNumeric == true || txtUserList.Text == string.Empty)
            {
                comboBoxUserList.SelectedItem = txtUserList.Text;
            }
            else
            {
                txtUserList.Text = string.Empty;
                MessageBox.Show("Error! unrecognized characters used! Please only use numerals");
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            // Logout section to enter back to the logout function to get on another user
            var result = MessageBox.Show("Are you sure you want to logout?", "Application Logout", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
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

        public void CollectionSetup()
        {
            var query = from c in collection.AsQueryable<DataClass>()
                        where c.account_number == comboBoxUserList.SelectedItem.ToString()
                        select c;
            foreach (var item in query)
            {
                userDatabase = item;
            }
        }
        public bool firstTimeSetup = true;

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {

            if (comboBoxUserList.SelectedItem == null || txtUserList.Text == string.Empty)
            {
                MessageBox.Show("Error! Please Select a User!");
            }
            else
            {
                if (firstTimeSetup == true || MessageBox.Show("Make sure to save all changes before leaving. Want to continue?", "Confirmation", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                {
                    firstTimeSetup = false;
                    currentUse = 1;
                    btnSaveChanges.Content = "Save Changes";
                    setup();
                }
            }
        }

        public void setup()
        {
            txtField1.Text = userDatabase.account_number;
            txtField2.Text = Convert.ToString(userDatabase.customer_number);
            txtField3.Text = userDatabase.cpr;
            txtField4.Text = null;
            txtField5.Text = userDatabase.Email;
            txtField6.Text = userDatabase.Phone_Number;

            txtFieldSecond1.Text = userDatabase.first_name;
            txtFieldSecond2.Text = userDatabase.last_name;
            txtFieldSecond3.Text = Convert.ToString(userDatabase.age);
            txtFieldSecond4.Text = Convert.ToString(userDatabase.birth_year);
            txtFieldSecond5.Text = userDatabase.birth_month;
            txtFieldSecond6.Text = userDatabase.birth_day;
            txtFieldSecond7.Text = Convert.ToString(userDatabase.rent_rate);
            txtFieldSecond8.Text = Convert.ToString(userDatabase.rent_rate_negative);
            comboboxGender.SelectedValue = userDatabase.Gender;
            comboBoxBalance.SelectedIndex = 0;
            ItemBalance1.IsSelected = true;
        }
        public bool? changedCheck()
        {
            bool? result = true;
            try
            {
                for (int switchValue = 1; switchValue < 15; switchValue++)
                {
                    string matchInput = null;
                    string databaseValue = null;
                    double? databaseValueInt = null;
                    string section = null;
                    double? matchInputInt = null;
                    passwordMatcher = false;

                    if (currentUse == 1)
                    {
                        switch (switchValue)
                        {
                            case 1:
                                databaseValue = userDatabase.account_number;
                                break;
                            case 2:
                                databaseValue = userDatabase.cpr;
                                break;
                            case 3:
                                databaseValue = userDatabase.Email;
                                break;
                            case 4:
                                databaseValue = userDatabase.Phone_Number;
                                break;

                            case 5:
                                databaseValue = userDatabase.first_name;
                                break;
                            case 6:
                                databaseValue = userDatabase.last_name;
                                break;
                            case 7:
                                databaseValue = Convert.ToString(userDatabase.age);
                                break;
                            case 8:
                                databaseValue = Convert.ToString(userDatabase.birth_year);
                                break;
                            case 9:
                                databaseValue = userDatabase.birth_month;
                                break;
                            case 10:
                                databaseValue = userDatabase.birth_day;
                                break;

                            case 11:
                                databaseValueInt = userDatabase.rent_rate;
                                break;
                            case 12:
                                databaseValueInt = userDatabase.rent_rate_negative;
                                break;
                            case 13:
                                switch (comboBoxBalance.SelectedIndex)
                                {
                                    case 1:
                                        databaseValueInt = userDatabase.DKK;
                                        break;
                                    case 2:
                                        databaseValueInt = userDatabase.EUR;
                                        break;
                                    case 3:
                                        databaseValueInt = userDatabase.USD;
                                        break;
                                    case 4:
                                        databaseValueInt = userDatabase.CNY;
                                        break;
                                    case 5:
                                        databaseValueInt = userDatabase.BTC;
                                        break;
                                    default:
                                        break;
                                }
                                matchInputInt = Convert.ToDouble(txtField7.Text);
                                break;
                            case 14:
                                databaseValue = userDatabase.Gender;
                                break;
                            default:
                                MessageBox.Show("Error at switch in \"ChangedCheck\"");
                                return null;
                        }
                    }

                    switch (switchValue)
                    {
                        case 1:
                            section = "account_number";
                            matchInput = txtField1.Text;
                            break;
                        case 2:
                            section = "cpr";
                            matchInput = txtField3.Text;
                            break;
                        case 3:
                            section = "Email";
                            matchInput = txtField5.Text;
                            break;
                        case 4:
                            section = "Phone_Number";
                            matchInput = txtField6.Text;
                            break;

                        case 5:
                            section = "first_name";
                            matchInput = txtFieldSecond1.Text;
                            break;
                        case 6:
                            section = "last_name";
                            matchInput = txtFieldSecond2.Text;
                            break;
                        case 7:
                            section = "age";
                            matchInputInt = Convert.ToInt32(txtFieldSecond3.Text);
                            break;
                        case 8:
                            section = "birth_year";
                            matchInputInt = Convert.ToDouble(txtFieldSecond4.Text);
                            break;
                        case 9:
                            section = "birth_month";
                            matchInput = txtFieldSecond5.Text;
                            break;
                        case 10:
                            section = "birth_day";
                            matchInput = txtFieldSecond6.Text;
                            break;

                        case 11:
                            section = "rent_rate";
                            matchInputInt = Convert.ToDouble(txtFieldSecond7.Text);
                            break;
                        case 12:
                            section = "rent_rate_negative";
                            matchInputInt = Convert.ToDouble(txtFieldSecond8.Text);
                            break;
                        case 13:
                            switch (comboBoxBalance.SelectedIndex)
                            {
                                case 1:
                                    section = "DKK";
                                    break;
                                case 2:
                                    section = "EUR";
                                    break;
                                case 3:
                                    section = "USD";
                                    break;
                                case 4:
                                    section = "CNY";
                                    break;
                                case 5:
                                    section = "BTC";
                                    break;
                                default:
                                    break;
                            }
                            matchInputInt = Convert.ToDouble(txtField7.Text);
                            break;
                        case 14:
                            section = "Gender";
                            matchInput = comboboxGender.Text;
                            break;
                        case 15:

                            matchInput = AdminHashing(txtField4.Text);
                            section = "password";
                            break;

                        default:
                            MessageBox.Show("Error at switch in \"ChangedCheck\"");
                            return null;
                    }


                    if (currentUse == 2) { databaseValue = null; databaseValueInt = null; } // for the account create, so it makes the lists needed for the save
                    if (databaseValue == matchInput && matchInput != null || databaseValueInt == matchInputInt && matchInputInt != null || passwordMatcher == true)
                    {
                        result = true;
                    }

                    else if (matchInput == "")
                    {
                        MessageBox.Show("Error! Input cannot be empty!");
                        return null;
                    }
                    else
                    {
                        if (matchInput != null)
                        {
                            changesValueList.Add(matchInput);
                            changesSectionList.Add(section);
                        }
                        else if (matchInputInt != null)
                        {
                            changesValueListInt.Add(matchInputInt);
                            changesSectionListInt.Add(section);
                        }
                    }
                }
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show($"catch: {e}");
                result = null;
            }
            return result;
        }
        #region // Hashing            
        public string AdminHashing(string input)
        {
            byte[] salt = new byte[16];

            if (currentUse == 1)
            {
                string savedPasswordHash = userDatabase.password;

                byte[] hashBytes = Convert.FromBase64String(savedPasswordHash.Replace(@"=", "+"));

                Array.Copy(hashBytes, 0, salt, 0, 16);

                var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 10000);

                byte[] hash = pbkdf2.GetBytes(20);

                int ok = 1;
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        ok = 0;
                }
                if (ok != 1)
                {
                    MessageBox.Show("Not matched");
                    passwordMatcher = false;

                    byte[] Result = new byte[salt.Length + hash.Length];

                    Array.Copy(salt, 0, Result, 0, 16);
                    Array.Copy(hash, 0, Result, 16, 20);

                    return (Convert.ToBase64String(Result));
                }
                else
                {
                    MessageBox.Show("matched");
                    passwordMatcher = true;
                    return null;
                }
            }

            else if (currentUse == 2)
            {
                var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 10000);

                byte[] hash = pbkdf2.GetBytes(20);
                byte[] Result = new byte[salt.Length + hash.Length];

                Array.Copy(salt, 0, Result, 0, 16);
                Array.Copy(hash, 0, Result, 16, 20);
                return (Convert.ToBase64String(Result));
            }
            else
            {
                MessageBox.Show("Error");
                return null;
            }


        }
        public bool passwordMatcher { get; set; }
        #endregion

        List<string> changesValueList = new List<string>();
        List<string> changesSectionList = new List<string>();

        List<double?> changesValueListInt = new List<double?>();
        List<string> changesSectionListInt = new List<string>();

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // 1 = find user / update user
            // 2 = create new user
            // 3 = delete current user
            if (currentUse == 1)
            {
                var result = MessageBox.Show("Are you sure you want to make following adjustments?", "Confirmation", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        BsonSender();
                        CollectionSetup();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else if (currentUse == 2)
            {
                accountCreateSection();
            }
            else if (currentUse == 3)
            {
                //delete account
            }
        }

        public void accountCreateSection()
        {

            changesValueList.Clear();
            changesSectionList.Clear();
            changesValueListInt.Clear();
            changesSectionListInt.Clear();
            changedCheck();

            // do this for the bsonsection change to make the code less, so that all items in the lists will be sent over there, maybe use a for loop?
            /*     foreach (var test in changesSectionListInt)
                 {
                     MessageBox.Show(test);

                 }*/
            int end = 0;
            bool whileEnd = false;
            List<int> customer_NumberList = new List<int>();

            foreach (DataClass item in newQuery)
            {
                customer_NumberList.Add(item.customer_number);
            }
            while (whileEnd == false)
            {
                end++;
                if (!customer_NumberList.Contains(end))
                {
                    changesValueListInt.Add(end);
                    changesSectionListInt.Add("customer_number");
                    MessageBox.Show(Convert.ToString(end));
                    whileEnd = true;
                }
            }
            

                bsonCreateSection( end);
            

            //  MessageBox.Show(Convert.ToString(changesValueList.Count));
        }

        public void bsonCreateSection(int customer_number)
        {
            // index out of range? changevalueList contains 9 elements (up to 8 index) but 9 is mention as index aim?
            DataClass newUser = new DataClass(ObjectId.GenerateNewId(), changesValueList[4], changesValueList[5], Convert.ToInt32(changesValueListInt[0]), Convert.ToInt32(changesValueListInt[4]), Convert.ToInt32(changesValueListInt[2]), Convert.ToInt32(changesValueListInt[3]),
                Convert.ToInt32(changesValueListInt[1]), changesValueList[6],changesValueList[7], changesValueList[1], customer_number, changesValueList[9], changesValueList[0], 0, 0, 0, 0, Convert.ToString( DateTime.Now), changesValueList[2], changesValueList[3], changesValueList[8]);

            collection.InsertOne(newUser);
        }

        public void BsonSender()
        {
            changesValueList.Clear();
            changesSectionList.Clear();
            changesValueListInt.Clear();
            changesSectionListInt.Clear();
        
            if (changedCheck() == true)
            {
                if (changesSectionList.Count > 0)
                {
                    for (int i = 0; i < changesValueList.Count; i++)
                    {
                        BsonSection(changesSectionList[i], changesValueList[i], double.NaN, userDatabase.customer_number, $"{changesSectionList[i]} - {changesValueList[i]}", "Admin Change");
                    }
                }
                if (changesSectionListInt.Count > 0)
                {
                    for (int i = 0; i < changesValueListInt.Count; i++)
                    {
                        BsonSection(changesSectionListInt[i], string.Empty, changesValueListInt[i], userDatabase.customer_number, $"{changesSectionListInt[i]} - {changesValueListInt[i]}", "Admin Change");
                    }
                }
                MessageBox.Show("Following input saved!");
            }
            else
            {
                MessageBox.Show("Error! Same input entered!");
            }

        }

        private void ComboBoxBalance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (comboBoxBalance.SelectedIndex)
                {
                    case 0:
                        txtField7.Text = "-";
                        break;
                    case 1:
                        txtField7.Text = Convert.ToString(userDatabase.DKK);
                        break;
                    case 2:
                        txtField7.Text = Convert.ToString(userDatabase.EUR);
                        break;
                    case 3:
                        txtField7.Text = Convert.ToString(userDatabase.USD);
                        break;
                    case 4:
                        txtField7.Text = Convert.ToString(userDatabase.CNY);
                        break;
                    case 5:
                        txtField7.Text = Convert.ToString(userDatabase.BTC);
                        break;
                }
            }
            catch { }
        }

        private void BtnNewUser_Click(object sender, RoutedEventArgs e)
        {
            currentUse = 2;
            firstTimeSetup = true;
            btnSaveChanges.Content = "Create new Account";
            clear();
        }
        public void clear()
        {
            comboBoxUserList.SelectedIndex = -1;
            txtField1.Text = string.Empty;
            txtField2.Text = string.Empty;
            txtField3.Text = string.Empty;
            txtField4.Text = string.Empty;
            txtField5.Text = string.Empty;
            txtField6.Text = string.Empty;
            txtField7.Text = string.Empty;

            txtFieldSecond1.Text = string.Empty;
            txtFieldSecond2.Text = string.Empty;
            txtFieldSecond3.Text = string.Empty;
            txtFieldSecond4.Text = string.Empty;
            txtFieldSecond5.Text = string.Empty;
            txtFieldSecond6.Text = string.Empty;
            txtFieldSecond7.Text = string.Empty;
            txtFieldSecond8.Text = string.Empty;

            comboBoxBalance.SelectedIndex = -1;
            comboboxGender.SelectedIndex = -1;
        }
    }
}
