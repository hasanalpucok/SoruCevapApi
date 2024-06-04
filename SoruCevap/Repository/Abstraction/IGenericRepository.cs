namespace SoruCevapApi
{
    public interface IGenericRepository<T> where T : class
    {

    

        //public IQueryable<T> GetQueryable();
        T FindById(int id);
        void Save(T entity);

        void Delete(int id);
        void Remove( T  entity);

        IQueryable<T> GetQueryable();
        public int GenerateUniqueIntKey();

        public void SaveChanges();



    }
}
