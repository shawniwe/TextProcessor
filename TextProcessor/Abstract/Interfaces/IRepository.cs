using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessor.Abstract.Interfaces
{
    public interface IRepository<T> where T: BaseEntity
    {
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        T Read(long id);
        IEnumerable<T> ReadAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteAll();
    }
}
