using OracleFiddler.Core.Entities;
using OracleFiddler.Core.Infrastructure;

namespace OracleFiddler.Core.Services
{
    public static class CodeGenerationExtensions
    {
        public static string GuessCSharpType(this TableColumn column)
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

        public static string GuessCSharpSymbolName(this string dbSymbolName)
        {
            return dbSymbolName.FromSnakeUpperToPascalCase();
        }
    }
}