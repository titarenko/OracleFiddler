using System.Collections.Generic;

namespace OracleFiddler.WebUi.Models.SchemaExplorer
{
    public class TabViewModel
    {
        public string Name { get; set; }

        public bool IsSelected { get; set; }

        public IList<TabViewModel> Items { get; set; } 
    }
}