using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Security.Cryptography;
using System.Collections;
using System;
using WPF_Bank_app.HelperClasses;

namespace WPF_Bank_app
{
    public partial class Loginscreen : Window
    {
        static string username;
        static bool login;
        static int loginTries = 0;
        public static IMongoQueryable<DataClass> query; //database
        public static IMongoCollection<BsonDocument> bsonCollection;
        public static IMongoCollection<DataClass> collection;
        public static IMongoCollection<BsonDocument> bsonHistory;
        public static IMongoCollection<DataClass> admin;
        public static IMongoQueryable<DataClass> adminQuery;
        public static IQueryable<DataClass> accountQuery;
        public int banTimerMin = -1;
        public int AccBanTimerMin = 1;
        public static DataClass database;
        public DateTime loginBan;
        public static ArrayList loginInputs = new ArrayList();
        public int loginType;
        
        public Loginscreen()
        {
            InitializeComponent();
        }

        public void Initiate()
        {
            var connectionString = "mongodb://userAdmin:silvereye@localhost:27017";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase("bankAppData");
            collection = db.GetCollection<DataClass>(DataClass.Name);

            bsonCollection = db.GetCollection<BsonDocument>("bankAppData");
            bsonHistory = db.GetCollection<BsonDocument>("HistoryTransactions");
            admin = db.GetCollection<DataClass>("AdminList");
            Login();
        }

        public IMongoQueryable<DataClass> MongoQueryableConverter(IMongoCollection<DataClass> input, string match)
        { // for multiple database matches / connection
            var newQuery = from c in input.AsQueryable<DataClass>()
                    where c.cpr == match
                    select c;            
            return newQuery;
        }

        public int ResultCounter(IMongoCollection<DataClass> input, string match)
        { // for multiple database matches / connection
            var restult = (from c in input.AsQueryable<DataClass>() select c)
                                  .Count(c => c.cpr == match);
            return restult;
        }

        #region // login
        public void Login()
        { 
            try
            {
                username = txtUsername.Text;

                query = MongoQueryableConverter(collection, username);
                adminQuery = MongoQueryableConverter(admin, username);

                var loginResult = ResultCounter(collection, username);
                var loginResultAdmin = ResultCounter(admin, username);
                                                                                                                                                                                                                                                                                                                                                                                                                     
                if (loginResult == 1 || loginResultAdmin == 1)
                {
                    // 1 = normal, 2 = admin
                    IMongoQueryable<DataClass> usage;
                    if (loginResult == 1)
                    {
                        usage = query;
                        loginType = 1;
                    }
                    else
                    {
                        usage = adminQuery;
                        loginType = 2;
                    }

                    foreach (DataClass output in usage)
                    {
                        database = output;
                    }
                    loginInputs.Add(txtUsername.Text);
                    
                    login = Hash.Hashing(txtPassword.Password, database.password);
                    //Hashing(txtPassword.Password);

                    DateTime dateTime = DateTime.Parse(database.loginStatusBanTimer);

                    if (dateTime > DateTime.Now)
                    {
                        MessageBox.Show($"Error your account have been locked until {database.loginStatusBanTimer}");
                    }
                    else
                    {
                        if (login == true)
                        {
                            return;
                        }

                        else
                        {
                            loginTries++;
                            if (loginTries <= 5)
                                
                                MessageBox.Show($"Error! Wrong entered credentials, Please try again! You have {5 - loginTries} left!");
                            else
                            {
                                MessageBox.Show($"Error! Wrong entered credentials, Please try again!");
                            }

                            int loginCountsAcc = loginInputs.OfType<string>().Where(x => x.Contains(txtUsername.Text)).Count();

                            if (loginCountsAcc >= 3)
                            {
                                AccBanTimerMin += AccBanTimerMin;
                                BanTimer();
                            }
                        }
                    }
                }

                else if (loginResult >= 2)

                {
                    MessageBox.Show($"Error LoginResult too high: {loginResult}");
                }

                else
                {
                    loginTries++;
                    MessageBox.Show($"Error! Wrong entered credentials, Please try again! You have {5 - loginTries} left!");
                }

                if (loginTries >= 5)
                {
                    banTimerMin += 2;

                    string banTimer = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                    loginBan = Convert.ToDateTime(banTimer).AddMinutes(banTimerMin);
                }
            }
            catch (System.IO.IOException exception)
            {
                MessageBox.Show($"Error login section: Login:{login}, Login tries: {loginTries}, Username: {username}, Password: {txtPassword} - {exception}");
            }
        }
        #endregion

