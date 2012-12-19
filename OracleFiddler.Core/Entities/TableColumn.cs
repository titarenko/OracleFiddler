using System.Collections.Generic;

namespace OracleFiddler.Core.Entities
{
    public class TableColumn
    {
        public virtual string Owner { get; set; }

        public virtual string TableName { get; set; }

        public virtual string Name { get; set; }

        public virtual string DataType { get; set; }

        public virtual int DataLength { get; set; }

        public virtual int DataPrecision { get; set; }

        public virtual int DataScale { get; set; }

        public virtual bool IsNullable { get; set; }

        public virtual IList<Constraint> Constraints { get; set; }

        protected bool Equals(TableColumn other)
        {
            return string.Equals(Owner, other.Owner) && string.Equals(TableName, other.TableName) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TableColumn) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Owner != null ? Owner.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TableName != null ? TableName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}