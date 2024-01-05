namespace ExpensesReport.Export.Application.ViewModels
{
    public class FileViewModel
    {
        public string? Name { get; set; }
        public string? Uri { get; set; }
        public string? ContentType { get; set; }
        public long? Size { get; set; }
        public Stream? Content { get; set; }
    }
}
