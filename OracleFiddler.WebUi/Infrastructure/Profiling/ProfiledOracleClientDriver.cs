using System.Data;
using NHibernate.Driver;
using StackExchange.Profiling;

namespace OracleFiddler.WebUi.Infrastructure.Profiling
{
    public class ProfiledOracleClientDriver : OracleClientDriver
    {
        public override IDbCommand CreateCommand()
        {
            IDbCommand command = base.CreateCommand();

            if (MiniProfiler.Current != null)
                command = DbCommandProxy.CreateProxy(command);

            return command;
        }
    }
}