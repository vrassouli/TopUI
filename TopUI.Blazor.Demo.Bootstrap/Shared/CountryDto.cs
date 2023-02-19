using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Demo.Bootstrap.Shared;

public class CountryDto
{
    [Display(Name = "Country Name")]
    public string Name { get; set; } = default!;
    public List<CityDto>? Cities{ get; set; }
    public string? CountryCode { get; set; }
    public int? Population { get; set; }
    public string? CallingCode { get; set; }
    public string? Capital { get; set; }

    public CountryDto()
    {
        Cities = new List<CityDto>();
    }

    public static IEnumerable<CountryDto> GetList()
    {
        for (int i = 0; i < 1000; i++)
        {
            var rnd = new Random(i).Next(1, 10000);

            yield return new CountryDto
            {
                Name = $"Country {i}",
                CountryCode = i.ToString(),
                CallingCode = $"+{rnd}",
                Capital = $"Capital {rnd}",
                Population = new Random(rnd).Next(1, 1000000),
            };
        }

        //yield return new CountryDto("United States", "US", 333287557, "+1", "Washington, D.C");
        //yield return new CountryDto("United Kingdom", "UK", 67791400, "+44", "London");
        //yield return new CountryDto("France", "FR", 67897000, "+33", "Paris");
        //yield return new CountryDto("Germany", "DE", 83695430, "+49", "Berlin");
        //yield return new CountryDto("Italy", "IT", 58853482, "+39", "Rome");
        //yield return new CountryDto("Iran", "IR", 86758304, "+98", "Tehran");
    }

    public static IEnumerable<TreeViewItemDto> GetTreeViewItems()
    {
        for (int i = 0; i < 10; i++)
        {
            var rnd = new Random(i).Next(1, 10000);

            yield return new TreeViewItemDto
            {
                Name = $"Country {i}",
                Population = new Random(rnd).Next(1, 1000000),

                SubItems = GetCities(new Random(rnd).Next(5, 10)).Select(x => new TreeViewItemDto { Name = x.Name, Population = x.Population }).ToList()
            };
        }
    }

    private static IEnumerable<CityDto> GetCities(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new CityDto($"City {i}", new Random(count).Next(5, 10));
        }
    }
}
