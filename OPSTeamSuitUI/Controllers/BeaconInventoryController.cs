using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPSTeamSuitUI.Controllers
{
    public class BeaconInventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
