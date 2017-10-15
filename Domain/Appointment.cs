using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Appointment
    {
		public string UserID { get; set; }
        public DateTime DateOfVisit { get; set; }
        public List<string> ProcedureIdList { get; set; }
		public string Remarks { get; set; }
    }
}
