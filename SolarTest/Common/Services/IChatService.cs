using System.Collections.Generic;
using Common.Models;

namespace Common.Services
{
    public interface IChatService
    {
        int? Create(Chat chat);
        List<Chat> GetAll();
    }
}