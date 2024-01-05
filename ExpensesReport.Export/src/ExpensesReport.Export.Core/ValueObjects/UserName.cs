﻿namespace ExpensesReport.Export.Core.ValueObjects
{
    public class UserName
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
