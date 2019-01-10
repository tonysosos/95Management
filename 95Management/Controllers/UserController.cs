using _95Management.Models;
using _95Management.Service;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace _95Management.Controllers
{
    public class UserController : ApiController
    {
        private ILog log = LogManager.GetLogger(typeof(UserController));
        [HttpPost]
        [Route("~/api/user/openid")]
        public HttpResponseMessage OpenID()
        {
            string inputstr = Request.Content.ReadAsStringAsync().Result;
            StringBuilder loginfo = new StringBuilder();
            loginfo.Append(inputstr);
            try
            {
                if (string.IsNullOrEmpty(inputstr))
                {
                    log.Error("User-OpenID 0001：输入参数为空");
                    return Request.CreateResponse(Error.Resp("0001", "授权失败"));
                }
                JObject input = JObject.Parse(inputstr);
                if (input["code"] == null || input["code"].ToString() == "")
                {
                    log.Error("User-OpenID 0001：输入参数中没有code");
                    return Request.CreateResponse(Error.Resp("0001", "授权失败"));
                }
                string code = input["code"].ToString();
                string appid = ConfigurationManager.AppSettings["appid"];
                string appsecret = ConfigurationManager.AppSettings["appsecret"];
                string url = ConfigurationManager.AppSettings["openidurl"];

                string param = string.Format("appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", appid, appsecret, code);

                url += param;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse == null)
                {
                    log.Error("User-OpenID 0003 Http请求返回为空");
                    return Request.CreateResponse(Error.Resp("0003", "获取用户信息失败"));
                }
                string httpRespStr = "";
                using (StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.Default))
                {
                    httpRespStr = sr.ReadToEnd();
                }
                JObject httpRespObj = JsonConvert.DeserializeObject<JObject>(httpRespStr);
                if (httpRespObj["openid"] == null || string.IsNullOrEmpty(httpRespObj["openid"].ToString()))
                {
                    log.Error("User-OpenID 0004 Http请求返回异常. " + httpRespStr);
                    return Request.CreateResponse(Error.Resp("0004", "获取用户信息失败"));
                }

                string openid = httpRespObj["openid"].ToString();
                string sessionkey = httpRespObj["session_key"].ToString();

                int userid = UserDAO.Instance.InsertNewUser(openid, sessionkey);
                if (userid == 0)
                {
                    log.Error("User-OpenID 0005 插入用户信息失败. " + httpRespStr);
                    return Request.CreateResponse(Error.Resp("0005", "插入用户信息失败"));
                }

                JObject res = new JObject();
                res.Add("retcode", "0000");
                res.Add("resmsg", "");
                res.Add("userid", userid);

                return Request.CreateResponse(res);
            }
            catch (Exception e)
            {
                log.Error("User-OpenID 0002：系统异常, " + e.Message + ", info: " + loginfo);
                return Request.CreateResponse(Error.Resp("0001", "授权失败"));
            }
        }

        
    }
}
