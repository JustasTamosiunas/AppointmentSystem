using System.Collections.Generic;
using AppointmentSystem.Domain;

namespace AppointmentSystem.BusinessContracts
{
    public interface IProcedureService
    {
		string CreateProcedure(Procedure procedure);
	    bool UpdateProcedure(Procedure procedure);
	    bool DeleteProcedure(string id);
	    List<Procedure> GetAllProcedures();
	    Procedure GetProcedure(string id);
	}
}