using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Examen3_Progra4.Models
{
    public class todoEstructure
    {
        public int IdTodo { get; set; }

        [Required]
        [Display(Name = "TO-DO Name")]
        public string nameTodo { get; set; }

        [Required]
        [Display(Name = "TO-DO Description")]
        public string descriptionTodo { get; set; }

        [Required]
        [Display(Name = "TO-DO Start Date")]
        public DateTime dateI { get; set; }

        [Required]
        [Display(Name = "TO-DO Finish Date")]
        public DateTime dateF { get; set; }

        [Required]
        [Display(Name = "TO-DO Status")]
        public string status { get; set; }

        public string userOwner { get; set; }

    }
}
