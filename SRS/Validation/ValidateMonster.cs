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

        public ValidationResult ValidateEmployeeCriticalInfo(Contractor contractorInformation)
        {
            ContractorCriticalErrorValidator validator = new ContractorCriticalErrorValidator(lookups);

            return validator.Validate(contractorInformation);
        }


        internal class ContractorCriticalErrorValidator : AbstractValidator<Contractor>
        {
            public ContractorCriticalErrorValidator(Dictionary<string, string[]> lookups)
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;


                #region Person

                //**********PERSON***********************************************************************************************
                RuleFor(Contractor => Contractor.Person.PersID)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required");

                RuleFor(Contractor => Contractor.Person.FirstName)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Contractor => Contractor.Person.LastName)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Contractor => Contractor.Person.MiddleName)
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Contractor => Contractor.Person.Suffix)
                    .MaximumLength(15)
                    .WithMessage($"{{PropertyName}} length must be 0-15");

                RuleFor(Contractor => Contractor.Person.SocialSecurityNumber)
                    .NotEmpty()
                    .WithMessage($"{{PropertyName}} is required")
                    .Length(9)
                    .WithMessage($"{{PropertyName}} length must be 9");

                Unless(e => string.IsNullOrEmpty(e.Person.Gender), () =>
                {
                    RuleFor(Contractor => Contractor.Person.Gender)
                        .Matches(@"^[mfMF]{1}$")
                        .WithMessage($"{{PropertyName}} must be one of these values: 'M', 'm', 'F', 'f'");
                });

                Unless(e => e.Person.ServiceComputationDateLeave.Equals(null), () =>
                {
                    RuleFor(Contractor => Contractor.Person.ServiceComputationDateLeave)
                        .ValidDate();
                });

                //RuleFor(Contractor => Contractor.Person.Region)
                //    .In(lookups["RegionCodes"]) 
                //    .MaximumLength(3)
                //    .WithMessage($"{{PropertyName}} length must be 0-3");

                //RuleFor(Contractor => Contractor.Person.JobTitle)
                //    .MaximumLength(70)
                //    .WithMessage($"{{PropertyName}} length must be 0-70");

                RuleFor(Contractor => Contractor.Person.HomeEmail)
                    .MaximumLength(64)
                    .WithMessage($"{{PropertyName}} length must be between 0-64");

                Unless(e => string.IsNullOrEmpty(e.Person.HomeEmail), () =>
                {
                    RuleFor(Contractor => Contractor.Person.HomeEmail)
                        .EmailAddress()
                        .WithMessage($"{{PropertyName}} must be a valid email address")
                        .Matches(@"(?i)^((?!gsa(ig)?.gov).)*$")
                        .WithMessage("Home email cannot end in gsa.gov. (Case Ignored)");
                });

                #endregion Person
                #region Address

                //***************************Address*******************************************************************
                RuleFor(Contractor => Contractor.Address.HomeAddress1)
                    //.NotEmpty()
                    //.WithMessage($"{{PropertyName}} is required")
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Contractor => Contractor.Address.HomeAddress2)
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Contractor => Contractor.Address.HomeAddress3)
                    .MaximumLength(60)
                    .WithMessage($"{{PropertyName}} length must be 0-60");

                RuleFor(Contractor => Contractor.Address.HomeCity)
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
                                RuleFor(Contractor => Contractor.Address.HomeState)
                                    //.NotEmpty()
                                    //.WithMessage($"{{PropertyName}} is required")
                                    .In(lookups["StateCodes"]);
                            });
                        });
                });

                RuleFor(Contractor => Contractor.Address.HomeZipCode)
                    .MaximumLength(10)
                    .WithMessage($"{{PropertyName}} length must be 0-10");

                Unless(e => string.IsNullOrEmpty(e.Address.HomeCountry), () =>
                {
                    RuleFor(Contractor => Contractor.Address.HomeCountry)
                    //.NotEmpty()
                    //.WithMessage($"{{PropertyName}} is required")
                    .In(lookups["CountryCodes"]);
                });

                #endregion Address

                #region Birth

                //******************************Birth***********************************************************************
                Unless(e => string.IsNullOrEmpty(e.Birth.CityOfBirth), () =>
                {
                    RuleFor(Contractor => Contractor.Birth.CityOfBirth)
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
                                RuleFor(Contractor => Contractor.Birth.StateOfBirth)
                                    .In(lookups["StateCodes"]);
                            });
                        });
                });

                Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfBirth), () =>
                {
                    RuleFor(Contractor => Contractor.Birth.CountryOfBirth)
                    .In(lookups["CountryCodes"]);
                });

                Unless(e => string.IsNullOrEmpty(e.Birth.CountryOfCitizenship), () =>
                {
                    RuleFor(Contractor => Contractor.Birth.CountryOfCitizenship)
                    .In(lookups["CountryCodes"]);
                });

                Unless(e => e.Birth.DateOfBirth.Equals(null), () =>
                {
                    RuleFor(Contractor => Contractor.Birth.DateOfBirth)
                    .ValidDate()
                    .WithMessage($"{{PropertyName}} must be valid date");
                });

                #endregion Birth
                #region Position

               // **********POSITION * *****************************************************************************************
                //RuleFor(Contractor => Contractor.Position.PositionControlNumber)
                //    .MaximumLength(15)
                //    .WithMessage($"{{PropertyName}} length must be 0-15");

                //RuleFor(Contractor => Contractor.Position.PositionOrganization)
                //    .MaximumLength(18)
                //    .WithMessage($"{{PropertyName}} length must be between 0-18");

                //RuleFor(Contractor => Contractor.Position.SupervisoryStatus)
                //    .MaximumLength(2)
                //    .WithMessage($"{{PropertyName}} length must be 0-2");

                //RuleFor(Contractor => Contractor.Position.PayPlan)
                //    .MaximumLength(3)
                //    .WithMessage($"{{PropertyName}} length must be 0-3");

                //RuleFor(Contractor => Contractor.Position.JobSeries)
                //    .MaximumLength(8)
                //    .WithMessage($"{{PropertyName}} length must be 0-8");

                //RuleFor(Contractor => Contractor.Position.PayGrade)
                //    .MaximumLength(3)
                //    .WithMessage($"{{PropertyName}} length must be between 0-3");

                //RuleFor(Contractor => Contractor.Position.WorkSchedule)
                //    .MaximumLength(1)
                //    .WithMessage($"{{PropertyName}} must be 0-1");

                //RuleFor(Contractor => Contractor.Position.PositionSensitivity)
                //    .MaximumLength(4)
                //    .WithMessage($"{{PropertyName}} length must be 0-4");

                //RuleFor(Contractor => Contractor.Position.DutyLocationCode)
                //    .MaximumLength(9)
                //    .WithMessage($"{{PropertyName}} length must be 0-9");

                //RuleFor(Contractor => Contractor.Position.DutyLocationCity)
                //    .MaximumLength(40)
                //    .WithMessage($"{{PropertyName}} length must be 0-40");

                //Unless(Contractor => string.IsNullOrEmpty(Contractor.Position.DutyLocationState), () =>
                //{
                //    RuleFor(Contractor => Contractor.Position.DutyLocationState)
                //        .In(lookups["StateCodes"]);
                //});

                //RuleFor(Contractor => Contractor.Position.DutyLocationCounty)
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

                //#endregion Position
                //#region Phone

                //**********PHONE*****************************************************************************************
                Unless(e => string.IsNullOrEmpty(e.Phone.HomePhone), () =>
                {
                    RuleFor(Contractor => Contractor.Phone.HomePhone)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                Unless(e => string.IsNullOrEmpty(e.Phone.HomeCell), () =>
                {
                    RuleFor(Contractor => Contractor.Phone.HomeCell)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                Unless(e => string.IsNullOrEmpty(e.Phone.WorkPhone), () =>
                {
                    RuleFor(Contractor => Contractor.Phone.WorkPhone)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                //Unless(e => string.IsNullOrEmpty(e.Phone.WorkFax), () =>
                //{
                    //RuleFor(Contractor => Contractor.Phone.WorkFax)
                    //.MaximumLength(24)
                    //.WithMessage($"{{PropertyName}} length must be 0-24")
                    //.ValidPhone()
                    //.WithMessage($"{{PropertyName}} must be a valid fax number");
                //});

                Unless(e => string.IsNullOrEmpty(e.Phone.WorkCell), () =>
                {
                    RuleFor(Contractor => Contractor.Phone.WorkCell)
                    .MaximumLength(24)
                    .WithMessage($"{{PropertyName}} length must be 0-24")
                    .ValidPhone()
                    .WithMessage($"{{PropertyName}} must be a valid phone number");
                });

                Unless(e => string.IsNullOrEmpty(e.Phone.WorkTextTelephone), () =>
                {
                    RuleFor(Contractor => Contractor.Phone.WorkTextTelephone)
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
