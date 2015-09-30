

namespace WcfServiceDemoOne.Model
{
    public class RouteStop
    {
        public double LNG { get; set; }
        public double LAT { get; set; }
        public RouteStop(double lng, double lat)
        {
            this.LNG = lng;
            this.LAT = lat;
        }
    }
}