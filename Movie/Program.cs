using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movie.Models;
using Movie.Repository;
using Movie.Service;



var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ vào container.
builder.Services.AddControllers();


// Thêm Swagger cho OpenAPI
builder.Services.AddScoped<JwtService>(); // Register JwtService as Scoped
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Thêm hỗ trợ file upload trong Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Version = "v1" });

    // Thêm phần hỗ trợ file upload
    c.OperationFilter<FileUploadOperation>();
});




// Đọc cấu hình từ appsettings.json
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<movieDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnn")));

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
}).AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


// Thêm các repository vào container
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieCategoryRepository<MovieCategories>, MovieCategoryRepository>();
builder.Services.AddScoped<IMovieActorRepository<MovieActors>, MovieActorRepository>();
builder.Services.AddScoped<ISeriesCategoryRepository<SeriesCategories>, SeriesCategoryRepository>();
builder.Services.AddScoped<ISeriesActorRepository<SeriesActors>, SeriesActorRepository>();
builder.Services.AddScoped<IMovieHome, MovieHomeRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IDirectorsRepository, DirectorRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ContentRepository>();

var app = builder.Build();

// Cấu hình pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins"); // Enable CORS policy
app.UseAuthorization();
app.MapControllers();

app.Run();


