using System.Collections.Generic;
using Common.Models;

namespace Common.Services
{
    public class ChatService : IChatService
    {
        private IChatRepository _repository;

        public ChatService(IChatRepository repository)
        {
            _repository = repository;
        }

        public int? Create(Chat chat)
        {
            return _repository.Create(chat);
        }

        public List<Chat> GetAll()
        {
            return _repository.GetAll();
        }
    }
}