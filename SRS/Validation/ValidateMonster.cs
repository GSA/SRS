using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using SRS.Models;
using SRS.Lookups;
using SRS.Mapping;
using SRS.Data;
using SRS.Utilities;

namespace SRS.Validation
{

    internal class ValidateMonster
    {

        private readonly Dictionary<string, string[]> lookups = new Dictionary<string, string[]>();

        public ValidateMonster(Lookup lookup)
        {

            lookups.Add("StateCodes", lookup.stateLookup.Select(s => s.Code).ToArray());
            lookups.Add("CountryCodes", lookup.countryLookup.Select(c => c.Code).ToArray());
            lookups.Add("RegionCodes", lookup.regionLookup.Select(c => c.Code).ToArray());
            lookups.Add("BuildingCodes", lookup.BuildingLookup.Select(c => c.BuildingId).ToArray());
        }

        public ValidationResult ValidateEmployeeCriticalInfo(Employee employeeInformation)
        {
            EmployeeCriticalErrorValidator validator = new EmployeeCriticalErrorValidator(lookups);

            return validator.Validate(employeeInformation);
        }


        internal class EmployeeCriticalErrorValidator : AbstractValidator<Employee>
        {
            public EmployeeCriticalErrorValidator(Dictionary<string, string[]> lookups)
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;


                #region Person

                //**********PERSON***********************************************************************************************
                RuleFor(Employee => Employee.Person.GCIMSID)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required");

