using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _95Management.Service
{
    public class Error
    {
        public static JObject Resp(string code, string msg)
        {
            JObject resp = new JObject();
            resp.Add("retcode", code);
            resp.Add("retmsg", msg);
            return resp;
        }
    }
}