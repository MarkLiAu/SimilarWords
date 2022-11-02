using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;

namespace WordSimilarityLib
{
    public class DbSqlite
    {
        public string _connString { get; set; }

        private SqliteConnection _conn, _connOnce;

        private List<UserProfile> userList;
        private List<Word> wordList;
        private List<Deck> deckList;
        private List<MemoryLogFibonacci> logList;

        public delegate void MapData(SqliteDataReader dr);

        public DbSqlite()
        {
            _connString = "";
            _conn =_connOnce= null;
            userList = new List<UserProfile>();
            wordList = new List<Word>();
            deckList = new List<Deck>();
            logList = new List<MemoryLogFibonacci>();
        }

        public DbSqlite(string conn) : this()
        {
            _connString = conn;
        }

        public void Open(string connectString=null)
        {
            if (string.IsNullOrWhiteSpace(connectString)) connectString = _connString;
            _conn = new SqliteConnection(_connString);
            _conn.Open();
        }

        public void Close()
        {
            if (_conn != null) _conn.Close();
            _conn = null;
        }

        public int ExecuteNonQuery(string cmdString)
        {
            //SqliteConnectionStringBuilder cb = new SqliteConnectionStringBuilder();
            //cb.DataSource = @"aaa_sqlite.db";
            //cb.Mode = SqliteOpenMode.ReadWriteCreate;
            //_connString = cb.ConnectionString;
            int result;
            SqliteConnection conn = _conn;
            try
            {
                if (conn == null)
                {
                    conn = new SqliteConnection(_connString);
                    conn.Open();
                }

                SqliteCommand cmd = new SqliteCommand(cmdString, conn);
                result = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn == null) conn.Close();
            }
            return result;
        }

        public object ExecuteScalar(string cmdString)
        {
            //SqliteConnectionStringBuilder cb = new SqliteConnectionStringBuilder();
            //cb.DataSource = @"aaa_sqlite.db";
            //cb.Mode = SqliteOpenMode.ReadWriteCreate;
            //_connString = cb.ConnectionString;
            object result;


            SqliteConnection conn = _conn;
            try
            {
                if (conn == null)
                {
                    conn = new SqliteConnection(_connString);
                    conn.Open();
                }

                SqliteCommand cmd = new SqliteCommand(cmdString, conn);
                result = cmd.ExecuteScalar();
            }
            finally
            {
                if (_conn == null) conn.Close();
            }

            return result;
        }

        public int GetLastRowId()
        {
            // The row ID is a 64-bit value - cast the Command result to an Int64.
            object obj = ExecuteScalar("select last_insert_rowid()");
            return Convert.ToInt32(obj);
        }

