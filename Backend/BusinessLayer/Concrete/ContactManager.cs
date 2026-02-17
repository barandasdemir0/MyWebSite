using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.ContactDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public sealed class ContactManager :GenericManager<Contact,ContactDto,CreateContactDto,UpdateContactDto>, IContactService
{

    private readonly IContactDal _contactDal;

    public ContactManager(IContactDal contactDal, IMapper mapper) : base(contactDal, mapper)
    {
        _contactDal = contactDal;
    }

    public async Task<ContactDto?> GetSingleAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _contactDal.GetSingleAsync(cancellationToken);
        if (entity==null)
        {
            return null;
        }
        return _mapper.Map<ContactDto>(entity);
    }

    public async Task<ContactDto> SaveAsync(UpdateContactDto updateContactDto, CancellationToken cancellationToken = default)
    {
        var query = await _contactDal.GetSingleAsync(cancellationToken); //veritabanından about tablosundaki tek kaydı çekiyor
        if (query == null) //veritananında hiç about kaydı yoksa 
        {
            query = _mapper.Map<Contact>(updateContactDto);//about entity dönüşümü yapılıypr
            await _contactDal.AddAsync(query);
        }
        else //aksi takdirde kayıt varsa
        {
            _mapper.Map(updateContactDto, query); //mevcut entity alanlarını güncelliyorsun
            await _contactDal.UpdateAsync(query, cancellationToken);
        }
        await _contactDal.SaveAsync(cancellationToken); //işte burada add ve update yapıyor yaptığına göre
        return _mapper.Map<ContactDto>(query); //dbye kaydedilen dtoyu bize getirir
    }

}
