using System;

namespace AppointmentSystem.Domain
{
    public class Procedure
    {
		public string ProcedureID { get; set; }
        public string Name { get; set; }
		public DateTimeOffset Duration { get; set; }

	    public static Procedure Build (string procedureId, string name, string duration)
	    {
		    int minutes = Convert.ToInt32(duration);
		    return new Procedure
		    {
			    ProcedureID = procedureId,
			    Name = name,
			    Duration = new DateTimeOffset(new DateTime(0, 0, 0, minutes / 60, minutes, 0))
		    };
	    }

	    public static Procedure Build(string name, string duration)
		{
		    int minutes = Convert.ToInt32(duration);
		    return new Procedure
		    {
			    Name = name,
			    Duration = new DateTimeOffset(new DateTime(0, 0, 0, minutes / 60, minutes, 0))
		    };
	    }

	    public static int GetDurationInMinutes(DateTimeOffset duration)
	    {
		    return duration.Hour * 60 + duration.Minute;
	    }
	}
}