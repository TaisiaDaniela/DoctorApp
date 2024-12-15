using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp.Models
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime AppointmentDateTime { get; set; } // Data și ora completă

        [Ignore] // Nu salvezi în baza de date
        public DateTime AppointmentDate
        {
            get => AppointmentDateTime.Date; // Se va folosi doar data
            set => AppointmentDateTime = value.Add(AppointmentTime); // Adăugăm ora la data selectată
        }

        [Ignore] // Nu salvezi în baza de date
        public TimeSpan AppointmentTime
        {
            get => AppointmentDateTime.TimeOfDay; // Obținem doar partea de timp
            set => AppointmentDateTime = AppointmentDateTime.Date.Add(value); // Setăm ora pe data existentă
        }
        [ForeignKey(nameof(Patient))] 
        public int PatientId { get; set; }

        [ForeignKey(nameof(Treatment))] 
        public int TreatmentID { get; set; }
        // Navigation properties (not directly stored in the database)
        [Ignore]
        public Patient SelectedPatient { get; set; }

        [Ignore]
        public Treatment SelectedTreatment { get; set; }
    }
}
