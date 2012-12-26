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

        public ActionResult Schemas()
        {
            return View(session
                            .Query<Table>()
                            .OrderBy(x => x.Owner)
                            .Select(x => x.Owner)
                            .Distinct());
        }

        //public ActionResult Index()
        //{
        //    var tables = session
        //        .Query<Table>()
        //        .OrderBy(x => x.Owner)
        //        .ThenBy(x => x.Name)
        //        .FetchMany(x => x.Columns)
        //        .ThenFetch(x => x.Constraints)
        //        .ToList();

        //    var model = new IndexViewModel
        //    {
        //        Tables = tables.Select(x => new TableViewModel
        //        {
        //            Owner = x.Owner,
        //            Name = x.Name,
        //            Columns = x.Columns.Select(c => new TableColumnViewModel
        //            {
        //                Name = c.Name,
        //                DataType = c.DataType,
        //                IsNullable = c.IsNullable,
        //                Constraints = c.Constraints.Select(z => z.Name).ToList()
        //            }).ToList(),
        //            EntityCode = new EntityGenerator().Generate(x)
        //        }).ToList(),
        //        Summary = new SummaryViewModel
        //        {
        //            TablesCount = tables.Count,
        //            AverageColumnsPerTable = (int) Math.Round(tables.Average(x => x.Columns.Count)),
        //            MaxColumnsPerTable = tables.Max(x => x.Columns.Count)
        //        }
        //    };

        //    return View(model);
        //}

        public ActionResult Schema(string id)
        {
            return View(session
                            .Query<Table>()
                            .Where(x => x.Owner == id)
                            .OrderBy(x => x.Name)
                            .Select(x => new TableViewModel
                            {
                                Name = x.Name,
                                Owner = x.Owner
                            }));
        }

        public ActionResult Table(string id, string owner)
        {
            var table = session
                .Query<Table>()
                .Where(x => x.Name == id && x.Owner == owner)
                .FetchMany(x => x.Columns)
                .ThenFetch(x => x.Constraints)
                .ToList()
                .First();

            return View(new TableViewModel
            {
                Columns = table.Columns.Select(column => new TableColumnViewModel
                {
                    Name = column.Name,
                    DataType = column.DataType,
                    IsNullable = column.IsNullable,
                    Constraints = column.Constraints.Select(constraint => constraint.Name).ToList()
                }).ToList(),
                EntityCode = new EntityGenerator().Generate(table)
            });
        }
    }
}