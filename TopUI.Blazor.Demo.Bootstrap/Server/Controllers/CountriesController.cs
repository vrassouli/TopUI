using Microsoft.AspNetCore.Mvc;
using TopUI.Blazor.Demo.Bootstrap.Shared;

namespace TopUI.Blazor.Demo.Bootstrap.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{

    private readonly ILogger<CountriesController> _logger;

    public CountriesController(ILogger<CountriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public PagedList<CountryDto> Get(int skip, int take)
    {
        var countries = CountryDto.GetList();
        var total = countries.Count();
        var list = countries.Skip(skip).Take(take).ToList();

        var pagedList = new PagedList<CountryDto>(list, total);

        return pagedList;
    }
}