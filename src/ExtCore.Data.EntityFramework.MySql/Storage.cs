using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;

namespace ExtCore.Data.EntityFramework.MySql
{
    public class Storage : IStorage
    {
        public static string ConnectionString { get; set; }
        public StorageContext StorageContext { get; private set; }

        public Storage()
        {
            this.StorageContext = new StorageContext(Storage.ConnectionString);
        }

        public TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            TRepository repository =
                ExtensionManager.GetInstance<TRepository>(a => a.FullName.ToLower().Contains("entityframework.mysql"));

            repository.SetStorageContext(this.StorageContext);
            return repository;
        }

        public void Save()
        {
            this.StorageContext.SaveChanges();
        }
    }
}
