using System.Collections.Generic;
using Common.Models;

namespace Common.Services
{
    public interface IBirthdayService
    {
        int? Create(Birthday birthday);
        void Delete(int id);
        Birthday Get(int id);
        List<Birthday> GetBirthdays();
        void Update(Birthday birthday);

        List<Birthday> GetTodaysBirthdays();
        List<Birthday> GetSoonBirthdays();
        List<Birthday> GetOutDatedBirthdays();
    }
}