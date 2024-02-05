namespace PiiDocIdentify.Functions.Dtos
{
    public class PiiDetectRequestDto
    {
        public List<ValueDto> Values { get; set; } = new();
    }

    public class ValueDto
    {
        public int RecordId { get; set; }
        public DataDto Data { get; set; } = null!;
    }

    public class DataDto
    {
        public ImageDto Image { get; set; } = null!;
    }

    public class ImageDto
    {
        public string Data { get; set; } = null!;
    }
}