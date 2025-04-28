using System.Configuration;
using System.Data;
using System.Windows;
using MyApp.Data;

namespace MyApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Inicjalizacja bazy danych
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var context = new BookDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
