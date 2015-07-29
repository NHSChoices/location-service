using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public interface IConnectionManager
    {
        IDbConnection OpenInMemoryDbConnection();

        IManagedDataReader GetReader(string statement, StatementParamaters paramsCollection);
    }
}
