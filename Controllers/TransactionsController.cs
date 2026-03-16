using System.Security.Claims;
using FinanceControl.DTOs;
using FinanceControl.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetAllAsync(GetUserId());
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _transactionService.GetByIdAsync(id, GetUserId());

            if (transaction == null)
                return NotFound(new { message = "Transaction not found." });

            return Ok(transaction);
        }

        // retorna totais de entrada, saida e saldo
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _transactionService.GetSummaryAsync(GetUserId());
            return Ok(summary);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _transactionService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TransactionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _transactionService.UpdateAsync(id, dto, GetUserId());

            if (updated == null)
                return NotFound(new { message = "Transaction not found." });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _transactionService.DeleteAsync(id, GetUserId());

            if (!deleted)
                return NotFound(new { message = "Transaction not found." });

            return NoContent();
        }
    }
}
