
using ConsoleApp;
using Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5017/api/");
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

var result = await httpClient.GetAsync("shoppingLists");

/*if(result.StatusCode != System.Net.HttpStatusCode.OK)
{
    Console.WriteLine($"Error: {(int)result.StatusCode} - {result.ReasonPhrase}");
}*/

/*if(!result.IsSuccessStatusCode)
{
    Console.WriteLine($"Error: {(int)result.StatusCode} - {result.ReasonPhrase}");
}*/

result.EnsureSuccessStatusCode(); //rzucamy wyjątek jeśli kod statusu nie wskazuje na sukces (2xx)


var json = await result.Content.ReadAsStringAsync();

Console.WriteLine(json);


var soppingLists = await result.Content.ReadFromJsonAsync<IEnumerable<ShoppingList>>();



result = await httpClient.GetAsync("people");
result.EnsureSuccessStatusCode();

var options = new JsonSerializerSettings
{
    PreserveReferencesHandling = PreserveReferencesHandling.Objects
};
//var people = await result.Content.ReadFromJsonAsync<IEnumerable<Person>>();
json= await result.Content.ReadAsStringAsync();
var people = JsonConvert.DeserializeObject<IEnumerable<Person>>(json, options);


var shoppingList = new ShoppingList
{
    Name = "Weekly Groceries"
};

result = await httpClient.PostAsJsonAsync("shoppingLists", shoppingList);
if(!result.IsSuccessStatusCode)
{
    Console.WriteLine($"Error: {(int)result.StatusCode} - {result.ReasonPhrase}");
    Console.WriteLine(await result.Content.ReadAsStringAsync());
}

shoppingList.Name = "Zakupy tygodniowe";
result = await httpClient.PostAsJsonAsync("shoppingLists", shoppingList);
result.EnsureSuccessStatusCode();

shoppingList.Id = result.Content.ReadFromJsonAsync<int>().Result;

shoppingList.CreatedAt = DateTime.Now.AddDays(-1);
result = await httpClient.PutAsJsonAsync($"shoppingLists/{shoppingList.Id}", shoppingList);
result.EnsureSuccessStatusCode();

//możliwe jest metod "skrótowych", które zwracają od razu zdeserializowany obiekt
//jesli odpowiedź nie jest sukcesem, to zostanie rzucony wyjątek
shoppingList = (await httpClient.GetFromJsonAsync<ShoppingList>($"shoppingLists/{shoppingList.Id}"))!;

Console.ReadLine();


WebApiClient webApiClient = new WebApiClient("http://localhost:5017/api/");
webApiClient.JsonSerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

people = await webApiClient.GetAsync<IEnumerable<Person>>("people");

var person = new Person
{
    FirstName = "Jan",
    LastName = "Kowalski",
    Age = 30,
    Secret = "ala ma kota i 0 pomysłow w głowie!"
};

    person.Id = await webApiClient.PostAsync<Person, int>("people", person);

Console.ReadLine();