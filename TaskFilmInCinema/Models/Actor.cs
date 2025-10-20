using System.ComponentModel.DataAnnotations;

namespace TaskFilmInCinema.Models
{
    public class Actor
    {
        [Key]
        public int Act_Id { get; set; }

        public string Img { get; set; } = string.Empty;

        
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
