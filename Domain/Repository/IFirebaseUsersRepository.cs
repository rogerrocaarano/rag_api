using Domain.Model;

namespace Domain.Repository;

public interface IFirebaseUsersRepository
{
    Task<Guid> AddFirebaseUser(string userId);
    Task<Guid> GetUserByFirebaseId(string userId);
}