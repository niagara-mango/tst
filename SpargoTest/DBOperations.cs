using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpargoTest
{
    class DBOperations
    {
        private string ExecuteSQLCommand(string _queryString, string _connectionString)
        {
            int lineCount = 0;
            string s = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(_queryString, connection);
                    command.Connection.Open();
                    lineCount = command.ExecuteNonQuery();
                    connection.Close();
                }
                return lineCount.ToString();
            }
            catch (Exception ex)
            {
                s = ex.Message;
                Console.WriteLine("\n" + s);
                return s;
            }
        }

        private bool CheckResult(string _s)
        {
            try
            {
                int t = Int32.Parse(_s);
                if (t > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        public bool InsertPharmacy(string _dbConnectionString, string _pharmacyName)
        {
            string sql = "IF(NOT EXISTS (SELECT [id] FROM [Pharmacy] WHERE [name] = '" + _pharmacyName.Trim()+"'))"
                             + "BEGIN INSERT INTO [Pharmacy] (name) VALUES('" + _pharmacyName.Trim() + "') END"; 
            return CheckResult(ExecuteSQLCommand(sql, _dbConnectionString));
        }


        public bool DeletePharmacy(string _dbConnectionString, string _pharmacyName)
        {

            string sql = "DELETE FROM [Pharmacy] WHERE [name] = '" + _pharmacyName.Trim() + "'"
                         + " AND NOT EXISTS(SELECT * FROM [PharmacyItems] WHERE [pharmacy_ref] IN (SELECT [id] FROM [Pharmacy] WHERE [name] = '" + _pharmacyName.Trim() + "'))";

            return CheckResult(ExecuteSQLCommand(sql, _dbConnectionString));
        }


        public bool InsertItem(string _dbConnectionString, string _itemName)
        {
            string sql = "IF(NOT EXISTS (SELECT [id] FROM [Items] WHERE [name] = '" + _itemName.Trim() + "'))"
                             + "BEGIN INSERT INTO [Items] (name) VALUES('" + _itemName.Trim() + "') END";

            return CheckResult(ExecuteSQLCommand(sql, _dbConnectionString));
        }


        public bool DeleteItem(string _dbConnectionString, string _itemName)
        {
            //сначала чистим таблицу связок. 
            //можно было бы этого не делать, если для этой таблицы указана возможность каскадноо удаления

            //безжалостно удаялем, тк в ТЗ не было указана необходимость проверки

            string sql = @"DELETE FROM [PharmacyItems] WHERE [item_ref] IN (SELECT [id] FROM [Items] WHERE [name] = '" + _itemName.Trim()+"')";

            if (CheckResult(ExecuteSQLCommand(sql, _dbConnectionString)))
            {
                sql = @"DELETE FROM [Items] WHERE [name] = '" + _itemName.Trim() + "'";
                return CheckResult(ExecuteSQLCommand(sql, _dbConnectionString));
            }
            return false;
        }


        public bool ChangeQuantity(string _dbConnectionString, List<string> _data)
        {
            string sql = "IF(EXISTS (SELECT * FROM [PharmacyItems] WHERE [pharmacy_ref] = (select [id] from [Pharmacy] where [name] = '" + _data[0] + "') AND [item_ref] = (select [id] from [Items] where [name] = '" + _data[1] + "') )) "
                            + "BEGIN "
                                + "UPDATE [PharmacyItems]  SET [quantity] = [quantity] + (" + _data[2] + ") "
                                + "WHERE [pharmacy_ref] = (SELECT [id] FROM [Pharmacy] WHERE [name] = '" + _data[0] + "') "
                                + "AND [item_ref] = (SELECT [id] FROM [Items] WHERE [name] = '" + _data[1] + "') "
                            + "END "
                            + "ELSE BEGIN "
                                + "INSERT INTO [PharmacyItems] ([pharmacy_ref],[item_ref],[quantity]) "
                                + "VALUES ((SELECT [id] FROM [Pharmacy] WHERE [name] = '" + _data[0] + "'),(SELECT [id] FROM [Items] WHERE [name] = '" + _data[1] + "')," + _data[2] + ") "
                            + "END ";

            return CheckResult(ExecuteSQLCommand(sql, _dbConnectionString));
        }

    }
}
