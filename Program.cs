using Microsoft.EntityFrameworkCore;
using SubscriptionApp.Models;
using SubscriptionApp.Repositories;
using SubscriptionsApp.Data;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Server=(localdb)\\MSSQLLocalDB;Database=SubscriptionsDb;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;

using var db = new AppDbContext(options);


db.Database.EnsureCreated(); //це щоб без міграцій

// Перевірка
if (!db.Users.Any())
{
    var users = new List<User>
    {
        new User
        {
            FirstName = "Андрій", LastName = "Коваль", BirthDate = new DateTime(2003, 5, 10), Email = "andriy@gmail.com",
            Subscriptions = new List<Subscription>
            {
                new Subscription { Title="Free plan", Price=0, StartDate=DateTime.Today.AddMonths(-2), EndDate=DateTime.Today.AddDays(-5), Type=SubscriptionType.Free },
                new Subscription { Title="Premium plan", Price=299, StartDate=DateTime.Today.AddMonths(-1), EndDate=DateTime.Today.AddMonths(1), Type=SubscriptionType.Premium },
            }
        },
        new User
        {
            FirstName = "Аліна", LastName = "Шевченко", BirthDate = new DateTime(2004, 3, 2), Email = "alina@gmail.com",
            Subscriptions = new List<Subscription>
            {
                new Subscription { Title="Standard plan", Price=149, StartDate=DateTime.Today.AddMonths(-1), EndDate=DateTime.Today.AddMonths(2), Type=SubscriptionType.Standard },
            }
        },
        new User
        {
            FirstName = "Богдан", LastName = "Іваненко", BirthDate = new DateTime(2002, 11, 20), Email = "bogdan@gmail.com"
        },
        new User
        {
            FirstName = "Анна", LastName = "Мельник", BirthDate = new DateTime(2001, 7, 15), Email = "anna@gmail.com",
            Subscriptions = new List<Subscription>
            {
                new Subscription { Title="Free trial", Price=0, StartDate=DateTime.Today.AddDays(-20), EndDate=DateTime.Today.AddDays(-1), Type=SubscriptionType.Free },
                new Subscription { Title="Premium", Price=299, StartDate=DateTime.Today, EndDate=DateTime.Today.AddMonths(1), Type=SubscriptionType.Premium },
                new Subscription { Title="Premium+", Price=399, StartDate=DateTime.Today.AddMonths(-6), EndDate=DateTime.Today.AddMonths(-5), Type=SubscriptionType.Premium },
            }
        },
        new User
        {
            FirstName = "Олег", LastName = "Петренко", BirthDate = new DateTime(2000, 1, 5), Email = "oleg@gmail.com",
            Subscriptions = new List<Subscription>
            {
                new Subscription { Title="Premium", Price=299, StartDate=DateTime.Today.AddMonths(-3), EndDate=DateTime.Today.AddMonths(1), Type=SubscriptionType.Premium },
            }
        }
    };

    db.Users.AddRange(users);
    db.SaveChanges();
}

Console.WriteLine("=== EF Core Queries ===\n");

var subscriptionRepo = new SubscriptionRepository(db); //Приклад використаня

var expiredFree = await subscriptionRepo.GetExpiredFreeSubscriptionsAsync();

foreach (var s in expiredFree)
{
    Console.WriteLine($"{s.Title} | UserId: {s.UserId}");
}
