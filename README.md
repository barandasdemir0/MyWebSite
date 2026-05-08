# 🚀 Baran Daşdemir — Portfolio
**Full-Stack .NET Developer Portfolio & Personal Website**
*Clean Architecture · RESTful API · AI-Powered Chatbot · Real-Time Communication*
[![Live Site](https://img.shields.io/badge/🌐_Live_Site-barandasdemir.com-0f0f23?style=for-the-badge)](https://www.barandasdemir.com)
[![API](https://img.shields.io/badge/⚡_API-api.barandasdemir.com-1e40af?style=for-the-badge)](https://api.barandasdemir.com)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity_Framework-10.0-512BD4?logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoftsqlserver&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-Real--Time-512BD4?logo=dotnet&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap&logoColor=white)
![Groq AI](https://img.shields.io/badge/Groq_AI-LLM-ff6600?logo=openai&logoColor=white)
![CI/CD](https://img.shields.io/badge/CI/CD-GitHub_Actions-2088FF?logo=githubactions&logoColor=white)
![GTmetrix](https://img.shields.io/badge/GTmetrix-A_Grade_(96%25)-4caf50?style=flat&logo=google&logoColor=white)
---
## 📋 İçindekiler
- [Hakkında](#-hakkında)
- [Öne Çıkan Özellikler](#-öne-çıkan-özellikler)
- [Mimari ve Teknoloji Yığını](#-mimari-ve-teknoloji-yığını)
- [Proje Yapısı](#-proje-yapısı)
- [Katman Detayları](#-katman-detayları)
- [Veritabanı Şeması](#-veritabanı-şeması)
- [Güvenlik](#-güvenlik)
- [Performans Optimizasyonları](#-performans-optimizasyonları)
- [CI/CD Pipeline](#-cicd-pipeline)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [Sayfalar](#-sayfalar)
- [Lisans](#-lisans)
---
## 🎯 Hakkında
Modern web teknolojileri kullanılarak geliştirilen, **N TIER Architecture** prensiplerine sadık, tam katmanlı bir kişisel portfolyo web uygulamasıdır. Backend ve Frontend tamamen ayrışmış bir yapıda olup, aralarındaki iletişim RESTful API üzerinden sağlanmaktadır.
Projenin amacı, profesyonel deneyimleri, projeleri, blog yazılarını ve teknik yetenekleri dinamik bir şekilde sergilemek; ziyaretçilere yapay zekâ destekli gerçek zamanlı sohbet asistanı ile etkileşimli bir deneyim sunmaktır.
---
## ✨ Öne Çıkan Özellikler
### 🤖 Yapay Zekâ Destekli Chatbot
- **Groq API** (LLM) entegrasyonu ile bağlam duyarlı akıllı sohbet asistanı
- **RAG (Retrieval-Augmented Generation)** mimarisi — Chatbot, veritabanındaki proje, blog, deneyim ve eğitim verilerini dinamik olarak çekerek doğru ve güncel cevaplar üretir
- **Sayfa bazlı bağlam enjeksiyonu** — Kullanıcının bulunduğu sayfaya göre ilgili verileri otomatik olarak AI'a sunar
- **Prompt Injection koruması** — Girdi/çıktı güvenlik katmanları (Input/Output Guardrails)
- **Akıllı önbellekleme** — Tekrar eden sorular için veritabanı tabanlı cache mekanizması
- **SignalR** ile gerçek zamanlı mesajlaşma (WebSocket)
### 🔐 Kurumsal Düzey Güvenlik
- **JWT (JSON Web Token)** tabanlı kimlik doğrulama ve yetkilendirme
- **Refresh Token** rotasyonu ile oturum güvenliği
- **Two-Factor Authentication (2FA)** desteği (QR Code ile)
- **OAuth 2.0** sosyal giriş (GitHub ve LinkedIn)
- **Rol tabanlı yetkilendirme** (RBAC) ve izin (permission) sistemi
- **Rate Limiting** ile brute-force koruması
- **CORS** politikaları
- **AES şifreleme** ile hassas verilerin korunması (API Key vb.)
- **HtmlSanitizer** ile XSS koruması
- **FluentValidation** ile kapsamlı girdi doğrulama
### 📊 Admin Paneli
- Tüm içeriklerin (Projeler, Bloglar, Deneyimler, Sertifikalar, Beceriler) CRUD yönetimi
- Kullanıcı yönetimi ve rol atama
- Mesaj kutusu ve bildirim sistemi
- Ziyaretçi defteri moderasyonu
- GitHub repo senkronizasyonu
- Site ayarları, SEO yapılandırması ve bakım modu kontrolü
- Chatbot ayarları ve AI model konfigürasyonu
- Centralized log monitoring (Serilog)
### 🌐 Çok Dilli Destek ve SEO
- **GTranslate** entegrasyonu ile çoklu dil desteği
- Dinamik **sitemap.xml** üretimi (Blog ve Proje slug'larıyla)
- **robots.txt** ve **llms.txt** dosyaları
- Open Graph ve Twitter Card meta etiketleri
- Dinamik Favicon yönetimi (Admin panelinden)
### ⚡ Performans
- **GTmetrix A Grade (%96)** performans puanı
- **561ms** sayfa yükleme süresi (Pingdom)
- **WebOptimizer** ile CSS bundling ve minification
- Ertelemeli (deferred) ve asenkron kaynak yükleme
- Response Compression (Gzip)
- 1 yıllık statik dosya önbellekleme (Cache-Control: immutable)
- Google Font preload stratejisi
- Bot-aware rendering (Lighthouse/PageSpeed için optimize edilmiş)
---
## 🏗 Mimari ve Teknoloji Yığını
```
┌──────────────────────────────────────────────────────────┐
│                     FRONTEND (MVC)                       │
│              ASP.NET Core 10 MVC + Razor                 │
│          Bootstrap 5.3 · AOS · Custom Theme              │
│              WebOptimizer · ViewComponents               │
└──────────────────────┬───────────────────────────────────┘
                       │ HTTP (HttpClient + JWT)
                       ▼
┌──────────────────────────────────────────────────────────┐
│                    BACKEND (Web API)                      │
│            ASP.NET Core 10 Web API + SignalR              │
│           Scalar (OpenAPI) · Rate Limiter                 │
│                JWT Auth · CORS · Serilog                  │
└──────────────────────┬───────────────────────────────────┘
                       │
          ┌────────────┼─────────────┐
          ▼            ▼             ▼
   ┌────────────┐ ┌─────────┐ ┌─────────────┐
   │ Business   │ │  DTO    │ │SharedKernel │
   │  Layer     │ │ Layer   │ │ (Enums,     │
   │ (Managers, │ │(Mapster)│ │ Exceptions) │
   │ Validators)│ │         │ │             │
   └─────┬──────┘ └─────────┘ └─────────────┘
         │
         ▼
┌──────────────────────────────────────────────────────────┐
│              DATA ACCESS LAYER (DAL)                      │
│       Entity Framework Core 10 · Code First               │
│    Repository Pattern · Unit of Work · Fluent API         │
└──────────────────────┬───────────────────────────────────┘
                       │
                       ▼
              ┌─────────────────┐
              │   SQL Server    │
              │   (MSSQL)       │
              └─────────────────┘
```
### Teknoloji Detayları
| Katman | Teknoloji | Amaç |
|--------|-----------|------|
| **Runtime** | .NET 10.0 | Uygulama çatısı |
| **ORM** | Entity Framework Core 10 | Veritabanı erişimi |
| **Database** | SQL Server (MSSQL) | Veri depolama |
| **Auth** | JWT Bearer + ASP.NET Identity | Kimlik doğrulama |
| **Real-Time** | SignalR | WebSocket iletişimi |
| **AI** | Groq API (LLM) | Chatbot zekâsı |
| **Validation** | FluentValidation 12 | Girdi doğrulama |
| **Mapping** | Mapster 10 | DTO - Entity dönüşümü |
| **Logging** | Serilog (SQL Server Sink) | Yapılandırılmış günlük kaydı |
| **Email** | MailKit | E-posta gönderimi |
| **Security** | HtmlSanitizer, QRCoder | XSS koruması, 2FA |
| **DI Scanner** | Scrutor | Otomatik bağımlılık kaydı |
| **API Docs** | Scalar (OpenAPI) | API dokümantasyonu |
| **CSS** | Bootstrap 5.3 + Custom Theme | Responsive tasarım |
| **Bundling** | LigerShark WebOptimizer | CSS/JS optimizasyonu |
| **CI/CD** | GitHub Actions + FTP Deploy | Otomatik dağıtım |
---
## 📁 Proje Yapısı
```
MyWebSite/
├── .github/
│   └── workflows/
│       └── deploy.yml                  # CI/CD Pipeline (API + MVC)
│
├── Backend/
│   ├── EntityLayer/                    # Domain Entities & Constants
│   │   ├── Entities/                   # 30 Entity sınıfı
│   │   └── Constants/                  # Chatbot, Role, Permission sabitleri
│   │
│   ├── SharedKernel/                   # Cross-Cutting Concerns
│   │   ├── Enums/                      # MessageFolder, TwoFactorProvider
│   │   └── Exceptions/                 # Custom exception sınıfları
│   │
│   ├── DataAccessLayer/                # Veri Erişim Katmanı
│   │   ├── Abstract/                   # Repository arayüzleri (IDal)
│   │   ├── Concrete/                   # Repository implementasyonları
│   │   ├── Configurations/             # 26 EF Core Fluent API yapılandırması
│   │   ├── Context/                    # DbContext
│   │   └── Migrations/                 # EF Core migration dosyaları
│   │
│   ├── DtoLayer/                       # Data Transfer Objects
│   │   └── [24 DTO klasörü]            # Create/Update/List DTO'ları
│   │
│   ├── BusinessLayer/                  # İş Mantığı Katmanı
│   │   ├── Abstract/                   # Manager arayüzleri
│   │   ├── Concrete/                   # 32 Manager implementasyonu
│   │   ├── Container/                  # DI kayıtları (Scrutor)
│   │   ├── Mapping/                    # Mapster profilleri
│   │   ├── Services/                   # Email, Encryption, Groq API
│   │   └── ValidationRules/            # 19 FluentValidation kuralı
│   │
│   └── WebApiLayer/                    # REST API
│       ├── Controllers/                # 31 API Controller
│       ├── Hubs/                       # SignalR ChatHub
│       ├── Middleware/                  # Exception & Health Check
│       ├── Seed/                       # Admin hesabı seed
│       └── Program.cs                  # API giriş noktası
│
├── Frontend/
│   └── WebUILayer/                     # MVC Presentation Layer
│       ├── Areas/Admin/                # Admin paneli (21 Controller)
│       ├── Controllers/                # 14 Public Controller
│       ├── Extension/                  # Service Extensions
│       ├── Helper/                     # JWT Token Handler
│       ├── Middleware/                  # Exception & Maintenance
│       ├── ViewComponents/             # Layout bileşenleri
│       ├── Views/                      # 13 View klasörü
│       └── wwwroot/Mytheme/portfolio/  # Özel tema
│           ├── css/                    # Modüler CSS mimarisi
│           │   ├── base/              # Variables, Reset
│           │   ├── components/        # Navbar, Cards, Forms...
│           │   ├── sections/          # Hero, Blog, GitHub...
│           │   └── pages/             # Contact, Resume...
│           └── js/                     # 11 modüler JS dosyası
│
└── MyWebSite.slnx                      # Solution dosyası
```
---
## 🧱 Katman Detayları
### Entity Layer (Domain)
30 entity sınıfı ile zengin bir domain modeli. `BaseEntity` üzerinden ortak alanlar (`Id`, `CreatedAt`, `IsActive`) miras alınır. Identity entegrasyonu `AppUser` üzerinden yapılmıştır.
**Temel Entity'ler:** Project, BlogPost, Experience, Education, Certificate, JobSkill, Hero, About, GuestBook, Message, SiteSettings, ChatbotCache, ChatbotSettings, Notification, GithubRepo, SocialMedia, Skill, Topic
**Güvenlik Entity'leri:** AppUser, RefreshToken, PasswordResetToken, RolePermission
### Data Access Layer
Repository Pattern + Unit of Work deseni. Her entity için ayrı `IDal` arayüzü ve implementasyonu. 26 adet Fluent API Configuration ile veritabanı şeması yönetimi. EF Core Code First yaklaşımı.
### Business Layer
32 adet Manager sınıfı ile kapsamlı iş mantığı. Scrutor ile otomatik bağımlılık tarama ve kayıt. 19 modül için FluentValidation kuralları. Mapster ile DTO - Entity dönüşümleri.
**Özel Servisler:**
- `ChatbotManager` — RAG tabanlı AI chatbot iş mantığı
- `PortfolioContextManager` — Sayfa bazlı dinamik veri toplama (14 repository)
- `TokenManager` — JWT Access + Refresh Token yönetimi
- `TwoFactorManager` — QR Code tabanlı 2FA
- `EmailManager` — MailKit ile e-posta gönderimi
- `EncryptionService` — AES şifreleme
- `GroqApiManager` — LLM API iletişimi
### Web API Layer
31 Controller ile kapsamlı RESTful API. Generic controller hiyerarşisi:
```
CrudController<T> (Base)
├── SecureCrudController<T>   → Admin (JWT korumalı)
└── PublicCrudController<T>   → Herkese açık (Read-only)
```
SignalR ChatHub ile gerçek zamanlı chatbot iletişimi. Scalar ile interaktif API dokümantasyonu. DataSeeder ile ilk çalıştırmada admin hesabı oluşturma.
### Web UI Layer (MVC)
14 Public Controller + 21 Admin Controller. ViewComponent tabanlı modüler layout sistemi:
- `LayoutPublicHeader` — SEO meta, preconnect, CSS bundle
- `LayoutPublicNavbar` — Responsive menü, tema ve dil değiştirici
- `LayoutPublicPreLoader` — Animasyonlu yükleme ekranı
- `LayoutPublicFooter` — Footer ve sosyal medya linkleri
- `LayoutPublicChatbot` — AI sohbet asistanı
- `LayoutPublicScripts` — JS bundle ve yapılandırma
**Middleware Zinciri:** Global Exception → Maintenance Mode → Status Code Pages → Routing
---
## 🗄 Veritabanı Şeması
```
AppUser ──────┬──── RefreshToken
              ├──── PasswordResetToken
              ├──── GuestBook
              └──── Message
Project ──────┬──── ProjectTopic ────── Topic
              │                          │
BlogPost ─────┴──── BlogTopic ──────────┘
JobSkillCategory ──── JobSkill
SiteSettings         ChatbotSettings ──── ChatbotCache
Hero                 Notification
About                GithubRepo
Contact              SocialMedia
Experience           Certificate
Education            Skill
```
---
## 🔒 Güvenlik
| Özellik | Uygulama |
|---------|----------|
| Kimlik Doğrulama | JWT Bearer Token + Cookie Auth (MVC) |
| Yetkilendirme | Rol tabanlı (Admin/User) + Permission bazlı politikalar |
| Token Güvenliği | Access + Refresh Token rotasyonu |
| 2FA | QR Code tabanlı TOTP (QRCoder) |
| Sosyal Giriş | OAuth 2.0 (GitHub, LinkedIn) |
| API Koruması | Rate Limiting (IP + Endpoint bazlı) |
| Veri Güvenliği | AES Encryption (API Key'ler için) |
| XSS Koruması | HtmlSanitizer (Guestbook, Blog vb.) |
| Girdi Doğrulama | FluentValidation (Server-side) |
| CORS | Origin bazlı beyaz liste |
| Chatbot Güvenliği | Input/Output Guardrails + Banned keyword filtreleme |
| Hassas Veriler | User Secrets (Dev) + GitHub Secrets (Prod) |
---
## ⚡ Performans Optimizasyonları
### Test Sonuçları (Mayıs 2026)
| Test Aracı | Sonuç | Durum |
|-----------|-------|-------|
| **GTmetrix** | A Grade (%96) | ✅ |
| **Pingdom** | 561ms Load Time | ✅ |
| **PageSpeed (SEO)** | 100/100 | ✅ |
| **PageSpeed (Erişilebilirlik)** | 98/100 | ✅ |
| **PageSpeed (En İyi Uygulamalar)** | 96/100 | ✅ |
### Uygulanan Optimizasyonlar
| Optimizasyon | Detay |
|-------------|-------|
| CSS Bundling | WebOptimizer ile 30+ CSS dosyası tek bundle.css'e |
| Local Bootstrap | CDN yerine kendi sunucudan (1.8s render-block tasarrufu) |
| Deferred Loading | AOS, Font Awesome, Devicons ertelemeli yükleme |
| Font Preload | Google Fonts asenkron yükleme (preload + onload) |
| Static Caching | max-age=31536000, immutable (1 yıl) |
| Bot Detection | Lighthouse/PageSpeed botları için optimize rendering |
| Image Optimization | WebP formatı + lazy loading + async decoding |
| Response Compression | Gzip middleware |
| DNS Prefetch | Kritik kaynaklar için preconnect/dns-prefetch |
---

## 📸 Sayfalar
| Sayfa | Açıklama |
|-------|----------|
| 🏠 **Ana Sayfa** | Hero section, typing effect, skill showcase, proje vitrin kartları |
| 👤 **Hakkımda** | Profesyonel biyografi, istatistikler, kariyer özeti |
| 📄 **Özgeçmiş** | Eğitim, deneyim, sertifikalar ve yetenek grafikleri |
| 💼 **Projeler** | Filtrelenebilir proje galerisi ve detay sayfaları |
| 📝 **Blog** | Kategori bazlı yazılar, okuma süresi, AI özet |
| 📒 **Ziyaretçi Defteri** | OAuth ile giriş, yorum yazma, moderasyon Yorum Yapmayı unutmayınn |
| 📬 **İletişim** | İletişim formu, sosyal medya linkleri |
| 🤖 **AI Chatbot** | Gerçek zamanlı sohbet, sayfa bazlı akıllı yanıtlar |
| ⚙️ **Admin Panel** | Dashboard, CRUD yönetimi, mesajlar, bildirimler |
---
## 📄 Lisans
Bu proje kişisel portfolyo amaçlıdır. Tüm hakları saklıdır.
---
**Baran Daşdemir** tarafından ❤️ ile geliştirilmiştir.
[🌐 Website](https://www.barandasdemir.com) · [💻 GitHub](https://github.com/barandasdemir0) · [🔗 LinkedIn](https://www.linkedin.com/in/baran-dasdemir/)
