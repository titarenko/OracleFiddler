using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using OracleFiddler.Core.Entities;
using System.Linq;
using OracleFiddler.Core.Services;
using OracleFiddler.WebUi.Models.SchemaExplorer;

namespace OracleFiddler.WebUi.Controllers
{
    public class SchemaExplorerController : Controller
    {
        private readonly ISession session;

        public SchemaExplorerController(ISession session)
        {
            this.session = session;
        }

        public ActionResult Index(string schema, string table)
        {
            var tables = GetTablesProjections();
            var schemas = GetSchemas(tables);

            if (schemas.Any())
            {
                if (schema == null)
                {
                    schema = schemas.First().Key;
                }
                if (table == null)
                {
                    table = schemas[schema].First();
                }
            }

            var tableEntity = GetTable(schema, table);

            return View(new IndexViewModel
            {
                Summary = GetSummary(tables),
                SchemaSummary = GetSummary(tables.Where(x => x.Owner == schema).ToList()),
                Schemas = schemas.Select(x => GetSchemaViewModel(schema, table, x)).ToList(),
                Table = GetTableViewModel(tableEntity)
            });
        }

        private static TableViewModel GetTableViewModel(Table tableEntity)
        {
            return new TableViewModel
            {
                Name = tableEntity.Name,
                Owner = tableEntity.Owner,
                Columns = tableEntity.Columns.Select(c => new TableColumnViewModel
                {
                    Name = c.Name,
                    DataType = c.DataType,
                    IsNullable = c.IsNullable,
                    Constraints = c.Constraints.Select(co => co.Name).ToList()
                }).ToList(),
                EntityCode = new EntityGenerator().Generate(tableEntity),
                MappingCode = new MappingGenerator().Generate(tableEntity)
            };
        }

        private static TabViewModel GetSchemaViewModel(string schema, string table, KeyValuePair<string, List<string>> x)
        {
            return new TabViewModel
            {
                Name = x.Key,
                IsSelected =  x.Key == schema,
                Items = x.Value.Select(t => new TabViewModel
                {
                    Name = t,
                    IsSelected = t == table
                }).ToList()
            };
        }

        private Table GetTable(string schema, string table)
        {
            return session.Query<Table>()
                .Where(x => x.Name == table && x.Owner == schema)
                .FetchMany(x => x.Columns)
                .ThenFetch(x => x.Constraints)
                .ToList()
                .First();
        }

        private Dictionary<string, List<string>> GetSchemas(IEnumerable<TableProjection> tables)
        {
            return tables
                .GroupBy(x => x.Owner)
                .ToDictionary(x => x.Key, x => x.Select(z => z.Name).ToList());
        }

        private List<TableProjection> GetTablesProjections()
        {
            return session
                .Query<Table>()
                .Select(x => new TableProjection
                {
                    Owner = x.Owner,
                    Name = x.Name,
                    ColumnsCount = x.Columns.Count
                })
                .OrderBy(x => x.Owner)
                .ThenBy(x => x.Name)
                .ToList();
        }

        private static SummaryViewModel GetSummary(ICollection<TableProjection> tables)
        {
            return new SummaryViewModel
            {
                TablesCount = tables.Count,
                AverageColumnsPerTable = (int) Math.Round(tables.Average(x => x.ColumnsCount)),
                MaxColumnsPerTable = tables.Max(x => x.ColumnsCount)
            };
        }

        class TableProjection
        {
            public string Name { get; set; }

            public string Owner { get; set; }

            public int ColumnsCount { get; set; }
        }
    }
}