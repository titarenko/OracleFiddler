using System.Collections.Generic;

namespace OracleFiddler.WebUi.Models.SchemaExplorer
{
    public class TableColumnViewModel
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNullable { get; set; }

        public IList<string> Constraints { get; set; }
    }
}