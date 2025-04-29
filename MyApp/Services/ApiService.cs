using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MyApp.Books;
using Newtonsoft.Json;

namespace MyApp.Services
{
    /// <summary>
    /// Usługa odpowiedzialna za pobieranie danych książek z zewnętrznego API.
    /// </summary>
    public class ApiService
    {
        /// <summary>
        /// Bazowy URL do zewnętrznego API książek.
        /// </summary>
        private const string BaseUrl = "https://stephen-king-api.onrender.com/api/books";

        /// <summary>
        /// Pobiera listę książek z zewnętrznego API asynchronicznie.
        /// </summary>
        /// <returns>Lista obiektów <see cref="Book"/> zawierająca dane książek.</returns>
        public async Task<List<Book>> GetBooksAsync()
        {
            using (var client = new HttpClient())
            {
                // Wysyłanie zapytania do API
                var response = await client.GetStringAsync(BaseUrl);

                // Deserializacja odpowiedzi do obiektu BooksResponse
                var booksResponse = JsonConvert.DeserializeObject<BooksResponse>(response);

                // Zwracanie danych książek
                return booksResponse.Data;
            }
        }
    }

    /// <summary>
    /// Reprezentuje odpowiedź API zawierającą dane książek.
    /// </summary>
    public class BooksResponse
    {
        /// <summary>
        /// Lista książek pobranych z API.
        /// </summary>
        public List<Book> Data { get; set; }
    }
}
