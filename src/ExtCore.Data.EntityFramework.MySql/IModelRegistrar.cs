using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.MySql
{
    public interface IModelRegistrar
    {
        void RegisterModels(ModelBuilder modelBuilder);
    }
}
