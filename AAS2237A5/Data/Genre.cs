using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AAS2237A5.Data
{
    [Table("Genre")]
    public class Genre
    {
        public Genre()
        {
            
        }

        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        public static implicit operator string(Genre v)
        {
            throw new NotImplementedException();
        }
    }
}