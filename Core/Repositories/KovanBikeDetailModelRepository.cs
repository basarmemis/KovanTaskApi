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
    public class KovanBikeDetailModelRepository : GenericRepository<KovanBikeDetailModel>, IKovanBikeDetailModelRepository
    {
        public KovanBikeDetailModelRepository(ClientService clientService) : base(clientService)
        {
        }
        public override async Task<KovanBikeDetailModel> GetById(string? bike_id = "")
        {
            return await _clientService.GetKovanBikeDetailData(bike_id);
        }
    }
}
