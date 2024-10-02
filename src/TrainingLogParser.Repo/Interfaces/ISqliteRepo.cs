namespace TrainingLogParser.Repo.Interfaces
{
    public interface ISqliteRepo<TEntity, in TKey> where TEntity : class
    {
        // TODO: Add other CRUD actions

        Task<TEntity> GetByIdAsync(TKey id);
        Task<int> CreateAsync(TEntity entity);
    }
}
