﻿using System.Data;
using System.Data.Common;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace OracleFiddler.WebUi.Infrastructure.Profiling
{
    public class DbCommandProxy : RealProxy
    {
        private readonly DbCommand instance;
        private readonly IDbProfiler profiler;

        private DbCommandProxy(DbCommand instance)
            : base(typeof (DbCommand))
        {
            this.instance = instance;
            profiler = MiniProfiler.Current;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage methodMessage = new MethodCallMessageWrapper((IMethodCallMessage) msg);

            ExecuteType executeType = GetExecuteType(methodMessage);

            if (executeType != ExecuteType.None)
                profiler.ExecuteStart(instance, executeType);

            object returnValue = methodMessage.MethodBase.Invoke(instance, methodMessage.Args);

            if (executeType == ExecuteType.Reader)
                returnValue = new ProfiledDbDataReader((DbDataReader) returnValue, instance.Connection, profiler);

            IMessage returnMessage = new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount,
                                                       methodMessage.LogicalCallContext, methodMessage);

            if (executeType == ExecuteType.Reader)
                profiler.ExecuteFinish(instance, executeType, (DbDataReader) returnValue);
            else if (executeType != ExecuteType.None)
                profiler.ExecuteFinish(instance, executeType, null);

            return returnMessage;
        }

        private static ExecuteType GetExecuteType(IMethodCallMessage message)
        {
            switch (message.MethodName)
            {
                case "ExecuteNonQuery":
                    return ExecuteType.NonQuery;
                case "ExecuteReader":
                    return ExecuteType.Reader;
                case "ExecuteScalar":
                    return ExecuteType.Scalar;
                default:
                    return ExecuteType.None;
            }
        }

        public static IDbCommand CreateProxy(IDbCommand instance)
        {
            var proxy = new DbCommandProxy(instance as DbCommand);

            return proxy.GetTransparentProxy() as IDbCommand;
        }
    }
}