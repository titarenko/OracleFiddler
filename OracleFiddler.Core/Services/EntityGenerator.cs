using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
using OracleFiddler.Core.Entities;
using OracleFiddler.Core.Infrastructure;
using Table = OracleFiddler.Core.Entities.Table;

namespace OracleFiddler.Core.Services
{
    public class EntityGenerator
    {
        public string Generate(Table table)
        {
            var unit = new CodeCompileUnit();
            var ns = new CodeNamespace("Entities");
            unit.Namespaces.Add(ns);
            ns.Types.Add(CreateClass(table));
            using (var stream = new StreamWriter(new MemoryStream()))
            {
                new CSharpCodeProvider().GenerateCodeFromCompileUnit(unit, stream, null);
                stream.Flush();
                stream.BaseStream.Position = 0;
                return new StreamReader(stream.BaseStream).ReadToEnd();
            }
        }

        private CodeTypeDeclaration CreateClass(Table table)
        {
            var declaration = new CodeTypeDeclaration(table.Name.FromSnakeUpperToPascalCase())
            {
                IsClass = true,
                Attributes = MemberAttributes.Public
            };
            declaration.Members.AddRange(
                table.Columns
                    .Select(x => new CodeSnippetTypeMember(string.Format(
                        "\t\tpublic virtual {0} {1} {{ get; set; }}",
                        GuessType(x),
                        x.Name.FromSnakeUpperToPascalCase())))
                    .Cast<CodeTypeMember>().ToArray());
            return declaration;
        }

        private Type GuessType(TableColumn column)
        {
            switch (column.DataType)
            {
                case "NUMBER":
                    return column.DataScale == 0
                               ? (column.IsNullable ? typeof (int?) : typeof (int))
                               : (column.IsNullable ? typeof (decimal?) : typeof(decimal));
                case "VARCHAR2":
                    return typeof (string);
                case "CHAR":
                    return column.DataLength == 1
                               ? (column.IsNullable ? typeof (bool?) : typeof (bool))
                               : typeof (string);
                case "DATE":
                    return column.IsNullable ? typeof (DateTime?) : typeof (DateTime);
                default:
                    return typeof (string);
            }
        }
    }
}