using FinanceControl.DTOs;
using FinanceControl.Models;
using FinanceControl.Repositories;

namespace FinanceControl.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponseDTO>> GetAllAsync(int userId);
        Task<TransactionResponseDTO?> GetByIdAsync(int id, int userId);
        Task<TransactionResponseDTO> CreateAsync(TransactionDTO dto, int userId);
        Task<TransactionResponseDTO?> UpdateAsync(int id, TransactionDTO dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
        Task<SummaryDTO> GetSummaryAsync(int userId);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TransactionResponseDTO>> GetAllAsync(int userId)
        {
            var transactions = await _transactionRepository.GetAllAsync(userId);
            return transactions.Select(MapToResponse);
        }

        public async Task<TransactionResponseDTO?> GetByIdAsync(int id, int userId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, userId);
            return transaction == null ? null : MapToResponse(transaction);
        }

        public async Task<TransactionResponseDTO> CreateAsync(TransactionDTO dto, int userId)
        {
            var transaction = new Transaction
            {
                Description = dto.Description,
                Amount = dto.Amount,
                Date = dto.Date,
                Type = dto.Type,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            var created = await _transactionRepository.CreateAsync(transaction);

            // busca de novo pra incluir a categoria no retorno
            var full = await _transactionRepository.GetByIdAsync(created.Id, userId);
            return MapToResponse(full!);
        }

        public async Task<TransactionResponseDTO?> UpdateAsync(int id, TransactionDTO dto, int userId)
        {
            var transaction = new Transaction
            {
                Id = id,
                Description = dto.Description,
                Amount = dto.Amount,
                Date = dto.Date,
                Type = dto.Type,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            var updated = await _transactionRepository.UpdateAsync(transaction);
            if (updated == null) return null;

            var full = await _transactionRepository.GetByIdAsync(updated.Id, userId);
            return MapToResponse(full!);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            return await _transactionRepository.DeleteAsync(id, userId);
        }

        public async Task<SummaryDTO> GetSummaryAsync(int userId)
        {
            var (income, expense) = await _transactionRepository.GetSummaryAsync(userId);
            var all = await _transactionRepository.GetAllAsync(userId);

            return new SummaryDTO
            {
                TotalIncome = income,
                TotalExpense = expense,
                Balance = income - expense,
                TotalTransactions = all.Count()
            };
        }

        private static TransactionResponseDTO MapToResponse(Transaction t) => new()
        {
            Id = t.Id,
            Description = t.Description,
            Amount = t.Amount,
            Date = t.Date,
            Type = t.Type,
            CategoryName = t.Category?.Name ?? string.Empty,
            CreatedAt = t.CreatedAt
        };
    }
}
