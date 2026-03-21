namespace EntityLayer.Entities;

public static class PermissionConsts
{
    public const string BlogCreate = "blog.create";
    public const string BlogEdit = "blog.edit";
    public const string BlogDelete = "blog.delete";
    public const string ProjectCreate = "project.create";
    public const string ProjectEdit = "project.edit";
    public const string ProjectDelete = "project.delete";
    public const string SkillManage = "skill.manage";
    public const string UserManage = "user.manage";
    public const string SiteSettings = "site.settings";
    // Key → Label eşlemesi
    private static readonly Dictionary<string, string> _labels = new()
    {
        [BlogCreate] = "Blog oluştur",
        [BlogEdit] = "Blog düzenle",
        [BlogDelete] = "Blog sil",
        [ProjectCreate] = "Proje oluştur",
        [ProjectEdit] = "Proje düzenle",
        [ProjectDelete] = "Proje sil",
        [SkillManage] = "Yetenekler",
        [UserManage] = "Kullanıcı yönetimi",
        [SiteSettings] = "Site ayarları",
    };
    /// <summary>
    /// Tüm permission'ları (Key, Label) çifti olarak döndürür.
    /// </summary>
    public static IReadOnlyList<(string Key, string Label)> GetAll()
    {
        return _labels.Select(x => (x.Key, x.Value)).ToList().AsReadOnly();
    }
}
