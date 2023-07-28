namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Get_Doctor_Response
    {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int CapacityPerDay { get; set; }
    }
}
