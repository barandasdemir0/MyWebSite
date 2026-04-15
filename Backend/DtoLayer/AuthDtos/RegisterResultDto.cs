namespace DtoLayer.AuthDtos;

public class RegisterResultDto
{
    public bool Success { get; set; }
    public List<string>? Errors { get; set; }
}
