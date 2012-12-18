﻿using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using OracleFiddler.Core.Entities;
using System.Linq;

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
            return View(session
                .Query<Table>()
                .Where(x => x.Owner == "UBSH")
                .OrderBy(x => x.Name)
                .Fetch(x => x.Columns));
        }
    }
}