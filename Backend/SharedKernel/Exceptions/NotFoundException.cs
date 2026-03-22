namespace SharedKernel.Exceptions;

public class NotFoundException:Exception
{
    public NotFoundException(string entity, object key) : base($"{entity} bulunamadı. (Anahtar : {key})")
    {

    }
}
