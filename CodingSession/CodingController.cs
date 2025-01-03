using System;
using System.Globalization;
using Dapper;
using Microsoft.Data.Sqlite;


namespace CodingSession
{
    internal class CodingController
    {
        private readonly Configuration _config;

        internal static List<CodingSessionModel> codingSessions = new();

        public CodingController()
        {
            _config = new Configuration();
        }

        internal void InsertQuery(DateTime start, DateTime end, double duration)
        {
            var codingSession = new CodingSessionModel(codingSessions.Count + 1, start, end, duration);

            using (var connection = new SqliteConnection(_config.connectionString))
            {
                var create_sql = "CREATE TABLE IF NOT EXISTS CodingSessions (Id INTEGER PRIMARY KEY AUTOINCREMENT, StartTime TEXT, EndTime TEXT, duration DOUBLE);";
                connection.Query(create_sql);

                var insert_sql = $"INSERT INTO CodingSessions (StartTime, EndTime, duration) VALUES ('{codingSession.StartTime.ToString("dd-MM-yyyy HH:mm:ss")}', '{codingSession.EndTime.ToString("dd-MM-yyyy HH:mm:ss")}', {codingSession.Duration});";
                connection.Query(insert_sql);
            }
        }

        internal bool DisplayQueryList()
        {
            using (var connection = new SqliteConnection(_config.connectionString))
            {
                var read_sql = "SELECT * FROM CodingSessions";
                var products = connection.Query(read_sql).ToList();
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                    var newCodeSession = new CodingSessionModel(
                        Convert.ToInt32(product.Id),
                        DateTime.ParseExact(product.StartTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-us")),
                        DateTime.ParseExact(product.EndTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-us")),
                        Convert.ToDouble(product.duration)
                    );

                    codingSessions.Add(newCodeSession);
                }
            }
            return true;
        }
    }
}
