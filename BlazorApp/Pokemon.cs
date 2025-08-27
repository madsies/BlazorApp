using System;
using System.Threading.Tasks;

namespace BlazorApp;

public class Pokemon
{
    public int id;
    public string name = "";
    public Pokemon(int ID, string Name)
    {
        id = ID;
        name = Name;
    }

}


public class Pokedex
{
    public List<Pokemon> pokeList;

    public Pokedex()
    {
        pokeList = new List<Pokemon>();
        //var task = Task.Run(fetchPokemonInfo);
        //task.Wait();
        
    }

    // One time run and then cache data.
    /*
        Cant cache all info; ~210MB of data, impossible to use JSON file.
        Caching basic information for a thumbnail, when a user clicks on it, give more info.
    */
    private async Task fetchPokemonInfo()
    {
        List<String> jsonData = new List<string>();
        HttpClient request = new HttpClient();
        for (int i = 0; i < 152; i++)
        {
            var contents = await request.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}/");
            jsonData.Add(await contents.Content.ReadAsStringAsync());
        }

        System.IO.File.WriteAllText("pokemon_cache.json", System.Text.Json.JsonSerializer.Serialize(jsonData));

    }
}
