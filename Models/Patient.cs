using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Models
{
    public class Patient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(250), Unique]
        public string Name { get; set; }
        [MaxLength(250), Unique]
        public string Address { get; set; }
        [OneToMany]
        public List<ListPatient> ListPatients { get; set; }
    }
}
