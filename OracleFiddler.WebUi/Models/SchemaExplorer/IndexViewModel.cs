using System.Collections.Generic;

namespace OracleFiddler.WebUi.Models.SchemaExplorer
{
    public class IndexViewModel
    {
        public IList<TableViewModel> Tables { get; set; }

        public SummaryViewModel Summary { get; set; }
    }
}