        public int GetData(string cmdString, MapData map)
        {
            List<List<Object>> result = new List<List<Object>>();

            int n = 0;
            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                con.Open();

                using (SqliteCommand cmd = new SqliteCommand(cmdString, con))
                {
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            map(dr);
                            n++;
                        }
                    }
                }

                con.Close();
            }
            return n;
        }

        public List<List<object>> GetData(string cmdString)
        {
            List<List<object>> result = new List<List<object>>();

            int n = 0;
            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                con.Open();

                using (SqliteCommand cmd = new SqliteCommand(cmdString, con))
                {
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int count = dr.FieldCount;
                            List<object> data = new List<object>();
                            for (int i=0;i<count;i++)
                            {
                                data.Add(dr[i]);
                            }
                            result.Add(data);
                            n++;
                        }
                    }
                }

                con.Close();
            }
            return result;
        }



        public void MapUser(SqliteDataReader dr)
        {
            UserProfile user = new UserProfile();
            int idx = 0;
            user.Id = Convert.ToInt32(dr[idx++]);
            user.Email = Convert.ToString(dr[idx++]);
            user.Username = Convert.ToString(dr[idx++]);
            user.Password = Convert.ToString(dr[idx++]);
            user.FirstName = Convert.ToString(dr[idx++]);
            user.LastName = Convert.ToString(dr[idx++]);
            user.MaxNewWord = Convert.ToInt32(dr[idx++]);
            user.DeckId = Convert.ToInt32(dr[idx++]);
            userList.Add(user);
        }

        public UserProfile GetUserProfile(string userKey)
        {
            string cmdString = $"SELECT * FROM users WHERE email=\"{userKey}\"";
            userList.Clear();
            int n = GetData(cmdString, MapUser);
            if (n<=0) return null;
            return userList[0];
        }


        public void MapDeck(SqliteDataReader dr)
        {
            Deck data = new Deck();
            int idx = 0;
            data.Id = Convert.ToInt32(dr[idx++]);
            data.Name = Convert.ToString(dr[idx++]);
            data.Owner = Convert.ToString(dr[idx++]);
            data.User = Convert.ToString(dr[idx++]);
            data.MaxNewWord = Convert.ToInt32(dr[idx++]);
            deckList.Add(data);
        }

        public List<Deck> GetUserDecks(int userid)
        {
            string cmdString = $"SELECT * FROM decks WHERE userid='${userid}'";
            deckList.Clear();
            int n = GetData(cmdString, MapDeck);
  //          if (n <= 0) return null;
            return deckList;
        }

        //cmdString = "CREATE TABLE IF NOT EXISTS words ( deckid INT, TEXT, name TEXT PRIMARY KEY,  frequency INTERGER ,   pronounciation TEXT 
        // , similarWords TEXT,  meaning TEXT, startTime TEXT, studyTime TEXT, interval INT, easiness INT ) ";

        public void MapWord(SqliteDataReader dr)
        {
            Word data = new Word();
            int idx = 0;
            data.id = Convert.ToInt32(dr[idx++]);
            idx++;  // userid
            idx++;  // deckid
            data.name = Convert.ToString(dr[idx++]);
            data.frequency = Convert.ToInt32(dr[idx++]);
            data.pronounciation = Convert.ToString(dr[idx++]);
            data.similarWords = Convert.ToString(dr[idx++]);
            data.meaningShort = Convert.ToString(dr[idx++]);
            data.startTime = Convert.ToDateTime(dr[idx++]);
            data.viewTime = Convert.ToDateTime(dr[idx++]);
            data.viewInterval = Convert.ToInt32(dr[idx++]);
            data.easiness = Convert.ToInt32(dr[idx++]);
            data.totalViewed = Convert.ToInt32(dr[idx++]);
            wordList.Add(data);
        }

        public List<Word> Getwords(int userid,int deckid, string name=null)
        {
            if (deckid <= 0) deckid = 1;        // test
            string cmdString = $"SELECT * FROM words WHERE userid={userid} AND deckid={deckid}";
            if (!string.IsNullOrWhiteSpace(name))
                cmdString += $" AND name='{name}' ";
            wordList.Clear();
            int n = GetData(cmdString, MapWord);
//            if (n <= 0) return wordList;
            return wordList;
        }



        public bool CreateUser(UserProfile user)
        {
            string cmdString = "INSERT INTO users ( email , username , password , firstname, lastname, max_new_word ,deckid, create_time, last_login_time ) "
                            + $" VALUES ( '{user.Email}', '{user.Username}', '{user.Password}', '{user.FirstName}', '{user.LastName}', '{user.MaxNewWord}', '{user.DeckId}', '{DateTime.UtcNow.ToString("o")}', '' ) ";
            ExecuteNonQuery(cmdString);
            user.Id = GetLastRowId();
            return true;
        }

        public int CreateDeck(UserProfile user, int shared)
        {
            string cmdString = "INSERT INTO decks ( name,  ownerid ,   userid,  max_new_word, shared ) "
                            + $" VALUES ( '{user.DeckName}', '{user.Id}', '{user.Id}','{user.MaxNewWord}', '{shared}' ) ";
            ExecuteNonQuery(cmdString);
            user.DeckId = GetLastRowId();
            return user.DeckId;
        }

        public Word InsertWord(int userid, int deckid, Word word)
        {
            string cmdString = "INSERT INTO words (userid, deckid, name, frequency, pronounciation, similar_words, meaning, start_time, study_time, interval, easiness,total_viewed) "
                            + $" VALUES ({userid},{deckid},'{word.name.Replace("'","''")}','{word.frequency}','{word.pronounciation.Replace("'", "''")}','{word.similarWords.Replace("'", "''")}','{word.meaningShort.Replace("'", "''")}','{word.startTime.ToString("o")}','{word.viewTime.ToString("o")}',{word.viewInterval},{word.easiness},{word.totalViewed}  ) ";
            int rc=ExecuteNonQuery(cmdString);
            word.id = GetLastRowId();
            return word;
        }

        public Word UpdateWord(int userid, int deckid, Word word)
        {
            if (word.id <= 0) return InsertWord(userid, deckid, word);

            string cmdString = $"UPDATE words SET name='{word.name.Replace("'", "''")}', frequency={word.frequency}, pronounciation='{word.pronounciation.Replace("'", "''")}', similar_words='{word.similarWords.Replace("'", "''")}', meaning='{word.meaningShort.Replace("'", "''")}', start_time='{word.startTime.ToString("o")}', study_time='{word.viewTime.ToString("o")}', interval={word.viewInterval}, easiness={word.easiness},total_viewed={word.totalViewed}  "
                            + $" WHERE id= {word.id} AND userid = {userid} AND deckid={deckid} ";
            int rc=ExecuteNonQuery(cmdString);
            return word;
        }

        public bool CreateDeck(UserProfile user, Dictionary<string, Word> wordList, int shared)
        {
            if(user.DeckId<0)
            {
                user.DeckId = CreateDeck(user, shared);
            }

            string[] columns = new string[] { "userid", "deckid", "name", "frequency", "pronounciation", "similar_words", "meaning", "start_time", "study_time", "interval", "easiness","total_viewed" };

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            SqliteConnection con = _conn;
            {
                using (var transaction = con.BeginTransaction())
                {
                    var command = con.CreateCommand();
                    command.CommandText = @" INSERT INTO  words (" + string.Join(',', columns) + " )  VALUES ( ";
                    var paras = from c in columns select '$' + c;
                    command.CommandText += string.Join(',', paras)+" )";
                    foreach (var c in columns)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "$"+c;
                        if (c == "userid") parameter.Value = user.Id;
                        else if (c == "deckid") parameter.Value = user.DeckId;
                        command.Parameters.Add(parameter);
                    }

                    // Insert a lot of data
                    foreach(var d in wordList)
                    {
                        for (int c = 0; c < command.Parameters.Count; c++)
                        {
                            var p = command.Parameters[c];
                            if (p.ParameterName == "$userid") continue;
                            else if (p.ParameterName == "$deckid") continue;
                            else if (p.ParameterName == "$name") command.Parameters[c].Value = d.Value.name;
                            else if (p.ParameterName == "$frequency") command.Parameters[c].Value = d.Value.frequency;
                            else if (p.ParameterName == "$pronounciation") command.Parameters[c].Value = d.Value.pronounciation;
                            else if (p.ParameterName == "$similar_words") command.Parameters[c].Value = d.Value.similarWords;
                            else if (p.ParameterName == "$meaning") command.Parameters[c].Value = d.Value.meaningShort;
                            else if (p.ParameterName == "$start_time") command.Parameters[c].Value = DateTime.MinValue.ToString("o");
                            else if (p.ParameterName == "$study_time") command.Parameters[c].Value = DateTime.MinValue.ToString("o");
                            else if (p.ParameterName == "$interval") command.Parameters[c].Value = int.MinValue;
                            else if (p.ParameterName == "$easiness") command.Parameters[c].Value = int.MinValue;
                            else if (p.ParameterName == "$total_viewed") command.Parameters[c].Value = int.MinValue;
                            else command.Parameters[c].Value = "";

                        }
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            sw.Stop();
            CommTools.Logger.Log(2, $" load into sqlite, count={wordList.Count}, time(s):{sw.ElapsedMilliseconds / 1000}");
            return true;
        }

        public bool CreateDb()
        {
            // create User tabl
            string cmdString = "CREATE TABLE IF NOT EXISTS users ( id INTEGER PRIMARY KEY,  email TEXT, username TEXT, password TEXT, firstname TEXT, lastname TEXT,max_new_word INT, deckid INT, create_time TEXT, last_login_time TEXT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS decks ( id INTEGER PRIMARY KEY, name TEXT ,  ownerid INT , userid INT, max_new_word INT, shared INT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS words ( id INTEGER PRIMARY KEY, userid INT, deckid INT, name TEXT,  frequency INT ,   pronounciation TEXT , similar_words TEXT,  meaning TEXT, start_time TEXT, study_time TEXT, interval INT, easiness INT,total_viewed INT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS logs (id INTEGER PRIMARY KEY, userid INT, deckid INT, name TEXT,  study_time TEXT, interval INT, easiness INT ) ";
            ExecuteNonQuery(cmdString);

            return true;
        }

    }
}
