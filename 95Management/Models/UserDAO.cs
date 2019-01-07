using _95Management.Service;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _95Management.Models
{
    public class UserDAO
    {
        static UserDAO _instance = null;
        static object locked = null;

        static UserDAO()
        {
            locked = new object();
        }

        public static UserDAO Instance
        {
            get
            {
                lock (locked)
                {
                    if (_instance == null)
                    {
                        _instance = new UserDAO();
                    }
                    return _instance;
                }
            }
        }

        public int InsertNewUser(string name, string nickname, string openid, string phoneno, DateTime jointime, string iconulr = "")
        {
            try
            {
                string sql = "insert into USERINFO (NAME, NICKNAME, OPENID, PHONENO, JOINTIME, ICONURL) values (@name, @nickname, @openid, @phoneno, @jointime, @iconulr)";
                MySqlConnection conn = SQLHelper.Instance.GetConnection();
                conn.Open();
                MySqlCommand comd = new MySqlCommand(sql, conn);
                comd.Parameters.AddWithValue("name", name);
                comd.Parameters.AddWithValue("nickname", nickname);
                comd.Parameters.AddWithValue("openid", openid);
                comd.Parameters.AddWithValue("phoneno", phoneno);
                comd.Parameters.AddWithValue("jointime", jointime);
                comd.Parameters.AddWithValue("iconulr", iconulr);

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