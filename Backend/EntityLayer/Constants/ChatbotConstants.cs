namespace EntityLayer.Constants;


public static class ChatbotConstants
{
    // --- GÜVENLİK (GUARDRAILS) KELİMELERİ ---
    public static readonly string[] BannedInputKeywords = {
        "kural", "prompt", "ignore", "unut", "simülasyon", "test mod",
        "json", "talimat", "sistem", "whitelist"
    };
    public static readonly string[] BannedOutputKeywords = {
        "<DatabaseContext>", "WHITELIST", "GÖREV:", "KATI KURALLAR"
    };
    // --- NİYET (INTENT) KELİMELERİ (TR & EN KARIŞIK) ---
    public static readonly string[] SocialKeywords = {
        "sosyal", "linkedin", "github", "instagram", "twitter", "takip", "social", "network", "connect"
    };

    public static readonly string[] ContactKeywords = {
        "iletişim", "email", "mail", "telefon", "ulaş", "adres", "konum", "nerede", "contact", "reach", "location", "address", "phone", "call"
    };

    public static readonly string[] AboutKeywords = {
        "kim", "hakkında", "tanıt", "neler yaparsın", "kendini", "merhaba", "sen kimsin", "about", "who", "yourself", "hello", "hi", "greeting"
    };

    public static readonly string[] SkillKeywords = {
        "yetenek", "teknoloji", "dil", "stack", "bildiğin", "kullandığın", "araç", "skill", "tools", "technology", "language", "framework", "know"
    };

    public static readonly string[] EducationKeywords = {
        "okul", "eğitim", "üniversite", "mezun", "okudun", "lise", "education", "school", "university", "degree", "graduated", "study", "college"
    };

    public static readonly string[] CertificateKeywords = {
        "sertifika", "kurs", "belge", "certificate", "course", "certification", "license", "training"
    };

    public static readonly string[] ExperienceKeywords = {
        "tecrübe", "deneyim", "çalıştın", "iş geçmişi", "özgeçmiş", "kariyer", "geçmiş", "experience", "resume", "work", "career", "background", "job", "cv"
    };

    public static readonly string[] ProjectKeywords = {
        "proje", "uygulama", "geliştirdin", "yaptığın", "yazılım", "project", "portfolio", "app", "software", "developed", "build", "created"
    };

    public static readonly string[] GithubKeywords = {
        "github", "repo", "kaynak kod", "açık kaynak", "source code", "open source", "repository"
    };

    public static readonly string[] BlogKeywords = {
        "blog", "makale", "yazı", "sayfa", "okuma", "page", "post", "article", "read", "writing"
    };

    public static readonly string[] SummaryKeywords = {
        "özetle", "özet", "bu sayfa", "kısaca", "summarize", "this page", "summary", "brief", "short", "tldr"
    };
}
