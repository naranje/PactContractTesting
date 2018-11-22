using System.Collections.Generic;
using Bookshelf.Model;

namespace Bookshelf.Store
{
  using System.Threading.Tasks;
  
  public interface IBookshelfStore
  {
    Model.Bookshelf Get(int userId);
    void Save(Model.Bookshelf bookRequest);
  }
}