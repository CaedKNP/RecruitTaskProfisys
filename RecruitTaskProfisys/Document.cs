using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitTaskProfisys
{
    public class Document
    {
        //Id;Type;Date;FirstName;LastName;City
        public int id { get; set;}
        public string Type { get; set;}
        public DateTime Date { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public Document()
        {
            id = -1;
            Type = "";
            Date = DateTime.Now;
            FirstName = "";
            LastName = "";
            City = "";
        }

        public Document(int _id, string _type, string _date, string _fName, string _lName, string _city)
        {
            id = _id;
            Type = _type;
            Date = DateTime.Parse(_date);
            FirstName = _fName;
            LastName = _lName;
            City = _city;
        }
    }
}
