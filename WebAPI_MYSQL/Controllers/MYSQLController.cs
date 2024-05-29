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
    [ApiController]
    public class MYSQLController : Controller
    {

        [HttpGet("init")]
        public string init()
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
        public string Get()
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
                //var Code = profile_input[i].地址;
                profile_sql_buf = (from temp in profile_sql
                                   where temp.地址 == profile_input[i].地址
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
                    string Name = profile_sql_buf[0].姓名;               
                    tableclass tableclass = profile_input[i];
                    tableclass.GUID = GUID;
                    tableclass.姓名 = Name;
                    tableclass.姓名 = "2條根";
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












        }
}
