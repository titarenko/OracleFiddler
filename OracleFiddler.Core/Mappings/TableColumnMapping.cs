using FluentNHibernate.Mapping;
using OracleFiddler.Core.Entities;

namespace OracleFiddler.Core.Mappings
{
    public class TableColumnMapping : ClassMap<TableColumn>
    {
        public TableColumnMapping()
        {
            Table("ALL_TAB_COLUMNS");
            CompositeId()
                .KeyProperty(x => x.Owner, "OWNER")
                .KeyProperty(x => x.TableName, "TABLE_NAME")
                .KeyProperty(x => x.Name, "COLUMN_NAME");
            Map(x => x.DataType, "DATA_TYPE");
            Map(x => x.DataLength, "DATA_LENGTH");
            Map(x => x.DataPrecision, "DATA_PRECISION");
            Map(x => x.DataScale, "DATA_SCALE");
            Map(x => x.IsNullable).Formula("CASE WHEN nullable = 'Y' THEN 1 ELSE 0 END").CustomType<bool>();
            HasManyToMany(x => x.Constraints)
                .Table("ALL_CONS_COLUMNS")
                .ParentKeyColumns.Add("OWNER", "TABLE_NAME", "COLUMN_NAME")
                .ChildKeyColumn("CONSTRAINT_NAME");
        }
    }
}