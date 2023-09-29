namespace ExpensesReport.Identity.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() { }

        public string?[]? Errors { get; set; }

        public BadRequestException(string message, IEnumerable<string?> errors) : base(message) => Errors = errors.ToArray();

        public BadRequestException(string message, Exception inner) : base(message, inner) { }
    }
}
