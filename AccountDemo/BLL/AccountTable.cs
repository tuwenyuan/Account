using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BLL
{
    public class AccountTable
    {
        private DAL.AccountTable dal;

        public AccountTable()
        {
            //在构造函数里生成DAL层的实例，这样保证在实例化本类的时候，才生成dal，不会
            //在程序运行时就生成dal,占用内存空间。
            dal = new DAL.AccountTable();
            
        }

        public List<Model.Account> GetAccountInfo()
        {
            return dal.GetAccountInfo();
        }

        public bool IsHasItems()
        {
            return dal.IsHaveItems();
        }

        public List<Model.Account> GetAllAccountInfo()
        {
            return dal.GetAllAccountInfo();
        }

        public bool InsertAccountInfo(Model.Account account,out string message)
        {
            if (account.Amount == 0)
            {
                message = "金额类型不正确或不能为零！";
                return false;
            }
            message = "";
            return dal.InsertAccount(account);
        }

        public bool DeleteAccountInfo(params string[] ids)
        {
            return dal.DeleteAccountInfo(ids);
        }

        public bool UpdataAccoutInfo(Model.Account account,out string message)
        {
            message = "";
            return dal.UpdataAccoutInfo(account);
        }

        public List<Model.Account> SerachByGroup(string jsonStr,out string message)
        {
            return dal.SerachByGroup(jsonStr,out message);
        }

        public bool InsertAllAccountInfo(List<Model.Account> accounts,BackgroundWorker backwork)
        {
            return dal.InsertAllAccountInfo(accounts,backwork);
        }


    }
}
