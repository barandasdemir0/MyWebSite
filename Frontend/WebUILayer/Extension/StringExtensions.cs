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


        // burası HTML etiketlerini kaldırmak için basit bir regex kullanır. Daha karmaşık HTML yapıları için daha gelişmiş bir yöntem gerekebilir.
        var plaintText = Regex.Replace(input, "<.*?>", string.Empty);

        if (plaintText.Length<= length)
        {
            return plaintText;
        }

        return plaintText.Substring(0, length) + "...";
    }
}
