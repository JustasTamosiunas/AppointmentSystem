using System.Collections.Generic;
using AppointmentSystem.Domain;

namespace AppointmentSystem.BusinessContracts
{
    public interface IUserService
    {
	    string CreateUser(User user);
	    bool UpdateUser(User user);
	    bool DeleteUser(string id);
	    List<User> GetAllUsers(string userId);
	    User GetUser(string id);
	}
}