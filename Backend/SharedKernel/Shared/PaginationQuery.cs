namespace SharedKernel.Shared;

public class PaginationQuery
{

    private int _pageSize = 10;
    private const int MaxPageSize = 50;// Hard Limit (Memory DoS Koruması)

    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        // Saldırgan 99999 da gönderse otomatik olarak 50'ye çekilecek
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value < 1 ? 1 : value);
    }
    public Guid? TopicId { get; set; }
}
