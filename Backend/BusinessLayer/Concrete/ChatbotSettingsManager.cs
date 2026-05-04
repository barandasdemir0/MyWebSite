using BusinessLayer.Abstract;
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

    public ChatbotSettingsManager(IChatbotSettingsDal chatbotSettingsDal, IMapper mapper,IUnitOfWork unitOfWork) : base(chatbotSettingsDal, mapper, unitOfWork)
    {
        _chatbotSettingsDal = chatbotSettingsDal;

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
            await _repository.AddAsync(query, cancellationToken);
        }
        else
        {
            _mapper.Map(updateDto, query);
            await _repository.UpdateAsync(query, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ChatbotSettingsDto>(query);
    }
}
