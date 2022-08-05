
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IGenericRepository<T> where T : GenericModel
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string? page = "", string? vehicle_type = "", string? bike_id = "");
        Task<T> GetById(string? bike_id = "");
    }
}
