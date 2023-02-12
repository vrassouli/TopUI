﻿using System;
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
    public string? CountryCode { get; set; }
    public int? Population { get; set; }
    public string? CallingCode { get; set; }
    public string? Capital { get; set; }

    public CountryDto()
    {

    }

    public CountryDto(string name, string countryCode, int population, string callingCode, string capital)
    {
        Name = name;
        CountryCode = countryCode;
        Population = population;
        CallingCode = callingCode;
        Capital = capital;
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
                Population = new Random(rnd).Next(1, 1000000)
            };
        }

        //yield return new CountryDto("United States", "US", 333287557, "+1", "Washington, D.C");
        //yield return new CountryDto("United Kingdom", "UK", 67791400, "+44", "London");
        //yield return new CountryDto("France", "FR", 67897000, "+33", "Paris");
        //yield return new CountryDto("Germany", "DE", 83695430, "+49", "Berlin");
        //yield return new CountryDto("Italy", "IT", 58853482, "+39", "Rome");
        //yield return new CountryDto("Iran", "IR", 86758304, "+98", "Tehran");
    }
}
