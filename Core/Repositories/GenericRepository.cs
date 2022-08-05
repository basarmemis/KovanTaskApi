using Core.IRepositories;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : GenericModel
    {
        protected readonly ClientService _clientService;
        public GenericRepository(ClientService clientService)
        {
            _clientService = clientService;
        }
        public virtual Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }
        public virtual async Task<T> Get(string? page = "", string? bike_id = "", string? vehicle_type = "")
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetById(string? bike_id = "")
        {
            throw new NotImplementedException();
        }
    }
}
