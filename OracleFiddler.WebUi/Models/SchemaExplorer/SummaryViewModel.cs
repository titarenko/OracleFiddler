namespace OracleFiddler.WebUi.Models.SchemaExplorer
{
    public class SummaryViewModel
    {
        public int TablesCount { get; set; }

        public int AverageColumnsPerTable { get; set; }

        public int MaxColumnsPerTable { get; set; }
    }
}