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

        private List<UserProfile> userList;
        private List<Word> wordList;
        private List<Deck> deckList;
        private List<MemoryLogFibonacci> logList;

        public delegate void MapData(SqliteDataReader dr);

        public DbSqlite()
        {
            _connString = "";
            userList = new List<UserProfile>();
            wordList = new List<Word>();
            deckList = new List<Deck>();
            logList = new List<MemoryLogFibonacci>();
        }

        public DbSqlite(string conn) : this()
        {
            _connString = conn;
        }

        public int ExecuteNonQuery(string cmdString)
        {
            //SqliteConnectionStringBuilder cb = new SqliteConnectionStringBuilder();
            //cb.DataSource = @"aaa_sqlite.db";
            //cb.Mode = SqliteOpenMode.ReadWriteCreate;
            //_connString = cb.ConnectionString;
            int result;
            
            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                //if (!File.Exists(con.DataSource)) File.Create(con.DataSource);
                con.Open();

                SqliteCommand cmd = new SqliteCommand(cmdString, con);
                result = cmd.ExecuteNonQuery();

                con.Close();
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

            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                //if (!File.Exists(con.DataSource)) File.Create(con.DataSource);
                con.Open();

                SqliteCommand cmd = new SqliteCommand(cmdString, con);
                cmd.ExecuteScalar();
                result = cmd.ExecuteNonQuery();

                con.Close();
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
            string cmdString = $"SELECT * FROM users WHERE email='{userKey}'";
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
            string cmdString = $"SELECT rowid, * FROM decks WHERE userid='${userid}'";
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
            wordList.Add(data);
        }

        public List<Word> Getwords(int userid,int deckid)
        {
            string cmdString = $"SELECT rowid, * FROM decks WHERE userid='${userid}' AND deckid=${deckid}";
            wordList.Clear();
            int n = GetData(cmdString, MapWord);
//            if (n <= 0) return wordList;
            return wordList;
        }



        public bool CreateUser(UserProfile user)
        {
            string cmdString = "INSERT INTO users ( id ,  email , username , password , firstname, lastname, max_new_word ,deckid, create_time, last_login_time ) "
                            + $" VALUES ( 0, '{user.Email}', '{user.Username}', '{user.Password}', '{user.FirstName}', '{user.LastName}', '{user.MaxNewWord}', '{user.DeckId}', '{DateTime.UtcNow.ToString()}', '' ) ";
            ExecuteNonQuery(cmdString);
            return true;
        }

        public int CreateDeck(UserProfile user, int shared)
        {
            string cmdString = "INSERT INTO decks ( name,  ownerid ,   userid,  max_new_word, shared ) "
                            + $" VALUES ( 0, '{user.DeckName}', '{user.Id}', '{user.Id}','{user.MaxNewWord}', '{shared}' ) ";
            ExecuteNonQuery(cmdString);
            user.DeckId = GetLastRowId();
            return user.DeckId;
        }

        public bool CreateDeck(UserProfile user, List<Word> wordList, int shared)
        {
            if(user.DeckId<0)
            {
                user.DeckId = CreateDeck(user, shared);
            }

            string[] columns = new string[] { "userid", "deckid", "name", "frequency", "pronounciation", "similar_words", "meaning", "start_time", "study_time", "interval", "easiness" };

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                using (var transaction = con.BeginTransaction())
                {
                    var command = con.CreateCommand();
                    command.CommandText = @" INSERT INTO  words (" + string.Join(',', columns) + " )  VALUES ( ";
                    var paras = from c in columns select '$' + c;
                    command.CommandText += string.Join(',', paras);
                    foreach (var c in columns)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "$"+c;
                        if (c == "userid") parameter.Value = user.Id;
                        else if (c == "deckid") parameter.Value = user.DeckId;
                        command.Parameters.Add(parameter);
                    }

                    // Insert a lot of data
                    for (int i = 0; i < wordList.Count; i++)
                    {
                        for (int c = 0; c < command.Parameters.Count; c++)
                        {
                            var p = command.Parameters[c];
                            if (p.ParameterName == "$name") command.Parameters[c].Value = wordList[i].name;
                            else if (p.ParameterName == "$frequency") command.Parameters[c].Value = wordList[i].frequency;
                            else if (p.ParameterName == "$pronounciation") command.Parameters[c].Value = wordList[i].pronounciation;
                            else if (p.ParameterName == "$similar_words") command.Parameters[c].Value = wordList[i].similarWords;
                            else if (p.ParameterName == "$meaning") command.Parameters[c].Value = wordList[i].meaningShort;
                            else if (p.ParameterName == "$start_time") command.Parameters[c].Value = wordList[i].startTime;
                            else if (p.ParameterName == "$study_time") command.Parameters[c].Value = wordList[i].viewTime;
                            else if (p.ParameterName == "$interval") command.Parameters[c].Value = wordList[i].viewInterval;
                            else if (p.ParameterName == "$easiness") command.Parameters[c].Value = wordList[i].easiness;
                            else command.Parameters[c].Value = "";

                        }
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            sw.Stop();
            return true;
        }

        public bool CreateDb()
        {
            // create User tabl
            string cmdString = "CREATE TABLE IF NOT EXISTS users ( Id INTERGER PRIMARY KEY,  email TEXT, username TEXT, password TEXT, firstname TEXT, lastname TEXT, deckid INT, create_time TEXT, last_login_time TEXT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS decks ( name TEXT ,  ownerid INTERGER , userid INT, max_new_word INT, shared INT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS words (userid INT, deckid INT, name TEXT PRIMARY KEY,  frequency INTERGER ,   pronounciation TEXT , similar_words TEXT,  meaning TEXT, start_time TEXT, study_time TEXT, interval INT, easiness INT ) ";
            ExecuteNonQuery(cmdString);
            cmdString = "CREATE TABLE IF NOT EXISTS logs ( deck TEXT, name TEXT PRIMARY KEY,  study_time TEXT, interval INT, easiness INT ) WITHOUT ROWID;";
            ExecuteNonQuery(cmdString);
            return true;
        }

    }
}
