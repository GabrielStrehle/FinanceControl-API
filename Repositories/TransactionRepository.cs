using FinanceControl.Data;
using FinanceControl.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync(int userId);
        Task<Transaction?> GetByIdAsync(int id, int userId);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction?> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(int id, int userId);
        Task<(decimal income, decimal expense)> GetSummaryAsync(int userId);
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(int userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id, int userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            var existing = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transaction.Id && t.UserId == transaction.UserId);

            if (existing == null) return null;

            existing.Description = transaction.Description;
            existing.Amount = transaction.Amount;
            existing.Date = transaction.Date;
            existing.Type = transaction.Type;
            existing.CategoryId = transaction.CategoryId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(decimal income, decimal expense)> GetSummaryAsync(int userId)
        {
            var income = await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == "income")
                .SumAsync(t => t.Amount);

            var expense = await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == "expense")
                .SumAsync(t => t.Amount);

            return (income, expense);
        }
    }
}
