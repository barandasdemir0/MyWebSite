using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using System.Text;

namespace BusinessLayer.Concrete;

public class PortfolioContextManager : IPortfolioContextService
{
    private readonly IProjectDal _projectDal;
    private readonly IBlogPostDal _blogPostDal;
    private readonly IExperienceDal _experienceDal;
    private readonly IEducationDal _educationDal;
    private readonly IAboutDal _aboutDal;
    private readonly ICertificateDal _certificateDal;
    private readonly IHeroDal _heroDal;
    private readonly IGithubRepoDal _githubRepoDal;
    private readonly IJobSkillDal _jobSkillDal;
    private readonly IJobSkillCategoryDal _jobSkillCategoryDal;
    private readonly ISiteSettingsDal _siteSettingsDal;
    private readonly ISocialMediaDal _socialMediaDal;
    private readonly IContactDal _contactDal;
    private readonly ISkillDal _skillDal;

    public PortfolioContextManager(
        IProjectDal projectDal, IBlogPostDal blogPostDal, IExperienceDal experienceDal,
        IEducationDal educationDal, IAboutDal aboutDal, ICertificateDal certificateDal,
        IHeroDal heroDal, IGithubRepoDal githubRepoDal, IJobSkillDal jobSkillDal,
        IJobSkillCategoryDal jobSkillCategoryDal, ISiteSettingsDal siteSettingsDal,
        ISocialMediaDal socialMediaDal, IContactDal contactDal, ISkillDal skillDal)
    {
        _projectDal = projectDal;
        _blogPostDal = blogPostDal;
        _experienceDal = experienceDal;
        _educationDal = educationDal;
        _aboutDal = aboutDal;
        _certificateDal = certificateDal;
        _heroDal = heroDal;
        _githubRepoDal = githubRepoDal;
        _jobSkillDal = jobSkillDal;
        _jobSkillCategoryDal = jobSkillCategoryDal;
        _siteSettingsDal = siteSettingsDal;
        _socialMediaDal = socialMediaDal;
        _contactDal = contactDal;
        _skillDal = skillDal;
    }

