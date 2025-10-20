using System.ComponentModel.DataAnnotations;

namespace TaskFilmInCinema.Models
{
    public class Category
    {
        [Key]
        public int Cat_Id { get; set; }
       public int Status { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
