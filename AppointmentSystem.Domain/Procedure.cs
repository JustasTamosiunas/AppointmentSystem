using System;

namespace AppointmentSystem.Domain
{
    public class Procedure
    {
		public string ProcedureID { get; set; }
        public string Name { get; set; }
		public TimeSpan Duration { get; set; }

	    public static Procedure Build (string name, string duration, string procedureId)
	    {
		    int minutes = Convert.ToInt32(duration);
		    return new Procedure
		    {
			    ProcedureID = procedureId,
			    Name = name,
			    Duration = new TimeSpan(0, minutes, 0)
		    };
	    }

	    public static Procedure Build(string name, string duration)
		{
		    int minutes = Convert.ToInt32(duration);
		    return new Procedure
		    {
			    Name = name,
			    Duration = new TimeSpan(0, minutes, 0)
			};
	    }
	}
}