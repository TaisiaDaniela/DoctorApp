using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public DateTime AppointmentDateTime { get; set; } // Data completă (data + ora)

        [Ignore] // Nu salvezi în baza de date
        public DateTime AppointmentDate
        {
            get => AppointmentDateTime.Date; // Returnează doar data (fără ora)
            set => AppointmentDateTime = value.Add(AppointmentTime); // Adaugă ora la data setată
        }

        [Ignore] // Nu salvezi în baza de date
        public TimeSpan AppointmentTime
        {
            get => AppointmentDateTime.TimeOfDay; // Returnează doar ora
            set => AppointmentDateTime = AppointmentDateTime.Date.Add(value); // Setează ora pe data existentă
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
        // Adaugă colecția de tratamente
        [Ignore]
        public ObservableCollection<Treatment> Treatments { get; set; } = new ObservableCollection<Treatment>();
        // Adaugă colecția de pacienti
        [Ignore]
        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();
    }
}
