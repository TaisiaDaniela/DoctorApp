using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DoctorApp.Models
{
    public class ListPatient
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ForeignKey(typeof(Appointment))]
        public int AppointmentID { get; set; }

        public int PatientID { get; set; }
    }
}
