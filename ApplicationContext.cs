using Microsoft.EntityFrameworkCore;

public class MovieContext : DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;
    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
}

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; } = ""; // Название
    public string Genre { get; set; } = ""; // Жанр
    public int Year { get; set; } // Год издания
    public int NumSeasons { get; set; } // Кол-во сезонов
    public int NumEpisodes { get; set; } // Кол-во эпизодов
    public string CountryIssue { get; set; } = ""; // Страна издания
    public DateTime? DateView { get; set; } // Дата просмотра
}