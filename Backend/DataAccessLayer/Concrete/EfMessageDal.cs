using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

namespace DataAccessLayer.Concrete;

public class EfMessageDal : GenericRepository<Message>, IMessageDal
{
    public EfMessageDal(AppDbContext context) : base(context)
    {
    }

    public async Task<(List<Message> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        IQueryable<Message> messages = _context.Messages.AsNoTrackingWithIdentityResolution(); //sorguyu oluştur context.messages sonrası aşağıda bu sorgunun başı
        var totalCount = await messages.CountAsync(cancellationToken);//toplam sayıyı hesapla select count(*) from messages gibi

        var items = await messages
            .OrderByDescending(x => x.CreatedAt) //bunları sırala ama tersine yani en yeni mesaj en üstte
            .Skip((page - 1) * size) //önceki sayfaları atla demek bu 
            .Take(size) // her sayfada gösterilecek kayıtı al
            .ToListAsync(cancellationToken); //ve listele

        return (items, totalCount);
    }

    public async Task<(List<Message> Items, int TotalCount)> GetByFolderPagesAsync(MessageFolder folder, int page, int size, CancellationToken cancellationToken = default)
    {
        IQueryable<Message> query;

        if (folder == MessageFolder.Trash)
        {
            query = _context.Messages.IgnoreQueryFilters().AsNoTrackingWithIdentityResolution().Where(x => x.IsDeleted == true);
        }

        else
        {
            query = _context.Messages.AsNoTrackingWithIdentityResolution().Where(x => x.Folder == folder);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x=>x.IsRead) // Önce okunmamış mesajları sırala
            .ThenByDescending(x=>x.CreatedAt)// Sonra oluşturulma tarihine göre sırala
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items, totalCount);

 




    }

    public async Task<Dictionary<string, int>> GetFolderCountAsync(CancellationToken cancellationToken = default)
    {
        // Veritabanına TEK BİR KERE gider ve SQL tarafında "COUNT(CASE WHEN...)" sorguları oluşturur.
        var stats = await _context.Messages.IgnoreQueryFilters().AsNoTracking()
            .GroupBy(x => 1) // Hepsini tek bir grupta topla ki toplu hesap yapabilelim
            .Select(g => new
            {
                InboxCount = g.Count(x => x.Folder == MessageFolder.Inbox && x.IsRead == false && x.IsDeleted == false),
                ReadCount = g.Count(x => x.IsRead == true && x.IsDeleted == false),
                StarredCount = g.Count(x => x.IsStarred == true && x.IsDeleted == false),
                SentCount = g.Count(x => x.Folder == MessageFolder.Sent && x.IsDeleted == false),
                DraftCount = g.Count(x => x.Folder == MessageFolder.Draft && x.IsDeleted == false),
                TrashCount = g.Count(x => x.IsDeleted == true)
            })
            .FirstOrDefaultAsync(cancellationToken);
        var counts = new Dictionary<string, int>();

        if (stats != null)
        {
            counts[MessageFolder.Inbox.ToString()] = stats.InboxCount;
            counts["IsRead"] = stats.ReadCount;
            counts["IsStarred"] = stats.StarredCount;
            counts[MessageFolder.Sent.ToString()] = stats.SentCount;
            counts[MessageFolder.Draft.ToString()] = stats.DraftCount;
            counts[MessageFolder.Trash.ToString()] = stats.TrashCount;
        }
        return counts;
    }

    public async Task<(List<Message> Items, int TotalCount)> GetReadPagesAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        IQueryable<Message> query = _context.Messages.AsNoTrackingWithIdentityResolution().Where(x => x.IsRead == true);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<(List<Message> Items, int TotalCount)> GetStarredPagesAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        IQueryable<Message> query = _context.Messages.AsNoTrackingWithIdentityResolution().Where(x => x.IsStarred == true);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Message?> RestoreDeletedByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await _context.Messages.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
    }
}
