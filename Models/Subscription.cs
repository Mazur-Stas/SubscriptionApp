namespace SubscriptionApp.Models;

public class Subscription
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public decimal Price { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public SubscriptionType Type { get; set; }

    // FK
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public enum SubscriptionType
{
    Free = 0,
    Standard = 1,
    Premium = 2
}