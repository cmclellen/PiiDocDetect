#nullable disable

namespace PiiDocIdentify.Functions.Dtos
{
    public class PiiDocumentInfoDto
    {
        public string DocumentType { get; set; }
        public string Address { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset DateOfExpiration { get; set; }
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
    }
}