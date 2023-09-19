using Infra.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Infra.Implement
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly FucarRentingManagementContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(FucarRentingManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<TEntity>();
        }
        public TEntity GetById(object id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).ToList();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _entities.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            //_context.Attach(entity);

            

            _context.Attach(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Remove(Func<TEntity, bool> filter)
        {
            _entities.Where(filter).AsQueryable().ExecuteDelete();
        }
        public void Remove(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _entities.RemoveRange(entities);
        }
        public void SaveChanges()
        {
             _context.SaveChanges();
        }
        public void Update(Expression<Func<TEntity, bool>>? filter, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
        {
            var query = _entities.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);

            }
            query.ExecuteUpdate(setPropertyCalls);
        }
    }
}
