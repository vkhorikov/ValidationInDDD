using System.Text.RegularExpressions;
using FluentValidation;

namespace Api
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesValidator());
            
            When(x => x.Email == null, () =>
            {
                RuleFor(x => x.Phone).NotEmpty();
            });
            When(x => x.Phone == null, () =>
            {
                RuleFor(x => x.Email).NotEmpty();
            });

            RuleFor(x => x.Email)
                .NotEmpty()
                .Length(0, 150)
                .EmailAddress()
                .When(x => x.Email != null, ApplyConditionTo.CurrentValidator);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches("^[2-9][0-9]{9}$")
                .When(x => x.Phone != null);




            //.Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}$"))
            //.When(x => x.Phone != null, ApplyConditionTo.CurrentValidator)
            //.WithMessage("The phone number is incorrect");
        }
    }

    public class StudentValidator : AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            RuleSet("Register", () =>
            {
                RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();
            });
            RuleSet("EditPersonalInfo", () =>
            {
                // No separate rules for EditPersonalInfo API yet
            });
            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesValidator());
        }
    }

    public class AddressesValidator : AbstractValidator<AddressDto[]>
    {
        public AddressesValidator()
        {
            RuleFor(x => x)
                .Must(x => x?.Length >= 1 && x.Length <= 3)
                .WithMessage("The number of addresses must be between 1 and 3")
                .ForEach(x =>
                {
                    x.NotNull();
                    x.SetValidator(new AddressValidator());
                });
        }
    }

    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty().Length(0, 100);
            RuleFor(x => x.City).NotEmpty().Length(0, 40);
            RuleFor(x => x.State).NotEmpty().Length(0, 2);
            RuleFor(x => x.ZipCode).NotEmpty().Length(0, 5);
        }
    }

    public class EditPersonalInfoRequestValidator : AbstractValidator<EditPersonalInfoRequest>
    {
        public EditPersonalInfoRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesValidator());
        }
    }
}
