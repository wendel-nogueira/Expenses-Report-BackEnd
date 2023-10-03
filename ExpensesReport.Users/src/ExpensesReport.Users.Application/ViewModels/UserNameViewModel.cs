namespace ExpensesReport.Users.Application.ViewModels
{
    public class UserNameViewModel(string firstName, string lastName)
    {
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
    }
}
