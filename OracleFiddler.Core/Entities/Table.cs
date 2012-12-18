using System.Collections.Generic;

namespace OracleFiddler.Core.Entities
{
    public class Table
    {
        public virtual string Owner { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsValid { get; set; }

        public virtual IList<TableColumn> Columns { get; set; }
    }
}