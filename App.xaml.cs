using System;
using DoctorApp.Data;
using System.IO;
namespace DoctorApp
{
    public partial class App : Application
    {
        static DoctorAppDatabase database;

        public static DoctorAppDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new DoctorAppDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DoctorApp.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
