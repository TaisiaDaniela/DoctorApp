using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using DoctorApp.Models;

namespace DoctorApp.Data
{
    public class DoctorAppDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DoctorAppDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Appointment>().Wait();
            _database.CreateTableAsync<Patient>().Wait();
            _database.CreateTableAsync<ListPatient>().Wait();
            _database.CreateTableAsync<Treatment>().Wait();
            _database.CreateTableAsync<ListTreatment>().Wait();
        }
        public async Task<List<Appointment>> GetAppointmentsAsync()
        {
            var appointments = await _database.Table<Appointment>().ToListAsync();

            // Încarcă pacientul și tratamentul asociat pentru fiecare programare
            foreach (var appointment in appointments)
            {
                appointment.SelectedPatient = await GetPatientByIdAsync(appointment.PatientId);
                appointment.SelectedTreatment = await GetTreatmentByIdAsync(appointment.TreatmentID);
            }

            return appointments;
        }


        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return await _database.Table<Patient>().FirstOrDefaultAsync(p => p.Id == patientId);
        }

        public async Task<Treatment> GetTreatmentByIdAsync(int treatmentId)
        {
            return await _database.Table<Treatment>().FirstOrDefaultAsync(t => t.ID == treatmentId);
        }












        public Task<int> SaveTreatmentAsync(Treatment treatment)
        {
            if (treatment.ID != 0)
            {
                return _database.UpdateAsync(treatment);
            }
            else
            {
                return _database.InsertAsync(treatment);
            }
        }

        public Task<int> DeleteTreatmentAsync(Treatment treatment)
        {
            return _database.DeleteAsync(treatment);
        }

        public Task<List<Treatment>> GetTreatmentsAsync()
        {
            return _database.Table<Treatment>().ToListAsync();
        }
        public Task<int> SaveListTreatmentAsync(ListTreatment listt)
        {
            if (listt.ID != 0)
            {
                return _database.UpdateAsync(listt);
            }
            else
            {
                return _database.InsertAsync(listt);
            }
        }

        public Task<List<Treatment>> GetListTreatmentsAsync(int appointmentid)
        {
            return _database.QueryAsync<Treatment>(
                "select T.ID, T.Type from Treatment T" +
                " inner join ListTreatment LT" +
                " on T.ID = LT.TreatmentID where LT.AppointmentID = ?",
                appointmentid);
        }











        public Task<int> SaveListPatientAsync(ListPatient listp)
                {
                    if (listp.ID != 0)
                    {
                        return _database.UpdateAsync(listp);
                    }
                    else
                    {
                        return _database.InsertAsync(listp);
                    }
                }

                public Task<List<Patient>> GetListPatientsAsync(int appointmentid)
                {
                    return _database.QueryAsync<Patient>(
                        "SELECT P.ID, P.Name, P.Address " +
                        "FROM Patient P " +
                        "INNER JOIN ListPatient LP ON P.ID = LP.PatientID " +
                        "WHERE LP.AppointmentID = ?",
                        appointmentid);
                }

                public Task<int> SavePatientAsync(Patient patient)
                {
                    if (patient.Id != 0)
                    {
                        return _database.UpdateAsync(patient);
                    }
                    else
                    {
                        return _database.InsertAsync(patient);
                    }
                }

                public Task<int> DeletePatientAsync(Patient patient)
                {
                    return _database.DeleteAsync(patient);
                }

                public Task<List<Patient>> GetPatientsAsync()
                {
                    return _database.Table<Patient>().ToListAsync();
                }

                public Task<Appointment> GetAppointmentAsync(int id)
                {
                    return _database.Table<Appointment>()
                                    .Where(i => i.ID == id)
                                    .FirstOrDefaultAsync();
                }

                public Task<int> SaveAppointmentAsync(Appointment slist)
                {
                    if (slist.ID != 0)
                    {
                        return _database.UpdateAsync(slist);
                    }
                    else
                    {
                        return _database.InsertAsync(slist);
                    }
                }

                public Task<int> DeleteAppointmentAsync(Appointment slist)
                {
                    return _database.DeleteAsync(slist);
                }
            }
        }
