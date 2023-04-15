using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
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
    public PagedList<CountryDto> Get(int skip, int take, string? orderBy, string? orderDir)
    {
        var countries = CountryDto.GetList().AsQueryable();
        var total = countries.Count();

        if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(orderDir) && orderDir != "None")
        {
            countries = countries.OrderBy($"{orderBy} {orderDir.ToLower()}");
        }

        var list = countries.Skip(skip).Take(take).ToList();

        var pagedList = new PagedList<CountryDto>(list, total);

        return pagedList;
    }
}