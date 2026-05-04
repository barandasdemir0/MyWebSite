using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Microsoft.Extensions.Logging;
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

    public PortfolioContextManager(IProjectDal projectDal, IBlogPostDal blogPostDal, IExperienceDal experienceDal, IEducationDal educationDal, IAboutDal aboutDal, ICertificateDal certificateDal, IHeroDal heroDal, IGithubRepoDal githubRepoDal, IJobSkillDal jobSkillDal, IJobSkillCategoryDal jobSkillCategoryDal)
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
    }

    public async Task<string> BuildContextAsync(string currentUrl, string lowerQuestion)
    {
        var contextBuilder = new StringBuilder();
        string lowerUrl = currentUrl.ToLower();
        // 1. HERO & HAKKIMDA
        if (lowerUrl == "/" || lowerUrl.Contains("about") || lowerQuestion.Contains("kim") || lowerQuestion.Contains("hakkında") || lowerQuestion.Contains("tanıt") || lowerQuestion.Contains("neler yaparsın"))
        {
            var heroes = await _heroDal.GetAllAsync();
            var hero = heroes.FirstOrDefault();
            if (hero != null)
            {
                contextBuilder.AppendLine("--- GİRİŞ VE VİZYON (HERO) ---");
                contextBuilder.AppendLine($"- Ünvanı: {hero.ProfessionalTitle}");
                contextBuilder.AppendLine($"- Ve Çalıştığı teknolojiler: {hero.ScrollingText}");
            }
            var abouts = await _aboutDal.GetAllAsync();
            var about = abouts.FirstOrDefault();
            if (about != null)
            {
                contextBuilder.AppendLine($"Baran'ın tam adı {about.FullName}'dir ve yazılım sektöründe profesyonel olarak {about.ExperienceYear} yıllık deneyimi bulunmaktadır.");
            }
        }
        // 2. İŞ YETENEKLERİ
        if (lowerUrl.Contains("skill") || lowerUrl.Contains("yetenek") || lowerUrl == "/" || lowerQuestion.Contains("yetenek") || lowerQuestion.Contains("teknoloji") || lowerQuestion.Contains("dil"))
        {
            var skills = await _jobSkillDal.GetAllAsync();
            var categories = await _jobSkillCategoryDal.GetAllAsync();
            contextBuilder.AppendLine("--- YETENEKLERİM VE TEKNOLOJİLER ---");
            foreach (var s in skills)
            {
                var cat = categories.FirstOrDefault(c => c.Id == s.JobSkillCategoryId);
                string catName = cat != null ? cat.CategoryName : "Genel";
                contextBuilder.AppendLine($"Baran, {catName} kategorisinde yer alan {s.JobSkillName} teknolojisine %{s.JobSkillPercentage} oranında yüksek seviyede hakimdir.");
            }
        }
        // 3. EĞİTİM
        if (lowerUrl.Contains("education") || lowerUrl.Contains("eğitim") || lowerUrl == "/" || lowerQuestion.Contains("okul") || lowerQuestion.Contains("eğitim") || lowerQuestion.Contains("üniversite") || lowerQuestion.Contains("mezun"))
        {
            var educations = await _educationDal.GetAllAsync();
            contextBuilder.AppendLine("--- EĞİTİM GEÇMİŞİM ---");
            foreach (var edu in educations)
            {
                contextBuilder.AppendLine($"Baran, {edu.EducationStartDate:yyyy} - {(edu.EducationFinishDate.HasValue ? edu.EducationFinishDate.Value.ToString("yyyy") : "Günümüz")} yılları arasında {edu.EducationSchoolName} okulunda {edu.EducationDegree} eğitimini almıştır.");
            }
        }
        // 4. SERTİFİKALAR
        if (lowerUrl.Contains("certificate") || lowerUrl.Contains("sertifika") || lowerQuestion.Contains("sertifika") || lowerQuestion.Contains("kurs") || lowerQuestion.Contains("belge"))
        {
            var certificates = await _certificateDal.GetAllAsync();
            contextBuilder.AppendLine("--- SERTİFİKALARIM ---");
            foreach (var cert in certificates)
            {
                contextBuilder.AppendLine($"Baran, {cert.IssueDate:yyyy} yılında {cert.IssuingCompany} kurumundan '{cert.CertificateName}' sertifikasını başarıyla almıştır.");
            }
        }
        // 5. DENEYİMLER
        if (lowerUrl.Contains("experience") || lowerUrl == "/" || lowerQuestion.Contains("tecrübe") || lowerQuestion.Contains("deneyim") || lowerQuestion.Contains("çalıştın") || lowerQuestion.Contains("iş geçmişi"))
        {
            var experiences = await _experienceDal.GetAllAsync();
            contextBuilder.AppendLine("--- İŞ DENEYİMLERİM ---");
            foreach (var e in experiences)
            {
                contextBuilder.AppendLine($"Baran, {e.ExperienceStartDate:yyyy} ile {(e.ExperienceFinishDate.HasValue ? e.ExperienceFinishDate.Value.ToString("yyyy") : "Günümüz")} yılları arasında {e.ExperienceCompanyName} şirketinde {e.ExperienceTitle} olarak görev yapmıştır. Bu pozisyondaki sorumlulukları: {e.ExperienceDescription}.");
            }
        }
        // 6. PROJELER
        if (lowerUrl.Contains("project") || lowerQuestion.Contains("proje") || lowerQuestion.Contains("uygulama") || lowerQuestion.Contains("geliştirdin"))
        {
            var projects = await _projectDal.GetAllAsync(x => x.IsPublished);
            contextBuilder.AppendLine("--- PROJELERİM ---");
            foreach (var p in projects)
            {
                var summary = !string.IsNullOrEmpty(p.AiSummary) ? p.AiSummary : p.ShortDescription;
                contextBuilder.AppendLine($"Baran'ın geliştirdiği önemli projelerden biri '{p.Name}' projesidir. Bu projede {p.Technologies} teknolojilerini kullanmıştır. Projenin detayı şöyledir: {summary}.");
            }
        }
        // 7. GITHUB REPOLARI
        if (lowerUrl.Contains("github") || lowerUrl.Contains("repo") || lowerQuestion.Contains("github") || lowerQuestion.Contains("repo") || lowerQuestion.Contains("kaynak kod") || lowerQuestion.Contains("açık kaynak"))
        {
            var repos = await _githubRepoDal.GetAllAsync();
            contextBuilder.AppendLine("--- GITHUB REPOLARIM ---");
            foreach (var repo in repos)
            {
                contextBuilder.AppendLine($"Baran'ın açık kaynaklı projelerinden biri '{repo.RepoName}' adlı Github reposudur. {repo.Language} teknolojisi kullanılarak yazılan bu projenin amacı şudur: {repo.Description}.");
            }
        }
        // 8. BLOGLAR
        if (lowerUrl.Contains("blog") || lowerQuestion.Contains("blog") || lowerQuestion.Contains("makale") || lowerQuestion.Contains("yazı"))
        {
            var blogs = await _blogPostDal.GetAllAsync(x => x.IsPublished);
            contextBuilder.AppendLine("--- BLOG YAZILARIM ---");
            foreach (var b in blogs)
            {
                var summary = !string.IsNullOrEmpty(b.AiSummary) ? b.AiSummary : b.Title;
                contextBuilder.AppendLine($"Baran'ın yazdığı profesyonel makalelerden biri '{b.Title}' başlığını taşımaktadır. Makalenin özeti şöyledir: {summary}.");
            }
        }
        return contextBuilder.ToString();
    }
}
