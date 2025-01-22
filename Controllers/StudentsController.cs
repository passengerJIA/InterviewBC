using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InterviewBC.Model;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace InterviewBC.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{

    private readonly ILogger<StudentsController> _logger;
    private HttpClient sharedClient = new HttpClient()
    {
        BaseAddress = new Uri($"https://pokeapi.co/api/v2/pokemon/")
    };

    public StudentsController(ILogger<StudentsController> logger)
    {
        _logger = logger;
        _logger.LogDebug(1, "NLog injected!!!");
    }

    [HttpGet(Name = "GetStudents")]
    public async Task<IActionResult> Get()
    {
        Random rnd = new Random();
        var limit = rnd.Next(20,100);
        var offset = rnd.Next(0, 132);
        using HttpResponseMessage response = await sharedClient.GetAsync($"?limit={limit}&offset={offset}");
        var r = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        var pokemons = JsonSerializer.Deserialize<PokemonResult>(r).results;
        if(pokemons.Count >= limit)
        {
            var generatedStudents = pokemons.Select((p, i) => new Student { Name =  p.name, Id = i+1}).ToList();
            var reorderedStudents = new List<Student>();
            for(var i = 0; i < Math.Ceiling((decimal)generatedStudents.Count / 2); i++)
            {
                reorderedStudents.Add(generatedStudents[i]);
                reorderedStudents.Add(generatedStudents[generatedStudents.Count - 1 - i]);   
            }
            if (generatedStudents.Count % 2 != 0)
            {
                reorderedStudents.Remove(reorderedStudents[reorderedStudents.Count - 1]);
            }
            return Ok(new Tuple<List<Student>, List<Student>> ( generatedStudents, reorderedStudents));
        }
        else
        {
            throw new Exception("Not enough pokemon, please try again!");
            return null;
        }
    }
}
