using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Account
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string IncomeSpending { get; set; }
        public decimal Amount { get; set; }
        public string Operation { get; set; }
        public string PaymentType { get; set; }
        public string Note { get; set; }
    }
}
