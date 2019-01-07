using _95Management.Models;
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
                string a = e.Message;
            }

            return Request.CreateResponse(res);
        }
    }
}
