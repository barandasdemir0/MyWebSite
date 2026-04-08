namespace SharedKernel.Shared;

public interface IHasId //ıdmizi heryerden erişilebilir yaptık böylelikle her classımıza ıdmizi eklmeiş olduk
{
    Guid Id { get; }
}
