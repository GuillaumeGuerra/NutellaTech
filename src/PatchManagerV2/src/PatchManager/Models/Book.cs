using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XOneBlotter.Models
{
    public class Book
    {
        //[Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public DateTime DatePublished { get; set; }
        public decimal Price { get; set; }
    }
}
