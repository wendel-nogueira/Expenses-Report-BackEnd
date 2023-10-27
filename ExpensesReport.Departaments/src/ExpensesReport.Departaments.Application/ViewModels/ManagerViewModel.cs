using ExpensesReport.Departaments.Core.Entities;

namespace ExpensesReport.Departaments.Application.ViewModels
{
    public class ManagerViewModel(IEnumerable<Guid> managersId)
    {
        public IEnumerable<Guid> ManagersId { get; set; } = managersId;

        public static ManagerViewModel FromEntity(IEnumerable<Manager> managers)
        {
            return new ManagerViewModel(managers.Select(manager => manager.ManagerId));
        }
    }
}
