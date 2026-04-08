namespace SharedKernel.Enums;

public enum MessageFolder
{
    Inbox = 0,      // Gelen Kutusu
    Sent = 1,       // Gönderilenler
    Draft = 2,      // Taslaklar
    Trash = 3       // Çöp Kutusu
}


//public enum MessageFolder
//{
//    Inbox,      // Gelen Kutusu
//    Sent,       // Gönderilenler
//    Draft,      // Taslaklar
//    Trash       // Çöp Kutusu
//}
//bu kullanımı bıraktım çünkü araya important yada başka değer gelirse veritabanı patlama riski çıkabilir 
