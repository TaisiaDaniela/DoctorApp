using Plugin.LocalNotification;
using System;
using DoctorApp.Models;

namespace DoctorApp;

public partial class ListEntryPage : ContentPage
{
    public Patient SelectedPatient { get; set; }
    public Treatment SelectedTreatment { get; set; }
    // Adăugăm variabilele pentru data și ora programării
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public ListEntryPage()
    {
        InitializeComponent();
    }



    public void ScheduleNotification(DateTime appointmentDateTime)
    {
        // Calculează momentul cu o zi înainte de programarea respectivă
        DateTime notificationTime = appointmentDateTime.AddDays(-1);

        // Creează notificarea
        var notification = new NotificationRequest
        {
            NotificationId = 1001, // Identificator unic pentru notificare
            Title = "Appointment Reminder",
            Description = "You have an appointment tomorrow!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notificationTime
            }
        };

        // Programează notificarea
        LocalNotificationCenter.Current.Show(notification);
    }


    async void OnAppointmentAddedClicked(object sender, EventArgs e)
    {
        // Creează o nouă programare
        var appointment = new Appointment
        {
            SelectedPatient = SelectedPatient,
            SelectedTreatment = SelectedTreatment,
            AppointmentDateTime = AppointmentDate.Date.Add(AppointmentTime) // Asigură-te că AppointmentDate și AppointmentTime sunt setate corect
        };

        // Salvează programarea în baza de date
        await App.Database.SaveAppointmentAsync(appointment);

        // Programează notificarea pentru o zi înainte de data programării
        ScheduleNotification(appointment.AppointmentDateTime);

        // Navighează către pagina de detalii a programării
        await Navigation.PushAsync(new ListPage
        {
            BindingContext = appointment
        });
    }





















    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Fetch appointments from the database
        var appointments = await App.Database.GetAppointmentsAsync();

        // Fetch associated patients and treatments
        foreach (var appointment in appointments)
        {
            // Fetch the Patient and Treatment by Id
            appointment.SelectedPatient = await App.Database.GetPatientByIdAsync(appointment.PatientId);
            appointment.SelectedTreatment = await App.Database.GetTreatmentByIdAsync(appointment.TreatmentID);
        }

        listView.ItemsSource = await App.Database.GetAppointmentsAsync();
        listView.ItemsSource = appointments;

    }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedAppointment = e.SelectedItem as Appointment;

            // Navigate to the ListPage with the selected Appointment
            await Navigation.PushAsync(new ListPage
            {
                BindingContext = selectedAppointment
            });
        }
    }
}
