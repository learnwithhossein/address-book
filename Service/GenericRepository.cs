using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AddressBook.Persist;
using AddressBook.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Service
{
    public class GenericRepository<T> where T : class
    {
        protected DataContext Context { get; }

        public GenericRepository(DataContext context)
        {
            Context = context;
        }

        public async Task Add(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var table = Context.Set<T>();
            var entity = await table.FindAsync(id);

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Update(T newEntity)
        {
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            var table = Context.Set<T>();
            var entity = await table.FindAsync(id);
            if (entity == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"Entity with key {id} not found.");
            }

            return entity;
        }
    }
}
