using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Tools;

namespace WcfServiceDemoOne
{
    // 注意: 如果更改此处的类名“Service1”，也必须更新 Web.config 和关联的 .svc 文件中对“Service1”的引用。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1 : IService1
    {
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public string RegisterUserInfo(BusRouteRquest requestinfo)
        {
            System.Object lockThis = new System.Object();
            lock (lockThis)
            {
                try
                {
                    WcfServiceBusPlanning.DAL.UserInfo userinfodal = new WcfServiceBusPlanning.DAL.UserInfo();
                    WcfServiceBusPlanning.Model.UserInfo userinfo = new WcfServiceBusPlanning.Model.UserInfo();
                    userinfo.Contact = requestinfo.Contact;
                    userinfo.StartName = requestinfo.StartName;
                    userinfo.EndName = requestinfo.EndName;
                    userinfo.DragPoints = requestinfo.DragPoints;
                    userinfo.RegDate = DateTime.Now;
                    Safe safe = new Safe();
                    userinfo.IP = safe.ClientIp();
                    //userinfo.Email = "zhouyong1508379@163.com";
                    //userinfo.Name = "zhouyong";
                    //userinfo.Pwd = "123456";
                    long userid = userinfodal.InsertUserInfo(userinfo);
                    List<WcfServiceBusPlanning.Model.RouteInfo> routeinfoList = new List<WcfServiceBusPlanning.Model.RouteInfo>();
                    string routeinfostr = requestinfo.RouteInfo;//"113.947187,22.747208;113.946796,22.747468;113.945736,22.748288;113.945405,22.748578;113.943763,22.750241;113.943563,22.750461;113.943483,22.750562;113.943373,22.750482;113.943463,22.750402;113.943462,22.750402;113.943613,22.750221;113.945195,22.748579;113.945666,22.748168;113.946276,22.747678;113.946676,22.747388;113.947627,22.746759;113.948267,22.74638;113.948387,22.74632;113.948647,22.746191;113.949147,22.745942;113.950357,22.745396;113.951337,22.74504;113.951696,22.744922;113.954034,22.744305;113.954384,22.744238;113.954454,22.744218;113.955103,22.744123;113.955363,22.744085;113.955752,22.744029;113.956151,22.743952;113.95709,22.743791;113.957339,22.743743;113.957928,22.743629;113.958208,22.743562;113.958427,22.743514;113.958917,22.7434;113.959116,22.743342;113.959276,22.743294;113.960364,22.742927;113.960544,22.742849;113.961062,22.742625;113.961232,22.742557;113.962619,22.741836;";
                    WcfServiceBusPlanning.DAL.RouteInfo routeinfodal = new WcfServiceBusPlanning.DAL.RouteInfo();
                    string[] routeinfo = routeinfostr.Split(';');
                    for (int i = 0; i < routeinfo.Length - 1; i++)
                    {
                        string[] lnglat = routeinfo[i].Split(',');
                        routeinfoList.Add(new WcfServiceBusPlanning.Model.RouteInfo()
                        {
                            RouteIndex = i + 1,
                            UserID = userid,
                            Lng = Convert.ToDouble(lnglat[0]),
                            Lat = Convert.ToDouble(lnglat[1])
                        });
                    }
                    routeinfodal.InsertRouteInfos(routeinfoList);
                }
                catch (Exception ex)
                {
                    string abc = ex.Message;
                    return "0";
                }
                return "1";
            }
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public RegRoute GetRegRoute(int userid)
        {
            RegRoute regroute = new RegRoute();
            WcfServiceBusPlanning.DAL.RouteInfo routeinfodal = new WcfServiceBusPlanning.DAL.RouteInfo();
            regroute.RouteList = routeinfodal.GetRouteStopByUserID(userid);
            return regroute;
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetRegCityNameByDate(int timelistid)
        {
            List<string> citynameList = null;
            string startdate = null, enddate = null;
            WcfServiceBusPlanning.DAL.UserInfo userinfodal = new WcfServiceBusPlanning.DAL.UserInfo();
            switch (timelistid)
            {
                case 1:
                    startdate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 2:
                    startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 3:
                    startdate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 4:
                    startdate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 5:
                    startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                default:
                    return null;
            }
            citynameList = userinfodal.GetRegCityNameByDate(startdate,enddate);
            return citynameList;
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public List<RegRoute> GetAllRouteInfoByCity(string cityname, int timelistid)
        {
            List<RegRoute> routeList = null;
            string startdate = null;
            string enddate = null;
            WcfServiceBusPlanning.DAL.RouteInfo routeinfodal = new WcfServiceBusPlanning.DAL.RouteInfo();
            switch (timelistid)
            {
                case 1:
                    startdate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 2:
                    startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 3:
                    startdate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 4:
                    startdate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                case 5:
                    startdate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    break;
                default:
                    return null;
            }
            routeList = routeinfodal.GetAllRouteInfoByCity(cityname, startdate, enddate);
            return routeList;
        }

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public List<RegionUserNo> GetRegionNewUser()
        {
            List<RegionUserNo> regionuserno = null;
            WcfServiceBusPlanning.DAL.UserInfo userinfodal = new WcfServiceBusPlanning.DAL.UserInfo();
            regionuserno = userinfodal.GetRegionNewUserByDate(DateTime.Now.ToString("yyyy-MM-dd"));
            return regionuserno;
        }
    }
}
