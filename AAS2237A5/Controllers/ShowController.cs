using AAS2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAS2237A5.Controllers
{
    public class ShowController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: Show
        public ActionResult Index()
        {
            return View(m.ShowGetAll());
        }

        // GET: Show/Details/5
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var theShow = m.ShowGetByIdWithInfo(id.GetValueOrDefault());

            if (theShow == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(theShow);
            }
        }

        // GET: Episodes/5/AddEpisode
        // Used "attribute routing" for a custom URL segment (resource)
        [Route("Shows/{id}/AddEpisode"), Authorize(Roles = "Clerk")]
        public ActionResult AddEpisode(int id)
        {
            // Attempt to get the associated object
            var show = m.ShowGetByIdWithInfo(id);

            var genres = m.GenreGetAll();
            var preSelectedGenre = genres.ElementAt(0).Id;

            if (show == null)
            {
                return HttpNotFound();
            }
            else
            {
                // TODO: 18 - Add Episode for a known Show
                // We send the show identifier to the form
                // There, it is hidden... <input type=hidden
                // We also pass on the name, so that the browser user
                // knows which show they're working with

                // Create and configure a form object
                var formModel = new EpisodeAddFormViewModel();
                formModel.ShowId = show.Id;
                formModel.ShowName = show.Name;

                //formModel.GenreId = preSelectedGenre;
                formModel.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name", selectedValue: preSelectedGenre);
                //IEnumerable<string> genres1 = m.GenreGetAll().Select(m => m.Name);
                //formModel.GenreList = new SelectList(genres1);
                return View(formModel);
            }
        }

        // POST: Episodes/5/AddEpisode
        // Used "attribute routing" for a custom URL segment (resource)
        [Route("Shows/{id}/AddEpisode"), Authorize(Roles = "Clerk")]
        [HttpPost]
        public ActionResult AddEpisode(EpisodeAddViewModel newItem)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.EpisodeAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // TODO: 20 - Must redirect to the Vehicles controller
                return RedirectToAction("Details", "Episode", new { id = addedItem.Id });
            }
        }
    }
}
