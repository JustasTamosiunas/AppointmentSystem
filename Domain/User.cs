namespace AppointmentSystem.Domain
{
    public class User
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
        public string UserRemarks { get; set; }
        public bool Admin { get; set; }
    }
}