    public async Task<string> BuildContextAsync(string currentUrl, string lowerQuestion)
    {
        var contextBuilder = new StringBuilder();
        string lowerUrl = currentUrl.ToLower();

        // ===== GENEL BİLGİLER (HER ZAMAN YÜKLENİR) =====
        var siteSettings = (await _siteSettingsDal.GetAllAsync()).FirstOrDefault();
        if (siteSettings != null)
        {
            contextBuilder.AppendLine("--- SİTE DURUM BİLGİSİ ---");
            if (siteSettings.IsAvailable)
                contextBuilder.AppendLine($"Baran şu anda yeni iş fırsatlarına ve projelere açıktır. Durum mesajı: {siteSettings.WorkStatus}");
            else
                contextBuilder.AppendLine("Baran şu anda aktif olarak yeni iş fırsatları aramamaktadır.");
        }

        // ===== SOSYAL MEDYA =====
        if (lowerQuestion.Contains("sosyal") || lowerQuestion.Contains("linkedin") || lowerQuestion.Contains("github") ||
            lowerQuestion.Contains("instagram") || lowerQuestion.Contains("twitter") || lowerQuestion.Contains("iletişim") ||
            lowerQuestion.Contains("ulaş") || lowerQuestion.Contains("takip") || lowerUrl.Contains("contact"))
        {
            var socials = await _socialMediaDal.GetAllAsync();
            if (socials.Any())
            {
                contextBuilder.AppendLine("--- SOSYAL MEDYA HESAPLARIM ---");
                foreach (var s in socials)
                    contextBuilder.AppendLine($"Baran'ın {s.SocialMediaName} hesabına şu adresten ulaşabilirsiniz: {s.SocialMediaUrl}");
            }
        }

        // ===== İLETİŞİM =====
        if (lowerUrl.Contains("contact") || lowerQuestion.Contains("iletişim") || lowerQuestion.Contains("email") ||
            lowerQuestion.Contains("mail") || lowerQuestion.Contains("telefon") || lowerQuestion.Contains("ulaş") ||
            lowerQuestion.Contains("adres") || lowerQuestion.Contains("konum"))
        {
            var contacts = await _contactDal.GetAllAsync();
            var contact = contacts.FirstOrDefault();
            if (contact != null)
            {
                contextBuilder.AppendLine("--- İLETİŞİM BİLGİLERİM ---");
                contextBuilder.AppendLine($"Baran'a e-posta ile {contact.Email} adresinden, telefon ile {contact.Phone} numarasından ulaşabilirsiniz. Şu anda {contact.Location} konumunda bulunmaktadır.");
            }
        }

        // 1. HERO & HAKKIMDA
        if (lowerUrl == "/" || lowerUrl.Contains("about") || lowerQuestion.Contains("kim") ||
            lowerQuestion.Contains("hakkında") || lowerQuestion.Contains("tanıt") ||
            lowerQuestion.Contains("neler yaparsın") || lowerQuestion.Contains("kendini") ||
            lowerQuestion.Contains("merhaba") || lowerQuestion.Contains("sen kimsin"))
        {
            var heroes = await _heroDal.GetAllAsync();
            var hero = heroes.FirstOrDefault();
            if (hero != null)
            {
                contextBuilder.AppendLine("--- GİRİŞ VE VİZYON (HERO) ---");
                contextBuilder.AppendLine($"Baran'ın profesyonel ünvanı '{hero.ProfessionalTitle}' olup, çalıştığı teknolojiler arasında {hero.ScrollingText} bulunmaktadır.");
            }
            var abouts = await _aboutDal.GetAllAsync();
            var about = abouts.FirstOrDefault();
            if (about != null)
            {
                contextBuilder.AppendLine("--- HAKKIMDA ---");
                contextBuilder.AppendLine($"Baran'ın tam adı {about.FullName}'dir ve yazılım sektöründe profesyonel olarak {about.ExperienceYear} yıllık deneyimi bulunmaktadır. Toplamda {about.ProjectCount} adet proje geliştirmiştir.");
                if (!string.IsNullOrEmpty(about.Bio))
                    contextBuilder.AppendLine($"Baran kendisini şöyle tanıtmaktadır: {about.Bio}");
                if (!string.IsNullOrEmpty(about.Greeting))
                    contextBuilder.AppendLine($"Karşılama mesajı: {about.Greeting}");
            }
        }

        // 2. İŞ YETENEKLERİ & TEKNOLOJİ STACK
        if (lowerUrl.Contains("skill") || lowerUrl == "/" || lowerUrl.Contains("about") ||
            lowerQuestion.Contains("yetenek") || lowerQuestion.Contains("teknoloji") ||
            lowerQuestion.Contains("dil") || lowerQuestion.Contains("stack") ||
            lowerQuestion.Contains("bildiğin") || lowerQuestion.Contains("kullandığın"))
        {
            var skills = await _jobSkillDal.GetAllAsync();
            var categories = await _jobSkillCategoryDal.GetAllAsync();
            contextBuilder.AppendLine("--- YETENEKLERİM VE TEKNOLOJİLER ---");
            foreach (var cat in categories)
            {
                var catSkills = skills.Where(s => s.JobSkillCategoryId == cat.Id).ToList();
                if (catSkills.Any())
                {
                    foreach (var s in catSkills)
                        contextBuilder.AppendLine($"Baran, {cat.CategoryName} kategorisinde yer alan {s.JobSkillName} teknolojisine %{s.JobSkillPercentage} oranında hakimdir.");
                }
            }

            var techSkills = await _skillDal.GetAllAsync();
            if (techSkills.Any())
                contextBuilder.AppendLine($"Baran'ın günlük iş akışında kullandığı araç ve teknolojiler şunlardır: {string.Join(", ", techSkills.Select(s => s.SkillName))}.");
        }

        // 3. EĞİTİM
        if (lowerUrl.Contains("education") || lowerUrl.Contains("resume") || lowerUrl == "/" ||
            lowerQuestion.Contains("okul") || lowerQuestion.Contains("eğitim") ||
            lowerQuestion.Contains("üniversite") || lowerQuestion.Contains("mezun") ||
            lowerQuestion.Contains("özgeçmiş"))
        {
            var educations = await _educationDal.GetAllAsync();
            contextBuilder.AppendLine("--- EĞİTİM GEÇMİŞİM ---");
            foreach (var edu in educations)
                contextBuilder.AppendLine($"Baran, {edu.EducationStartDate:yyyy} - {(edu.EducationFinishDate.HasValue ? edu.EducationFinishDate.Value.ToString("yyyy") : "Günümüz")} yılları arasında {edu.EducationSchoolName} okulunda {edu.EducationDegree} eğitimini almıştır.");
        }

        // 4. SERTİFİKALAR
        if (lowerUrl.Contains("certificate") || lowerUrl.Contains("resume") ||
            lowerQuestion.Contains("sertifika") || lowerQuestion.Contains("kurs") ||
            lowerQuestion.Contains("belge") || lowerQuestion.Contains("özgeçmiş"))
        {
            var certificates = await _certificateDal.GetAllAsync();
            contextBuilder.AppendLine("--- SERTİFİKALARIM ---");
            foreach (var cert in certificates)
                contextBuilder.AppendLine($"Baran, {cert.IssueDate:yyyy} yılında {cert.IssuingCompany} kurumundan '{cert.CertificateName}' sertifikasını başarıyla almıştır.");
        }

        // 5. DENEYİMLER
        if (lowerUrl.Contains("experience") || lowerUrl.Contains("resume") || lowerUrl == "/" ||
            lowerQuestion.Contains("tecrübe") || lowerQuestion.Contains("deneyim") ||
            lowerQuestion.Contains("çalıştın") || lowerQuestion.Contains("iş geçmişi") ||
            lowerQuestion.Contains("özgeçmiş") || lowerQuestion.Contains("kariyer"))
        {
            var experiences = await _experienceDal.GetAllAsync();
            contextBuilder.AppendLine("--- İŞ DENEYİMLERİM ---");
            foreach (var e in experiences)
                contextBuilder.AppendLine($"Baran, {e.ExperienceStartDate:yyyy} ile {(e.ExperienceFinishDate.HasValue ? e.ExperienceFinishDate.Value.ToString("yyyy") : "Günümüz")} yılları arasında {e.ExperienceCompanyName} şirketinde {e.ExperienceTitle} olarak görev yapmıştır. Bu pozisyondaki sorumlulukları: {e.ExperienceDescription}.");
        }

        // 6. PROJELER
        if (lowerUrl.Contains("project") || lowerQuestion.Contains("proje") ||
            lowerQuestion.Contains("uygulama") || lowerQuestion.Contains("geliştirdin") ||
            lowerQuestion.Contains("yaptığın"))
        {
            var projects = await _projectDal.GetAllAsync(x => x.IsPublished);
            contextBuilder.AppendLine("--- PROJELERİM ---");
            foreach (var p in projects)
            {
                var summary = !string.IsNullOrEmpty(p.AiSummary) ? p.AiSummary
                    : !string.IsNullOrEmpty(p.Description) ? Truncate(p.Description, 300)
                    : p.ShortDescription;
                contextBuilder.AppendLine($"Baran'ın geliştirdiği önemli projelerden biri '{p.Name}' projesidir. Bu projede {p.Technologies} teknolojilerini kullanmıştır. Projenin detayı şöyledir: {summary}.");
                if (!string.IsNullOrEmpty(p.GithubUrl))
                    contextBuilder.AppendLine($"  Bu projenin kaynak koduna {p.GithubUrl} adresinden ulaşılabilir.");
                if (!string.IsNullOrEmpty(p.WebsiteUrl))
                    contextBuilder.AppendLine($"  Bu projenin canlı demosunu {p.WebsiteUrl} adresinden inceleyebilirsiniz.");
            }
        }

        // 7. GITHUB REPOLARI
        if (lowerUrl.Contains("github") || lowerUrl.Contains("repo") ||
            lowerQuestion.Contains("github") || lowerQuestion.Contains("repo") ||
            lowerQuestion.Contains("kaynak kod") || lowerQuestion.Contains("açık kaynak"))
        {
            var repos = await _githubRepoDal.GetAllAsync();
            contextBuilder.AppendLine("--- GITHUB REPOLARIM ---");
            foreach (var repo in repos)
                contextBuilder.AppendLine($"Baran'ın açık kaynaklı projelerinden biri '{repo.RepoName}' adlı Github reposudur. {repo.Language} teknolojisi kullanılarak yazılan bu projenin amacı şudur: {repo.Description}.");
        }

        // 8. BLOGLAR
        if (lowerUrl.Contains("blog") || lowerQuestion.Contains("blog") ||
            lowerQuestion.Contains("makale") || lowerQuestion.Contains("yazı") ||
            lowerQuestion.Contains("özetle") || lowerQuestion.Contains("summarize") ||
            lowerQuestion.Contains("sayfa") || lowerQuestion.Contains("page"))
        {
            var blogs = await _blogPostDal.GetAllAsync(x => x.IsPublished);
            contextBuilder.AppendLine("--- BLOG YAZILARIM ---");
            foreach (var b in blogs)
            {
                var summary = !string.IsNullOrEmpty(b.AiSummary) ? b.AiSummary
                    : !string.IsNullOrEmpty(b.Content) ? Truncate(b.Content, 500)
                    : b.Title;
                contextBuilder.AppendLine($"Baran'ın yazdığı profesyonel makalelerden biri '{b.Title}' başlığını taşımaktadır. Bu makale {b.Technologies} teknolojileri hakkındadır ve yaklaşık {b.ReadTime} dakikada okunabilir. Makalenin özeti şöyledir: {summary}.");
            }
        }

        // ===== SAYFA BAZLI GENEL CONTEXT =====
        if (lowerQuestion.Contains("özetle") || lowerQuestion.Contains("summarize") ||
            lowerQuestion.Contains("bu sayfa") || lowerQuestion.Contains("this page"))
        {
            if (contextBuilder.Length < 100)
            {
                var abouts = await _aboutDal.GetAllAsync();
                var about = abouts.FirstOrDefault();
                if (about != null)
                    contextBuilder.AppendLine($"Baran Daşdemir, {about.ExperienceYear} yıllık deneyime sahip profesyonel bir yazılım geliştiricisidir. {about.Bio}");
            }
        }

        return contextBuilder.ToString();
    }

    private static string Truncate(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        text = System.Text.RegularExpressions.Regex.Replace(text, "<.*?>", " ");
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
        return text.Length <= maxLength ? text : text[..maxLength] + "...";
    }
}
