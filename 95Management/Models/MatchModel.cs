using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _95Management.Models
{
    public class MatchModel
    {
        public DateTime MatchTime { get; set; }
        public string Location { get; set; }
        public string Opponent { get; set; }
        public JERSEYS Jersey { get; set; }
        public int Goal { get; set; }
        public int Fumble { get; set; }
    }
}