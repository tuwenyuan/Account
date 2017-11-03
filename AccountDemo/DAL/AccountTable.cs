using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class AccountTable
    {

        public bool IsHaveItems()
        {
            string sql = "select top 1 * from Account";
            OleDbDataReader dataread = DbHelperOleDb.ExecuteReader(sql);
            if (dataread.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Model.Account> GetAllAccountInfo()
        {
            string sql = "select * from Account";
            OleDbDataReader dataread = DbHelperOleDb.ExecuteReader(sql);
            List<Model.Account> accounts = new List<Model.Account>();
            try
            {
                if (dataread.HasRows)
                {
                    while (dataread.Read())
                    {
                        accounts.Add(new Model.Account { ID = int.Parse(dataread["AccountId"].ToString()), Date = DateTime.Parse(dataread["DateTime"].ToString()), IncomeSpending = dataread["IncomeSpending"].ToString(), Amount = decimal.Parse(dataread["Amount"].ToString()), Operation = dataread["Operation"].ToString(), PaymentType = dataread["PaymentType"].ToString(), Note = dataread["Note"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return accounts;
        }

        public List<Model.Account> GetAccountInfo()
        {
            string sql = "select * from Account where year(date())=year([DateTime]) and month(date()) = month([DateTime])";
            OleDbDataReader dataread = DbHelperOleDb.ExecuteReader(sql);
            List<Model.Account> accounts = new List<Model.Account>();
            try
            {
                if (dataread.HasRows)
                {
                    while (dataread.Read())
                    {
                        accounts.Add(new Model.Account { ID = int.Parse(dataread["AccountId"].ToString()), Date = DateTime.Parse(dataread["DateTime"].ToString()), IncomeSpending = dataread["IncomeSpending"].ToString(), Amount = decimal.Parse(dataread["Amount"].ToString()), Operation = dataread["Operation"].ToString(), PaymentType = dataread["PaymentType"].ToString(), Note = dataread["Note"].ToString() });

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return accounts;
        }

        public bool InsertAccount(Model.Account account)
        {
            string sql = "INSERT INTO Account([DateTime], IncomeSpending, Amount, Operation, PaymentType, [Note]) VALUES (@DateTime,@IncomeSpending,@Amount,@Operation,@PaymentType,@Note)";


            OleDbParameter[] parameters = {
                new OleDbParameter("@DateTime",OleDbType.DBDate),
                new OleDbParameter("@IncomeSpending",account.IncomeSpending),
                new OleDbParameter("@Amount",OleDbType.Decimal),
                new OleDbParameter("@Operation", account.Operation),
                new OleDbParameter("@PaymentType", account.PaymentType),
                new OleDbParameter("@Note", account.Note)
            };
            parameters[0].Value = account.Date;
            parameters[2].Value = account.Amount;

            return DbHelperOleDb.ExecuteSql(sql, parameters) > 0;

        }

        public bool DeleteAccountInfo(params string[] ids)
        {
            string strid = string.Join(",", ids);
            string sql = "delete from Account where AccountId in (" + strid + ")";
            return DbHelperOleDb.ExecuteSql(sql) > 0;
        }

        public bool UpdataAccoutInfo(Model.Account account)
        {
            string sql = "UPDATE  Account SET  [DateTime] =@DateTime, IncomeSpending =@IncomeSpending, Amount =@Amount, Operation =@Operation, PaymentType =@PaymentType, [Note] =@Note where AccountId=@AccountId";
            OleDbParameter[] parameters = {
                new OleDbParameter("@DateTime",OleDbType.DBDate),
                new OleDbParameter("@IncomeSpending",account.IncomeSpending),
                new OleDbParameter("@Amount",OleDbType.Decimal),
                new OleDbParameter("@Operation", account.Operation),
                new OleDbParameter("@PaymentType", account.PaymentType),
                new OleDbParameter("@Note", account.Note),
                new OleDbParameter("@AccountId",OleDbType.BigInt)
            };
            parameters[0].Value = account.Date;
            parameters[2].Value = account.Amount;
            parameters[6].Value = account.ID;

            return DbHelperOleDb.ExecuteSql(sql, parameters)>0;
        }

        public List<Model.Account> SerachByGroup(string jsonStr,out string message)
        {
            JObject jo = JObject.Parse(jsonStr);
            List<OleDbParameter> parameters = new List<OleDbParameter>();
            StringBuilder strb = new StringBuilder();
            strb.Append("select * from Account where");
            strb.Append(" [DateTime]>=@startTime and [DateTime]<=@endTime");
            OleDbParameter start = new OleDbParameter("@startTime", OleDbType.DBDate);
            start.Value = jo["startTime"].Value<DateTime>();
            parameters.Add(start);
            OleDbParameter end = new OleDbParameter("@endTime", OleDbType.DBDate);
            end.Value = jo["endTime"].Value<DateTime>();
            message = "从" + jo["startTime"].Value<DateTime>().ToString("yyyy-MM-dd") + "至" + jo["endTime"].Value<DateTime>().ToString("yyyy-MM-dd") + "";
            parameters.Add(end);
            
            if (!string.IsNullOrWhiteSpace(jo["cominSpending"].Value<string>()))
            {
                strb.Append(" and IncomeSpending=@cominSpending");
                parameters.Add(new OleDbParameter("@cominSpending", jo["cominSpending"].Value<string>()));
            }
            if (!string.IsNullOrWhiteSpace(jo["Opration"].Value<string>()))
            {
                strb.Append(" and Operation=@Opration");
                parameters.Add(new OleDbParameter("@Opration", jo["Opration"].Value<string>()));
                message += " " + jo["Opration"].Value<string>() + "同志";
            }
            if (!string.IsNullOrWhiteSpace(jo["comoutType"].Value<string>()))
            {
                strb.Append(" and PaymentType=@comoutType");
                parameters.Add(new OleDbParameter("@comoutType", jo["comoutType"].Value<string>()));
                message+=" "+jo["comoutType"].Value<string>()+"";
            }
            if (!string.IsNullOrWhiteSpace(jo["Note"].Value<string>()))
            {
                strb.Append(" and [Note] like '%"+jo["Note"].Value<string>()+"%'");
                message+=" 备注包含\""+jo["Note"].Value<string>()+"\"的字符";
            }
            message+="共 收入:{0}元,支出:{1}元,剩余:{2}元";



            OleDbDataReader dataread = DbHelperOleDb.ExecuteReader(strb.ToString(), parameters.ToArray());

            List<Model.Account> accounts = new List<Model.Account>();
            try
            {
                if (dataread.HasRows)
                {
                    while (dataread.Read())
                    {
                        accounts.Add(new Model.Account { ID = int.Parse(dataread["AccountId"].ToString()), Date = DateTime.Parse(dataread["DateTime"].ToString()), IncomeSpending = dataread["IncomeSpending"].ToString(), Amount = decimal.Parse(dataread["Amount"].ToString()), Operation = dataread["Operation"].ToString(), PaymentType = dataread["PaymentType"].ToString(), Note = dataread["Note"].ToString() });

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return accounts;

            
        }

        public bool InsertAllAccountInfo(List<Model.Account> accounts,BackgroundWorker bw)
        {
            return DbHelperOleDb.ExecuteSql(accounts,bw)>0;
        }

    }
}
