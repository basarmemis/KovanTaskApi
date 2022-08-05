using Core.IRepositories;
using Models.KovanApiModel;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class KovanModelRepository : GenericRepository<KovanModel>, IKovanModelRepository
    {
        public KovanModelRepository(ClientService clientService) : base(clientService)
        {
        }
        public override async Task<KovanModel> Get(string? page = "", string? vehicle_type = "", string? bike_id = "")
        {

            return await _clientService.GetKovanData(page, vehicle_type, bike_id);
        }
    }
}
