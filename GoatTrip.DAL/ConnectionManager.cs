using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class ConnectionManager: IConnectionManager
    {
        private static SQLiteConnectionStringBuilder conStr = new SQLiteConnectionStringBuilder()
        {
            FullUri = "file::memory:?cache=shared", 
            JournalMode = SQLiteJournalModeEnum.Off,
            Pooling = true,
            
            Version = 3

        };
        
        private string _dbFileLocation = @"C:\DASProjects\Development\LocationServiceTest\LocationCSVs\locations.db";
        private string _connectionString;
        private SQLiteConnection _diskDbConnection;
        private static SQLiteConnection inMemConnection = new SQLiteConnection(conStr.ToString() + ";Max Pool Size=100;");
        private bool _memConnectionInitialised = false;
        private bool _memConnectionInitialising = false;
        private bool _dbDiscConnectectionOnly = false;

        public ConnectionManager(string dbFileLocation, bool useDiscConnectionOnly)
        {
            _dbDiscConnectectionOnly = useDiscConnectionOnly;
            _dbFileLocation = dbFileLocation;
            _connectionString = "data source=" + _dbFileLocation + "; Version=3; Pooling=True; Max Pool Size=100;";
           _diskDbConnection = new SQLiteConnection(_connectionString);
        }

        public bool InMemoryDbInitialised
        {
            get { return _memConnectionInitialised; }
        }

        public IDbConnection OpenInMemoryDbConnection()
        {
            var conn = GetSqLiteInMemoryDbConnection();
            conn.Open();
           
            return conn;
        }

        private SQLiteConnection GetSqLiteInMemoryDbConnection()
        {

            if(!_dbDiscConnectectionOnly) EnsureInMemConnectionInitilised();
            if(_memConnectionInitialised)
                return new SQLiteConnection(inMemConnection.ConnectionString);
            return new SQLiteConnection(_diskDbConnection.ConnectionString);
        }

        private void EnsureInMemConnectionInitilised()
        {
            if (inMemConnection.State != ConnectionState.Open && !_memConnectionInitialising)
            {
                CopyToInMemory();   
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

        private async void CopyToInMemory()
        {
            _memConnectionInitialising = true;
            _diskDbConnection.Open();
            inMemConnection.Open();
            await Task.Run(() => _diskDbConnection.BackupDatabase(inMemConnection, "main", "main", -1, null, 0));
            _memConnectionInitialised = true;
            _diskDbConnection.Close();
            _memConnectionInitialising = false;
        }

    }

    public class StatementParamaters : Dictionary<string, object>
    { }
}
