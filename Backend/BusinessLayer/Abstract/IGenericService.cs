using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGenericService</*TDto*/ TEntity, TListDto, TCreateDto, TUpdateDto> where TEntity : class //hangi entity üzerinde çalışıyorum / veri österirken hangi dto kullanılacak /
        // veri oluştururken hangi dto kullanılacak / güncellenirken hangi dto kullanılacak
    {

        #region  getirme işlemleri okuma işlemleri

        Task<List<TListDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TListDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default);

        #endregion
        // neden tlistdto kullanıcıya name category add ve updatede gösterilir ama geri dönerken atanan tüm bilgileri görürken kullanıcı silinme güncellenme durumlarını görüyor ve bu silme güncellenme durumu create ve update bağlı bir durum değil
        #region crud işlemleri
        Task<TListDto> AddAsync(TCreateDto dto, CancellationToken cancellationToken = default);
        Task<TListDto?> UpdateAsync(Guid guid,TUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default);

        #endregion

        #region eski kodlar

        // yukarıdaki tdto kısmını sildik sebebi veri girerken tdto dto yapıyorduk ama böyle tcreatedto dupdatedto yaptık
        // ve neden tlistdto döndürdük about tablosunda örnek vermek gerekirse a b c d e alanları var createde b c d alanlarını aldık ama döndürürken a b c d e alanlarını döndürüypooruz yani tlistdtoda ne varsa daha doğrusu 




        //Task<List<TDto>> GetAllAsync();
        //Task<TDto?> GetByIdAsync(Guid guid);

        //Task<TDto> AddAsync(TDto dto);
        //Task<TDto> UpdateAsync(TDto dto);
        //Task DeleteAsync(Guid guid);

        #endregion
    }
}
