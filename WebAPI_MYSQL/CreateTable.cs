using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Basic;
using System.Text.Json.Serialization;


namespace WebAPI_MYSQL
{
    [EnumDescription("profile_api")]
    public enum enum_profile_api
    {
        [Description("GUID,VARCHAR,50,PRIMARY")]
        GUID,
        [Description("姓名,VARCHAR,100,INDEX")]
        姓名,
        [Description("地址,VARCHAR,500,NONE")]
        地址,
        [Description("加入時間,DATETIME,50,NONE")]
        加入時間,
        [Description("備註,VARCHAR,30,NONE")]
        備註



    }

    public class tableclass
    {
        ///<summary>
        ///GUID
        ///</summary>
        [JsonPropertyName("GUID")]
        public string GUID { get; set; }
        ///<summary>
        //姓名
        ///</summary>
        [JsonPropertyName("Name")]
        public string 姓名 { get; set; }
        ///<summary>
        //地址
        ///</summary>
        [JsonPropertyName("Address")]
        public string 地址 { get; set; }
        ///<summary>
        //加入時間
        ///</summary>
        [JsonPropertyName("add_time")]
        public string 加入時間 { get; set; }
        ///<summary>
        //備註
        ///</summary>
        [JsonPropertyName("Note")]
        public string 備註 { get; set; }
    }
}
