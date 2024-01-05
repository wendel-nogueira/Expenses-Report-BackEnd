namespace ExpensesReport.Expenses.Application.ViewModels
{
    public class IdentityViewModel(Guid id, string email, bool isDeleted)
    {
        public Guid Id { get; set; } = id;
        public string Email { get; set; } = email;
        public bool IsDeleted { get; set; } = isDeleted;
    }
}
