using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BonTemps.Models;

namespace BonTemps.Controllers
{
    public class Table_layoutController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var tableList = _db.Table_layout.ToList();
            var tableLayout = tableList.Select(l => l.LayoutY).Distinct().Select(i => tableList.Where(t => t.LayoutY == i).Distinct().ToList()).ToList();

            ViewBag.tableLayout = tableLayout;
            return View();
        }

        public ActionResult Edit()
        {
            var tableList = _db.Table_layout.ToList();
            var tableLayout = tableList.Select(l => l.LayoutY).Distinct().Select(i => tableList.Where(t => t.LayoutY == i).Distinct().ToList()).ToList();

            ViewBag.tableLayout = tableLayout;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int x, int y, bool table)
        {
            if (x != 0 && y != 0)
            {
                var layoutObject = _db.Table_layout.FirstOrDefault(t => t.LayoutX == x && t.LayoutY == y);

                if (layoutObject != null)
                {
                    layoutObject.IsTable = table;

                    _db.Table_layout.Attach(layoutObject);
                    var entry = _db.Entry(layoutObject);
                    entry.Property(e => e.IsTable).IsModified = true;

                    _db.SaveChanges();

                    return Json("Succes");

                }
            }
            return Json("error, x and/or y are empty");
        }

    }
}
