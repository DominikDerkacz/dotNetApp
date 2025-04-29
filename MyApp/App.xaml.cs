using System.Configuration;
using System.Data;
using System.Windows;
using MyApp.Data;

namespace MyApp
{
    /// <summary>
    /// Główna aplikacja, która inicjalizuje bazę danych przy starcie.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Metoda wywoływana podczas uruchomienia aplikacji.
        /// </summary>
        /// <param name="e">Argumenty uruchomieniowe aplikacji.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Inicjalizuje bazę danych
            InitializeDatabase();
        }

        /// <summary>
        /// Inicjalizuje bazę danych aplikacji.
        /// </summary>
        /// <remarks>
        /// Zapewnia, że baza danych jest utworzona, jeśli nie istnieje.
        /// </remarks>
        private void InitializeDatabase()
        {
            // Tworzy kontekst bazy danych i zapewnia, że baza jest utworzona
            using (var context = new BookDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
