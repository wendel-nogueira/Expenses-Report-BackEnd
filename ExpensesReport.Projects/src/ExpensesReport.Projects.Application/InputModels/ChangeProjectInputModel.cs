namespace ExpensesReport.Projects.Application.InputModels
{
    public class ChangeProjectInputModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public Guid DepartamentId { get; set; }
    }
}
