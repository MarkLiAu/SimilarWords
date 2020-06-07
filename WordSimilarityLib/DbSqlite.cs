using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;

namespace WordSimilarityLib
{
    public class DbSqlite
    {
        public string _connString { get; set; }

        public DbSqlite()
        {
            _connString = "";
        }

        public DbSqlite(string conn)
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


        public List<List<Object>> readWords(string cmdString)
        {
            List<List<Object>> result = new List<List<Object>>();

            using (SqliteConnection con = new SqliteConnection(_connString))
            {
                con.Open();

                using (SqliteCommand cmd = new SqliteCommand(cmdString, con))
                {
                    using (SqliteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            List<Object> record = new List<Object>();
                            foreach (var d in rdr)
                            {
                                if (d == DBNull.Value) record.Add(null);
                                else record.Add(d);
                                result.Add(record);
                            }
                        }
                    }
                }

                con.Close();
            }
            return result;
        }

    }
}
