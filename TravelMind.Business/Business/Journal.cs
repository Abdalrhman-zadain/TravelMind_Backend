using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Journal
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? TripId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? Date { get; set; }
        public DateTime CreatedAt { get; set; }

        public JournalDTO DTO => new JournalDTO(
            Id, UserId, TripId, Title, Content, Date, CreatedAt
        );

        public Journal(JournalDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            TripId = dto.TripId;
            Title = dto.Title;
            Content = dto.Content;
            Date = dto.Date;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = JournalData.AddJournal(DTO);
            return this.Id != -1;
        }

        private bool _Update()
        {
            return JournalData.UpdateJournal(DTO);
        }

        public static List<JournalDTO> GetByUserId(int userId)
        {
            return JournalData.GetJournalsByUserId(userId);
        }

        public static Journal Find(int id)
        {
            JournalDTO dto = JournalData.GetJournalById(id);
            if (dto != null)
                return new Journal(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return JournalData.DeleteJournal(id);
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