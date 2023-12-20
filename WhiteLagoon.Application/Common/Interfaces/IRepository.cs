using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class //here generic work like this as we dont know which class will implement this, we T is type of class but nt known of which type, Villa or VillaNumber
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);  //we can use it like var allVillas = villaRepository.GetAll(); OR var filteredVillas = villaRepository.GetAll(v => v.Price < 1000); OR var filteredAndIncludedVillas = villaRepository.GetAll(v => v.Occupancy > 2, "VillaNumber"); For the eager loading
        T Get(Expression<Func<T, bool>>? filter, string? includeProperties = null);
        void Add(T entity);
        bool Any(Expression<Func<T, bool>> filter); //Here null will not be optional
        void Remove(T entity);
        

    }
}
