using System;
using System.Collections.Generic;
using System.Text;

namespace WordSimilarityLib
{
    public class WordStudyModel
    {
        public DbSqlite _db { get; set; }

        public WordStudyModel()
        {
            _db = null;
        }
        public WordStudyModel(string connString)
        {
            _db = new DbSqlite(connString);
        }

        public UserProfile GetUserProfile(string userKey)
        {
            return (_db.GetUserProfile(userKey));
        }

        public bool CreateUser(UserProfile user)
        {
            return _db.CreateUser(user);
        }

        public bool CreateDb()
        {
            return _db.CreateDb();
        }

        public string ProcessAdmin(string cmd)
        {
            return $"OK: now={DateTime.Now}, UTCNow={DateTime.UtcNow}";
        }
    }
}
