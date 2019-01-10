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

        public int InsertNewUser(string openid, string sessionkey)
        {
            int userid = 0;
            MySqlConnection conn = SQLHelper.Instance.GetConnection();
            try
            {
                conn.Open();
                
                //先查询有没有该用户
                string sql = "select * from USERINFO where OPENID=@openid";
                MySqlCommand comd = new MySqlCommand(sql, conn);
                comd.Parameters.AddWithValue("openid", openid);

                MySqlDataReader reader = comd.ExecuteReader();
                while (reader.Read())
                {
                    userid = Convert.ToInt32(reader["USERID"]);
                }
                if (userid != 0) return userid;

                //没有该用户则插入
                sql = "insert into USERINFO (OPENID, SESSIONKEY) values (@openid, @sessionkey)";
                comd = new MySqlCommand(sql, conn);
                comd.Parameters.AddWithValue("openid", openid);
                comd.Parameters.AddWithValue("sessionkey", sessionkey);

                int res = comd.ExecuteNonQuery();

                if (res==1)
                {
                    //查询新插入用户的userid
                    sql = "select * from USERINFO where OPENID=@openid";
                    comd = new MySqlCommand(sql, conn);
                    comd.Parameters.AddWithValue("openid", openid);

                    reader = comd.ExecuteReader();
                    while (reader.Read())
                    {
                        userid = Convert.ToInt32(reader["USERID"]);
                    }
                    return userid;
                }
                else
                {
                    return res;
                }
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
                conn.Close();
                return 0;
            }
        }

        public int UpdateUser(int userid, string name, string nickname, string openid, string phoneno, DateTime jointime, string iconulr = "")
        {
            MySqlConnection conn = SQLHelper.Instance.GetConnection();
            try
            {
                string sql = "insert into USERINFO (NAME, NICKNAME, OPENID, PHONENO, JOINTIME, ICONURL) values (@name, @nickname, @openid, @phoneno, @jointime, @iconulr)";
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
                conn.Close();
                return 0;
            }
        }

    }
}