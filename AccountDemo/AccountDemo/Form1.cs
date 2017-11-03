using DBUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AccountDemo
{
    public partial class Account : Form
    {
        private BLL.AccountTable bll;
        private bool IsBackUp = false;
        /// <summary>
        /// 关闭窗体提示
        /// </summary>
        FormCloese frm = new FormCloese();
        /// <summary>
        /// 总支出
        /// </summary>
        private decimal sumSpendingAmount { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        private decimal sumIncomeAmount { get; set; }
        public Account()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.skinEngine1.SkinFile = "Skk/PageColor2.ssk";
            bll = new BLL.AccountTable();
            AddHeaders();
            AddListViewItems();
        }

        private List<Model.Account> accountrecords;

        /// <summary>
        /// 加载listItems
        /// </summary>
        private void AddListViewItems()
        {
            listView1.Items.Clear();
            sumSpendingAmount = 0;
            sumIncomeAmount = 0;
            accountrecords = bll.GetAccountInfo();
            for (int i = 0; i < accountrecords.Count; i++)
            {
                ListViewItem item = new ListViewItem(new string[] { accountrecords[i].ID.ToString(), accountrecords[i].Date.ToString("yyyy-MM-dd"), accountrecords[i].IncomeSpending, accountrecords[i].Amount.ToString(), accountrecords[i].Operation, accountrecords[i].PaymentType, accountrecords[i].Note });
                item.Tag = accountrecords[i].ID;

                if (accountrecords[i].IncomeSpending == "支出")
                {
                    sumSpendingAmount += accountrecords[i].Amount;
                }
                else
                {
                    sumIncomeAmount += accountrecords[i].Amount;
                }

                if (i % 2 == 0)
                {
                    item.BackColor = Color.FromArgb(247, 247, 222);
                }
                this.listView1.Items.Add(item);
            }



            this.labMessage.Text = string.Format("本月总共收入：{0}元  支出：{1}元 剩余：{2}元", sumIncomeAmount.ToString("0.##"), sumSpendingAmount.ToString("0.##"), (sumIncomeAmount - sumSpendingAmount).ToString("0.##"));
        }


        /// <summary>
        /// 加载listview头部信息
        /// </summary>
        private void AddHeaders()
        {

            listView1.Columns.Clear();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            listView1.GridLines = true;//表格是否显示网格线
            listView1.FullRowSelect = true;//是否选中整行



            listView1.View = View.Details;//设置显示方式
            listView1.Scrollable = true;//是否自动显示滚动条
            listView1.MultiSelect = true;//是否可以选择多行

            listView1.Columns.Add("ID", "记录编号");
            listView1.Columns.Add("Date", "日期");
            listView1.Columns.Add("IncomeSpending", "收入支出");
            listView1.Columns.Add("Amount", "金额");
            listView1.Columns.Add("Operation", "操作人");
            listView1.Columns.Add("PaymentType", "收支类型");
            listView1.Columns.Add("Note", "备注");

            //自动适应宽度,-1根据内容设置宽度,-2根据标题设置宽度.
            //listView1.Columns["ID"].Width = -2;
            //listView1.Columns["Date"].Width = -1;
            //listView1.Columns["IncomeSpending"].Width = -2;
            //listView1.Columns["Amount"].Width = -2;
            //listView1.Columns["Operation"].Width = -1;
            //listView1.Columns["PaymentType"].Width = -1;
            //listView1.Columns["Note"].Width = -1;

            listView1.Columns["ID"].Width = -2;
            listView1.Columns["Date"].Width = 80;
            listView1.Columns["IncomeSpending"].Width = -2;
            listView1.Columns["Amount"].Width = 80;
            listView1.Columns["Operation"].Width = 100;
            listView1.Columns["PaymentType"].Width = 100;
            listView1.Columns["Note"].Width = 1200;


        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemInsert_Click(object sender, EventArgs e)
        {
            InsertRecord dialog = new InsertRecord();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IsBackUp = true;
                AddListViewItems();
            }
        }

        //bool IsShowfrmMain = true;
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowMainForm();

            //if (IsShowfrmMain)
            //{
            //    this.Show();
            //    this.Activate();
            //    IsShowfrmMain = false;
            //}
            //else
            //{
            //    this.Hide();
            //    this.IsShowfrmMain = true;

            //}
        }

        private void Account_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker != null)
            {
                e.Cancel = true;
                MessageBox.Show("当前正在同步数据，不能关闭窗体！");
                return;
            }
            
            if (frm.ifright)
            {
                e.Cancel = true;
                frm.ShowDialog();
                if (frm.isright)
                {
                    e.Cancel = true;
                    frm.isright = false;
                }
                else
                {
                    //检查数据有没有修改 新增 删除 还原  如果有操作则备份所有数据
                    
                    ifright(frm.ifright);
                }
            }
            //this.Hide();
            //this.IsShowfrmMain = true;
            //e.Cancel = true;
        }

        public void ifright(bool isfall)
        {
            if (isfall)
            {
                HideMainForm();
            }
            else
            {
                if (IsBackUp)
                {
                    this.labMessage.Text = "正在关闭窗体……";
                    Assembly entryAssenbly = Assembly.GetEntryAssembly();
                    string Path = System.IO.Path.GetDirectoryName(entryAssenbly.Location) + @"\Data\BackUpData";
                    string size = "0";
                    float length = Convert.ToSingle(Helper.GetDirectoryLength(Path));
                    if (!Directory.Exists(Path))
                        Directory.CreateDirectory(Path);
                    if (length < 1024)
                    {
                        size = string.Format("{0:N0}", length) + "B";
                    }
                    else if (length > 1024)
                    {
                        if (length < 1048576)
                        {
                            size = string.Format("{0:N1}", length / 1024) + "KB";
                        }
                        else
                        {
                            size = string.Format("{0:N2}", length / 1048576) + "MB";
                            MessageBox.Show(string.Format("当前备份文件已经达到{0},请主要清理备份文件，\r\n备份文件路径:\r\n{1}", size, Path));
                        }
                    }
                    List<string> strs = new List<string>();
                    List<Model.Account> list = bll.GetAllAccountInfo();
                    foreach (var item in list)
                    {
                        strs.Add(item.Date.ToString("yyyy-MM-dd") + "|" + item.IncomeSpending + "|" + item.Amount.ToString("0.##") + "|" + item.Operation + "|" + item.PaymentType + "|" + item.Note.Replace("\r\n", " "));
                    }
                    Helper.Save(Path + @"\"+DateTime.Now.ToString("yyyy-MM-dd")+".txt", strs.ToArray());
                }
                Application.Exit();
            }

        }



        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.notifyIcon1.Dispose();//退出应用程序要先释放右下角小图标的资源
            //this.Close();
            //this.Dispose();
            //Application.Exit();
            frm.ifright = false;
            ExitMainForm();
        }

        /// <summary>
        /// 删除选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("暂时只有系统管理员才可以删除！");
            return;
            if (listView1.CheckedItems.Count > 0)
            {
                //this.listView1.CheckedItems[0]
                if (MessageBox.Show("确定删除当前选中？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    List<string> ids = new List<string>();
                    for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    {
                        //ids.Add(listView1.CheckedItems[i].SubItems[0].Text);
                        ids.Add(listView1.CheckedItems[i].Tag.ToString());
                    }
                    if (bll.DeleteAccountInfo(ids.ToArray()))
                    {
                        AddListViewItems();
                        IsBackUp = true;
                        MessageBox.Show("删除成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选中当前要删除的项！");
            }
        }


        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemUpdata_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count == 1)
            {

                InsertRecord dialog = new InsertRecord(GetCheckeItems(listView1.CheckedItems)[0]);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AddListViewItems();
                    IsBackUp = true;
                    MessageBox.Show("修改成功！");
                }


            }
            else
            {
                MessageBox.Show("只能选择一项要修改的记录！");
            }
        }

        /// <summary>
        /// 获取listview选中的项
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private Model.Account[] GetCheckeItems(ListView.CheckedListViewItemCollection items)
        {
            List<Model.Account> accounts = new List<Model.Account>();
            for (int i = 0; i < items.Count; i++)
            {
                accounts.Add(new Model.Account
                {
                    ID = int.Parse(items[i].Tag.ToString()),
                    Amount = decimal.Parse(items[i].SubItems[3].Text),
                    Date = DateTime.Parse(items[i].SubItems[1].Text),
                    IncomeSpending = items[i].SubItems[2].Text,
                    Operation = items[i].SubItems[4].Text,
                    PaymentType = items[i].SubItems[5].Text,
                    Note = items[i].SubItems[6].Text
                });
            }
            return accounts.ToArray();
        }

        /// <summary>
        /// 隐藏主窗体
        /// </summary>
        private void HideMainForm()
        {
            this.Hide();
        }
        /// <summary>
        /// //显示主窗口
        /// </summary>
        private void ShowMainForm()
        {

            this.Show();

            this.WindowState = FormWindowState.Normal;

            // this.Activate();

        }
        /// <summary>
        /// //关闭(退出)
        /// </summary>
        private void ExitMainForm()
        {
            if (backgroundWorker != null)
            {
                MessageBox.Show("当前正在同步数据，不能关闭窗体！");
                return;
            }
            if (MessageBox.Show("您确定要退出化本系统吗？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                this.notifyIcon1.Visible = false;

                this.Close();
                this.Dispose();

                Application.Exit();

            }

        }

        #region 右键菜单处理 显示 隐藏 退出 关于
        private void menuItem_Show_Click(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        private void menuItem_Hide_Click(object sender, EventArgs e)
        {
            HideMainForm();
        }

        private void MenuItem_Exit_Click(object sender, EventArgs e)
        {
            frm.ifright = false;
            ExitMainForm();
        }

        private void MenuItem_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("本软件由作者涂文远制造，QQ:499216359!");
        }
        #endregion


        /// <summary>
        /// 组合查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            isCombinationQuery = false;
            CombinationQuery cq = new CombinationQuery(SerachByGroup);
            cq.Show();
            if (isCombinationQuery)
                AddListViewItems();
        }
        private bool isCombinationQuery = false;
        object lck = new object();
        public void SerachByGroup(string jsonStr)
        {
            this.labMessage.Text = "正在努力加载中……";
            lock (lck)
            {
                isCombinationQuery = true;
                listView1.Items.Clear();
                string message = null;
                sumSpendingAmount = 0;
                sumIncomeAmount = 0;
                accountrecords = bll.SerachByGroup(jsonStr, out message);
                for (int i = 0; i < accountrecords.Count; i++)
                {
                    ListViewItem item = new ListViewItem(new string[] { accountrecords[i].ID.ToString(), accountrecords[i].Date.ToString("yyyy-MM-dd"), accountrecords[i].IncomeSpending, accountrecords[i].Amount.ToString(), accountrecords[i].Operation, accountrecords[i].PaymentType, accountrecords[i].Note });
                    item.Tag = accountrecords[i].ID;

                    if (accountrecords[i].IncomeSpending == "支出")
                    {
                        sumSpendingAmount += accountrecords[i].Amount;
                    }
                    else
                    {
                        sumIncomeAmount += accountrecords[i].Amount;
                    }

                    if (i % 2 == 0)
                    {
                        item.BackColor = Color.FromArgb(247, 247, 222);
                    }
                    this.listView1.Items.Add(item);
                }

                //listView1.Columns["ID"].Width = -2;
                //listView1.Columns["Date"].Width = -1;
                //listView1.Columns["IncomeSpending"].Width = -2;
                //listView1.Columns["Amount"].Width = 80;
                //listView1.Columns["Operation"].Width = 100;
                //listView1.Columns["PaymentType"].Width = 100;
                //listView1.Columns["Note"].Width = 1200;
                if (sumIncomeAmount != 0)
                {
                    if (sumSpendingAmount != 0)
                        this.labMessage.Text = string.Format(message, sumIncomeAmount.ToString("0.##"), sumSpendingAmount.ToString("0.##"), (sumIncomeAmount - sumSpendingAmount).ToString("0.##"));
                    else
                        this.labMessage.Text = (string.Format(message, sumIncomeAmount.ToString("0.##"), sumSpendingAmount.ToString("0.##"), (sumIncomeAmount - sumSpendingAmount).ToString("0.##"))).Replace("支出:0元,", "");
                }
                else
                {
                    this.labMessage.Text = (string.Format(message, sumIncomeAmount.ToString("0.##"), sumSpendingAmount.ToString("0.##"), (sumIncomeAmount - sumSpendingAmount).ToString("0.##"))).Replace("收入:0元,", "");
                }
            }
        }

        private bool isDescending = true;
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    //accountrecords.Sort(s => s.ID);
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.ID).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords
                                          orderby entry.ID ascending
                                          select entry).ToList();
                        isDescending = true;
                    }
                    break;
                case 1:
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.Date).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords
                                          orderby entry.Date ascending
                                          select entry).ToList();
                        isDescending = true;
                    }
                    break;
                case 2:
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.IncomeSpending).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords orderby entry.IncomeSpending ascending select entry).ToList();
                        isDescending = true;
                    }
                    break;
                case 3:
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.Amount).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords orderby entry.Amount ascending select entry).ToList();
                        isDescending = true;
                    }
                    break;
                case 4:
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.Operation).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords orderby entry.Operation ascending select entry).ToList();
                        isDescending = true;
                    }
                    break;
                case 5:
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.PaymentType).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords orderby entry.PaymentType ascending select entry).ToList();
                        isDescending = true;
                    }
                    break;
                case 6:
                    if (isDescending)
                    {
                        accountrecords = accountrecords.OrderByDescending(s => s.Note).ToList();
                        isDescending = false;
                    }
                    else
                    {
                        accountrecords = (from entry in accountrecords orderby entry.Note ascending select entry).ToList();
                        isDescending = true;
                    }
                    break;
            }
            listView1.Items.Clear();
            for (int i = 0; i < accountrecords.Count; i++)
            {
                ListViewItem item = new ListViewItem(new string[] { accountrecords[i].ID.ToString(), accountrecords[i].Date.ToString("yyyy-MM-dd"), accountrecords[i].IncomeSpending, accountrecords[i].Amount.ToString(), accountrecords[i].Operation, accountrecords[i].PaymentType, accountrecords[i].Note });
                item.Tag = accountrecords[i].ID;

                if (i % 2 == 0)
                {
                    item.BackColor = Color.FromArgb(247, 247, 222);
                }
                this.listView1.Items.Add(item);
            }

        }

        private void ToolStripMenuItemBackup_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files(*.txt)|*.txt";
            saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMddhhmmss");
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.RestoreDirectory = true;
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string localFilePath = saveFileDialog.FileName;
                List<string> strs = new List<string>();
                foreach (var item in accountrecords)
                {
                    strs.Add(item.Date.ToString("yyyy-MM-dd") + "|" + item.IncomeSpending + "|" + item.Amount.ToString("0.##") + "|" + item.Operation + "|" + item.PaymentType + "|" + item.Note.Replace("\r\n", " "));
                }
                Helper.Save(localFilePath, strs.ToArray());
                MessageBox.Show("备份成功");
            }
        }

        /// <summary>
        /// 后台线程对象
        /// </summary>
        private BackgroundWorker backgroundWorker;
        private void ToolStripMenuItemRestore_Click(object sender, EventArgs e)
        {
            if (bll.IsHasItems())
            {
                MessageBox.Show("请先手动删除数据库后，再还原数据！");
                return;
            }
            IsBackUp = true;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            List<Model.Account> accounts = new List<Model.Account>();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // string[] array = File.ReadAllLines(openFileDialog.FileName);
                string[] array = Helper.Read(openFileDialog.FileName);
                foreach (var item in array)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        continue;
                    string[] accountStrList = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    try
                    {
                        accounts.Add(new Model.Account { Date = DateTime.Parse(accountStrList[0]), IncomeSpending = accountStrList[1], Amount = decimal.Parse(accountStrList[2]), Operation = accountStrList[3], PaymentType = accountStrList[4], Note = accountStrList[5] });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(item+"加载失败！");
                    }
                }
                //ThreadPool.QueueUserWorkItem(() =>
                //{
                //    bll.InsertAllAccountInfo(accounts);
                //}, null);
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.DoWork += backgroundWorker_DoWork;
                backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
                backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                backgroundWorker.RunWorkerAsync(accounts);

            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("已取消后台线程！");
            else
                MessageBox.Show("完成");
            backgroundWorker.Dispose();
            backgroundWorker = null;
            AddListViewItems();
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.labMessage.Text = string.Format("已同步完成{0}条数据", e.ProgressPercentage);
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bll.InsertAllAccountInfo((List<Model.Account>)e.Argument, backgroundWorker);
        }




    }
}
