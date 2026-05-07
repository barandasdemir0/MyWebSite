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
            contextBuilder.AppendLine("[SITE_STATUS]");
            contextBuilder.AppendLine($"IsAvailable={siteSettings.IsAvailable}");
            contextBuilder.AppendLine($"WorkStatus={siteSettings.WorkStatus}");
            contextBuilder.AppendLine();
        }

        // ===== SOSYAL MEDYA =====
        if (lowerQuestion.Contains("sosyal") || lowerQuestion.Contains("linkedin") || lowerQuestion.Contains("github") ||
            lowerQuestion.Contains("instagram") || lowerQuestion.Contains("twitter") || lowerQuestion.Contains("iletişim") ||
            lowerQuestion.Contains("ulaş") || lowerQuestion.Contains("takip") || lowerUrl.Contains("contact"))
        {
            var socials = await _socialMediaDal.GetAllAsync();
            if (socials.Any())
            {
                contextBuilder.AppendLine("[SOCIAL_MEDIA]");
                foreach (var s in socials)
                    contextBuilder.AppendLine($"{s.SocialMediaName}={s.SocialMediaUrl}");
                contextBuilder.AppendLine();
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
                contextBuilder.AppendLine("[CONTACT_INFO]");
                contextBuilder.AppendLine($"Email={contact.Email}");
                contextBuilder.AppendLine($"Phone={contact.Phone}");
                contextBuilder.AppendLine($"Location={contact.Location}");
                contextBuilder.AppendLine();
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
                contextBuilder.AppendLine("[HERO]");
                contextBuilder.AppendLine($"ProfessionalTitle={hero.ProfessionalTitle}");
                contextBuilder.AppendLine($"Technologies={hero.ScrollingText}");
                contextBuilder.AppendLine();
            }

            var abouts = await _aboutDal.GetAllAsync();
            var about = abouts.FirstOrDefault();
            if (about != null)
            {
                contextBuilder.AppendLine("[ABOUT]");
                contextBuilder.AppendLine($"FullName={about.FullName}");
                contextBuilder.AppendLine($"ExperienceYear={about.ExperienceYear}");
                contextBuilder.AppendLine($"ProjectCount={about.ProjectCount}");
                if (!string.IsNullOrEmpty(about.Bio))
                    contextBuilder.AppendLine($"Bio={about.Bio}");
                if (!string.IsNullOrEmpty(about.Greeting))
                    contextBuilder.AppendLine($"Greeting={about.Greeting}");
                contextBuilder.AppendLine();
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

            contextBuilder.AppendLine("[JOB_SKILLS]");
            foreach (var cat in categories)
            {
                var catSkills = skills.Where(s => s.JobSkillCategoryId == cat.Id).ToList();
                if (catSkills.Any())
                {
                    contextBuilder.AppendLine($"Category={cat.CategoryName}");
                    foreach (var s in catSkills)
                        contextBuilder.AppendLine($"- {s.JobSkillName}: {s.JobSkillPercentage}%");
                }
            }
            contextBuilder.AppendLine();

            var techSkills = await _skillDal.GetAllAsync();
            if (techSkills.Any())
            {
                contextBuilder.AppendLine("[DAILY_TOOLS]");
                contextBuilder.AppendLine($"Tools={string.Join(", ", techSkills.Select(s => s.SkillName))}");
                contextBuilder.AppendLine();
            }
        }

        // 3. EĞİTİM
        if (lowerUrl.Contains("education") || lowerUrl.Contains("resume") || lowerUrl == "/" ||
            lowerQuestion.Contains("okul") || lowerQuestion.Contains("eğitim") ||
            lowerQuestion.Contains("üniversite") || lowerQuestion.Contains("mezun") ||
            lowerQuestion.Contains("özgeçmiş"))
        {
            var educations = await _educationDal.GetAllAsync();
            if (educations.Any())
            {
                contextBuilder.AppendLine("[EDUCATION]");
                foreach (var edu in educations)
                {
                    string endDate = edu.EducationFinishDate.HasValue ? edu.EducationFinishDate.Value.ToString("yyyy") : "Present";
                    contextBuilder.AppendLine($"- School={edu.EducationSchoolName} | Degree={edu.EducationDegree} | Period={edu.EducationStartDate:yyyy}-{endDate}");
                }
                contextBuilder.AppendLine();
            }
        }

        // 4. SERTİFİKALAR
        if (lowerUrl.Contains("certificate") || lowerUrl.Contains("resume") ||
            lowerQuestion.Contains("sertifika") || lowerQuestion.Contains("kurs") ||
            lowerQuestion.Contains("belge") || lowerQuestion.Contains("özgeçmiş"))
        {
            var certificates = await _certificateDal.GetAllAsync();
            if (certificates.Any())
            {
                contextBuilder.AppendLine("[CERTIFICATES]");
                foreach (var cert in certificates)
                    contextBuilder.AppendLine($"- Name={cert.CertificateName} | Issuer={cert.IssuingCompany} | Year={cert.IssueDate:yyyy}");
                contextBuilder.AppendLine();
            }
        }

        // 5. DENEYİMLER
        if (lowerUrl.Contains("experience") || lowerUrl.Contains("resume") || lowerUrl == "/" ||
            lowerQuestion.Contains("tecrübe") || lowerQuestion.Contains("deneyim") ||
            lowerQuestion.Contains("çalıştın") || lowerQuestion.Contains("iş geçmişi") ||
            lowerQuestion.Contains("özgeçmiş") || lowerQuestion.Contains("kariyer"))
        {
            var experiences = await _experienceDal.GetAllAsync();
            if (experiences.Any())
            {
                contextBuilder.AppendLine("[EXPERIENCE]");
                foreach (var e in experiences)
                {
                    string endDate = e.ExperienceFinishDate.HasValue ? e.ExperienceFinishDate.Value.ToString("yyyy") : "Present";
                    contextBuilder.AppendLine($"- Company={e.ExperienceCompanyName} | Title={e.ExperienceTitle} | Period={e.ExperienceStartDate:yyyy}-{endDate}");
                    contextBuilder.AppendLine($"  Description={e.ExperienceDescription}");
                }
                contextBuilder.AppendLine();
            }
        }

        // 6. PROJELER
        if (lowerUrl.Contains("project") || lowerQuestion.Contains("proje") ||
            lowerQuestion.Contains("uygulama") || lowerQuestion.Contains("geliştirdin") ||
            lowerQuestion.Contains("yaptığın"))
        {
            var projects = await _projectDal.GetAllAsync(x => x.IsPublished);
            if (projects.Any())
            {
                contextBuilder.AppendLine("[PROJECTS]");
                foreach (var p in projects)
                {
                    var summary = !string.IsNullOrEmpty(p.AiSummary) ? p.AiSummary
                        : !string.IsNullOrEmpty(p.Description) ? Truncate(p.Description, 300)
                        : p.ShortDescription;

                    contextBuilder.AppendLine($"- Name={p.Name}");
                    contextBuilder.AppendLine($"  Technologies={p.Technologies}");
                    contextBuilder.AppendLine($"  Description={summary}");
                    if (!string.IsNullOrEmpty(p.GithubUrl))
                        contextBuilder.AppendLine($"  GithubUrl={p.GithubUrl}");
                    if (!string.IsNullOrEmpty(p.WebsiteUrl))
                        contextBuilder.AppendLine($"  WebsiteUrl={p.WebsiteUrl}");
                }
                contextBuilder.AppendLine();
            }
        }

        // 7. GITHUB REPOLARI
        if (lowerUrl.Contains("github") || lowerUrl.Contains("repo") ||
            lowerQuestion.Contains("github") || lowerQuestion.Contains("repo") ||
            lowerQuestion.Contains("kaynak kod") || lowerQuestion.Contains("açık kaynak"))
        {
            var repos = await _githubRepoDal.GetAllAsync();
            if (repos.Any())
            {
                contextBuilder.AppendLine("[GITHUB_REPOS]");
                foreach (var repo in repos)
                {
                    contextBuilder.AppendLine($"- RepoName={repo.RepoName} | Language={repo.Language}");
                    contextBuilder.AppendLine($"  Description={repo.Description}");
                }
                contextBuilder.AppendLine();
            }
        }

        // 8. BLOGLAR
        if (lowerUrl.Contains("blog") || lowerQuestion.Contains("blog") ||
            lowerQuestion.Contains("makale") || lowerQuestion.Contains("yazı") ||
            lowerQuestion.Contains("özetle") || lowerQuestion.Contains("summarize") ||
            lowerQuestion.Contains("sayfa") || lowerQuestion.Contains("page"))
        {
            var blogs = await _blogPostDal.GetAllAsync(x => x.IsPublished);
            if (blogs.Any())
            {
                contextBuilder.AppendLine("[BLOG_POSTS]");
                foreach (var b in blogs)
                {
                    var summary = !string.IsNullOrEmpty(b.AiSummary) ? b.AiSummary
                        : !string.IsNullOrEmpty(b.Content) ? Truncate(b.Content, 500)
                        : b.Title;

                    contextBuilder.AppendLine($"- Title={b.Title}");
                    contextBuilder.AppendLine($"  Technologies={b.Technologies}");
                    contextBuilder.AppendLine($"  ReadTime={b.ReadTime} min");
                    contextBuilder.AppendLine($"  Summary={summary}");
                }
                contextBuilder.AppendLine();
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
                {
                    contextBuilder.AppendLine("[FALLBACK_SUMMARY]");
                    contextBuilder.AppendLine($"FullName={about.FullName}");
                    contextBuilder.AppendLine($"ExperienceYear={about.ExperienceYear}");
                    contextBuilder.AppendLine($"Bio={about.Bio}");
                    contextBuilder.AppendLine();
                }
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
