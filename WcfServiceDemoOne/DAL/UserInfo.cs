using System;
using DB.Utils;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using WcfServiceDemoOne;

namespace WcfServiceBusPlanning.DAL
{
    public class UserInfo
    {
        public long InsertUserInfo(WcfServiceBusPlanning.Model.UserInfo userinfo)
        {
            long recordid = 0;
            string retidstring = "Select userinfo_autoinc_seq.nextval from dual";
            Object result = OracleHelper.GetSingle(retidstring, null);
            bool convertresult = long.TryParse(result.ToString(), out recordid);
            if (!convertresult)
            {
                return 0;
            }
            string insertstring = "insert into UserInfo(ID,Contact,IP,StartName,EndName,DragPoints,Email,Name,Pwd,Flag) values(:id,:contact,:ip,:startname,:endname,:dragpoints,:email,:name,:pwd,0)";
            int insertresult = OracleHelper.ExecuteNonQuery(System.Data.CommandType.Text, insertstring, new OracleParameter[]
            {
                new OracleParameter("id", recordid),
                new OracleParameter("contact", userinfo.Contact),
                new OracleParameter("ip",userinfo.IP),
                new OracleParameter("startname",userinfo.StartName),
                new OracleParameter("endname",userinfo.EndName),
                new OracleParameter("dragpoints",userinfo.DragPoints),
                new OracleParameter("email",userinfo.Email),
                new OracleParameter("name",userinfo.Name),
                new OracleParameter("pwd",userinfo.Pwd)
            });
            if (insertresult < 1)
            {
                return 0;
            }
            return recordid;
        }

