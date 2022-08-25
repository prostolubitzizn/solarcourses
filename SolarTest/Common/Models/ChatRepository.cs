using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Npgsql;

namespace Common.Models
{

    public interface IChatRepository
    {
        int? Create(Chat chat);
        List<Chat> GetAll();
    }
    public class ChatRepository : IChatRepository
    {
        string connectionString = null;

        public ChatRepository(string conn)
        {
            connectionString = conn;
        }
        
        public int? Create(Chat chat)
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO \"Chat\" (\"ChatId\") VALUES(@ChatId) RETURNING \"ChatId\"";
                int? chatId = db.Query<int>(sqlQuery, chat).FirstOrDefault();
                return chatId;
            }
        }

        public List<Chat> GetAll()
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                return db.Query<Chat>("SELECT * FROM \"Chat\"").ToList();
            }
        }
    }
}