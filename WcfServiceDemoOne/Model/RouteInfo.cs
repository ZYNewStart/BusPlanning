
namespace WcfServiceBusPlanning.Model
{
    public class RouteInfo
    {
        private long _id;
        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private long _userid;
        public long UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        private long _routeIndex;
        public long RouteIndex
        {
            get { return _routeIndex; }
            set { _routeIndex = value; }
        }

        private double _lat;
        public double Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        private double _lng;
        public double Lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
    }
}
