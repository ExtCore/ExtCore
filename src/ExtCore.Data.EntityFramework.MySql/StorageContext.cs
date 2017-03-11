using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.MySql
{
    public class StorageContext : DbContext, IStorageContext
    {
        private string connectionString { get; set; }

        public StorageContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(this.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var modelRegistrar in ExtensionManager.GetInstances<IModelRegistrar>(a => a.FullName.ToLower().Contains("entityframework.mysql")))
            {
                modelRegistrar.RegisterModels(modelBuilder);
            }
        }
    }
}
