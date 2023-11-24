using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AAS2237A5.Data
{
    [Table("Show")]
    public class Show
    {
        // Constructor
        public Show() 
        {
            ReleaseDate = DateTime.Now;
            Actors = new HashSet<Actor>();
            Episodes = new HashSet<Episode>();
        }

        // Columns
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [Required, StringLength(250)]
        public string Coordinator { get; set; }

        // Navigation Property
        public ICollection<Actor> Actors { get; set; }  

        public ICollection<Episode> Episodes { get; set;}
    }
}