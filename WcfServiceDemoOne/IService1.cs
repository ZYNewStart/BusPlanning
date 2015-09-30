using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfServiceDemoOne
{
    // 注意: 如果更改此处的接口名称 "IService1"，也必须更新 Web.config 中对 "IService1" 的引用。
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

         [OperationContract]
         string RegisterUserInfo(BusRouteRquest requestinfo);

        [OperationContract]
        RegRoute GetRegRoute(int userid);

        [OperationContract]
        List<RegRoute> GetAllRouteInfoByCity(string cityname,int timelistid);

        [OperationContract]
        List<string> GetRegCityNameByDate(int timelistid);

        [OperationContract]
        List<RegionUserNo> GetRegionNewUser();
    }

    [DataContract]
    public class RegionUserNo
    {
        [DataMember]
        public string RegionName { get; set; }

        [DataMember]
        public Int32 UserNo { get; set; }

        public RegionUserNo(string name, Int32 no)
        {
            RegionName = name;
            UserNo = no;
        }
    }

    [DataContract]
    public class RouteStop
    {
        [DataMember]
        public double LNG { get; set; }

        [DataMember]
        public double LAT { get; set; }

        public RouteStop(double lng, double lat)
        {
            this.LNG = lng;
            this.LAT = lat;
        }
    }

    [DataContract]
    public class RegRoute
    {
        public RegRoute()
        {
            RouteList = new List<RouteStop>();
        }

        [DataMember]
        public List<RouteStop> RouteList { get; set; }

        [DataMember]
        public string Contact { get; set; }

        [DataMember]
        public string RegDate { get; set; }

        [DataMember]
        public string DragPoints { get; set; }
    }

    [DataContract]
    public class BusRouteRquest
    {
        private string contact = null;
        private string ip = null;
        private string startName = null;
        private string endName = null;
        private string dragPoints = null;
        private string routeInfo = null;


        [DataMember]
        public string Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        [DataMember]
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        [DataMember]
        public string StartName
        {
            get { return startName; }
            set { startName = value; }
        }

        [DataMember]
        public string EndName
        {
            get { return endName; }
            set { endName = value; }
        }

        [DataMember]
        public string DragPoints
        {
            get { return dragPoints; }
            set { dragPoints = value; }
        }

        [DataMember]
        public string RouteInfo
        {
            get { return routeInfo; }
            set { routeInfo = value; }
        }
    }
}
