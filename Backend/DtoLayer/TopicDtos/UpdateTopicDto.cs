namespace DtoLayer.TopicDtos;

public class UpdateTopicDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
