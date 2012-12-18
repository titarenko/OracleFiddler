using System.Text;
using OracleFiddler.Core.Entities;
using OracleFiddler.Core.Infrastructure;
using Table = OracleFiddler.Core.Entities.Table;

namespace OracleFiddler.Core.Services
{
    public class EntityGenerator
    {
        public string Generate(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("public class " + table.Name.FromSnakeUpperToPascalCase());
            code.AppendLine("{");

            foreach (var column in table.Columns)
            {
                code.AppendFormat(
                    "\tpublic virtual {0} {1} {{ get; set; }}",
                    GuessType(column),
                    column.Name.FromSnakeUpperToPascalCase());
                code.AppendLine();
            }

            code.AppendLine("}");

            return code.ToString();
        }

        private string GuessType(TableColumn column)
        {
            switch (column.DataType)
            {
                case "NUMBER":
                    return column.DataScale == 0
                               ? (column.IsNullable ? "int?" : "int")
                               : (column.IsNullable ? "decimal?" : "decimal");
                case "VARCHAR2":
                    return "string";
                case "CHAR":
                    return column.DataLength == 1
                               ? (column.IsNullable ? "bool?" : "bool")
                               : "string";
                case "DATE":
                    return column.IsNullable ? "DateTime?" : "DateTime";
                default:
                    return "string";
            }
        }
    }
}