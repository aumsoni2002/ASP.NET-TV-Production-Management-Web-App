using AAS2237A5.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAS2237A5.Models
{
    public class EpisodeBaseViewModel : EpisodeAddViewModel
    {
        // Constructor
        public EpisodeBaseViewModel()
        {

        }

        // Columns
        
        [Required, StringLength(250)]
        public string Clerk { get; set; }

    }

    public class EpisodeAddViewModel
    {
        // Constructor
        public EpisodeAddViewModel()
        {
            AirDate = DateTime.Now;
        }

        // Columns
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Season")]
        public int SeasonNumber { get; set; }

        [Display(Name = "Episode")]
        public int EpisodeNumber { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required, Display(Name = "Date Aired"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime AirDate { get; set; }

        [Required, StringLength(250), Display(Name = "Image")]
        public string ImageUrl { get; set; }

        // Navigation Property
        //[Required]
        public ShowBaseViewModel Show { get; set; }

        // Identifier for the associated item - required!
        [Range(1, Int32.MaxValue)]
        public int ShowId { get; set; }

        /*[Range(1, Int32.MaxValue)]
        public int GenreId { get; set; }*/


    }

    public class EpisodeAddFormViewModel : EpisodeAddViewModel 
    {
        public EpisodeAddFormViewModel()
        {

        }

        [Display(Name = "Genre")]
        public SelectList GenreList { get; set; }

        // TODO: 25 - Display the name of the associated item
        public string ShowName { get; set; }
    }

    public class EpisodeWithShowNameViewModel : EpisodeAddViewModel
    {

    }
}