using AAS2237A5.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAS2237A5.Models
{
    public class ShowBaseViewModel : ShowAddViewModel
    {
        // Constructor
        public ShowBaseViewModel() { }

        // Columns
        [Required, StringLength(250)]
        public string Coordinator { get; set; }

        // Navigation Property
    }

    public class ShowAddViewModel
    {
        // Constructor
        public ShowAddViewModel()
        {
            ReleaseDate = DateTime.Now;
            ActorIds = new List<int>();
        }

        // Columns
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [Required, Display(Name = "Release Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required, StringLength(250), Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Range(1, Int32.MaxValue)]
        public int ActorId { get; set; }

        public IEnumerable<int> ActorIds { get; set; }
    }

    public class ShowAddFormViewModel : ShowAddViewModel
    {        
        [Display(Name = "Genre")]
        public SelectList GenreList { get; set; }

        [Display(Name = "Actors")]
        public MultiSelectList ActorList { get; set; }

        // TODO: 25 - Display the name of the associated item
        public string ActorName { get; set; }
    }

    public class ShowWithInfoViewModel : ShowBaseViewModel 
    {
        public ShowWithInfoViewModel()
        {
            Actors = new List<ActorBaseViewModel>();
            Episodes = new List<EpisodeBaseViewModel>();
        }

        // Navigation Property
        public IEnumerable<ActorBaseViewModel> Actors { get; set; }

        public IEnumerable<EpisodeBaseViewModel> Episodes { get; set; }
    }
}