using System.Collections.Generic;

namespace OracleFiddler.WebUi.Models.SchemaExplorer
{
    public class TableViewModel
    {
        public string Owner { get; set; }

        public string Name { get; set; }

        public string EntityCode { get; set; }

        public IList<TableColumnViewModel> Columns { get; set; }
    }
}