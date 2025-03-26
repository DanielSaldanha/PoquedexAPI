using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using PoquedexAPI.Model;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
namespace PoquedexAPI.Controllers
{
    public class PokedexController : ControllerBase
    {
        private readonly IHttpClientFactory _IHttpClientFactory;
        public PokedexController(IHttpClientFactory IHttpClientFactory)
        {
            _IHttpClientFactory = IHttpClientFactory;
        }
        [HttpGet("pegue um pokemon aleatorio")]
        public async Task<IActionResult> P0k3m0n()
        {
            Random r = new Random();
            int Id = r.Next(1, 151);

            var client = _IHttpClientFactory.CreateClient("CurrencyAPI");
            var response = await client.GetAsync($"pokemon/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erro ao obter o Pokémon {Id}.");
            }

            //var pokemon = await response.Content.ReadFromJsonAsync<Pokemon
            var pokemonJson = await response.Content.ReadAsStringAsync();
            var pokemon = JsonSerializer.Deserialize<Pokemon>(pokemonJson);
            if (pokemon == null)
            {
                return NotFound("Pokémon não encontrado.");
            }
            return Ok(pokemon);
        }
        [HttpGet("pegue um pokemon via nome")]
        public async Task<IActionResult> P0k3m0nName(string name)
        {
        
            var client = _IHttpClientFactory.CreateClient("CurrencyAPI");
            var response = await client.GetAsync($"pokemon/{name}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erro ao obter o Pokémon {name}.");
            }

            //var pokemon = await response.Content.ReadFromJsonAsync<Pokemon
            var pokemon = await response.Content.ReadAsStringAsync();//var pokemonJson = await response.Content.ReadAsStringAsync();
            // var pokemon = JsonSerializer.Deserialize<Pokemon>(pokemonJson);
            // as dua modificaçoes acima sao necessarias caso o retorno seja um json(voce acesse o segundo get)
            if (pokemon == null)
            {
                return NotFound("Pokémon não encontrado.");
            }
            return Ok(pokemon);
        }
        [HttpGet("pegue um pokemon via id")]
        public async Task<IActionResult> P0k3m0nName(int Id)
        {

            var client = _IHttpClientFactory.CreateClient("CurrencyAPI");
            var response = await client.GetAsync($"pokemon/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Erro ao obter o Pokémon {Id}.");
            }

            //var pokemon = await response.Content.ReadFromJsonAsync<Pokemon
            var pokemonJson = await response.Content.ReadAsStringAsync();
            var pokemon = JsonSerializer.Deserialize<Pokemon>(pokemonJson);
            if (pokemon == null)
            {
                return NotFound("Pokémon não encontrado.");
            }
            return Ok(pokemon);
        }


        //
    }
}
