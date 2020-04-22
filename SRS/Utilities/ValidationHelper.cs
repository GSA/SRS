﻿
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
        public enum Monster { MonsterFile = 2 };

        public ValidationHelper()
        {

        }

        public string GetErrors(IList<ValidationFailure> failures, Monster monster)
        {
            StringBuilder errors = new StringBuilder();

            foreach (var rule in failures)
            {
                errors.Append(rule.ErrorMessage.Remove(0, rule.ErrorMessage.IndexOf('.') + (int)monster));
                errors.Append(",");
            }

            return errors.ToString();
        }
    }
}
