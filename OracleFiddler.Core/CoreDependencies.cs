using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Driver;
using OracleFiddler.Core.Entities;

namespace OracleFiddler.Core
{
    public class CoreDependencies<TDirver> : Module where TDirver : IDriver
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(
                    context => Fluently
                                   .Configure()
                                   .Database(OracleDataClientConfiguration
                                                 .Oracle10
                                                 .Driver<TDirver>()
                                                 .ConnectionString(x => x.FromConnectionStringWithKey("default")))
                                   .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Identified>())
                                   .BuildSessionFactory())
                .SingleInstance()
                .As<ISessionFactory>();
        }
    }
}