namespace Simple.CQRS.Domain
{
    public interface IDbContextFactory
    {
        IDbContext GetContext();
    }
}
