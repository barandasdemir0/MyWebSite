using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Constants; 
using System.Text.Json;
using System.Text.RegularExpressions;

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

    public async Task<string> BuildContextAsync(string currentUrl, string lowerQuestion, CancellationToken cancellationToken=default)
    {
        string lowerUrl = currentUrl?.ToLower() ?? "";
        lowerQuestion = lowerQuestion?.ToLower() ?? "";

        // Yapay zekaya gidecek verileri tutacağımız Dictionary (JSON mimarisi)
        var contextData = new Dictionary<string, object>();

        // ===== GENEL BİLGİLER (Best Practice: GetAsync ve tracking: false) =====
        var siteSettings = await _siteSettingsDal.GetAsync(x => true, tracking: false, cancellationToken:cancellationToken);
        if (siteSettings != null)
            contextData["SiteStatus"] = new { siteSettings.IsAvailable, siteSettings.WorkStatus };

        // ===== SOSYAL MEDYA =====
        if (HasAny(lowerUrl, ChatbotConstants.SocialKeywords) || HasAny(lowerQuestion, ChatbotConstants.SocialKeywords))
        {
            var socials = await _socialMediaDal.GetAllAsync(tracking: false);
            if (socials.Any()) contextData["SocialMedia"] = socials.Select(s => new { s.SocialMediaName, s.SocialMediaUrl }).ToList();
        }

        // ===== İLETİŞİM (Best Practice: GetAsync) =====
        if (HasAny(lowerUrl, ChatbotConstants.ContactKeywords) || HasAny(lowerQuestion, ChatbotConstants.ContactKeywords))
        {
            var contact = await _contactDal.GetAsync(x => true, tracking: false);
            if (contact != null) contextData["ContactInfo"] = new { contact.Email, contact.Phone, contact.Location };
        }

        // ===== 1. HERO & HAKKIMDA (Best Practice: GetAsync) =====
        bool isHomeOrAbout = lowerUrl == "/" || lowerUrl.Contains("about");
        if (isHomeOrAbout || HasAny(lowerQuestion, ChatbotConstants.AboutKeywords))
        {
            var hero = await _heroDal.GetAsync(x => true, tracking: false);
            if (hero != null) contextData["Hero"] = new { hero.ProfessionalTitle, Technologies = hero.ScrollingText };

            var about = await _aboutDal.GetAsync(x => true, tracking: false);
            if (about != null) contextData["About"] = new
            {
                about.FullName,
                about.ExperienceYear,
                about.ProjectCount,
                Bio = Truncate(about.Bio, 1000),
                Greeting = Truncate(about.Greeting, 500)
            };
        }

        // ===== 2. İŞ YETENEKLERİ & TEKNOLOJİ STACK =====
        if (isHomeOrAbout || lowerUrl.Contains("skill") || HasAny(lowerQuestion, ChatbotConstants.SkillKeywords))
        {
            var skills = await _jobSkillDal.GetAllAsync(tracking: false);
            var categories = await _jobSkillCategoryDal.GetAllAsync(tracking: false);
            var jobSkillsList = categories.Select(cat => new {
                Category = cat.CategoryName,
                Skills = skills.Where(s => s.JobSkillCategoryId == cat.Id).Select(s => new { Name = s.JobSkillName, Percentage = s.JobSkillPercentage }).ToList()
            }).Where(c => c.Skills.Any()).ToList();

            if (jobSkillsList.Any()) contextData["JobSkills"] = jobSkillsList;

            var techSkills = await _skillDal.GetAllAsync(tracking: false);
            if (techSkills.Any()) contextData["DailyTools"] = techSkills.Select(s => s.SkillName).ToList();
        }

        // ===== 3. EĞİTİM =====
        bool isResume = lowerUrl.Contains("resume");
        if (isHomeOrAbout || isResume || lowerUrl.Contains("education") || HasAny(lowerQuestion, ChatbotConstants.EducationKeywords))
        {
            var educations = await _educationDal.GetAllAsync(tracking: false);
            if (educations.Any()) contextData["Education"] = educations.Select(e => new {
                School = e.EducationSchoolName,
                Degree = e.EducationDegree,
                StartDate = e.EducationStartDate.ToString("yyyy"),
                EndDate = e.EducationFinishDate.HasValue ? e.EducationFinishDate.Value.ToString("yyyy") : "Present"
            }).ToList();
        }

        // ===== 4. SERTİFİKALAR =====
        if (isResume || lowerUrl.Contains("certificate") || HasAny(lowerQuestion, ChatbotConstants.CertificateKeywords))
        {
            var certificates = await _certificateDal.GetAllAsync(tracking: false);
            if (certificates.Any()) contextData["Certificates"] = certificates.Select(c => new {
                Name = c.CertificateName,
                Issuer = c.IssuingCompany,
                Year = c.IssueDate.ToString("yyyy")
            }).ToList();
        }

        // ===== 5. DENEYİMLER =====
        if (isHomeOrAbout || isResume || lowerUrl.Contains("experience") || HasAny(lowerQuestion, ChatbotConstants.ExperienceKeywords))
        {
            var experiences = await _experienceDal.GetAllAsync(tracking: false);
            if (experiences.Any()) contextData["Experience"] = experiences.Select(e => new {
                Company = e.ExperienceCompanyName,
                Title = e.ExperienceTitle,
                StartDate = e.ExperienceStartDate.ToString("yyyy"),
                EndDate = e.ExperienceFinishDate.HasValue ? e.ExperienceFinishDate.Value.ToString("yyyy") : "Present",
                Description = Truncate(e.ExperienceDescription, 500)
            }).ToList();
        }

        // ===== 6. PROJELER =====
        if (lowerUrl.Contains("project") || HasAny(lowerQuestion, ChatbotConstants.ProjectKeywords))
        {
            var projects = await _projectDal.GetAllAsync(x => x.IsPublished, tracking: false);
            if (projects.Any()) contextData["Projects"] = projects.Select(p => new {
                p.Name,
                p.Technologies,
                Description = !string.IsNullOrEmpty(p.AiSummary) ? p.AiSummary : Truncate(!string.IsNullOrEmpty(p.Description) ? p.Description : p.ShortDescription, 300),
                p.GithubUrl,
                p.WebsiteUrl
            }).ToList();
        }

        // ===== 7. GITHUB REPOLARI =====
        if (lowerUrl.Contains("github") || lowerUrl.Contains("repo") || HasAny(lowerQuestion, ChatbotConstants.GithubKeywords))
        {
            var repos = await _githubRepoDal.GetAllAsync(tracking: false);
            if (repos.Any()) contextData["GithubRepos"] = repos.Select(r => new { r.RepoName, r.Language, r.Description }).ToList();
        }

        // ===== 8. BLOGLAR =====
        if (lowerUrl.Contains("blog") || HasAny(lowerQuestion, ChatbotConstants.BlogKeywords))
        {
            var blogs = await _blogPostDal.GetAllAsync(x => x.IsPublished, tracking: false);
            if (blogs.Any()) contextData["BlogPosts"] = blogs.Select(b => new {
                b.Title,
                b.Technologies,
                ReadTimeMinutes = b.ReadTime,
                Summary = !string.IsNullOrEmpty(b.AiSummary) ? b.AiSummary : Truncate(b.Content, 500)
            }).ToList();
        }

        // ===== FALLBACK =====
        if (!contextData.ContainsKey("About") && HasAny(lowerQuestion, ChatbotConstants.SummaryKeywords))
        {
            var about = await _aboutDal.GetAsync(x => true, tracking: false);
            if (about != null) contextData["FallbackSummary"] = new { about.FullName, about.ExperienceYear, Bio = Truncate(about.Bio, 300) };
        }

        // Verileri temiz bir JSON'a dönüştür
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(contextData, jsonOptions);
    }

    private static bool HasAny(string text, string[] keywords)
    {
        if (string.IsNullOrEmpty(text)) return false;
        return keywords.Any(k => text.Contains(k));
    }

    private static string Truncate(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        text = Regex.Replace(text, "<.*?>", " ");
        text = Regex.Replace(text, @"\s+", " ").Trim();
        return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
    }
}
