using System.Collections.Generic;
using AppointmentSystem.Domain;

namespace AppointmentSystem.BusinessContracts
{
    public interface IAppointmentService
    {
	    string CreateAppointment(Appointment appointment);
	    bool UpdateAppointment(Appointment appointment);
	    bool DeleteAppointment(string id);
	    List<Appointment> GetAllAppointments(string appointmentId);
	    Appointment GetAppointment(string id);
	}
}