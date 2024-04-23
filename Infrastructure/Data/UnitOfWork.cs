using Core.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly PostgressDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public UnitOfWork(PostgressDbContext context, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public IBookRepository BookRepository { get { return _bookRepository; } }
        public IUserRepository UserRepository { get { return _userRepository; } }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task Dispose()
        {
            await _context.DisposeAsync();
            return;
        }
    }
}
