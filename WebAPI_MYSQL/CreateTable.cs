using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Basic;

namespace WebAPI_MYSQL
{
    [EnumDescription("profile_api")]
    public  enum enum_profile_api
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
}
