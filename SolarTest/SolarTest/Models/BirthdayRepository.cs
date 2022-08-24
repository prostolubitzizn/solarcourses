using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Npgsql;

namespace SolarTest.Models
{
    public interface IBirthdayRepository
    {
        int? Create(Birthday birthday);
        void Delete(int id);
        Birthday Get(int id);
        List<Birthday> GetBirthdays();
        void Update(Birthday birthday);
    }

    public class BirthdayRepository : IBirthdayRepository
    {
        string connectionString = null;

        public BirthdayRepository(string conn)
        {
            connectionString = conn;
        }
        
        public List<Birthday> GetBirthdays()
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                return db.Query<Birthday>("SELECT * FROM \"Birthdays\"").ToList();
            }
        }


        public Birthday Get(int id)
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                return db.Query<Birthday>("SELECT * FROM " +"\"Birthdays\" WHERE \"Id\" = @id", new { id }).FirstOrDefault();
            }
        }

        public int? Create(Birthday birthday)
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO \"Birthdays\" (\"BirthDate\", \"PhotoUrl\", \"FullName\") VALUES(@BirthDate, @PhotoUrl,@FullName) RETURNING \"Id\"";
                int? userId = db.Query<int>(sqlQuery, birthday).FirstOrDefault();
                return userId;
            }
        }

        public void Update(Birthday birthday)
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE \"Birthdays\" SET \"BirthDate\" = @BirthDate, \"PhotoUrl\" = @PhotoUrl,\"FullName\" = @FullName  WHERE \"Id\" = @Id";
                db.Execute(sqlQuery, birthday);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new NpgsqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM \"Birthdays\" WHERE \"Id\" = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}