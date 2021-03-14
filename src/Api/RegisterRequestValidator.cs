using FluentValidation;

namespace Api
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();
            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesValidator());
            RuleFor(x => x.Phone).SetInheritanceValidator(x =>
            {
                x.Add<USPhoneNumberDto>(new USPhoneNumberValidator());
                x.Add<InternationalPhoneNumberDto>(new InternationalPhoneNumberValidator());
            });
        }
    }

    public class USPhoneNumberValidator : AbstractValidator<USPhoneNumberDto>
    {
        public USPhoneNumberValidator()
        {
        }
    }

    public class InternationalPhoneNumberValidator : AbstractValidator<InternationalPhoneNumberDto>
    {
        public InternationalPhoneNumberValidator()
        {
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
