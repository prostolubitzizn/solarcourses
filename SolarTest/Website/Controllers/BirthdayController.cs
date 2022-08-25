using System.Collections.Generic;
using System.IO;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BirthdayController : ControllerBase
    {

        private readonly IBirthdayService _birthdayService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BirthdayController(IBirthdayService birthdayService, IWebHostEnvironment webHostEnvironment)
        {
            _birthdayService = birthdayService;
            _webHostEnvironment = webHostEnvironment;
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

            if (file == null)
            {
                return BadRequest();
            }

            string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "UploadFiles");

            string filePath = Path.Combine(dirPath, file.FileName);
            //process the form data
		
            return Ok("good");
        }
    }
}