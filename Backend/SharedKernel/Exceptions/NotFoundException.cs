namespace SharedKernel.Exceptions;

public class NotFoundException:BaseException
{
    public NotFoundException(string entity, object key) : base($"{entity} bulunamadı. (Anahtar : {key})") 
        //buradaki entity usermı başka bir şeymi keyde idsi  örnek gerekirse user bulunamadı, userId gibi mantık
    {

    }
}