                RuleFor(Employee => Employee.Person.FirstName)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Employee => Employee.Person.LastName)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Employee => Employee.Person.MiddleName)
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Employee => Employee.Person.Suffix)
                    .MaximumLength(15)
                    .WithMessage($"{{PropertyName}} length must be 0-15");

                RuleFor(Employee => Employee.Person.SocialSecurityNumber)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required")
                    .Length(9)
                    .WithMessage($"{{PropertyName}} length must be 9");

                Unless(e => string.IsNullOrEmpty(e.Person.Gender), () =>
                {
                    RuleFor(Employee => Employee.Person.Gender)
                        .Matches(@"^[mfMF]{1}$")
                        .WithMessage($"{{PropertyName}} must be one of these values: 'M', 'm', 'F', 'f'");
                });

                Unless(e => e.Person.ServiceComputationDateLeave.Equals(null), () =>
                {
                    RuleFor(Employee => Employee.Person.ServiceComputationDateLeave)
                        .ValidDate();
                });

                //RuleFor(Employee => Employee.Person.Region).In(lookups["RegionCodes"])
                //    .MaximumLength(3)
                //    .WithMessage($"{{PropertyName}} length must be 0-3");

                //RuleFor(Employee => Employee.Person.JobTitle)
                //    .MaximumLength(70)
                //    .WithMessage($"{{PropertyName}} length must be 0-70");

                RuleFor(Employee => Employee.Person.HomeEmail)
                    .MaximumLength(64)
                    .WithMessage($"{{PropertyName}} length must be between 0-64");

                Unless(e => string.IsNullOrEmpty(e.Person.HomeEmail), () =>
                {
                    RuleFor(Employee => Employee.Person.HomeEmail)
                        .EmailAddress()
                        .WithMessage($"{{PropertyName}} must be a valid email address")
                        .Matches(@"(?i)^((?!gsa(ig)?.gov).)*$")
                        .WithMessage("Home email cannot end in gsa.gov. (Case Ignored)");
                });

                #endregion Person
                #region Address

                //***************************Address*******************************************************************
                RuleFor(Employee => Employee.Address.HomeAddress1)
                    //.NotEmpty()
                    //.WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Employee => Employee.Address.HomeAddress2)
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Employee => Employee.Address.HomeAddress3)
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Employee => Employee.Address.HomeCity)
                    //.NotEmpty()
                    //.WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(50)
                    .WithMessage($"{{PropertyName}} length must be 0-50");

                Unless(e => string.IsNullOrEmpty(e.Address.HomeCountry), () =>
                {
                    When(e => e.Address.HomeCountry.ToLower().Equals("us") ||
                        e.Address.HomeCountry.ToLower().Equals("ca") ||
                        e.Address.HomeCountry.ToLower().Equals("mx"), () =>
                        {
                            Unless(e => string.IsNullOrEmpty(e.Address.HomeState), () =>
                            {
                                RuleFor(Employee => Employee.Address.HomeState)
                                    //.NotEmpty()
                                    //.WithMessage($"{{PropertyName}} is required")
                                    .In(lookups["StateCodes"]);
                            });
                        });
                });

                RuleFor(Employee => Employee.Address.HomeZipCode)
                    .MaximumLength(10)
                    .WithMessage($"{{PropertyName}} length must be 0-10");

                Unless(e => string.IsNullOrEmpty(e.Address.HomeCountry), () =>
                {
                    RuleFor(Employee => Employee.Address.HomeCountry)
                    //.NotEmpty()
                    //.WithMessage($"{{PropertyName}} is required")
                    .In(lookups["CountryCodes"]);
                });

                #endregion Address

                #region Birth

                //******************************Birth***********************************************************************
                Unless(e => string.IsNullOrEmpty(e.Birth.CityOfBirth), () =>
                {
                    RuleFor(Employee => Employee.Birth.CityOfBirth)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24");
                });

                Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfBirth), () =>
                {
                    When(e => e.Birth.CountryOfBirth.ToLower().Equals("us") ||
                        e.Address.HomeCountry.ToLower().Equals("ca") ||
                        e.Address.HomeCountry.ToLower().Equals("mx"), () =>
                        {
                            Unless(e => string.IsNullOrEmpty(e.Birth.StateOfBirth), () =>
                            {
                                RuleFor(Employee => Employee.Birth.StateOfBirth)
                                    .In(lookups["StateCodes"]);
                            });
                        });
                });

                Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfBirth), () =>
                {
                    RuleFor(Employee => Employee.Birth.CountryOfBirth)
                    .In(lookups["CountryCodes"]);
                });

                Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfCitizenship), () =>
                {
                    RuleFor(Employee => Employee.Birth.CountryOfCitizenship)
                    .In(lookups["CountryCodes"]);
                });

                Unless(e => e.Birth.DateOfBirth.Equals(null), () =>
                {
                    RuleFor(Employee => Employee.Birth.DateOfBirth)
                    .ValidDate()
                    .WithMessage($"{{PropertyName}} must be valid date");
                });

                #endregion Birth
                #region Position

                //**********POSITION******************************************************************************************
                //RuleFor(Employee => Employee.Position.PositionControlNumber)
                //    .MaximumLength(15)
                //    .WithMessage($"{{PropertyName}} length must be 0-15");

                //RuleFor(Employee => Employee.Position.PositionOrganization)
                //    .MaximumLength(18)
                //    .WithMessage($"{{PropertyName}} length must be between 0-18");

                //RuleFor(Employee => Employee.Position.SupervisoryStatus)
                //    .MaximumLength(2)
                //    .WithMessage($"{{PropertyName}} length must be 0-2");

                //RuleFor(Employee => Employee.Position.PayPlan)
                //    .MaximumLength(3)
                //    .WithMessage($"{{PropertyName}} length must be 0-3");

                //RuleFor(Employee => Employee.Position.JobSeries)
                //    .MaximumLength(8)
                //    .WithMessage($"{{PropertyName}} length must be 0-8");

                //RuleFor(Employee => Employee.Position.PayGrade)
                //    .MaximumLength(3)
                //    .WithMessage($"{{PropertyName}} length must be between 0-3");

                //RuleFor(Employee => Employee.Position.WorkSchedule)
                //    .MaximumLength(1)
                //    .WithMessage($"{{PropertyName}} must be 0-1");

                //RuleFor(Employee => Employee.Position.PositionSensitivity)
                //    .MaximumLength(4)
                //    .WithMessage($"{{PropertyName}} length must be 0-4");

                //RuleFor(Employee => Employee.Position.DutyLocationCode)
                //    .MaximumLength(9)
                //    .WithMessage($"{{PropertyName}} length must be 0-9");

                //RuleFor(Employee => Employee.Position.DutyLocationCity)
                //    .MaximumLength(40)
                //    .WithMessage($"{{PropertyName}} length must be 0-40");

                //Unless(Employee => string.IsNullOrEmpty(Employee.Position.DutyLocationState), () =>
                //{
                //    RuleFor(Employee => Employee.Position.DutyLocationState)
                //        .In(lookups["StateCodes"]);
                //});

                //RuleFor(Employee => Employee.Position.DutyLocationCounty)
                //    .MaximumLength(40)
                //    .WithMessage($"{{PropertyName}} must be 0-40");

                //Unless(Employee => Employee.Position.PositionStartDate.Equals(null), () =>
                //{
                //    RuleFor(Employee => Employee.Position.PositionStartDate)
                //    .ValidDate();
                //});

                //RuleFor(Employee => Employee.Position.AgencyCodeSubelement)
                //    .MaximumLength(4)
                //    .WithMessage($"{{PropertyName}} length must be 0-4");

                #endregion Position
                #region Phone

                //**********PHONE*****************************************************************************************
                Unless(e => string.IsNullOrEmpty(e.Phone.HomePhone), () =>
                {
                    RuleFor(Employee => Employee.Phone.HomePhone)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                Unless(e => string.IsNullOrEmpty(e.Phone.HomeCell), () =>
                {
                    RuleFor(Employee => Employee.Phone.HomeCell)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                Unless(e => string.IsNullOrEmpty(e.Phone.WorkPhone), () =>
                {
                    RuleFor(Employee => Employee.Phone.WorkPhone)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                //Unless(e => string.IsNullOrEmpty(e.Phone.WorkFax), () =>
                //{ 
                //    RuleFor(Employee => Employee.Phone.WorkFax)
                //    .MaximumLength(24)
                //    .WithMessage($"{{PropertyName}} length must be 0-24")
                //    .ValidPhone()
                //    .WithMessage($"{{PropertyName}} must be a valid phone number");
                //});

                Unless(e => string.IsNullOrEmpty(e.Phone.WorkCell), () =>
                {
                    RuleFor(Employee => Employee.Phone.WorkCell)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                Unless(e => string.IsNullOrEmpty(e.Phone.WorkTextTelephone), () =>
                {
                    RuleFor(Employee => Employee.Phone.WorkTextTelephone)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                #endregion Phone

            }
        }
    }
}
