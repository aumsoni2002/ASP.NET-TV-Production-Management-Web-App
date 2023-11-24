using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AAS2237A5.Data
{
    [Table("Episode")]
    public class Episode
    {
        // Constructor
        public Episode() 
        {
            AirDate = DateTime.Now;
        }

        // Columns
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        public int SeasonNumber { get; set; } 

        public int EpisodeNumber { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AirDate { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [Required, StringLength(250)]
        public string Clerk { get; set; }


        // Navigation Property
        [Required]
        public Show Show { get; set; }
    }
}