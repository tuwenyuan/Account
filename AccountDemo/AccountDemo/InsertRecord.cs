using DBUtility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

namespace AccountDemo
{
    public partial class InsertRecord : Form
    {
        private Model.Account account;
        public InsertRecord()
        {
            InitializeComponent();
            
            //string result = Helper.ReaderTxtFile(Helper.GetLocalWay("\\Data\\IncomeSpendingType.txt"));
            
            //JObject jo = JObject.Parse(result);
            //JArray ja = jo["listItems"].Value<JArray>();
            //List<string> strlist = new List<string>();
            //List<string> strNames = new List<string>();
            //foreach (JObject item in ja)
            //{
            //    strlist.Add(item["IncomeSpendingName"].Value<string>());
            //}
            //this.cmbIOType.DataSource = strlist;
            //this.cmbIOType.SelectedIndex = 0;

            //JArray jaName = jo["Opration"].Value<JArray>();
            //foreach (var item in jaName)
            //{
            //    strNames.Add(item["Name"].Value<string>());
            //}

            string result = Helper.ReaderTxtFile(Helper.GetLocalWay("//Data//Person.xml"));
            XmlDocument document = new XmlDocument();
            document.LoadXml(result);

            List<string> strlist = new List<string>();
            List<string> strNames = new List<string>();
            //加载人员信息
            XmlNode xn = document.SelectSingleNode("root");
            XmlNodeList xnls = xn.ChildNodes;
            foreach (XmlNode item in xnls[0].ChildNodes)
            {
                //string id = item.Attributes["id"].Value;
                strlist.Add(item.InnerText);
            }
            this.comboBox3.DataSource = strlist;
            this.comboBox3.SelectedIndex = 0;
            //加载支付类型
            foreach (XmlNode item in xnls[1].ChildNodes)
            {
                strNames.Add(item.InnerText);
            }
            this.cmbIOType.DataSource = strNames;
            this.cmbIOType.SelectedIndex = 0;
        }


        public InsertRecord(Model.Account account):this()
        {
            this.Text = "修改记录";
            this.account = account;
            this.dtpTime.Value = account.Date;
            this.txtAmount.Text = account.Amount.ToString("0.##");
            this.cmbInputOut.Text = account.IncomeSpending;
            this.comboBox3.Text = account.Operation;
            this.cmbIOType.Text = account.PaymentType;
            this.txtNote.Text = account.Note;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //检查并添加数据
            Model.Account account = new Model.Account();
            decimal amount = 0;
            decimal.TryParse(this.txtAmount.Text,out amount);
            account.Amount = amount;
            account.Date = this.dtpTime.Value;
            account.IncomeSpending = this.cmbInputOut.Text;
            account.Operation = this.comboBox3.Text;
            account.PaymentType = this.cmbIOType.Text;
            account.Note = this.txtNote.Text;

            if (amount == 0)
            {
                String str = this.txtNote.Text;
                MatchCollection mc = Regex.Matches(str, "\\d+\\.?\\d*");
                decimal num = 0;
                decimal sum = 0;
                for (int i = 0; i < mc.Count; i++)
                {
                    decimal.TryParse(mc[i].Value,out num);
                    sum = num + sum;
                }
                account.Amount = sum;
            }
            

            string message = null;
            BLL.AccountTable bll = new BLL.AccountTable();
            bool result = false;
            if (this.Text == "修改记录")
            {
                account.ID = this.account.ID;
                result = bll.UpdataAccoutInfo(account, out message);
            }
            else
                result = bll.InsertAccountInfo(account, out message);
            

            if (result)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("失败："+message);
            }
        }

        private void InsertRecord_Load(object sender, EventArgs e)
        {
            this.cmbInputOut.SelectedIndex = 0;
            this.SetStyle(ControlStyles.DoubleBuffer |ControlStyles.UserPaint |ControlStyles.AllPaintingInWmPaint,true);
            this.UpdateStyles();
        }

        
    }
}
