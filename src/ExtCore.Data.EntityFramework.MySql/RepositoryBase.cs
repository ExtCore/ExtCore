using ExtCore.Data.Abstractions;
using ExtCore.Data.Models.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.MySql
{
    public abstract class RepositoryBase<TEntity>:IRepository where TEntity : class, IEntity
    {
        protected StorageContext storageContext;
        protected DbSet<TEntity> dbSet;

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext as StorageContext;
            this.dbSet = this.storageContext.Set<TEntity>();
        }
    }
}
