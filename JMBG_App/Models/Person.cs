using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JMBG_App.Models
{
    public class Person
    {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Jmbg { get; set; }
    }
}
