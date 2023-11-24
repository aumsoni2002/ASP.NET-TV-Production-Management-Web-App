using AAS2237A5.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AAS2237A5.Models
{
    public class ActorBaseViewModel : ActorAddViewModel
    {
        // Constructor
        public ActorBaseViewModel() 
        {
            
        }
       
        // Columns
        [Required, StringLength(250)]
        public string Executive { get; set; }
    }

    public class ActorAddViewModel
    {
        // Columns
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(150), Display(Name = "Alternate Name")]
        public string AlternateName { get; set; }

        [Display(Name = "Birth Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Height (m)"), DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double? Height { get; set; }

        [Required, StringLength(250), Display(Name = "Image")]
        public string ImageUrl { get; set; }
    }

    public class ActorWithShowInfoViewModel : ActorBaseViewModel
    {
        public ActorWithShowInfoViewModel()
        {
            Shows = new List<ShowBaseViewModel>();
        }

        // Navigation Property
        [Display(Name = "Appeared In")]
        public IEnumerable<ShowBaseViewModel> Shows { get; set; }

    }
}