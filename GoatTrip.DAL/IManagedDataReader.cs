using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public interface IManagedDataReader : IDisposable
    {
        IDataReader DataReader { get; }
        IDbConnection Connection { get; }

        bool Read();
    }
}
