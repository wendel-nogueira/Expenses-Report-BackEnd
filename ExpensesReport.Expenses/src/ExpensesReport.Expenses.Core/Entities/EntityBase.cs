using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpensesReport.Users.Core.Entities
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            Id = ObjectId.GenerateNewId().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }


        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Activate()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.Now;
        }

        public void Deactivate()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
