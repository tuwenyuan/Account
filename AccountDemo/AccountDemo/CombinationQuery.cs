using DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AccountDemo
{
    public partial class CombinationQuery : Form
    {
        public delegate void MyDelegate(string str);
        private MyDelegate del;
        public CombinationQuery()
        {
            InitializeComponent();
        }

        public CombinationQuery(MyDelegate del)
            : this()
        {
            this.del = del;
        }

        private void CombinationQuery_Load(object sender, EventArgs e)
        {
            this.dtpStart.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
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
            //加载支付类型
            foreach (XmlNode item in xnls[1].ChildNodes)
            {
                strNames.Add(item.InnerText);
            }
            this.cmbcomoutType.DataSource = strNames;

            this.cmbOpration.DataSource = strlist;
            this.cmbOpration.Text = "";
            this.cmbcomoutType.Text = "";

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.dtpStart.Value > this.dtpEnd.Value)
            {
                if (string.Compare(this.dtpStart.Value.ToString("yyyy-MM-dd"), this.dtpEnd.Value.ToString("yyyy-MM-dd")) != 0)
                {
                    MessageBox.Show("初始日期不能比结束日期大！");
                    return;
                }
            }
            StringBuilder strb = new StringBuilder();
            strb.Append("{\"startTime\":\"" + dtpStart.Value + "\",");
            strb.Append("\"endTime\":\"" + this.dtpEnd.Value + "\",");
            strb.Append("\"cominSpending\":\"" + this.cmbcominSpending.Text + "\",");
            strb.Append("\"comoutType\":\"" + this.cmbcomoutType.Text + "\",");
            strb.Append("\"Opration\":\"" + this.cmbOpration.Text + "\",");
            strb.Append("\"Note\":\"" + this.txtNote.Text + "\"}");
            IAsyncResult ascResult = del.BeginInvoke(strb.ToString(), null, null);
            
            //del(strb.ToString());
        }

    }
}
