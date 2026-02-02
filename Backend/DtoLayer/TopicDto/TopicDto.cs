using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.TopicDto;

public class TopicDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsDeleted { get; set; } = false;
}
