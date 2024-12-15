using DoctorApp.Models;
using System.Collections.ObjectModel;

namespace DoctorApp;

public partial class ListPage : ContentPage
{
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

        // Populate the list of patients
        var patients = await App.Database.GetPatientsAsync();
        Patients.Clear();
        foreach (var patient in patients)
        {
            Patients.Add(patient);
        }

        // Assign the list of patients to the listView (for patient display)
        patientlistView.ItemsSource = Patients;

        // Load treatments for the current appointment
        var appointment = (Appointment)BindingContext;
        var treatments = await App.Database.GetListTreatmentsAsync(appointment.ID);

        // Add treatments to the ObservableCollection
        Treatments.Clear();
        foreach (var treatment in treatments)
        {
            Treatments.Add(treatment);
        }

        // Assign treatments to the treatment picker (dropdown)
        treatmentPicker.ItemsSource = Treatments;
    }












    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Obține obiectul Appointment din BindingContext
        var slist = (Appointment)BindingContext;

        // Verifică dacă pacientul a fost selectat și setează ID-ul pacientului
        if (SelectedPatient != null)
        {
            slist.PatientId = SelectedPatient.Id;
        }

        // Combină data și ora într-un singur câmp AppointmentDateTime
        slist.AppointmentDateTime = slist.AppointmentDateTime.Date.Add(AppointmentTime); // AppointmentTime este de tip TimeSpan

        // Salvează programarea în baza de date
        await App.Database.SaveAppointmentAsync(slist);

        // Navighează înapoi la pagina anterioară
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
