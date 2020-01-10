using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bank_app
{
    public partial class DataClass
    {
        // Database constructor
        public static readonly string Name = "bankAppData";

        public ObjectId Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int age { get; set; }
        public double DKK { get; set; }
        public double rent_rate { get; set; }
        public double rent_rate_negative { get; set; }
        public int birth_year { get; set; }
        public string birth_month { get; set; }
        public string birth_day { get; set; }
        public string cpr { get; set; }
        public int customer_number { get; set; }
        public string password { get; set; }
        public string account_number { get; set; }
        public double USD { get; set; }
        public double EUR { get; set; }
        public double CNY { get; set; }
        public double BTC { get; set; }
        public string loginStatusBanTimer { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string Gender { get; set; }

        public DataClass(ObjectId Id, string first_name, string last_name, int age, double DKK, double rent_rate, double rent_rate_negative, int birth_year, string birth_month, string birth_day, string cpr, int customer_number, string password, string account_number, double usd, double eur, double cny, double btc, string loginStatusBanTimer, string email, string phone_number, string gender)
        {
            this.Id = Id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.age = age;
            this.DKK = DKK;
            this.rent_rate = rent_rate;
            this.rent_rate_negative = rent_rate_negative;
            this.birth_year = birth_year;
            this.birth_month = birth_month;
            this.birth_day = birth_day;
            this.cpr = cpr;
            this.customer_number = customer_number;
            this.password = password;
            this.account_number = account_number;
            this.USD = usd;
            this.EUR = eur;
            this.CNY = cny;
            this.BTC = btc;
            this.loginStatusBanTimer = loginStatusBanTimer;
            this.Email = email;
            this.Phone_Number = phone_number;
            this.Gender = gender;
        }
    }
}

