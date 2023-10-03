namespace ExpensesReport.Users.Core.ValueObjects
{
    public record UserName
    {
        public UserName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