        #region // Hashing
        //public static  bool Hashing(string input)
        //{
        //    string savedPasswordHash = database.password;
           
        //    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash.Replace(@"=","+"));

        //    byte[] salt = new byte[16];
        //    Array.Copy(hashBytes, 0, salt, 0, 16);

        //    var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 10000);
        //    byte[] hash = pbkdf2.GetBytes(20);

        //    var test = Convert.ToBase64String(hash);

        //    hash = Convert.FromBase64String(test);

        //    int ok = 1;
        //    for (int i = 0; i < 20; i++)
        //    {
        //        if (hashBytes[i + 16] != hash[i])
        //            ok = 0;
        //    }
        //    if (ok == 1)
        //    {
        //        return login = true;
        //    }
        //    else
        //    {
        //        return login = false;
        //    }
        //}

        #endregion

        public void BanTimer()
        {
            // Time setup
            {
                // timer change
                string newDate = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year} {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                DateTime newDateTime = DateTime.Parse(newDate);
                newDateTime = newDateTime.AddMinutes(AccBanTimerMin);

                var input = $"{newDateTime.Day}-{newDateTime.Month}-{newDateTime.Year} {newDateTime.Hour}:{newDateTime.Minute}:{newDateTime.Second}";
                BsonSection("loginStatusBanTimer", input, double.NaN, 0, string.Empty, $"Login Failure: locked until {input}");                               
            }
        }

        public static void BsonSection(string section, string input, double? value, int newCustomer, string diff, string use)
        {
            string newParam = string.Empty;
            int currentCustomer = database.customer_number;
            if (use == "Admin Change")
            {
                if (input != string.Empty)
                {
                    newParam = $" {{ $set: {{ '{section}': '{input}' }} }}";
                }
                else
                {
                    newParam = $" {{ $set: {{ '{section}': '{value}' }} }}";
                }
            }

            else
            {
                MainWindow mainWindow = new MainWindow();

                if (input != string.Empty && mainWindow.current != 4)
                {
                    newParam = $" {{ $set: {{ '{section}': '{input}' }} }}";
                }
                else
                {
                    newParam = $" {{ $set: {{ '{section}': '{value}' }} }}";
                }
            }
            if (0 < newCustomer)
            {
                currentCustomer = newCustomer;
            }

            BsonDocument newFilterDoc = BsonDocument.Parse($" {{ 'customer_number':{currentCustomer} }} ");
            BsonDocument newDocument = BsonDocument.Parse(newParam);
            bsonCollection.UpdateOne(newFilterDoc, newDocument);

            newFilterDoc = BsonDocument.Parse($" {{ 'customer_number':{currentCustomer} }}");
            newDocument = BsonDocument.Parse($"{{ $push: {{ 'History': '{diff} {use}' }} }}");
            bsonHistory.UpdateOne(newFilterDoc, newDocument);            
        }


        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (loginBan > DateTime.Now)
            {
                MessageBox.Show($"Error you have been blocked from trying again until: {loginBan}");
            }
            else
            {
                Initiate();
                if (login == true)
                {
                    MessageBox.Show("You have now succesfully logged in!");
                    if (login)
                    try
                    {
                        this.Hide();
                            if (loginType == 1)
                            {
                                MainWindow main = new MainWindow();
                                main.Show();
                            }
                            else if (loginType == 2)
                            {
                                Adminside adminside = new Adminside();
                                adminside.Show();
                            }
                    }
                    catch (System.IO.IOException exception)
                    {
                        MessageBox.Show($"Error Window change section: BtnSubmit_click: {exception}");
                    }
                }
            }
        }


        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
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
    }
}