using Microsoft.EntityFrameworkCore;
using SubscriptionApp.Models;
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

// 1
var q1 = db.Users
    .Where(u => u.FirstName.StartsWith("А"))
    .ToList();

Console.WriteLine("1) Ім'я починається на 'А':");
foreach (var u in q1)
    Console.WriteLine($"- {u.Id}: {u.FirstName} {u.LastName}");
Console.WriteLine();

// 2
var q2 = db.Users
    .Where(u => u.Subscriptions.Any())
    .Include(u => u.Subscriptions)
    .ToList();

Console.WriteLine("2) Користувачі, у яких є підписки:");
foreach (var u in q2)
    Console.WriteLine($"- {u.FirstName} {u.LastName} (subscriptions: {u.Subscriptions.Count})");
Console.WriteLine();

// 3
var q3 = db.Users
    .Where(u => u.Subscriptions.Any(s => s.Type == SubscriptionType.Premium))
    .Select(u => u.FirstName)
    .Distinct()
    .Take(5)
    .ToList();

Console.WriteLine("3) Перші 5 імен користувачів з Premium підпискою:");
foreach (var name in q3)
    Console.WriteLine($"- {name}");
Console.WriteLine();

// 4
var q4 = db.Users
    .Include(u => u.Subscriptions)
    .OrderByDescending(u => u.Subscriptions.Count)
    .FirstOrDefault();

Console.WriteLine("4) Користувач з найбільшою кількістю підписок:");
if (q4 is null)
{
    Console.WriteLine("- Немає користувачів.");
}
else
{
    Console.WriteLine($"- {q4.FirstName} {q4.LastName} | Email: {q4.Email} | Count: {q4.Subscriptions.Count}");
}
Console.WriteLine();

// 5
var today = DateTime.Today;

var q5 = db.Subscriptions
    .Where(s => s.Type == SubscriptionType.Free && s.EndDate < today)
    .Include(s => s.User)
    .ToList();

Console.WriteLine("5) Free підписки, які вже закінчились:");
foreach (var s in q5)
{
    Console.WriteLine($"- {s.Title} ({s.StartDate:yyyy-MM-dd} - {s.EndDate:yyyy-MM-dd}) | User: {s.User.FirstName} {s.User.LastName}");
}

Console.WriteLine("\nDone.");
