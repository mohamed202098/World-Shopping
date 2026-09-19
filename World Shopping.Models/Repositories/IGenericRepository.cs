using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace World_Shopping.Models.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // _Context.Categories.ToList();
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate = null, string? Includeword = null);
        IEnumerable<T> GetAllByInclude(string? Includeword = null);
        T GetFirstorDafault(Expression<Func<T, bool>> perdicate, string? Includeword);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entites);
    }
}
