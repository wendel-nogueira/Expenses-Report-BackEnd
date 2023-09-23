namespace ExpensesReport.Users.Application.ViewModels
{
    public class UserNameViewModel
    {
        public UserNameViewModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }
}
