
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Utilities
{
    internal class ValidationHelper
    {
        public enum Contractor { ContractorFile = 2 };

        public ValidationHelper()
        {

        }

        public string GetErrors(IList<ValidationFailure> failures, Contractor contractor)
        {
            StringBuilder errors = new StringBuilder();

            foreach (var rule in failures)
            {
                errors.Append(rule.ErrorMessage.Remove(0, rule.ErrorMessage.IndexOf('.') + (int)contractor));
                errors.Append(",");
            }

            return errors.ToString();
        }
    }
}
