using Hospital.Shared.Shared;


namespace Hospital.Domain.LoggerEntity
{
    public class Log : BaseEntity
    {
        public string Method { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string ContentType { get; set; }
        public string Headers { get; set; }
        public string Payload { get; set; }
        public string Response { get; set; }
        public string ResponseCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public DateTime RespondedOn { get; set; }
    }
}
