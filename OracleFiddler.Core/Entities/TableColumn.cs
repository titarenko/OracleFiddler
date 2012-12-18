namespace OracleFiddler.Core.Entities
{
    public class TableColumn
    {
        public virtual string Name { get; set; }

        public virtual string DataType { get; set; }

        public virtual int DataLength { get; set; }

        public virtual int DataPrecision { get; set; }

        public virtual int DataScale { get; set; }

        public virtual bool IsNullable { get; set; }
    }
}