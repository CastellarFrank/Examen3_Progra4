using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Examen3_Progra4.Models
{
    public class meetingStructure
    {
        public int meetId { get; set; }

        [Required]
        [Display(Name = "Meeting Name")]
        public string meetName { get; set; }

        [Required]
        [Display(Name = "Meeting Description")]
        public string meetDescription { get; set; }

        [Required]
        [Display(Name = "Meeting Date")]
        public DateTime meetDate { get; set; }

        [Required]
        [Display(Name = "Meeting Length")]
        public double meetLength { get; set; }

        [Required]
        [Display(Name = "Meeting Latitude")]
        public double meetLatitude { get; set; }

        public string userOwner { get; set; }
    }
}
