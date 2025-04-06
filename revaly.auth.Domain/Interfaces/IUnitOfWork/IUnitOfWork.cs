using revaly.auth.Domain.Interfaces.Repositories.IUserRepository;

namespace revaly.auth.Domain.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        void Dispose();
    }
}
