namespace ExpensesReport.Expenses.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() { }

        public string?[]? Errors { get; set; }

        public BadRequestException(string message, string?[]? errors) : base(message) => Errors = errors;
        public BadRequestException(string message, Exception inner) : base(message, inner) { }
    }
}
