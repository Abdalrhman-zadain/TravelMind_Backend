using Microsoft.AspNetCore.Mvc;
using TravelMind.Business.Business;
using TravelMind.DataAccess.DTO;

namespace TravelMind.API.Controllers
{
    [Route("api/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        // GET /api/expenses/user/1
        [HttpGet("user/{userId}")]
        public ActionResult<List<ExpenseDTO>> GetByUserId(int userId)
        {
            var list = Expense.GetByUserId(userId);
            if (list.Count == 0)
                return NotFound($"No expenses found for user {userId}!");
            return Ok(list);
        }

        // GET /api/expenses/trip/1
        [HttpGet("trip/{tripId}")]
        public ActionResult<List<ExpenseDTO>> GetByTripId(int tripId)
        {
            var list = Expense.GetByTripId(tripId);
            if (list.Count == 0)
                return NotFound($"No expenses found for trip {tripId}!");
            return Ok(list);
        }

        // GET /api/expenses/1
        [HttpGet("{id}")]
        public ActionResult<ExpenseDTO> GetById(int id)
        {
            Expense expense = Expense.Find(id);
            if (expense == null)
                return NotFound($"Expense with ID {id} not found!");
            return Ok(expense.DTO);
        }

        // POST /api/expenses
        [HttpPost]
        public ActionResult<ExpenseDTO> Create(ExpenseDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Description) || dto.Amount <= 0)
                return BadRequest("Description and valid amount are required!");

            var expense = new Expense(dto, Expense.enMode.AddNew);
            if (expense.Save())
                return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense.DTO);

            return StatusCode(500, "Failed to add expense!");
        }

        // PUT /api/expenses/1
        [HttpPut("{id}")]
        public ActionResult<ExpenseDTO> Update(int id, ExpenseDTO dto)
        {
            Expense expense = Expense.Find(id);
            if (expense == null)
                return NotFound($"Expense with ID {id} not found!");

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Category = dto.Category;
            expense.Date = dto.Date;

            if (expense.Save())
                return Ok(expense.DTO);

            return StatusCode(500, "Failed to update expense!");
        }

        // DELETE /api/expenses/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Expense expense = Expense.Find(id);
            if (expense == null)
                return NotFound($"Expense with ID {id} not found!");

            if (Expense.Delete(id))
                return Ok($"Expense with ID {id} deleted successfully!");

            return StatusCode(500, "Failed to delete expense!");
        }
    }
}