        public Model.UserInfo GetUserInfoByUserID(int userid)
        {
            Model.UserInfo userinfo = null;
            OracleDataReader reader = null;
            try
            {
                reader = OracleHelper.ExecuteReader("SELECT REGDATE,CONTACT,IP,STARTNAME,ENDNAME,DRAGPOINTS,EMAIL,NAME,PWD,FLAG FROM USERINFO WHERE ID = ：id", new OracleParameter[]
                {
                    new OracleParameter("id",userid)
                });
                if (reader.RowSize > 0)
                {
                    userinfo = new Model.UserInfo();
                    userinfo.ID = userid;
                    userinfo.RegDate = reader.GetDateTime(0);
                    userinfo.Contact = reader.GetString(1);
                    userinfo.IP = reader.GetString(2);
                    userinfo.StartName = reader.GetString(3);
                    userinfo.EndName = reader.GetString(4);
                    userinfo.DragPoints = reader.GetString(5);
                    userinfo.Email = reader.GetString(6);
                    userinfo.Name = reader.GetString(7);
                    userinfo.Pwd = reader.GetString(8);
                    userinfo.Flag = reader.GetInt32(9);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return userinfo;
        }

        public Model.UserInfo GetUserInfoByContact(string contact)
        {
            Model.UserInfo userinfo = null;
            OracleDataReader reader = null;
            try
            {
                reader = OracleHelper.ExecuteReader("SELECT REGDATE,CONTACT,IP,STARTNAME,ENDNAME,DRAGPOINTS,EMAIL,NAME,PWD,FLAG,ID FROM USERINFO WHERE CONTACT = ：contact", new OracleParameter[]
                {
                    new OracleParameter("contact",contact)
                });
                if (reader.RowSize > 0)
                {
                    userinfo = new Model.UserInfo();
                    userinfo.RegDate = reader.GetDateTime(0);
                    userinfo.Contact = reader.GetString(1);
                    userinfo.IP = reader.GetString(2);
                    userinfo.StartName = reader.GetString(3);
                    userinfo.EndName = reader.GetString(4);
                    userinfo.DragPoints = reader.GetString(5);
                    userinfo.Email = reader.GetString(6);
                    userinfo.Name = reader.GetString(7);
                    userinfo.Pwd = reader.GetString(8);
                    userinfo.Flag = reader.GetInt32(9);
                    userinfo.ID = reader.GetInt64(10);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return userinfo;
        }

        /// <summary>
        /// 获取起点或终点在同一城市的用户信息
        /// </summary>
        /// <param name="city">城市名</param>
        /// <returns></returns>
        public List<Model.UserInfo> GetAllUserInfoByCity(string cityname)
        {
            List<Model.UserInfo> userinfoList = null;
            OracleDataReader reader = null;
            try
            {
                reader = OracleHelper.ExecuteReader("SELECT REGDATE,CONTACT,IP,STARTNAME,ENDNAME,DRAGPOINTS,EMAIL,NAME,PWD,FLAG,ID FROM USERINFO WHERE STARTNAME = ：cityname OR ENDNAME = ：cityname ", new OracleParameter[]
                {
                    new OracleParameter("cityname",cityname)
                });
                if (reader.RowSize > 0)
                {
                    userinfoList = new List<Model.UserInfo>();
                    while (reader.Read())
                    {
                        Model.UserInfo userinfo = new Model.UserInfo();
                        userinfo.RegDate = reader.GetDateTime(0);
                        userinfo.Contact = reader.GetString(1);
                        userinfo.IP = reader.GetString(2);
                        userinfo.StartName = reader.GetString(3);
                        userinfo.EndName = reader.GetString(4);
                        userinfo.DragPoints = reader.GetString(5);
                        userinfo.Email = reader.GetString(6);
                        userinfo.Name = reader.GetString(7);
                        userinfo.Pwd = reader.GetString(8);
                        userinfo.Flag = reader.GetInt32(9);
                        userinfo.ID = reader.GetInt64(10);
                        userinfoList.Add(userinfo);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return userinfoList;
        }

        public List<string> GetRegCityNameByDate(string startdate, string enddate)
        {
            List<string> citynameList = null;
            OracleDataReader reader = null;
            try
            {
                //获取起点或终点中不同的城市，尝试distinct STARTNAME, ENDNAME或者group by STARTNAME, ENDNAME
                reader = OracleHelper.ExecuteReader("(SELECT DISTINCT STARTNAME AS cityname FROM USERINFO WHERE REGDATE < to_date(:enddate,'yyyy-mm-dd hh24:mi:ss') AND REGDATE > to_date(:startdate,'yyyy-mm-dd hh24:mi:ss')) UNION (SELECT DISTINCT ENDNAME AS cityname FROM USERINFO WHERE REGDATE < to_date(:enddate,'yyyy-mm-dd hh24:mi:ss') AND REGDATE > to_date(:startdate,'yyyy-mm-dd hh24:mi:ss'))", new OracleParameter[]
                {
                    new OracleParameter(":enddate",enddate),
                    new OracleParameter(":startdate",startdate),
                    new OracleParameter(":enddate",enddate),
                    new OracleParameter(":startdate",startdate)
                });
                if (reader.RowSize > 0)
                {
                    citynameList = new List<string>();
                    while (reader.Read())
                    {
                        citynameList.Add(reader.GetString(0));
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return citynameList;
        }

        public List<RegionUserNo> GetRegionNewUserByDate(string currentDate)
        {
            List<RegionUserNo> regionuserno = null;
            OracleDataReader reader = null;
            try
            {
                reader = OracleHelper.ExecuteReader("SELECT STARTNAME,Count(*) AS COUNT  FROM USERINFO WHERE to_char(REGDATE,'YYYY-MM-DD') = :currentdate  GROUP BY STARTNAME", new OracleParameter[]
                {
                    new OracleParameter(":currentdate",currentDate)
                });
                if (reader.RowSize > 0)
                {
                    regionuserno = new List<RegionUserNo>();
                    while (reader.Read())
                    {
                        regionuserno.Add(new RegionUserNo(reader.GetString(0),reader.GetInt32(1)));
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return regionuserno;
        }
    }
}