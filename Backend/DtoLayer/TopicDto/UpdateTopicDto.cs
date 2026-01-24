using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.TopicDto;

public class UpdateTopicDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
