using System.Text;
using Table = OracleFiddler.Core.Entities.Table;

namespace OracleFiddler.Core.Services
{
    public class EntityGenerator : IGenerator
    {
        public string Generate(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("public class " + table.Name.GuessCSharpSymbolName());
            code.AppendLine("{");

            foreach (var column in table.Columns)
            {
                code.AppendFormat(
                    "\tpublic virtual {0} {1} {{ get; set; }}",
                    column.GuessCSharpType(),
                    column.Name.GuessCSharpSymbolName());
                code.AppendLine();
            }

            code.AppendLine("}");

            return code.ToString();
        }
    }
}