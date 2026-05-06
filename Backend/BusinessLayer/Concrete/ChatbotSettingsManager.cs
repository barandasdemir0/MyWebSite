using BusinessLayer.Abstract;
using BusinessLayer.Services;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.ChatbotSettingsDtos;
using MapsterMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessLayer.Concrete;

public class ChatbotSettingsManager : GenericManager<ChatbotSettings,ChatbotSettingsDto,CreateChatbotSettingsDto,UpdateChatbotSettingsDto> ,IChatbotSettingsService
{
    private readonly IChatbotSettingsDal _chatbotSettingsDal;
    private readonly EncryptionService _encryptionService;

    public ChatbotSettingsManager(IChatbotSettingsDal chatbotSettingsDal, IMapper mapper, IUnitOfWork unitOfWork, EncryptionService encryptionService) : base(chatbotSettingsDal, mapper, unitOfWork)
    {
        _chatbotSettingsDal = chatbotSettingsDal;
        _encryptionService = encryptionService;
    }

    public async Task<ChatbotSettingsDto?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _chatbotSettingsDal.GetActiveSettingsAsync();
        if (entity==null)
        {
            return null;
        }
        return _mapper.Map<ChatbotSettingsDto>(entity);
    }

    public async Task<ChatbotSettingsDto> SaveAsync(UpdateChatbotSettingsDto updateDto, CancellationToken cancellationToken = default)
    {
        var query = await _chatbotSettingsDal.GetActiveSettingsAsync();
        if (query==null)
        {
            query = _mapper.Map<ChatbotSettings>(updateDto);
            query.ApiKey = _encryptionService.Encrypt(updateDto.ApiKey ?? string.Empty);
            await _repository.AddAsync(query, cancellationToken);
        }
        else
        {
            _mapper.Map(updateDto, query);
            query.ApiKey = _encryptionService.Encrypt(updateDto.ApiKey ?? string.Empty);
            await _repository.UpdateAsync(query, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ChatbotSettingsDto>(query);
    }
}
