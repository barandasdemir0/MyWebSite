using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;

namespace WebUILayer.Extension;

public static class StringExtensions
{
    public static string StripHtmlAndTruncate(this string input,int length = 100)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(input);

        // Bütün HTML etiketlerini uçur, sadece saf metni (plain text) al
        string plainText = htmlDoc.DocumentNode.InnerText;
        // Ekranda çirkin görünen &nbsp; gibi HTML kodlarını normal boşluğa çevir
        plainText = WebUtility.HtmlDecode(plainText).Trim();

        if (plainText.Length<= length)
        {
            return plainText;
        }

        return plainText.Substring(0, length) + "...";
    }
}
