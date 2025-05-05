using Domain.Model;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ChatsRepository;

public class FirebaseUsersRepository(ChatsDb dbContext) : IFirebaseUsersRepository
{
    public async Task<Guid> AddFirebaseUser(string userId)
    {
        var firebaseUser = new FirebaseUser
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await dbContext.FirebaseUsers.AddAsync(firebaseUser);
            await dbContext.SaveChangesAsync();
            return firebaseUser.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Guid> GetUserByFirebaseId(string userId)
    {
        var firebaseUser = await dbContext.FirebaseUsers
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (firebaseUser == null)
            throw new Exception("Firebase user not found");

        return firebaseUser.Id;
    }
}