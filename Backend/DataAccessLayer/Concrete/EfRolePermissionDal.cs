using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfRolePermissionDal : IRolePermissionDal
{
    private readonly AppDbContext _appDbContext;

    public EfRolePermissionDal(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _appDbContext.RolePermissions.Where(x => x.RoleName == roleName).Select(y => y.Permission).ToListAsync(cancellationToken); // verilen rollere göre veritabanında nelere izin verilmiş onları çekiuor
    }

    public async Task SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken) //rolleri sıfırlayıp yeniden inşa edecek metodumuz 
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken); //transaction başlatıyor işlem yarıda kalırsa rollback oluyor ve db bozulmuyıor

        var existing = await _appDbContext.RolePermissions.Where(x => x.RoleName == roleName).ToListAsync(cancellationToken); //rolleri listeliyor
        _appDbContext.RolePermissions.RemoveRange(existing); //permissonları siliyor remove range ile toplıuca siliyor liste içindeki tüm kayıtları siliyor
        foreach (var item in permissions) //yeni permissonları ekliyor
        {
            _appDbContext.RolePermissions.Add(new RolePermission // permissionu ekle
            {
                RoleName = roleName, //hangi role hangi özellik
                Permission = item
            });


        }
        await _appDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken); // yaptığım tüm değişikliği kalıcı olarak veritaanına yaz diyoruz commit async ile
    }
}
