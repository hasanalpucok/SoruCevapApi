using Microsoft.EntityFrameworkCore;
using SoruCevap.Models;
using System.Linq;

namespace SoruCevapApi
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        public ApplicationDbContext context;

        DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            _dbSet = context.Set<T>();
        }

        public void Delete(int id)
        {
             _dbSet.Remove(_dbSet.Find(id));
        }

        public T FindById(int id)
        {

            return _dbSet.Find(id);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        

        public void Save(T entity)
        {
             _dbSet.Add(entity);
            context.SaveChanges();

        }

        public int GenerateUniqueIntKey()
        {
           
            return 0;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            context.SaveChanges();
        }
    }
}
