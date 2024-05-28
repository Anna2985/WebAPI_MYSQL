using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLUI;
using Basic;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_MYSQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MYSQLController : Controller
    {
        // GET: api/<MYSQLController>
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

            SQLControl sQLControl = new SQLControl(Server, DB, table.TableName, UserName, Password, Port, MySql.Data.MySqlClient.MySqlSslMode.None);

            if (!sQLControl.IsTableCreat()) sQLControl.CreatTable(table);
            else sQLControl.CheckAllColumnName(table, true);
            return table;
        }


        // GET api/<MYSQLController>/way
        //show ALL
        [HttpGet("show/{TableName}")]
        public string Get(string TableName)
        {
            
            SQLControl sqlControl = new SQLControl("127.0.0.1", "adress", "user", "66437068");
            List<object[]> rows_value = sqlControl.GetAllRows(TableName);
            if (rows_value.Count == 0)
            {
                return "沒有資料啦";
            }
            return rows_value.JsonSerializationt(true); 
        }

        // POST api/<MYSQLController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MYSQLController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MYSQLController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }




        
       //"2024-05-28T14:04:34.736597",
       
    }
}
