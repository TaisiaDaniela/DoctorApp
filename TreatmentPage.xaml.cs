using DoctorApp.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace DoctorApp;

public partial class TreatmentPage : ContentPage
{
    Appointment ap;

    public TreatmentPage(Appointment slist)
    {
        InitializeComponent();
        ap = slist;
    }
    async void OnSaveTreatmentButtonClicked(object sender, EventArgs e)
    {
        var treatment = (Treatment)BindingContext;
        await App.Database.SaveTreatmentAsync(treatment);
        listView.ItemsSource = await App.Database.GetTreatmentsAsync();
    }

    async void OnDeleteTreatmentButtonClicked(object sender, EventArgs e)
    {
        var treatment = listView.SelectedItem as Treatment;
        await App.Database.DeleteTreatmentAsync(treatment);
        listView.ItemsSource = await App.Database.GetTreatmentsAsync();
    }

    async void OnAddTreatmentButtonClicked(object sender, EventArgs e)
    {
        Treatment t;
        if (listView.SelectedItem != null)
        {
            t = listView.SelectedItem as Treatment;
            var lt = new ListTreatment()
            {
                AppointmentID = ap.ID,
                TreatmentID = t.ID
            };
            await App.Database.SaveListTreatmentAsync(lt);
            t.ListTreatments = new List<ListTreatment> { lt };

            await Navigation.PopAsync();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetTreatmentsAsync();
    }
}