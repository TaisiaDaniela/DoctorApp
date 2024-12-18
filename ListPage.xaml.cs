using DoctorApp.Models;
using System.Collections.ObjectModel;

namespace DoctorApp;

public partial class ListPage : ContentPage
{
    private TimeSpan AppointmentTime;
    private DateTime AppointmentDate;
    private int PatientId;
    private int TreatmentID;

    public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();
    public ObservableCollection<Treatment> Treatments { get; set; } = new ObservableCollection<Treatment>(); // Add this property
    public Patient SelectedPatient { get; set; }
    public Treatment SelectedTreatment { get; set; }

    public ListPage()
    {
        InitializeComponent();
        BindingContext = this;
    }


    // OnChooseButtonClicked for adding treatments
    async void OnChooseTreatmentButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TreatmentPage((Appointment)this.BindingContext)
        {
            BindingContext = new Treatment()
        });
    }

    // Example of interacting with treatmentListView
    async void OnAddTreatmentButtonClicked(object sender, EventArgs e)
    {
        // This method could be used to add treatments to treatmentListView
        var treatment = new Treatment(); // Create a new treatment
                                         // Add logic to save treatment and refresh the treatmentListView
        await App.Database.SaveTreatmentAsync(treatment);

        // Refresh treatment list
        var appointment = (Appointment)BindingContext;
        treatmentlistView.ItemsSource = await App.Database.GetListTreatmentsAsync(appointment.ID);
    }






    //// OnAppearing for Treatments and Patients (load both lists)
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Obține lista de pacienți
        var patients = await App.Database.GetPatientsAsync();
        Patients.Clear();
        foreach (var patient in patients)
        {
            Patients.Add(patient);
        }
        patientlistView.ItemsSource = Patients;

        // Încarcă tratamentele disponibile din baza de date
        var treatments = await App.Database.GetTreatmentsAsync();

        // Populează lista de tratamente
        var appointment = (Appointment)BindingContext;
        appointment.Treatments.Clear();
        foreach (var treatment in treatments)
        {
            appointment.Treatments.Add(treatment);
        }

        // Legătura Picker pentru tratamente
        treatmentPicker.ItemsSource = appointment.Treatments;
    }










    


    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var appointment = (Appointment)BindingContext;

        // Verifică dacă pacientul și tratamentul sunt selectate
        if (SelectedPatient != null)
        {
            appointment.PatientId = SelectedPatient.Id;
        }

        if (SelectedTreatment != null)
        {
           appointment.TreatmentID = SelectedTreatment.ID;
        }

        if (string.IsNullOrWhiteSpace(appointment.Description))
        {
            await DisplayAlert("Error", "Please enter a description.", "OK");
            return;
        }

        // Combină data și ora într-un singur câmp
        appointment.AppointmentDateTime = appointment.AppointmentDate.Add(appointment.AppointmentTime);

        // Salvează programarea în baza de date
        await App.Database.SaveAppointmentAsync(appointment);

        // Navigare înapoi la pagina anterioară
        await Navigation.PopAsync();
    }




    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (Appointment)BindingContext;
        await App.Database.DeleteAppointmentAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        // Navighează la pagina pentru adăugarea unui pacient
        await Navigation.PushAsync(new PatientPage((Appointment)this.BindingContext));

        // Reîncarcă lista pacienților după întoarcerea din pagina PatientPage
        var patients = await App.Database.GetPatientsAsync();
        Patients.Clear();
        foreach (var patient in patients)
        {
            Patients.Add(patient);
        }

        // Actualizează dropdown-ul cu pacienți
        patientPicker.ItemsSource = Patients;
    }
}