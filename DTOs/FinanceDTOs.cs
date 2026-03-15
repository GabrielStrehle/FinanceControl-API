using System.ComponentModel.DataAnnotations;

namespace FinanceControl.DTOs
{
    // ===================== CATEGORY =====================

    public class CategoryDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression("^(income|expense)$", ErrorMessage = "Type must be 'income' or 'expense'")]
        public string Type { get; set; } = string.Empty;
    }

    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    // ===================== TRANSACTION =====================

    public class TransactionDTO
    {
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression("^(income|expense)$", ErrorMessage = "Type must be 'income' or 'expense'")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }
    }

    public class TransactionResponseDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class SummaryDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public int TotalTransactions { get; set; }
    }
}
