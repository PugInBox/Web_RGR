using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;";
builder.Services.AddDbContext<MovieContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/movies", async (MovieContext db) => await db.Movies.ToListAsync());

app.MapGet("/api/movies/{id:int}", async (int id, MovieContext db) =>
{
    Movie? movie = await db.Movies.FirstOrDefaultAsync(u => u.Id == id);
    if (movie == null) return Results.NotFound(new { message = "Фильм по id не найден" });
    return Results.Json(movie);
});

app.MapGet("/api/movies/genre/{genre?}", async (string? genre, MovieContext db) =>
{
    var movies = db.Movies.Where(u => EF.Functions.Like(u.Genre, "%"+genre+"%"));
    if (movies == null) return Results.NotFound(new { message = "Фильм по жанру не найден" });
    return Results.Json(movies);
});

app.MapGet("/api/movies/name/{name?}", async (string? name, MovieContext db) =>
{
    var movies = db.Movies.Where(u => EF.Functions.Like(u.Name, "%"+name+"%"));
    if (movies == null) return Results.NotFound(new { message = "Фильм по названию не найден" });
    return Results.Json(movies);
});

app.MapDelete("/api/movies/{id:int}", async (int id, MovieContext db) =>
{
    Movie? movie = await db.Movies.FirstOrDefaultAsync(u => u.Id == id);
    if (movie == null) return Results.NotFound(new { message = "Не получилось удалить фильм" });
    db.Movies.Remove(movie);
    await db.SaveChangesAsync();
    return Results.Json(movie);
});

app.MapPost("/api/movies", async (Movie movie, MovieContext db) =>
{
    await db.Movies.AddAsync(movie);
    await db.SaveChangesAsync();
    return movie;
});

app.MapPut("/api/movies", async (Movie movieData, MovieContext db) =>
{
    var movie = await db.Movies.FirstOrDefaultAsync(u => u.Id == movieData.Id);
    if (movie == null) return Results.NotFound(new { message = "Такого фильма нет" });
    movie.Name = movieData.Name;
    movie.Genre = movieData.Genre;
    movie.Year = movieData.Year;
    movie.NumSeasons = movieData.NumSeasons;
    movie.NumEpisodes = movieData.NumEpisodes;
    movie.CountryIssue = movieData.CountryIssue;
    movie.DateView = movieData.DateView;
    await db.SaveChangesAsync();
    return Results.Json(movie);
});

app.Run();