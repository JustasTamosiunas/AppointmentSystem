using FluentValidation;

namespace AppointmentSystem.Domain
{
    public class UserValidator : AbstractValidator<User>
    {
	    public UserValidator()
	    {
		    RuleFor(r => r.UserID).NotEmpty();
			RuleFor(r => r.Email).NotEmpty();
		    RuleFor(r => r.Password).NotEmpty();
		    RuleFor(r => r.Name).NotEmpty();
		    RuleFor(r => r.Surname).NotEmpty();
	    }
    }
}