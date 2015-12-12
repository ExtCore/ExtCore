
using Microsoft.Data.Entity;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  public interface IModelRegistrar
  {
    void RegisterModels(ModelBuilder modelbuilder);
  }
}