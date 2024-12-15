using DoctorApp.Models;
using DoctorApp.Data;

namespace DoctorApp;

public partial class PatientPage : ContentPage
{
    Appointment ap;

    public PatientPage(Appointment slist)
    {
        InitializeComponent();
        ap = slist;
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var patient = (Patient)BindingContext;

        // Verificare: nume și adresă sunt obligatorii
        if (string.IsNullOrWhiteSpace(patient?.Name) || string.IsNullOrWhiteSpace(patient?.Address))
        {
            await DisplayAlert("Error", "Please enter valid patient details (Name and Address).", "OK");
            return;
        }

        // Salvează pacientul în baza de date (sau actualizează-l dacă există)
        await App.Database.SavePatientAsync(patient);

        // Creează legătura între programare și pacient
        var listPatient = new ListPatient
        {
            AppointmentID = ap.ID, // ID-ul programării
            PatientID = patient.Id // ID-ul pacientului
        };

        // Salvează legătura în baza de date
        await App.Database.SaveListPatientAsync(listPatient);

        // Actualizează lista de pacienți legați de programare
        listView.ItemsSource = await App.Database.GetListPatientsAsync(ap.ID);

        // Navighează înapoi la pagina anterioară
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var selectedPatient = listView.SelectedItem as Patient;

        if (selectedPatient == null)
        {
            await DisplayAlert("Error", "Please select a patient to delete.", "OK");
            return;
        }

        // Șterge pacientul selectat
        await App.Database.DeletePatientAsync(selectedPatient);

        // Reînnoiește lista pacienților
        listView.ItemsSource = await App.Database.GetPatientsAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Încarcă lista de pacienți
        listView.ItemsSource = await App.Database.GetPatientsAsync();

        // Setează BindingContext pentru un pacient nou
        BindingContext = new Patient();
    }

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var selectedPatient = listView.SelectedItem as Patient;

        if (selectedPatient == null)
        {
            await DisplayAlert("Error", "Please select a patient to add.", "OK");
            return;
        }

        // Creează legătura între programare și pacient
        var listPatient = new ListPatient
        {
            AppointmentID = ap.ID, // ID-ul programării
            PatientID = selectedPatient.Id // ID-ul pacientului
        };

        // Salvează legătura în baza de date
        await App.Database.SaveListPatientAsync(listPatient);

        // Reînnoiește lista pacienților legați de programare
        listView.ItemsSource = await App.Database.GetListPatientsAsync(ap.ID);

        // Navighează înapoi la pagina anterioară
        await Navigation.PopAsync();
    }
}
