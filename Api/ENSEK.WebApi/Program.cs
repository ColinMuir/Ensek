using ENSEK.WebApi.Infrastucture.FileProcessors;
using ENSEK.WebApi.Infrastucture.Repository;
using ENSEK.WebApi.Infrastucture.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureEntityFramework(builder);

ConfirgureService(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("default", policy =>
{
    policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("default");

app.MapControllers();

app.Run();

void ConfigureEntityFramework(WebApplicationBuilder builder)
{
    var logger = LoggerFactory.Create(logBuilder => logBuilder.AddConsole());

    var connectionString = builder.Configuration["connectionStrings:MeterReadsConnectionString"];

    builder.Services.AddDbContext<MeterReadingDbContext>(dbContextOptions =>
    {
        dbContextOptions.UseLoggerFactory(logger);
        dbContextOptions.UseSqlServer(connectionString, sqlServerDbContextOptionsBuilder =>
        {
            sqlServerDbContextOptionsBuilder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
    });
}

void ConfirgureService(IServiceCollection services)
{
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddTransient<IMeterReadingService, MeterReadingService>();
    services.AddTransient<IBatchService, BatchService>();
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<IFileProcessor, CSVProcessor>();
}