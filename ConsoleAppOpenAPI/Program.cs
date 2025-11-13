using MyWebApi;

using var httpClient = new HttpClient();
contractClient client = new contractClient(httpClient);

var shoppingLists = await client.ShoppingListsAllAsync();

var people = await client.PeopleAllAsync("");

Console.ReadLine();
