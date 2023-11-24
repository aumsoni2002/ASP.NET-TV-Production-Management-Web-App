using AAS2237A5.Data;
using AAS2237A5.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

// ************************************************************************************
// WEB524 Project Template V2 == 2237-0cb84679-1cb9-400b-b700-ebcc80d36e30
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

namespace AAS2237A5.Controllers
{
    public class Manager
    {

        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Product, ProductBaseViewModel>();

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();

                // Genre
                cfg.CreateMap<Genre, GenreBaseViewModel>();

                // Actor
                cfg.CreateMap<Actor, ActorBaseViewModel>();
                cfg.CreateMap<Actor, ActorAddViewModel>();
                cfg.CreateMap<Actor, ActorWithShowInfoViewModel>();
                cfg.CreateMap<ActorAddViewModel, Actor>();

                // Show
                cfg.CreateMap<Show, ShowBaseViewModel>();
                cfg.CreateMap<ShowAddViewModel, Show>();
                cfg.CreateMap<Show, ShowWithInfoViewModel>();              

                // Episode
                cfg.CreateMap<Episode, EpisodeBaseViewModel>();
                cfg.CreateMap<Episode, EpisodeWithShowNameViewModel>();
                cfg.CreateMap<EpisodeAddViewModel, Episode>();

            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }


        // Add your methods below and call them from controllers. Ensure that your methods accept
        // and deliver ONLY view model objects and collections. When working with collections, the
        // return type is almost always IEnumerable<T>.
        //
        // Remember to use the suggested naming convention, for example:
        // ProductGetAll(), ProductGetById(), ProductAdd(), ProductEdit(), and ProductDelete().

