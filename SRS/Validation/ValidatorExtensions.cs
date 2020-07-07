using System;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SRS.Validation
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> In<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, params TProperty[] validOptions)
        {
            return ruleBuilder
                .Must(validOptions.Contains)
                .WithMessage("{PropertyName} submitted is not valid");
        }

        public static IRuleBuilderOptions<T, TProperty> ValidDate<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            DateTime date;
            return ruleBuilder
                .Must(e => DateTime.TryParse(e.ToString(), out date))
                .WithMessage("{PropertyName} must be a valid date");
        }
 
    }
}
