using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Bank_app
{
   public class Accounts
    {
        public string Account_number { get; set; }

        public Accounts(string account_number)
        {
            Account_number = account_number;
        }
    }
}
