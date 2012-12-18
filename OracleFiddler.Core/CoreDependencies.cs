using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Driver;
using OracleFiddler.Core.Entities;

namespace OracleFiddler.Core
{
    public class CoreDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(
                    context => Fluently
                                   .Configure()
                                   .Database(OracleDataClientConfiguration
                                                 .Oracle10
                                                 .Driver<OracleClientDriver>()
                                                 .ConnectionString(x => x.FromConnectionStringWithKey("default")))
                                   .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Identified>())
                                   .BuildSessionFactory())
                .SingleInstance()
                .As<ISessionFactory>();
        }
    }
}