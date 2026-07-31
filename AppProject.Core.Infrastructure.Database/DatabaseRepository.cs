using System;
using AppProject.Core.Contracts;
using AppProject.Core.Infrastructure.Database.Entities;
using AppProject.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Core.Infrastructure.Database;

public class DatabaseRepository(
    ApplicationDbContext applicationDbContext,
    IUserContext userContext)
    : IDatabaseRepository
{
    public async Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        await this.SetAuditFieldAsync(entity, isInsert: true, cancellationToken: cancellationToken);
        await applicationDbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task InsertAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        await this.InsertAsync(entity, cancellationToken);
        await this.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        await this.SetAuditFieldAsync(entity, isInsert: false, cancellationToken: cancellationToken);
        applicationDbContext.Set<TEntity>().Update(entity);
    }

    public async Task UpdateAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        await this.UpdateAsync(entity, cancellationToken);
        await this.SaveAsync(cancellationToken);
    }

    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        applicationDbContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task DeleteAndSaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        await this.DeleteAsync(entity, cancellationToken);
        await this.SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
           await applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException concurrencyException)
        {
            throw new AppException(ExceptionCode.Concurrency, innerException: concurrencyException);
        }
    }

    private async Task SetAuditFieldAsync<TEntity>(TEntity entity, bool isInsert, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        var currentUser = await userContext.GetCurrentUserAsync(cancellationToken);

        var now = DateTime.UtcNow;

        if (isInsert)
        {
            entity.CreatedAt = now;
            entity.CreatedByUserName = currentUser.UserName;
            entity.CreatedByUserId = currentUser.UserId;
        }

        entity.UpdatedAt = now;
        entity.UpdatedByUserName = currentUser.UserName;
        entity.UpdatedByUserId = currentUser.UserId;
    }
}
