using System.ComponentModel.DataAnnotations;

namespace TaskFilmInCinema.Models
{
    public class Cinema
    {
        [Key]
        public int Cin_Id { get; set; }
        public string Img { get; set; } = string.Empty;

    }
}
