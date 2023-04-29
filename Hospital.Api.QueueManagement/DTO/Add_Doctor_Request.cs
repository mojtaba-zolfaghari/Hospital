namespace Hospital.Api.QueueManagement.DTO
{
    public class Add_Doctor_Request
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CapacityPerDay { get; set; }
    }
}
