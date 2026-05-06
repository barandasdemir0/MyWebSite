using DtoLayer.LogDtos;

namespace WebUILayer.Areas.Admin.Models;

public class LogsIndexViewModel:BasePaginationViewModel
{
    public List<ResultLogDto> ResultLogDtos { get; set; } = new();
}
