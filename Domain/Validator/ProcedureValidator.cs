using FluentValidation;

namespace AppointmentSystem.Domain
{
    public class ProcedureValidator : AbstractValidator<Procedure>
	{
	    public ProcedureValidator()
	    {
		    RuleFor(r => r.Name).NotEmpty();
		    RuleFor(r => r.Duration).NotEmpty();
	    }
	}
}