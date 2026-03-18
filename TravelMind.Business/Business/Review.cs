using TravelMind.DataAccess.Data;
using TravelMind.DataAccess.DTO;

namespace TravelMind.Business.Business
{
    public class Review
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlaceType { get; set; }
        public int PlaceId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewDTO DTO => new ReviewDTO(
            Id, UserId, PlaceType, PlaceId, Rating, Comment, CreatedAt
        );

        public Review(ReviewDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            PlaceType = dto.PlaceType;
            PlaceId = dto.PlaceId;
            Rating = dto.Rating;
            Comment = dto.Comment;
            CreatedAt = dto.CreatedAt;
            Mode = mode;
        }

        private bool _AddNew()
        {
            this.Id = ReviewData.AddReview(DTO);
            return this.Id != -1;
        }

        public static List<ReviewDTO> GetByPlace(string placeType, int placeId)
        {
            return ReviewData.GetReviewsByPlace(placeType, placeId);
        }

        public static List<ReviewDTO> GetByUserId(int userId)
        {
            return ReviewData.GetReviewsByUserId(userId);
        }

        public static Review Find(int id)
        {
            ReviewDTO dto = ReviewData.GetReviewById(id);
            if (dto != null)
                return new Review(dto, enMode.Update);
            return null;
        }

        public static bool Delete(int id)
        {
            return ReviewData.DeleteReview(id);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew()) { Mode = enMode.Update; return true; }
                    return false;
            }
            return false;
        }
    }
}