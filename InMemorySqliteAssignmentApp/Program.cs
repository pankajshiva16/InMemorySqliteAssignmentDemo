using FluentValidation;
using InMemorySqliteAssignmentApi;
using InMemorySqliteAssignmentApp.Domain.Entities;
using InMemorySqliteAssignmentApp.Domain.Helpers;
using InMemorySqliteAssignmentApp.Infrastructure_Data_;
using InMemorySqliteAssignmentApp.Services_BusinessLayer_.Abstract;
using InMemorySqliteAssignmentApp.Services_BusinessLayer_.Implementation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data.Common;


var builder = WebApplication.CreateBuilder(args);

/*/ Registering Serilogger /*/
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddSingleton<DbConnection, SqliteConnection>(serviceProvider =>
{
    var connection = new SqliteConnection("Data Source=:memory:");
    connection.Open();
    return connection;
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connection = serviceProvider.GetRequiredService<DbConnection>();
    options.UseSqlite(connection);
});


var provider = builder.Services.BuildServiceProvider().UpdateDatabase();


/*/ Registering Customer Service with validators /*/
builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();
builder.Services.AddTransient<ICustomerService, CustomerService>();

builder.Services.AddControllers().AddNewtonsoftJson(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
});

var app = builder.Build();

// Adding Data to Sqlite In-Memory
AddCustomerData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add Serilog
app.UseSerilogRequestLogging();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddCustomerData(WebApplication app) 
{
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<AppDbContext>();


    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    Customer customer1 = new Customer
    {
        Id = new Guid("6bdc1ad2-5175-4e7f-89dd-9d07c976ba3d"),
        FullName = "Vinod Kambli",
        DateOfBirth = new DateOnly(1977, 2, 17),
        CustomerAvtar = String.Empty
    };

    var customer2 = new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Avinash Raikwar",
        DateOfBirth = new DateOnly(1990, 8, 17),
        CustomerAvtar = String.Empty
    };

    var customer3 = new Customer
    {
        Id = Guid.NewGuid(),
        FullName = "Pradeep Chauhan",
        DateOfBirth = new DateOnly(1987, 2, 17),
        CustomerAvtar = String.Empty
    };

    new Customer()
    {
        Id = new Guid("6ca62410-ad0e-4449-b0fd-b8b4ab86b526"),
        FullName = "John Mercy",
        DateOfBirth = new DateOnly(1978, 06, 22),
        CustomerAvtar = string.Empty
    };

    db.Customers.Add(customer1);
    db.Customers.Add(customer2);
    db.Customers.Add(customer3);

    db.SaveChanges();
}