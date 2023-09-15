using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Interface
{
    public interface IBaseService<TEntity> where TEntity : class { 
    
            TEntity GetById(object id);
            IEnumerable<TEntity> GetAll();
            void Create(TEntity entity);
            void Update(TEntity entity);
            void Delete(TEntity entity);
            void DeleteRange(IEnumerable<TEntity> entities);

    }
}
