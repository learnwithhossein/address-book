using Persist;
using Service.Common;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class GenericRepository<T> where T : class
    {
        protected DataContext Context { get; }

        public GenericRepository(DataContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var table = Context.Set<T>();
            var entity = table.Find(id);

            Context.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T newEntity)
        {
            Context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public T GetById(int id)
        {
            var table = Context.Set<T>();
            var entity = table.Find(id);
            if (entity == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"Entity with key {id} not found.");
            }

            return entity;
        }
    }
}
