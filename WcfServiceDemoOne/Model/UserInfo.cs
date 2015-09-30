using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceBusPlanning.Model
{
    public class UserInfo
    {
        private long _id;
        public long ID 
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        private DateTime _regDate;
        public DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }


        private string _ip;
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private string _startName;
        public string StartName
        {
            get { return _startName; }
            set { _startName = value; }
        }

        private string _endName;
        public string EndName
        {
            get { return _endName; }
            set { _endName = value; }
        }

        private string _dragPoints;
        public string DragPoints
        {
            get { return _dragPoints; }
            set { _dragPoints = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _pwd;
        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        private int _flag;
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}