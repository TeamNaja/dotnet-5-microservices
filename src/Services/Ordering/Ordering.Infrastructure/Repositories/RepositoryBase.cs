namespace Ordering.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Ordering.Application.Contracts.Persistence;
    using Ordering.Domain.Common;
    using Ordering.Infrastructure.Persistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext orderContext;

        public RepositoryBase(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await orderContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await orderContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeString = null, 
            bool disableTracking = true, 
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = orderContext.Set<T>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(includeString))
            {
                query = query.Include(includeString);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            List<Expression<Func<T, object>>> includes = null, 
            bool disableTracking = true, 
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = orderContext.Set<T>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes is not null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            return await orderContext.Set<T>().Where(x => x.Id.Equals(Id)).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            orderContext.Set<T>().Add(entity);

            await orderContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            orderContext.Entry(entity).State = EntityState.Modified;

            await orderContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            orderContext.Set<T>().Remove(entity);

            await orderContext.SaveChangesAsync(cancellationToken);
        }
    }
}
