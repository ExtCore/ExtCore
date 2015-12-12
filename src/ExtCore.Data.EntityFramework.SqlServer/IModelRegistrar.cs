
using Microsoft.Data.Entity;

namespace ExtCore.Data.EntityFramework.SqlServer
{
  public interface IModelRegistrar
  {
    void RegisterModels(ModelBuilder modelbuilder);
  }
}