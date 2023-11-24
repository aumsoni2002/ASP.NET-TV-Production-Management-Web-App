using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAS2237A5.Controllers
{
    public class EpisodeController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: Episode
        public ActionResult Index()
        {
            return View(m.EpisodeGetAll());
        }

        // GET: Episode/Details/5
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var theEpisode = m.EpisodeGetByIdWithShowName(id.GetValueOrDefault());

            if (theEpisode == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(theEpisode);
            }
        }
    }
}
