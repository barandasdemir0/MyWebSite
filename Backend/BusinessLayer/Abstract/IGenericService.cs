using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGenericService</*TDto*/ TEntity, TListDto, TCreateDto, TUpdateDto> where TEntity : class
    {

        #region  getirme işlemleri okuma işlemleri

        Task<List<TListDto>> GetAllAsync();
        Task<TListDto?> GetByIdAsync(Guid guid);

        #endregion

        #region crud işlemleri
        Task<TListDto> AddAsync(TCreateDto dto);
        Task<TListDto?> UpdateAsync(Guid guid,TUpdateDto dto);
        Task DeleteAsync(Guid guid);

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
