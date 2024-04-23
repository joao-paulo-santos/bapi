namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IBookRepository BookRepository { get; }
        Task<int> SaveChangesAsync();
        Task Dispose();
    }
}
