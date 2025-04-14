namespace Domain.Repository;

public interface IContextRepository
{
    void addContext();

    void addFragment();

    void getContext();

    void getFragment();

    void updateContext();

    void deleteContext();

    void deleteFragment();

    void updateFragment();
}