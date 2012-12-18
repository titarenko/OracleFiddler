using System;
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

        public ActionResult Index()
        {
            var tables = session
                .Query<Table>()
                .OrderBy(x => x.Owner)
                .ThenBy(x => x.Name)
                .Fetch(x => x.Columns)
                .ToList();

            var model = new IndexViewModel
            {
                Tables = tables.Select(x => new TableViewModel
                {
                    Owner = x.Owner,
                    Name = x.Name,
                    Columns = x.Columns.Select(c => new TableColumnViewModel
                    {
                        Name = c.Name,
                        DataType = c.DataType,
                        IsNullable = c.IsNullable
                    }).ToList(),
                    EntityCode = new EntityGenerator().Generate(x)
                }).ToList(),
                Summary = new SummaryViewModel
                {
                    TablesCount = tables.Count,
                    AverageColumnsPerTable = (int) Math.Round(tables.Average(x => x.Columns.Count)),
                    MaxColumnsPerTable = tables.Max(x => x.Columns.Count)
                }
            };

            return View(model);
        }
    }
}