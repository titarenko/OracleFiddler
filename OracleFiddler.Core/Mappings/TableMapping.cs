using FluentNHibernate.Mapping;
using OracleFiddler.Core.Entities;

namespace OracleFiddler.Core.Mappings
{
    public class TableMapping : ClassMap<Table>
    {
        public TableMapping()
        {
            Table("ALL_TABLES");
            Id(x => x.Name, "TABLE_NAME");
            Map(x => x.Owner, "OWNER");
            Map(x => x.IsValid).Formula("CASE WHEN status = 'VALID' THEN 1 ELSE 0 END").CustomType<bool>();
            HasMany(x => x.Columns).KeyColumn("TABLE_NAME");
        }
    }
}