using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SolarTest.Models;

namespace SolarTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BirthdayController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Birthday> Get()
        {
            return new List<Birthday>()
            {
                new Birthday
                {
                    BirthDate = new DateTime(2022,08,18),
                    FullName = "Natasha V.A.",
                    PhotoUrl = "http://vk.com/huisoi",
                    IsSoon = false,
                    IsOutDated = true,
                },
                new Birthday
                {
                    BirthDate = DateTime.Now,
                    FullName = "Kirill Sukhkasf'db s",
                    PhotoUrl = "http://1488228.com/hui.png",
                    IsSoon = true,
                    IsOutDated = false,
                }
            };
        }
    }
}