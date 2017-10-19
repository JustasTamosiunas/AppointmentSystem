using FluentValidation;

namespace AppointmentSystem.Domain
{
    public class AppointmentValidatorr : AbstractValidator<Appointment>
    {
	    public AppointmentValidatorr()
	    {
		    RuleFor(r => r.UserID).NotEmpty();
		    RuleFor(r => r.DateOfVisit).NotEmpty();
		    RuleFor(r => r.ProcedureIdList).NotEmpty();
	    }
    }
}