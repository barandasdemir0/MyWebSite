using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.ProjectDtos;

public class ProjectListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // Kart için
    public string ShortDescription { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // Modal için Eklenenler
    public string Description { get; set; } = string.Empty; // "Proje Hakkında" kısmı
    public string? WebsiteUrl { get; set; } // "Demo" butonu
    public string? GithubUrl { get; set; }  // "GitHub" butonu
    public string Technologies { get; set; } = string.Empty; // Badge'ler için

    public bool IsFeatured { get; set; }

    // Topic listesi (Navigation'dan gelecek isimler)
    public List<string> Topics { get; set; } = new();
}
