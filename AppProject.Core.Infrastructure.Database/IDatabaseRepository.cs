using System;
using AppProject.Core.Infrastructure.Database.Entities;

namespace AppProject.Core.Infrastructure.Database;

public interface IDatabaseRepository
{
    public Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    public Task InsertAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    public Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    public Task UpdateAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    public Task DeleteAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    public Task SaveAsync(CancellationToken cancellationToken = default);
}
