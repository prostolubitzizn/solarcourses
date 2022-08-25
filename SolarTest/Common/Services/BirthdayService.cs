using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Common.Services
{
    public class BirthdayService : IBirthdayService
    {
        private readonly IBirthdayRepository _birthdayRepository;

        public BirthdayService(IBirthdayRepository birthdayRepository)
        {
            _birthdayRepository = birthdayRepository;
        }

        public int? Create(Birthday birthday)
        {
            return _birthdayRepository.Create(birthday);
        }

        public void Delete(int id)
        {
            _birthdayRepository.Delete(id);
        }

        public Birthday Get(int id)
        {
            var bd = _birthdayRepository.Get(id);

            bd.BirthDateString = bd.BirthDate.ToString("MM/dd/yyyy");
            return bd;
        }

        public List<Birthday> GetBirthdays()
        {
            var bds = _birthdayRepository.GetBirthdays().OrderByDescending(x => x.BirthDate).ToList();

            foreach (var bd  in bds)
            {
                bd.BirthDateString = bd.BirthDate.ToString("MM/dd/yyyy");
            }
          
            return bds;
        }

        public void Update(Birthday birthday)
        {
            _birthdayRepository.Update(birthday);
        }

        public List<Birthday> GetTodaysBirthdays()
        {
            var birthdays = GetBirthdays();
            return birthdays.Where(bd => bd.BirthDate.Date == DateTime.Today.Date).ToList();
        }

        public List<Birthday> GetSoonBirthdays()
        {
            var birthdays = GetBirthdays();
            return birthdays.Where(bd => bd.BirthDate.Date > DateTime.Today.Date && bd.BirthDate.Date <= DateTime.Today.Date.AddDays(3)).ToList();
        }

        public List<Birthday> GetOutDatedBirthdays()
        {
            var birthdays = GetBirthdays();
            return birthdays.Where(bd => bd.BirthDate.Date < DateTime.Today.Date).ToList();
        }
    }
}