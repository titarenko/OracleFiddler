using System.Collections.Generic;

namespace OracleFiddler.WebUi.Models.SchemaExplorer
{
    public class IndexViewModel
    {
        public SummaryViewModel Summary { get; set; }

        public IList<TabViewModel> Schemas { get; set; }

        public TableViewModel Table { get; set; }

        public SummaryViewModel SchemaSummary { get; set; }
    }
}