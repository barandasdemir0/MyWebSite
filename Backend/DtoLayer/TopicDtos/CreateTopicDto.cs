using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.TopicDtos;

public class CreateTopicDto
{
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
