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

        private readonly IBirthdayRepository _birthdayRepository;

        public BirthdayController(IBirthdayRepository birthdayRepository)
        {
            _birthdayRepository = birthdayRepository;
        }

        [HttpGet]
        public IEnumerable<Birthday> Get()
        {
            return new List<Birthday>()
            {
            };
        }

        [HttpGet]
        [Route("getBirthday/{id}")]
        public Birthday GetUser(int id)
        {
            return _birthdayRepository.Get(id);
        }

        [HttpPost]
        [Route("insertBirthday")]
        public int? Insert(Birthday birthday)
        {
            return _birthdayRepository.Create(birthday);
        }
    }
}