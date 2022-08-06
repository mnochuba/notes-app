using CCSANoteApp.DB;
using CCSANoteApp.DB.Configurations;
using CCSANoteApp.DB.Repositories;
using CCSANoteApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(DBConfiguration)).Get<DBConfiguration>());

builder.Services.AddScoped<INoteService,NoteService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<NoteRepository>();
builder.Services.AddSingleton<SessionFactory>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAuthenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy("MustBeAStaff", policy =>
    {
        policy.RequireRole("Staff", "Manager");
        policy.RequireClaim("MustBeAbove18");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
