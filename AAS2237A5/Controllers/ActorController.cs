using AAS2237A5.Data;
using AAS2237A5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAS2237A5.Controllers
{
    public class ActorController : Controller
    {
        // Reference to the data manager
        private Manager m = new Manager();

        // GET: Actor
        public ActionResult Index()
        {
            return View(m.ActorGetAll());
        }

        // GET: Actor/Details/5
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var theActor = m.ActorGetByIdWithShowInfo(id.GetValueOrDefault());

            if (theActor == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(theActor);
            }
        }

        // GET: Actor/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            var model = new ActorAddViewModel();
            return View(model);
        }

        // POST: Actor/Create
        [HttpPost, Authorize(Roles = "Executive")]
        public ActionResult Create(ActorAddViewModel newItem)
        {
            
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ActorAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }

        // GET: Actors/5/AddShow
        // TODO: 17 - Used "attribute routing" for a custom URL segment (resource)
        [Route("Actors/{id}/AddShow"), Authorize(Roles = "Coordinator")]
        public ActionResult AddShow(int id)
        {
            // Attempt to get the associated object
            var actor = m.ActorGetByIdWithShowInfo(id);

            var genres = m.GenreGetAll();
            var preSelectedGenre = genres.ElementAt(0).Id;

            if (actor == null)
            {
                return HttpNotFound();
            }
            else
            {        
                // Create and configure a form object
                var formModel = new ShowAddFormViewModel();
                formModel.ActorId = actor.Id;
                formModel.ActorName = actor.Name;

                // Get all actors and preselect the known actor
                 var selectedValues = new List<int> { actor.Id };

                formModel.ActorList = new MultiSelectList
                    (items: m.ActorGetAll(),
                    dataValueField: "Id",
                    dataTextField: "Name",
                    selectedValues: selectedValues);

                // formModel.GenreId = preSelectedGenre;
                formModel.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name", selectedValue: preSelectedGenre);
                // IEnumerable<string> genres1 = m.GenreGetAll().Select(m => m.Name);
                // formModel.GenreList = new SelectList(genres1);

                return View(formModel);
            }
        }

        // POST: Actors/5/AddShow
        // TODO: 19 - Used "attribute routing" for a custom URL segment (resource)
        [Route("Actors/{id}/AddShow"), Authorize(Roles = "Coordinator")]
        [HttpPost]
        public ActionResult AddShow(ShowAddViewModel newItem)
        {   
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ShowAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // TODO: 20 - Must redirect to the Vehicles controller
                return RedirectToAction("details", "show", new { id = addedItem.Id });
            }
        }
    }
}
