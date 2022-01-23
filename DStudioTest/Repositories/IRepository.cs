using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DStudioTest.Repositories
{
    /// <summary>
    /// Interface for repositories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : new()
    {
        IEnumerable<T> GetByFilter(params string[] parameters);
        T GetByKey(string id);
        void Create(T item);
        string Update(T item);
    }
}
