using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var client = new MongoClient(mongoConnectionString);
var database = client.GetDatabase("logindb");
var usersCollection = database.GetCollection<User>("Users");

var app = builder.Build();

app.MapPost("/api/auth/login", async (LoginDto loginDto) =>
{
    var user = await usersCollection.Find(u => 
        u.Username == loginDto.Username && u.Password == loginDto.Password)
        .FirstOrDefaultAsync();

    return user != null ? Results.Ok("Login successful") : Results.Unauthorized();
});

app.Run();

record LoginDto(string Username, string Password);

class User
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
