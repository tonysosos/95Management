using _95Management.Service;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _95Management.Models
{
    public class MatchDAO
    {
        static MatchDAO _instance = null;
        static object locked = null;

        static MatchDAO()
        {
            locked = new object();
        }

        public static MatchDAO Instance
        {
            get
            {
                lock (locked)
                { 
                    if (_instance == null)
                    {
                        _instance = new MatchDAO();
                    }
                    return _instance;
                }
            }
        }

        public MatchModel GetMatch(int matchid)
        {
            MySqlConnection conn = SQLHelper.Instance.GetConnection();
            try
            {
                string sql = "select * from MATCHINFO where MATCHID=@id";
                conn.Open();
                MySqlCommand comd = new MySqlCommand(sql, conn);
                comd.Parameters.AddWithValue("id", matchid);
                MySqlDataReader reader = comd.ExecuteReader();
                MatchModel res = new MatchModel();

                while (reader.Read())
                {
                    res.MatchTime = (DateTime)reader["MATCHTIME"];
                    res.Location = reader["LOCATION"] as string;
                    res.Opponent = reader["OPPONENT"] as string;
                    res.Jersey = (JERSEYS)Enum.ToObject(typeof(JERSEYS), reader["JERSEY"]);
                    res.Goal = Convert.ToInt32(reader["GOAL"]);
                    res.Fumble = Convert.ToInt32(reader["FUMBLE"]);
                }
                return res;
            }
            catch (MySqlException mysqlex)
            {
                switch (mysqlex.Number)
                {
                    case 0:
                        //"Cannot connect to server.  Contact administrator"
                        break;
                    case 1045:
                        //"Invalid username/password, please try again"
                        break;
                }
                return null;
            }
            catch (Exception e)
            {
                string a = e.Message;
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        
        public int InsertNewMatch(DateTime matchtime, string location, string opponent, JERSEYS jersey)
        {
            try
            {
                string sql = "insert into MATCHINFO (MATCHTIME, LOCATION, OPPONENT, JERSEY) values (@matchtime, @location, @opponent, @jersey)";
                MySqlConnection conn = SQLHelper.Instance.GetConnection();
                conn.Open();
                MySqlCommand comd = new MySqlCommand(sql, conn);
                comd.Parameters.AddWithValue("matchtime", matchtime);
                comd.Parameters.AddWithValue("location", location);
                comd.Parameters.AddWithValue("opponent", opponent);
                comd.Parameters.AddWithValue("jersey", Convert.ToInt32(jersey));

                int res = comd.ExecuteNonQuery();
                return res;
            }
            catch (MySqlException mysqlex)
            {
                switch (mysqlex.Number)
                {
                    case 0:
                        //"Cannot connect to server.  Contact administrator"
                        break;
                    case 1045:
                        //"Invalid username/password, please try again"
                        break;
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}