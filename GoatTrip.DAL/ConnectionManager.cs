using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace GoatTrip.DAL
{
    public class ConnectionManager: IConnectionManager
    {
        private string _dbFileLocation = @"C:\DASProjects\Development\LocationServiceTest\LocationCSVs\";
        private string _connectionString;
        private SQLiteConnection _diskDbConnection;
        private static SQLiteConnection imMemConnection = new SQLiteConnection("FullUri=file::memory:?cache=shared");

        public ConnectionManager(string dbFileLocartion)
        {
            _dbFileLocation = dbFileLocartion;
            _connectionString = "data source=" + _dbFileLocation + "test.db?cache=shared; Version=3;";
           _diskDbConnection = new SQLiteConnection(_connectionString);
        }

        public  IDbConnection GetOpenInMemoryDbConnection()
        {
            if (imMemConnection.State != ConnectionState.Open)
            {
                _diskDbConnection.Open();
                imMemConnection.Open();
                CopyToInMemory();
                _diskDbConnection.Close();
            }
            return new SQLiteConnection("FullUri=file::memory:?cache=shared");
        }

        public IDataReader GetReader(string statement, StatementParamaters statementParamaters)
        {
            using (SQLiteCommand command = new SQLiteCommand(statement, new SQLiteConnection("FullUri=file::memory:?cache=shared")))
            {
                foreach (var parameter in statementParamaters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                   return reader; 
                }
            }
        }

        private void CopyToInMemory()
        {
            _diskDbConnection.BackupDatabase(imMemConnection, "main", "main", -1, null, 0);
        }

    }

    public class StatementParamaters : Dictionary<string, object>
    { }
}
