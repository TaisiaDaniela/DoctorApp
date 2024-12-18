using Plugin.LocalNotification;
using System;
using DoctorApp.Models;
using Plugin.LocalNotification.AndroidOption;

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
        // Verifică dacă appointmentDateTime este valid
        if (appointmentDateTime == DateTime.MinValue)
        {
            Console.WriteLine("Eroare: Data programării este invalidă.");
            return;
        }

        // Calculează momentul notificării cu o zi înainte
        DateTime notificationTime = appointmentDateTime.AddDays(-1);

        // Evită notificările în trecut
        if (notificationTime <= DateTime.Now)
        {
            Console.WriteLine("Eroare: Nu poți programa o notificare în trecut.");
            return;
        }

        var notification = new NotificationRequest
        {
            NotificationId = 1001,
            Title = "Appointment Reminder",
            Description = $"You have an appointment tomorrow at {appointmentDateTime:HH:mm}.",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notificationTime
            }
        };

        LocalNotificationCenter.Current.Show(notification);
    }


    async void OnAppointmentAddedClicked(object sender, EventArgs e)
    {
        // Verifică dacă datele sunt valide
        //if (AppointmentDate == DateTime.MinValue)
        //{
        //    await DisplayAlert("Eroare", "Te rog selectează o dată validă pentru programare.", "OK");
        //    return;
        //}

        //if (AppointmentTime == TimeSpan.Zero)
        //{
        //    await DisplayAlert("Eroare", "Te rog selectează o oră validă pentru programare.", "OK");
        //    return;
        //}

        // Creează o nouă programare
        var appointment = new Appointment
        {
            SelectedPatient = SelectedPatient,
            SelectedTreatment = SelectedTreatment,
            AppointmentDateTime = AppointmentDate.Date.Add(AppointmentTime)
        };

        // Verifică dacă programarea este în viitor
        //if (appointment.AppointmentDateTime <= DateTime.Now)
        //{
        //    await DisplayAlert("Eroare", "Data programării trebuie să fie în viitor.", "OK");
        //    return;
        //}

        // Salvează programarea în baza de date
        await App.Database.SaveAppointmentAsync(appointment);

        // Programează notificarea pentru o zi înainte de data programării

        // Navighează către pagina de detalii a programării
        await Navigation.PushAsync(new ListPage
        {
            BindingContext = appointment
        });
    }

   

    protected override async void OnAppearing()
    {
        base.OnAppearing();

       // Preia lista de programări din baza de date
        var appointments = await App.Database.GetAppointmentsAsync();

        foreach (var appointment in appointments)
        {
            // Încarcă pacientul asociat
            if (appointment.PatientId > 0)
            {
                appointment.SelectedPatient = await App.Database.GetPatientByIdAsync(appointment.PatientId);
           }

            // Încarcă tratamentul asociat
            if (appointment.TreatmentID > 0)
           {
                appointment.SelectedTreatment = await App.Database.GetTreatmentByIdAsync(appointment.TreatmentID);
           }
        }

        // Afișează programările
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
