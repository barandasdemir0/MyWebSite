using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Concrete;

public class UserAdminManager : IUserAdminService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserDal _userDal;
    private readonly IMapper _mapper;

    public UserAdminManager(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUserDal userDal, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userDal = userDal;
        _mapper = mapper;
    }
    // türkçe: Kullanıcıyı onayla, reddet, rol ata ve kullanıcıları listele işlemlerini gerçekleştiren metotlar
    public async Task<bool> ApproveUserAsync(string userId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId); // Kullanıcıyı ID'sine göre bul
        if (user == null)
        {
            return false;
        }// Kullanıcıyı onayla
        user.IsApproved = true;
        user.EmailConfirmed = true; // E-posta onayını da true yaparak kullanıcıyı aktif hale getir
        await _userManager.UpdateAsync(user); // Kullanıcıya rol atama işlemi

        var currentRoles = await _userManager.GetRolesAsync(user); // Kullanıcının mevcut rollerini al
        await _userManager.RemoveFromRolesAsync(user, currentRoles); // Kullanıcının mevcut rollerini kaldır
        await _userManager.AddToRoleAsync(user, role); // Kullanıcıya yeni rolü ata

        return true; // İşlemin başarılı olduğunu döndür
    }


    // Türkçe: Kullanıcıya belirli bir rol atayan metot
    public async Task<bool> AssignRoleAsync(string UserId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(UserId); // Kullanıcıyı ID'sine göre bul
        if (user == null) // Eğer kullanıcı bulunamazsa false döndür
        {
            return false;
        }
        if (!await _roleManager.RoleExistsAsync(role)) // Eğer rol mevcut değilse yeni rol oluştur
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid> // Yeni rol oluştur
            {
                Name = role // Rol adı olarak parametre olarak verilen rolü kullan
            });

        }

        var currentRoles = await _userManager.GetRolesAsync(user); // Kullanıcının mevcut rollerini al
        await _userManager.RemoveFromRolesAsync(user, currentRoles); // Kullanıcının mevcut rollerini kaldır
        await _userManager.AddToRoleAsync(user, role); // Kullanıcıya yeni rolü ata
        return true;// İşlemin başarılı olduğunu döndür
    }


    // Türkçe: Onaylanmış kullanıcıları listeleyen metot
    public async Task<List<ApprovedUserDto>> GetAllUserAsync(CancellationToken cancellationToken)
    {
        var users = await _userDal.GetApprovedUserAsync(cancellationToken); // Onaylanmış kullanıcıları veritabanından çek
        var userIds = users.Select(u => u.Id).ToList(); // Kullanıcı ID'lerini bir listeye dönüştür
        var rolesMap = await _userDal.GetUserRolesBatchAsync(userIds, cancellationToken); // Kullanıcı ID'lerine göre rollerin toplu olarak çekildiği bir metot çağırarak kullanıcı-roller eşlemesini al

        // Kullanıcıları DTO'lara dönüştürürken rollerini de ekleyerek liste oluştur
        return users.Select(item =>
        {
            // Her kullanıcı için DTO'ya dönüştürme işlemi yaparken, rollerini de ekleyerek yeni bir DTO oluştur
            var dto = _mapper.Map<ApprovedUserDto>(item);
            dto = dto with // C# 9.0 record tipi ile immutability sağlanarak yeni bir DTO oluşturuluyor with ifadesi ile mevcut DTO'nun diğer özelliklerini koruyarak sadece Role özelliği güncelleniyor
            {
                Role = rolesMap.TryGetValue(item.Id, out var roles) ? roles.FirstOrDefault() ?? RoleConsts.User : RoleConsts.User // Kullanıcının rollerini kontrol ederek, eğer roller varsa ilk rolü atar, yoksa varsayılan olarak "User" rolünü atar
            };
            return dto;

        }).ToList();



    }


    // Türkçe: Onaylanmamış kullanıcıları listeleyen metot
    public async Task<List<PendingUserDto>> GetPendingUsersAsync(CancellationToken cancellationToken)
    {
        // Onaylanmamış kullanıcıları veritabanından çek ve DTO'lara dönüştürerek liste olarak döndür
        var user = await _userDal.GetPendingUserAsync(cancellationToken);
        return _mapper.Map<List<PendingUserDto>>(user); // Veritabanından çekilen kullanıcıları DTO'lara dönüştürerek liste olarak döndür
    }


    // Türkçe: Kullanıcıyı reddeden metot
    public async Task<bool> RejectUserAsync(string userId, CancellationToken cancellationToken)
    {
        // Kullanıcıyı ID'sine göre bul
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {// Eğer kullanıcı bulunamazsa false döndür
            return false;
        }
        var result = await _userManager.DeleteAsync(user);// Kullanıcıyı silme işlemi yaparak reddet
        return result.Succeeded;// Silme işleminin başarılı olup olmadığını döndür
    }
}
