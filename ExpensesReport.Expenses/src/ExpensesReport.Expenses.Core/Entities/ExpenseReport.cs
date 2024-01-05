using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ExpensesReport.Expenses.Core.Entities
{
    public class ExpenseReport : EntityBase
    {
        public ExpenseReport(Guid userId, Guid departamentId, Guid projectId, ExpenseReportStatus? status, decimal totalAmount, decimal? amountApproved, decimal? amountRejected, decimal? amountPaid, Guid? paidById, DateTime? paidDate, string statusNotes, string? proofOfPayment, string? paidDateTimeZone)
        {
            UserId = userId;
            DepartamentId = departamentId;
            ProjectId = projectId;
            Status = status;
            TotalAmount = totalAmount;
            AmountApproved = amountApproved;
            AmountRejected = amountRejected;
            AmountPaid = amountPaid;
            PaidById = paidById;
            PaidDate = paidDate;
            PaidDateTimeZone = paidDateTimeZone;
            StatusNotes = statusNotes;
            ProofOfPayment = proofOfPayment;

            Expenses = new List<Expense>();
            Signatures = new List<Signature>();
        }

        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid DepartamentId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ProjectId { get; set; }

        public ExpenseReportStatus? Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? AmountApproved { get; set; }
        public decimal? AmountRejected { get; set; }
        public decimal? AmountPaid { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid? PaidById { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? PaidDateTimeZone { get; set; }
        public string? StatusNotes { get; set; }
        public string? ProofOfPayment { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Signature> Signatures { get; set; }

        public void Update(Guid departamentId, Guid projectId, ExpenseReportStatus status, decimal? amountPaid, Guid? paidById, DateTime? paidDate, string? statusNotes, string? proofOfPayment, string? paidDateTimeZone)
        {
            DepartamentId = departamentId;
            ProjectId = projectId;
            Status = status;
            AmountPaid = amountPaid;
            PaidById = paidById;
            PaidDate = paidDate;
            PaidDateTimeZone = paidDateTimeZone;
            StatusNotes = statusNotes;
            ProofOfPayment = proofOfPayment;
            UpdatedAt = DateTime.Now;
        }
    }
}
