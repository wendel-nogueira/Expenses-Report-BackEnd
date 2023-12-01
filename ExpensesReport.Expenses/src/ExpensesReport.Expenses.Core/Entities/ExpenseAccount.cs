using ExpensesReport.Expenses.Core.Enums;
using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Expenses.Core.Entities
{
    public class ExpenseAccount : EntityBase
    {
        public ExpenseAccount(string name, string code, string description, AccountType type)
        {
            Name = name;
            Code = code;
            Description = description;
            Type = type;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public AccountType Type { get; set; }

        public void Update(string name, string code, string description, AccountType type)
        {
            Name = name;
            Code = code;
            Description = description;
            Type = type;
            UpdatedAt = DateTime.Now;
        }
    }
}
