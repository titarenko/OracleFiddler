using Iesi.Collections.Generic;

namespace OracleFiddler.Core.Entities
{
    public class Table
    {
        public virtual string Owner { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsValid { get; set; }

        public virtual ISet<TableColumn> Columns { get; set; }

        protected bool Equals(Table other)
        {
            return string.Equals(Owner, other.Owner) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Table) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Owner != null ? Owner.GetHashCode() : 0)*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}