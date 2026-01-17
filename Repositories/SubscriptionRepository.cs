using Microsoft.EntityFrameworkCore;
using SubscriptionApp.Models;
using SubscriptionsApp.Data;

namespace SubscriptionApp.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    // ===== CRUD =====

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Subscription>> GetAllAsync()
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task AddAsync(Subscription subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var subscription = await _context.Subscriptions.FindAsync(id);
        if (subscription is null) return;

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync();
    }

   

    public async Task<List<Subscription>> GetByUserIdAsync(int userId)
    {
        return await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetByTypeAsync(SubscriptionType type)
    {
        return await _context.Subscriptions
            .Where(s => s.Type == type)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetActiveSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Where(s => s.EndDate >= DateTime.Today)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetExpiredSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Where(s => s.EndDate < DateTime.Today)
            .ToListAsync();
    }

    public async Task<List<Subscription>> GetExpiredFreeSubscriptionsAsync()
    {
        return await _context.Subscriptions
            .Where(s => s.Type == SubscriptionType.Free && s.EndDate < DateTime.Today)
            .ToListAsync();
    }

    public async Task<bool> UserHasActiveSubscriptionAsync(int userId)
    {
        return await _context.Subscriptions
            .AnyAsync(s => s.UserId == userId && s.EndDate >= DateTime.Today);
    }

    public async Task<Subscription?> GetLastSubscriptionForUserAsync(int userId)
    {
        return await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync();
    }
}