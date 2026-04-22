using DtoLayer.MessageDtos;
using SharedKernel.Enums;

namespace WebUILayer.Areas.Admin.Models;

public class MessageIndexViewModel:BasePaginationViewModel
{
    public List<MessageDto> messageDtos { get; set; } = new();
    public Dictionary<string, int> FolderCounts { get; set; } = new();
    public string ActiveCategory { get; set; } = "inbox";

    public string ActiveTitle => ActiveCategory switch
    {
        "inbox" => "Gelen Kutusu",
        "starred" => "Önemli",
        "read" => "Okundu",
        "sent" => "Gönderilenler",
        "draft" => "Taslaklar",
        "trash" => "Çöp Kutusu",
        _ => "Gelen Kutusu"
    };

    public int GetCount(string key) => FolderCounts.TryGetValue(key, out var c) ? c : 0;
    // GetCount metodu, FolderCounts sözlüğünde belirli bir anahtar (key) için karşılık gelen değeri (count) döndürür. Eğer anahtar sözlükte bulunmazsa, varsayılan olarak 0 döndürür. Bu yöntem, belirli bir mesaj kategorisindeki mesaj sayısını hızlıca almak için kullanılır. Örneğin, "inbox" kategorisindeki mesaj sayısını öğrenmek istediğinizde, GetCount("inbox") çağrısı yaparak bu bilgiyi alabilirsiniz.

    // C# 7.0 ile tanıtılan "out var" ifadesi, bir değişkeni tanımlarken aynı zamanda onu bir metoda argüman olarak geçirme imkanı sağlar. Bu durumda, "out var c" ifadesi, "c" adlı bir değişkeni tanımlar ve bu değişkenin değerini "TryGetValue" metodundan alır. Eğer "TryGetValue" metodu başarılı olursa, "c" değişkeni sözlükteki değeri içerir; aksi takdirde, "c" değişkeni varsayılan değere (örneğin, int için 0) sahip olur.

}
