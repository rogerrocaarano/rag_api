using Domain.Model;

namespace Domain.Repository;

public interface IContextRepository
{
    Task<Guid> AddContext(string name, string description, List<string> tags);

    Task<Guid> AddFragment(string name, List<string> tags, int sequenceId);

    Task<Context> GetContext(Guid contextId);

    Task<Fragment> GetFragment(Guid fragmentId);
    
    Task<List<Fragment>> GetFragmentsByContextId(Guid contextId);
    
    Task<Fragment> GetFragmentBySequenceId(Guid contextId, int sequenceId);

    Task UpdateContext(Context context);

    Task DeleteContext(Guid contextId);

    Task DeleteFragment(Guid fragmentId);

    Task UpdateFragment(Fragment fragment);
}