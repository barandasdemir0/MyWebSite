namespace SharedKernel.Exceptions;

public class BusinessException:BaseException
{
    public BusinessException(string message) : base(message)
    {

    }
}
//Exception, uygulama çalışırken oluşan hataları temsil eden yapılardır. “Bir şey yanlış gitti” demenin kontrollü yolu