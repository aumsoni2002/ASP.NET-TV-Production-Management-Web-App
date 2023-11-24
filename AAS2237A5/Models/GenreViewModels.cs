using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AAS2237A5.Models
{
    public class GenreBaseViewModel
    {
        // Columns
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }
    }
}