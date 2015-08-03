using System.Data;

namespace GoatTrip.DAL
{
    public interface IConnectionManager
    {
        IDbConnection OpenInMemoryDbConnection();

        IManagedDataReader GetReader(string statement, StatementParamaters paramsCollection);
    }
}
