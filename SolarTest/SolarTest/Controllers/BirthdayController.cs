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
        [Route("allBirthdays")]
        public IEnumerable<Birthday> GetAll()
        {
            return _birthdayRepository.GetBirthdays();
        }

        [HttpGet]
        [Route("todayBirthday")]
        public IEnumerable<Birthday> GetTodayBirthdays()
        {
            var birthdays = _birthdayRepository.GetBirthdays();
            return birthdays.Where(bd => bd.BirthDate.Date == DateTime.Today.Date);
        }

        [HttpGet]
        [Route("soonBirthday")]
        public IEnumerable<Birthday> GetSoonBirthdays()
        {
            var birthdays = _birthdayRepository.GetBirthdays();
            return birthdays.Where(bd => bd.BirthDate.Date > DateTime.Today.Date && bd.BirthDate.Date <= DateTime.Today.Date.AddDays(3));
        }

        [HttpGet]
        [Route("getBirthday/{id}")]
        public Birthday GetUser(int id)
        {
            return _birthdayRepository.Get(id);
        }

        [HttpPut]
        [Route("insertBirthday")]
        public int? Insert(Birthday birthday)
        {
            return _birthdayRepository.Create(birthday);
        }

        [HttpPut]
        [Route("updateBirthday")]
        public bool Update(Birthday birthday)
        {
            _birthdayRepository.Update(birthday);
            return true;
        }

        [HttpDelete]
        [Route("deleteBirthday")]
        public bool Delete(int id)
        {
            _birthdayRepository.Delete(id);
            return true;
        }
    }
}