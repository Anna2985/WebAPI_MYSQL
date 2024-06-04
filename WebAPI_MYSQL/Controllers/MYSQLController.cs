using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLUI;
using Basic;
using Newtonsoft.Json;
using HIS_DB_Lib;





// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_MYSQL.Controllers
{


    [Route("api/[controller]")]

    public class MYSQLController : Controller
    {
        //swagger/index.html
        [HttpGet("init")]
        public string INIT()
        {
            List<Table> tables = CheckCreateTable();
            Table table = tables.GetTable(new enum_profile_api());
            if (table == null)
            {
                return "get table is null!";
            }
            return "OK!";
        }
     
        [HttpGet("addrow")]
        public string GET()
        {
            List<Table> tables = CheckCreateTable();
            Table table = tables.GetTable(new enum_profile_api());
            if (table == null)
            {
                return "get table is null!";
            }
            SQLControl sQLControl = new SQLControl(table);
            object[] value = new object[new enum_profile_api().GetLength()];
            value[(int)enum_profile_api.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_profile_api.姓名] = "四面佛";
            value[(int)enum_profile_api.地址] = "泰國 ";
            value[(int)enum_profile_api.加入時間] = DateTime.Now.ToDateTimeString_6();
            value[(int)enum_profile_api.備註] = "這是第4筆資料";

            sQLControl.AddRow(null, value);
            return $"已新增 \n {value.JsonSerializationt(true)}";

        }

        [HttpGet("search/{tableName}")]
        public string Search(string tableName, string searchValue)
        {
            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            sqlControl.TableName = tableName;
            List<object[]> rows_value = sqlControl.GetRowsByDefult(tableName, (int)enum_profile_api.地址, searchValue);
            if (rows_value.Count == 0)
            {
                return "無相關資料";
            }
            return rows_value.JsonSerializationt(true);



        }

        //create MYSQL DATABASE
        private List<Table> CheckCreateTable()
        {
            List<Table> tables = new List<Table>();
            tables.Add(CheckCreateTable("127.0.0.1", "adress", "user", "66437068", 3306, new enum_profile_api()));
            return tables;
        }

        static public Table CheckCreateTable(string server, string db, string user, string password, uint port, Enum Enum)
        {
            Table table = new Table(Enum);

            string Server = server;
            string DB = db;
            string UserName = user;
            string Password = password;
            uint Port = port;
            table.Server = Server;
            table.DBName = DB;
            table.Username = UserName;
            table.Password = Password;
            table.Port = Port.ToString();

            SQLControl sQLControl = new SQLControl(Server, DB, table.TableName, UserName, Password, Port, MySql.Data.MySqlClient.MySqlSslMode.Disabled);

            if (!sQLControl.IsTableCreat()) sQLControl.CreatTable(table);
            else sQLControl.CheckAllColumnName(table, true);
            return table;
        }

        [HttpGet("show/{TableName}")]
        public string showtable(string TableName)
        {

            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            List<object[]> table = sqlControl.GetAllRows(TableName);
            return table.JsonSerializationt(true);
        }




        [HttpGet("show/{TableName}/{ColumnName}")]
        public string showcolumn(string TableName, string ColumnName)
        {

            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            object[] column_value = sqlControl.GetColumnValue(TableName,ColumnName);
            if (column_value.Length == 0)
            {
                return "無資料";
            }
            return column_value.JsonSerializationt(true);
        }

        //drop table
        [HttpGet("drop/{TableName}")]
        public string DropTable(string TableName)
        {

            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            int result = sqlControl.DropTable(TableName);
            if (result == 1)
            {
                return $"Table {TableName} dropped successfuly!";
            }
            else
            {
                return $"Failed to drop table {TableName}";
            }
        }
        /// <summary>
        /// 以姓名更新地址資料
        /// </summary>
        /// <remarks>
        ///   {
        ///     "Data": [
        ///     
        ///     
        ///     
        ///     
        ///  
        ///     }
        ///    
        ///   }        /// 
        /// </remarks>
        /// <param name="returnData"></param>
        /// <returns></returns>
        [HttpPost("update_adress_by_name")]
        public string POST_update_adress_by_name([FromBody] returnData returnData)
        {
            Table table = new Table(new enum_profile_api());
            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            List<tableclass> profile_input = returnData.Data.ObjToClass<List<tableclass>>();
            string tableName = table.TableName;
            string searchValue = profile_input[0].姓名;
            List<object[]> row_value = sqlControl.GetRowsByDefult(tableName, (int)enum_profile_api.姓名,searchValue);
            List<tableclass> profile_sql = row_value.SQLToClass<tableclass, enum_profile_api>();
            List<tableclass> profile_sql_replace = new List<tableclass>();
            List<object[]> list_profile_replace = new List<object[]>();
            tableclass tableclass = profile_input[0];
            return "OK";

            

            
            //list_profile_add = profile_sql_add.ClassToSQL<tableclass, enum_profile_api>();
            //list_profile_replace = profile_sql_replace.ClassToSQL<tableclass, enum_profile_api>();
            //if (list_profile_add.Count > 0) sqlControl.AddRows(table.TableName, list_profile_add);
            //if (list_profile_replace.Count > 0) sqlControl.UpdateByDefulteExtra(table.TableName, list_profile_replace);

            //returnData.Data = "";
            //returnData.Code = 200;
            //returnData.Result = $"修改姓名,新增<{list_profile_add.Count}>筆,修改<{list_profile_replace.Count}>筆";
            //return returnData.JsonSerializationt(true);

        }

        [HttpPost("update")]
        public string POST_update([FromBody] returnData returnData)
        {
            Table table = new Table(new enum_profile_api());
            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            List<object[]> row_value = sqlControl.GetAllRows(table.TableName);
            List<tableclass> profile_sql = row_value.SQLToClass<tableclass, enum_profile_api>();
            List<tableclass> profile_sql_buf = new List<tableclass>();
            List<tableclass> profile_sql_add = new List<tableclass>();
            List<tableclass> profile_sql_replace = new List<tableclass>();

            List<tableclass> profile_input = returnData.Data.ObjToClass<List<tableclass>>();

            List<object[]> list_profile_add = new List<object[]>();
            List<object[]> list_profile_replace = new List<object[]>();

            for (int i = 0; i < profile_input.Count; i++)
            {
                string Code = profile_input[i].地址;
                profile_sql_buf = (from temp in profile_sql
                                   where temp.地址 == Code
                                   select temp).ToList();
                if (profile_sql_buf.Count == 0)
                {
                    string GUID = Guid.NewGuid().ToString();
                    tableclass tableclass = profile_input[i];
                    tableclass.GUID = GUID;
                    tableclass.加入時間 = DateTime.Now.ToDateTimeString();                  
                    profile_sql_add.Add(tableclass);
                }
                else
                {
                    string GUID = profile_sql_buf[0].GUID;
                    //string Name = profile_sql_buf[0].姓名;               
                    tableclass tableclass = profile_input[i];
                    tableclass.GUID = GUID;
                    tableclass.姓名 = "0000條根"; //HERE!!
                    tableclass.加入時間 = DateTime.Now.ToDateTimeString();
                    profile_sql_replace.Add(tableclass);
                }


            }
            list_profile_add = profile_sql_add.ClassToSQL<tableclass, enum_profile_api>();
            list_profile_replace = profile_sql_replace.ClassToSQL<tableclass, enum_profile_api>();
            if (list_profile_add.Count > 0) sqlControl.AddRows(table.TableName, list_profile_add);
            if (list_profile_replace.Count > 0) sqlControl.UpdateByDefulteExtra(table.TableName, list_profile_replace);

            returnData.Data = "";
            returnData.Code = 200;
            returnData.Result = $"修改姓名,新增<{list_profile_add.Count}>筆,修改<{list_profile_replace.Count}>筆";
            return returnData.JsonSerializationt(true);

        }
        [HttpPost("get_by_post_time_st_end")] //參考醫令資料的orderT
        public  string POST_get_by_op_time_st_end([FromBody] returnData returnData)
        {
            Table table = new Table(new enum_profile_api());
            MyTimerBasic myTimerBasic = new MyTimerBasic();

            string 起始時間 = returnData.ValueAry[0];
            string 結束時間 = returnData.ValueAry[1];

            DateTime date_st = 起始時間.StringToDateTime();
            DateTime date_ed = 結束時間.StringToDateTime();

            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            List<object[]> list_value_buf = sqlControl.GetRowsByBetween(table.TableName, (int)enum_profile_api.加入時間, date_st.ToDateTimeString(), date_ed.ToDateTimeString());
            List<tableclass> tableclasses = list_value_buf.SQLToClass<tableclass, enum_profile_api>();
            returnData.Result = $"取得資料共<{tableclasses.Count}>筆";
            returnData.Data = tableclasses;
            returnData.TimeTaken = myTimerBasic.ToString();
            return returnData.JsonSerializationt(true);

        }


        [HttpPost("delete")]
        public string POST_delete([FromBody] returnData returnData)
        {
            Table table = new Table(new enum_profile_api());
            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            string tablename = table.TableName;
            
            List<tableclass> profile_sql_buf = new List<tableclass>();
            List<tableclass> profile_sql_add = new List<tableclass>();
            List<tableclass> profile_sql_replace = new List<tableclass>();

            List<tableclass> profile_input = returnData.Data.ObjToClass<List<tableclass>>();
            string searchvalue = profile_input[0].姓名;
            List<object[]> row_value = sqlControl.GetRowsByDefult(tablename, (int)enum_profile_api.姓名, searchvalue);
            sqlControl.DeleteExtra(tablename, row_value);
            
                                   
            returnData.Data = "";
            returnData.Code = 200;
            returnData.Result = "刪除資料";
            return returnData.JsonSerializationt(true);

        }










    }
}
