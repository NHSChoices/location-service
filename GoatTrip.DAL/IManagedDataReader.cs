using System;
using System.Data;

namespace GoatTrip.DAL
{
    public interface IManagedDataReader : IDisposable
    {
        IDataReader DataReader { get; }
        IDbConnection Connection { get; }

        bool Read();
    }
}