        // Genre
        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            var allGenres = ds.Genres.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(allGenres);
        }

        // Actor
        public IEnumerable<ActorBaseViewModel> ActorGetAll()
        {
            var allActors = ds.Actors.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Actor>, IEnumerable<ActorBaseViewModel>>(allActors);
        }

        public ActorWithShowInfoViewModel ActorAdd(ActorAddViewModel newItem)
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;
            if (user == null)
            {
                return null;
            }
            else 
            {
                var addedItem = ds.Actors.Add(mapper.Map<ActorAddViewModel, Actor>(newItem));
                addedItem.Executive = user;
                ds.SaveChanges();
                return (addedItem == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(addedItem);
            }
        }

        public ActorWithShowInfoViewModel ActorGetByIdWithShowInfo(int id)
        {
            // Attempt to fetch the object
            var theActor = ds.Actors.Include("Shows").SingleOrDefault(a => a.Id == id);

            // Return the result, or null if not found
            return (theActor == null) ? null : mapper.Map<Actor, ActorWithShowInfoViewModel>(theActor);
        }

        // Show
        public IEnumerable<ShowBaseViewModel> ShowGetAll()
        {
            var allShows = ds.Shows.OrderBy(a => a.Name);
            return mapper.Map<IEnumerable<Show>, IEnumerable<ShowBaseViewModel>>(allShows);
        }

        public ShowWithInfoViewModel ShowAdd(ShowAddViewModel newItem) 
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;        
            if (user == null)
            {
                return null;
            }
            else
            {
                
                var addedItem = ds.Shows.Add(mapper.Map<ShowAddViewModel, Show>(newItem));

                addedItem.Coordinator = user;
                // Create a collection to store actors for the current show
                var showActors = new List<Actor>();

                foreach (var actorId in newItem.ActorIds)
                {
                    // Find the actor by ID
                    var actor = ds.Actors.Find(actorId);

                    // Check if the actor was found
                    if (actor != null)
                    {
                        // Add the actor to the show's collection
                        showActors.Add(actor);
                    }
                }

                // Assign the collection of actors to the show
                addedItem.Actors = showActors;

                ds.SaveChanges();

                return (addedItem == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(addedItem);
            }

        }

        public ShowWithInfoViewModel ShowGetByIdWithInfo(int id)
        {
            // Attempt to fetch the object
            var theShow = ds.Shows
                .Include("Actors")
                .Include("Episodes")
                .SingleOrDefault(a => a.Id == id);

            // Return the result, or null if not found
            return (theShow == null) ? null : mapper.Map<Show, ShowWithInfoViewModel>(theShow);
        }

        // Episode
        public IEnumerable<EpisodeBaseViewModel> EpisodeGetAll()
        {
            var allEpisodes = ds.Episodes
                                .Include("Show")
                                .OrderBy(a => a.Name)
                                .ThenBy(a => a.SeasonNumber)
                                .ThenBy(a => a.EpisodeNumber);

            return mapper.Map<IEnumerable<Episode>, IEnumerable<EpisodeBaseViewModel>>(allEpisodes);
        }

        public EpisodeWithShowNameViewModel EpisodeAdd(EpisodeAddViewModel newItem)
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;
            //var genre = ds.Genres.Find(newItem.GenreId);
            var show = ds.Shows.Find(newItem.ShowId);
            if (user == null || show == null)
            {
                return null;
            }
            else
            {
                var addedItem = ds.Episodes.Add(mapper.Map<EpisodeAddViewModel, Episode>(newItem));
                // addedItem.Genre = genre.Name;
                addedItem.Clerk = user;
                addedItem.Show = show;
                ds.SaveChanges();
                return (addedItem == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(addedItem);
            }
        }

        public EpisodeWithShowNameViewModel EpisodeGetByIdWithShowName(int id)
        {
            // Attempt to fetch the object
            var theEpisode = ds.Episodes
                .Include("Show")
                .SingleOrDefault(a => a.Id == id);

            // Return the result, or null if not found
            return (theEpisode == null) ? null : mapper.Map<Episode, EpisodeWithShowNameViewModel>(theEpisode);
        }

        // *** Add your methods ABOVE this line **

        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        // Add some programmatically-generated objects to the data store
        // You can write one method or many methods but remember to
        // check for existing data first.  You will call this/these method(s)
        // from a controller action.

        // Loading Roles
        public bool LoadRoles()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.RoleClaims.Count() == 0)
            {
                // Add role claims here
                ds.RoleClaims.Add(new RoleClaim() { Name = "Admin" });
                ds.RoleClaims.Add(new RoleClaim() { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim() { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim() { Name = "Clerk" });

                ds.SaveChanges();
                done = true;
            }

            // You may load additional entities here, or you may 
            // choose to create a new method altogether.

            return done;
        }

        // Loading Genres
        public bool LoadGenres()
        {
            // Return if there's existing data
            if (ds.Genres.Count() > 0) { return false; }

            // Otherwise...
            // Create and add objects
            ds.Genres.Add(new Genre { Name = "Action" });
            ds.Genres.Add(new Genre { Name = "comedy" });
            ds.Genres.Add(new Genre { Name = "Drama" });
            ds.Genres.Add(new Genre { Name = "Thriller" });
            ds.Genres.Add(new Genre { Name = "Adventure" });
            ds.Genres.Add(new Genre { Name = "Romance" });
            ds.Genres.Add(new Genre { Name = "Science Fiction" });
            ds.Genres.Add(new Genre { Name = "Documentry" });
            ds.Genres.Add(new Genre { Name = "Horror" });
            ds.Genres.Add(new Genre { Name = "Family" });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        // Loading Actors
        public bool LoadActors()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Return if there's existing data
            if (ds.Actors.Count() > 0) { return false; }

            // Otherwise...
            // Create and add objects
            ds.Actors.Add(new Actor { 
                Name = "Shah Rukh Khan", 
                AlternateName = "SRK", 
                BirthDate = new DateTime(1965, 11, 2), 
                Executive = user, 
                Height = 1.69, 
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/Shah_Rukh_Khan_graces_the_launch_of_the_new_Santro.jpg/330px-Shah_Rukh_Khan_graces_the_launch_of_the_new_Santro.jpg"
            });

            ds.Actors.Add(new Actor
            {
                Name = "Akshay Kumar",
                AlternateName = "Khiladi",
                BirthDate = new DateTime(1967, 9, 9),
                Executive = user,
                Height = 1.78,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2e/Akshay_Kumar.jpg/330px-Akshay_Kumar.jpg"
            });

            ds.Actors.Add(new Actor
            {
                Name = "Salman Khan",
                AlternateName = "Bhai",
                BirthDate = new DateTime(1965, 12, 27),
                Executive = user,
                Height = 1.74,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/95/Salman_Khan_in_2023_%281%29_%28cropped%29.jpg/330px-Salman_Khan_in_2023_%281%29_%28cropped%29.jpg"
            });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        // Loading Shows
        public bool LoadShows()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Return if there's existing data
            if (ds.Shows.Count() > 0) { return false; }

            // Salman Khan
            // Fetch the actor object, because we need it
            var theBhai = ds.Actors.SingleOrDefault(a => a.Name == "Salman Khan");
            if (theBhai == null) { return false; }

            ds.Shows.Add(new Show
            {
                Actors = new Actor[] { theBhai },
                Name = "Bigg Boss 17", 
                Coordinator = user,
                ImageUrl = "https://akm-img-a-in.tosshub.com/sites/visualstory/wp/2023/10/image-1345.png?size=*:900", 
                Genre = "Drama", 
                ReleaseDate = new DateTime(2023, 10, 15)
            });

            ds.Shows.Add(new Show
            {
                Actors = new Actor[] { theBhai },
                Name = "10 Ka Dum",
                Coordinator = user,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e7/DusKaDum10.png",
                Genre = "Family",
                ReleaseDate = new DateTime(2008, 6, 6)
            });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        // Loading Episodes
        public bool LoadEpisodes()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Return if there's existing data
            if (ds.Episodes.Count() > 0) { return false; }

            // Fetch the shows object because we need it.
            // Bigg Boss 17
            var theBiggBoss = ds.Shows.SingleOrDefault(a => a.Name == "Bigg Boss 17");
            if (theBiggBoss == null) { return false; }
            ds.Episodes.Add(new Episode
            {
                Name = "S17E1",
                EpisodeNumber = 1,
                SeasonNumber = 17,
                Clerk = user, 
                AirDate = new DateTime(2023, 10, 15), 
                Genre = "Drama", 
                ImageUrl = "https://biggbosslive.org.in/wp-content/uploads/2023/10/16-8_11zon.jpg", 
                Show = theBiggBoss
            });
            ds.Episodes.Add(new Episode
            {
                Name = "S17E2",
                EpisodeNumber = 2,
                SeasonNumber = 17,
                Clerk = user,
                AirDate = new DateTime(2023, 10, 16),
                Genre = "Drama",
                ImageUrl = "https://pbs.twimg.com/ext_tw_video_thumb/1715020607228690432/pu/img/Kf8nvVAgNCkvZMxl.jpg",
                Show = theBiggBoss
            });
            ds.Episodes.Add(new Episode
            {
                Name = "S17E3",
                EpisodeNumber = 3,
                SeasonNumber = 17,
                Clerk = user,
                AirDate = new DateTime(2023, 10, 17),
                Genre = "Drama",
                ImageUrl = "https://latesthdmovies.charity/wp-content/uploads/2023/10/bigg-boss-season-17-episode-2-44931-poster.jpg",
                Show = theBiggBoss
            });

            // 10 Ka Dum
            var theDusKaDum = ds.Shows.SingleOrDefault(a => a.Name == "10 Ka Dum");
            if (theDusKaDum == null) { return false; }
            ds.Episodes.Add(new Episode
            {
                Name = "S1E1",
                EpisodeNumber = 1,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2008, 6, 6),
                Genre = "Family",
                ImageUrl = "https://images.indianexpress.com/2017/04/salman-10-ka-dum-759.jpg",
                Show = theDusKaDum
            });
            ds.Episodes.Add(new Episode
            {
                Name = "S1E2",
                EpisodeNumber = 2,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2008, 6, 7),
                Genre = "Family",
                ImageUrl = "https://pbs.twimg.com/media/Dhq3jEJX0AYG16B.jpg",
                Show = theDusKaDum
            });
            ds.Episodes.Add(new Episode
            {
                Name = "S1E3",
                EpisodeNumber = 3,
                SeasonNumber = 1,
                Clerk = user,
                AirDate = new DateTime(2008, 6, 8),
                Genre = "Family",
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR9GHcaoRybI7tqTH9LrVMMpuLfMDWrfdoLrVc1J7ASJCvYR9sXIqeYcLaoLelArJ-TrFE&usqp=CAU",
                Show = theDusKaDum
            });

            // Save changes
            ds.SaveChanges();

            return true;
        }

        // Removing Roles
        public bool RemoveRoles()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Genres
        public bool RemoveGenres()
        {
            try
            {
                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Actors
        public bool RemoveActors()
        {
            try
            {
                foreach (var e in ds.Actors)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Shows
        public bool RemoveShows()
        {
            try
            {
                foreach (var e in ds.Shows)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Removing Episodes
        public bool RemoveEpisodes()
        {
            try
            {
                foreach (var e in ds.Episodes)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    #endregion

    #region RequestUser Class

    // This "RequestUser" class includes many convenient members that make it
    // easier work with the authenticated user and render user account info.
    // Study the properties and methods, and think about how you could use this class.

    // How to use...
    // In the Manager class, declare a new property named User:
    //    public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value:
    //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }

        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }

        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }

        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    #endregion

}