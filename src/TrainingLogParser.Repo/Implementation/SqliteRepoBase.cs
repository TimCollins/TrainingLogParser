using TrainingLogParser.Repo.Interfaces;

namespace TrainingLogParser.Repo.Implementation
{
    public abstract class SqliteRepoBase<TEntity, TKey> : ISqliteRepo<TEntity, TKey> where TEntity : class, new()
    {
        //private readonly CoreSqlDapperContext _coreSqlDapperContext;

        public async Task<int> CreateAsync(TEntity entity)
        {
            //using var connection = _coreSqlDapperContext.CreateConnection();

            //var key = await connection.InsertAsync(entity);

            //return key;

            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(TKey id)
        {
            if (id == null)
            {
                throw new ArgumentException($"{nameof(id)} in {nameof(GetByIdAsync)} is a required field");
            }

            throw new NotImplementedException();
        }
    }
}
