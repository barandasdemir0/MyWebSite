using CV.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Abstract
{
    public interface IProjectDal:IGenericRepository<Project>
    {
        Task<Project?> RestoreDeleteByIdAsync(Guid guid);
        Task<(List<Project> Items, int TotalCount)> GetAdminListPagesAsync(int page, int size, Guid? topicId);
        //biz burada task ile asenkron çalıştırmayı aldık burada listprojectin içinde projeleri listesini ıtems olarak döndürdük totalcount ile toplam veriyi aldırdık ardından int page ile sayfa sayısı sayfa 1 sayfa 2 vs int size ile sayfadaki veri sayısını belirledik guid ilede topicıd ile filtrelemeyi seçtik 
    }
}
