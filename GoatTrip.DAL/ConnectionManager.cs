using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        private static SQLiteConnectionStringBuilder conStr = new SQLiteConnectionStringBuilder()
        {
            FullUri = "file::memory:?cache=shared", 
            JournalMode = SQLiteJournalModeEnum.Wal,
            Pooling = true,
            Version = 3

        };
        private string _dbFileLocation = @"C:\DASProjects\Development\LocationServiceTest\LocationCSVs\locations.db";
        private string _connectionString;
        private SQLiteConnection _diskDbConnection;
        private static SQLiteConnection inMemConnection = new SQLiteConnection(conStr.ToString());
        public ConnectionManager(string dbFileLocation)
        {
            _dbFileLocation = dbFileLocation;
            _connectionString = "data source=" + _dbFileLocation + "; Version=3;";
           _diskDbConnection = new SQLiteConnection(_connectionString);
        }

        public IDbConnection OpenInMemoryDbConnection()
        {
            var conn = GetSqLiteInMemoryDbConnection();
            conn.Open();
           
            return conn;
        }

        private SQLiteConnection GetSqLiteInMemoryDbConnection()
        {
            EnsureInMemConnectionInitilised();
            return new SQLiteConnection(inMemConnection.ConnectionString);
        }

        private void EnsureInMemConnectionInitilised()
        {
            if (inMemConnection.State != ConnectionState.Open)
            {
                _diskDbConnection.Open();
                inMemConnection.Open();
                CopyToInMemory();
                _diskDbConnection.Close();
            }
        }

        public IManagedDataReader GetReader(string statement, StatementParamaters statementParamaters)
        {
            
            SQLiteCommand command = new SQLiteCommand(statement, GetSqLiteInMemoryDbConnection());
       
            foreach (var parameter in statementParamaters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return new ManagedDataReader(command);
        }

        private void CopyToInMemory()
        {
            _diskDbConnection.BackupDatabase(inMemConnection, "main", "main", -1, null, 0);
        }

    }

    public class StatementParamaters : Dictionary<string, object>
    { }
}
