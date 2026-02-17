using System.Text.RegularExpressions;

namespace BusinessLayer.Extensions;

public static class SlugExtension
{
    public static string AutoSlug(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return "";
        } // eğer textin içeriği null veya boşsa boş yap ""

        text = text.ToLowerInvariant(); //hepsini küçük harfe çevir


        //aşağıdaki küçük harflerde ı gibi türkçe harfleri ingilizce uyumluluk adına i gibi şeyleri çevir
        text = text.Replace("ı", "i")
                   .Replace("ğ", "g")
                   .Replace("ü", "u")
                   .Replace("ş", "s")
                   .Replace("ö", "o")
                   .Replace("ç", "c");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");  // harf rakam tire dışındaki ifadeleri sil
        text = Regex.Replace(text, @"\s+", "-").Trim(); // boşlukları tire yap

        text = Regex.Replace(text, @"-+", "-"); //yanyana gelen birden fazla tireleri tek tire yap

        return text;
    }
}
