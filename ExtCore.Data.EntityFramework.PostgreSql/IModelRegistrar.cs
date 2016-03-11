using Microsoft.Data.Entity;

namespace ExtCore.Data.EntityFramework.PostgreSql
{
  public interface IModelRegistrar
  {
    void RegisterModels(ModelBuilder modelbuilder);
  }
}