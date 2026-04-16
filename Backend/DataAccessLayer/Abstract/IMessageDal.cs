using CV.EntityLayer.Entities;
using SharedKernel.Enums;

namespace DataAccessLayer.Abstract;

public interface IMessageDal:IGenericRepository<Message>
{
    //tüm mesajlar listeli
    Task<(List<Message> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default);

    //folder bazlı sayfalama ınbox send draft

    Task<(List<Message> Items, int TotalCount)> GetByFolderPagesAsync(MessageFolder folder, int page, int size, CancellationToken cancellationToken=default);

    //yıldızlı mesajları sayfalı getir

    Task<(List<Message> Items, int TotalCount)> GetStarredPagesAsync(int page, int size, CancellationToken cancellationToken = default);

    //okunmuş mesajları sayfalı getir

    Task<(List<Message> Items, int TotalCount)> GetReadPagesAsync(int page, int size, CancellationToken cancellationToken = default);


    //sidebar badge sayıları her klasör için kaç mesaj var mantığı
    Task<Dictionary<string, int>> GetFolderCountAsync(CancellationToken cancellationToken = default);

    //silinmiş mesajları restore et

    Task<Message?> RestoreDeletedByIdAsync(Guid guid, CancellationToken cancellationToken = default);


}
