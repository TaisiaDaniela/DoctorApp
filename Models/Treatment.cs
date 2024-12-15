using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Models
{
    public class Treatment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(250)]
        public string Type { get; set; }
        [OneToMany]
        public List<ListTreatment> ListTreatments { get; set; }
    }
}
