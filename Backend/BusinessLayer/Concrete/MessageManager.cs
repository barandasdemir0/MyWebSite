using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.MessageDtos;
using MapsterMapper;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class MessageManager : GenericManager<Message,MessageDto,CreateMessageDto,UpdateMessageDto> ,IMessageService
{

    private readonly IMessageDal _messageDal;
    private readonly IEmailService _emailService;

    public MessageManager(IMessageDal messageDal, IMapper mapper, IEmailService emailService) : base(messageDal, mapper)
    {
        _messageDal = messageDal;
        _emailService = emailService;
    }

    public override async Task<MessageDto> AddAsync(CreateMessageDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var entity = _mapper.Map<Message>(dto);
        await _repository.AddAsync(entity,cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        try
        {
            if (dto.Folder == MessageFolder.Inbox)
            {
                // Ziyaretçi mesaj gönderdi → Admin'e bildirim maili
                var emailBody = $@"
                    <h3>Yeni Mesaj Bildirimi</h3>
                    <p><strong>Gönderen:</strong> {dto.SenderName} ({dto.SenderEmail})</p>
                    <p><strong>Konu:</strong> {dto.Subject}</p>
                    <hr/>
                    <p>{dto.Body}</p>
                    <hr/>
                    <p><em>Bu mesaj barandasdemir.com iletişim formundan gönderilmiştir.</em></p>";

                await _emailService.SendAsync("barandasdemir.bd@gmail.com", $"Yeni Mesaj : {dto.Subject} ", emailBody, cancellationToken);
            }

            else if (dto.Folder == MessageFolder.Sent)
            {
                var emailBody = $@"
                    <h3>{dto.Subject}</h3>
                    <p>{dto.Body}</p>
                    <hr/>
                    <p><em>Bu mesaj Baran Dasdemir tarafından gönderilmiştir.</em></p>
                    <p><em>Yanıtlamak için:barandasdemir.bd@gmail.com</em></p>";

                await _emailService.SendAsync(dto.ReceiverEmail, dto.Subject, emailBody, cancellationToken);
            }
            // Draft ise e-posta gönderilmez — sadece veritabanına kayıt
        }
        catch
        {
            // E-posta gönderilemese bile mesaj veritabanına kayıt edildi
            // Loglama yapılabilir ama mesaj kaybedilmez
        }
        return _mapper.Map<MessageDto>(entity);

    }

    public async Task<PagedResult<MessageListDto>> GetAllAdmin(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _messageDal.GetAdminListPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);
        return _mapper.Map<List<MessageListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }

    public async Task<PagedResult<MessageListDto>> GetByFolderAsync(MessageFolder folder, PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items,totalCount) = await _messageDal.GetByFolderPagesAsync(folder, paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);

        return _mapper.Map<List<MessageListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }

    public async Task<MessageDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetByIdAsync(guid, tracking: false, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<MessageDto>(entity);
    }

    public async Task<Dictionary<string, int>> GetFolderCountsAsync(CancellationToken cancellationToken = default)
    {
        return await _messageDal.GetFolderCountAsync(cancellationToken);
    }

    public async Task<PagedResult<MessageListDto>> GetReadAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _messageDal.GetReadPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);

        return _mapper.Map<List<MessageListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }

    public async Task<PagedResult<MessageListDto>> GetStarredAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _messageDal.GetStarredPagesAsync(paginationQuery.PageNumber, paginationQuery.PageSize, cancellationToken);

        return _mapper.Map<List<MessageListDto>>(items).ToPagedResult(paginationQuery.PageNumber, paginationQuery.PageSize, totalCount);
    }

    public async Task<bool> MarkAsReadAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity==null)
        {
            return false;
        }
        entity.IsRead = true;
        await _messageDal.UpdateAsync(entity, cancellationToken);
        await _messageDal.SaveAsync(cancellationToken);
        return true;
    }

 

    public async Task<MessageListDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.RestoreDeletedByIdAsync(guid, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _messageDal.UpdateAsync(entity, cancellationToken);
        await _messageDal.SaveAsync(cancellationToken);
        return _mapper.Map<MessageListDto>(entity);
    }

    public async Task<bool> ToggleStarAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _messageDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity==null)
        {
            return false;
        }
        entity.IsStarred = !entity.IsStarred;
        await _messageDal.UpdateAsync(entity, cancellationToken);
        await _messageDal.SaveAsync(cancellationToken);
        return true;
    }

    public override async Task<MessageDto?> UpdateAsync(Guid guid, UpdateMessageDto dto, CancellationToken cancellationToken = default)
    {
        throw new BusinessException("Mesajlar güncellenemez!");
    }
}
