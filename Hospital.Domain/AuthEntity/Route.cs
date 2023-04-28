using Hospital.Shared.Shared;

namespace Hospital.Domain.AuthEntity
{
    public class Route : BaseEntity
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public bool IpLimited { get; set; }
    }
}
