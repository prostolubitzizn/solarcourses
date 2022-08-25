using System.Collections.Generic;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SolarTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BirthdayController : ControllerBase
    {

        private readonly IBirthdayService _birthdayService;

        public BirthdayController(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }

        [HttpGet] 
        [Route("allBirthdays")]
        public IEnumerable<Birthday> GetAll()
        {
            return _birthdayService.GetBirthdays();
        }

        [HttpGet]
        [Route("todayBirthday")]
        public IEnumerable<Birthday> GetTodayBirthdays()
        {
            return _birthdayService.GetTodaysBirthdays();
        }

        [HttpGet]
        [Route("soonBirthday")]
        public IEnumerable<Birthday> GetSoonBirthdays()
        {
            return _birthdayService.GetSoonBirthdays();
        }

        [HttpGet]
        [Route("outdatedBirthday")]
        public IEnumerable<Birthday> GetOutdatedBirthdays()
        {
            return _birthdayService.GetOutDatedBirthdays();
        }
        

        [HttpGet]
        [Route("getBirthday/{id}")]
        public Birthday GetUser(int id)
        {
            return _birthdayService.Get(id);
        }

        [HttpPut]
        [Route("insertBirthday")]
        public int? Insert(Birthday birthday)
        {
            return _birthdayService.Create(birthday);
        }

        [HttpPut]
        [Route("updateBirthday")]
        public bool Update(Birthday birthday)
        {
            _birthdayService.Update(birthday);
            return true;
        }

        [HttpDelete]
        [Route("deleteBirthday")]
        public bool Delete(int id)
        {
            _birthdayService.Delete(id);
            return true;
        }
        
        [HttpPost]
        [Route("uploadImage")]
        public IActionResult Image(IFormFile file)
        {
            //process the form data
		
            return Ok("good");
        }
    }
}