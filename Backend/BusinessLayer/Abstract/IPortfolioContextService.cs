namespace BusinessLayer.Abstract;

public interface IPortfolioContextService
{
    Task<string> BuildContextAsync(string currentUrl, string lowerQuestion, CancellationToken cancellationToken = default);
}
