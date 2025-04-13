using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MyApp.Books;
using Newtonsoft.Json;

public class ApiService
{
    private const string BaseUrl = "https://stephen-king-api.onrender.com/api/books";

    public async Task<List<Book>> GetBooksAsync()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync(BaseUrl);
            var booksResponse = JsonConvert.DeserializeObject<BooksResponse>(response);
            return booksResponse.Data;
        }
    }
}

public class BooksResponse
{
    public List<Book> Data { get; set; }
}
