using DB.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using WcfServiceDemoOne;

namespace WcfServiceBusPlanning.DAL
{
    public class RouteInfo
    {
        public bool InsertRouteInfos(List<WcfServiceBusPlanning.Model.RouteInfo> routeinfos)
        {
            if (routeinfos == null || routeinfos.Count < 1)
            {
                return false;
            }
            Dictionary<string, object> columnRowData = new Dictionary<string, object>();
            int count = routeinfos.Count;
            long[] id = new long[count];
            long[] userid = new long[count];
            long[] routeindex = new long[count];
            double[] lat = new double[count];
            double[] lng = new double[count];
            for (int i = 0; i < count; i++)
            {
                id[i] = routeinfos[i].ID;
                userid[i] = routeinfos[i].UserID;
                routeindex[i] = routeinfos[i].RouteIndex;
                lat[i] = routeinfos[i].Lat;
                lng[i] = routeinfos[i].Lng;
            }
            columnRowData.Add("ID", id);
            columnRowData.Add("UserID", userid);
            columnRowData.Add("RouteIndex", routeindex);
            columnRowData.Add("Lat", lat);
            columnRowData.Add("Lng", lng);
            int insertresult = OracleHelper.BatchInsert("RouteInfo", columnRowData, count);
            if (insertresult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<RouteStop> GetRouteStopByUserID(int userid)
        {
            List<RouteStop> pointList = null;
            OracleDataReader reader = null;
            try
            {
                reader = OracleHelper.ExecuteReader("SELECT LNG,LAT FROM ROUTEINFO WHERE USERID =:userid ORDER BY ROUTEINDEX",
                                            new OracleParameter[] { new OracleParameter("userid", userid) });
                pointList = new List<RouteStop>();
                while (reader.Read())
                {
                    pointList.Add(new RouteStop(reader.GetDouble(0), reader.GetDouble(1)));
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
            return pointList;
        }

        /// <summary>
        /// 获取起点或终点在同一城市的路线
        /// </summary>
        /// <param name="city">城市名</param>
        /// <returns></returns>
        public List<RegRoute> GetAllRouteInfoByCity(string cityname,string startdate,string enddate)
        {
            
            List<RegRoute> routeinfoList = null;
            OracleDataReader reader = null;
            try
            {
                //SELECT LNG,LAT,USERID FROM RouteInfo r LEFT JOIN UserInfo u ON u.ID = r.USERID WHERE (STARTNAME = '深圳市' OR ENDNAME = '深圳市') AND REGDATE < to_date('2015-09-07 00:00:00','yyyy-mm-dd hh24:mi:ss') AND REGDATE > to_date('2015-09-01 00:00:00','yyyy-mm-dd hh24:mi:ss') ORDER BY USERID,ROUTEINDEX
                reader = OracleHelper.ExecuteReader("SELECT LNG,LAT,USERID FROM RouteInfo r LEFT JOIN UserInfo u ON u.ID = r.USERID WHERE (STARTNAME = :cityname OR ENDNAME = :cityname) AND REGDATE < to_date(:enddate,'yyyy-mm-dd hh24:mi:ss') AND REGDATE > to_date(:startdate,'yyyy-mm-dd hh24:mi:ss') ORDER BY USERID,ROUTEINDEX", new OracleParameter[]
                {
                    new OracleParameter(":cityname",cityname),
                    new OracleParameter(":cityname",cityname),
                    new OracleParameter(":enddate",enddate),
                    new OracleParameter(":startdate",startdate)
                });
                if (reader.RowSize > 0)
                {
                    routeinfoList = new List<RegRoute>();
                    RegRoute regRoute = null;
                    Int64 userid =  0;
                    //Console.WriteLine(reader.HasRows.ToString());
                    while (reader.Read())
                    {
                        if (userid != reader.GetInt64(2))
                        {
                            if (userid != 0)
                            {
                                routeinfoList.Add(regRoute);
                            }
                            regRoute = new RegRoute();
                            userid = reader.GetInt64(2);
                        }
                        regRoute.RouteList.Add(new RouteStop(reader.GetDouble(0), reader.GetDouble(1)));
                    }
                    routeinfoList.Add(regRoute);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return routeinfoList;
        }
    }
}