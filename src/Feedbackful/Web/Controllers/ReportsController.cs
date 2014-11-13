using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace Web.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            var chart = new DotNet.Highcharts.Highcharts("chart")
                .SetXAxis(new XAxis
                {
                    Categories = new[] { "Answer 1", "Answer 2", "Answer 3", "Answer 4" }
                })
                .SetSeries(new Series
                {
                    Data = new Data(new object[] { 25, 25, 25, 25 })
                });

            return View(chart);
        }

    }
}
