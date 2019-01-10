using _95Management.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace _95Management.Controllers
{
    public class MatchController : ApiController
    {
        private ILog log = LogManager.GetLogger(typeof(MatchController));
        public HttpResponseMessage Get(int id)
        {
            JObject res = new JObject();
            try
            {
                MatchModel matchinfo = MatchDAO.Instance.GetMatch(id);
                res.Add("matchtime", matchinfo.MatchTime.ToString("f"));
                res.Add("location", matchinfo.Location);
                res.Add("opponent", matchinfo.Opponent);
                res.Add("jersey", Convert.ToInt32(matchinfo.Jersey));
                res.Add("goal", matchinfo.Goal);
                res.Add("fumble", matchinfo.Fumble);
            }
            catch (Exception e)
            {
                log.Error("Match-Get 异常：" + e.Message + ". MatchId：" + id);
                res.Add("retcode", "0002");
                res.Add("retmsg", "异常："+e.Message);
                return Request.CreateResponse(res);
            }
            res.Add("retcode", "0000");
            res.Add("retmsg", "");
            return Request.CreateResponse(res);
        }
    }
}
