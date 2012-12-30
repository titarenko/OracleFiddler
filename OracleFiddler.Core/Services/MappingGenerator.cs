using System.Text;
using OracleFiddler.Core.Entities;
using System.Linq;

namespace OracleFiddler.Core.Services
{
    public class MappingGenerator : IGenerator
    {
        public string Generate(Table table)
        {
            var code = new StringBuilder();

            var className = table.Name.GuessCSharpSymbolName();

            code.AppendFormat("public class {0}Mapping : ClassMap<{0}>", className);
            code.AppendLine();
            code.AppendLine("{");

            code.AppendFormat("\tpublic {0}Mapping()", className);
            code.AppendLine();
            code.AppendLine("\t{");

            code.AppendFormat("\t\tTable(\"{0}\");", table.Name);
            code.AppendLine();

            foreach (var column in table.Columns)
            {
                code.AppendFormat(
                    "\t\t{2}(x => x.{0}).Column(\"{1}\");",
                    column.Name.GuessCSharpSymbolName(),
                    column.Name,
                    column.Constraints.Any(x => x.Type == ConstraintType.PrimaryKey) ? "Id" : "Map");
                code.AppendLine();
            }

            code.AppendLine("\t}");
            code.AppendLine("}");

            return code.ToString();
        }
    }
}