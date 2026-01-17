namespace SubscriptionApp.Models;

public interface ISubscriptionRepository
{
    // CRUD
    Task<Subscription?> GetByIdAsync(int id);
    Task<List<Subscription>> GetAllAsync();
    Task AddAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
    Task DeleteAsync(int id);
    
    Task<List<Subscription>> GetByUserIdAsync(int userId);
    Task<List<Subscription>> GetByTypeAsync(SubscriptionType type);
    Task<List<Subscription>> GetActiveSubscriptionsAsync();
    Task<List<Subscription>> GetExpiredSubscriptionsAsync();
    Task<List<Subscription>> GetExpiredFreeSubscriptionsAsync();
    Task<bool> UserHasActiveSubscriptionAsync(int userId);
    Task<Subscription?> GetLastSubscriptionForUserAsync(int userId);
}