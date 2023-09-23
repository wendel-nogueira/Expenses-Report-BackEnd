﻿using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Core.ValueObjects
{
    public record UserName
    {
        public UserName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            Validate();
        }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must be between 2 and 50 characters", MinimumLength = 2)]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must be between 2 and 50 characters", MinimumLength = 2)]
        public string LastName { get; init; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public void Validate()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, context, results, true))
                throw new Exception(string.Join(" | ", results.Select(x => x.ErrorMessage)));
        }
    }
}