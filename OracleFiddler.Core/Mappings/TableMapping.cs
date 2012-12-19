using FluentNHibernate.Mapping;
using OracleFiddler.Core.Entities;

namespace OracleFiddler.Core.Mappings
{
    public class TableMapping : ClassMap<Table>
    {
        public TableMapping()
        {
            Table("ALL_TABLES");
            CompositeId()
                .KeyProperty(x => x.Owner, "OWNER")
                .KeyProperty(x => x.Name, "TABLE_NAME");
            Map(x => x.IsValid).Formula("CASE WHEN status = 'VALID' THEN 1 ELSE 0 END").CustomType<bool>();
            HasMany(x => x.Columns).KeyColumns.Add("OWNER", "TABLE_NAME").AsSet();
        }
    }
}