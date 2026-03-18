using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Expense
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TripId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime? Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public ExpenseDTO DTO => new ExpenseDTO(
            Id, UserId, TripId, Description,
            Amount, Category, Date, CreatedAt
        );

        public Expense(ExpenseDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            TripId = dto.TripId;
            Description = dto.Description;
            Amount = dto.Amount;
            Category = dto.Category;
            Date = dto.Date;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = ExpenseData.AddExpense(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return ExpenseData.UpdateExpense(DTO);
        }

        public static List<ExpenseDTO> GetByUserId(int userId)
        {
            return ExpenseData.GetExpensesByUserId(userId);
        }

        public static List<ExpenseDTO> GetByTripId(int tripId)
        {
            return ExpenseData.GetExpensesByTripId(tripId);
        }

        public static Expense Find(int id)
        {
            ExpenseDTO dto = ExpenseData.GetExpenseById(id);
            if (dto != null)
                return new Expense(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return ExpenseData.DeleteExpense(id);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew()) { Mode = enMode.Update; return true; }
                    return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}