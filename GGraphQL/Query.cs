using Core.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Models.AuthenticationModel;
using Models.BikeModel;
using Models.KovanApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GGraphQL
{
    public class Query
    {
        public Query()
        {
        }
        public async Task<KovanModel> kovanmodel([Service] UserManager<User> _userManager, [Service] IHttpContextAccessor _httpContextAccessor, [Service] IKovanModelRepository _qLRepository, string? page, string? vehicle_type, string? bike_id)
        {
            var unAuthorizedError = ErrorBuilder.New()
                .SetMessage("UnAuthorized")
                .SetCode("401")
                .Build();

            var Context = _httpContextAccessor.HttpContext;
            if (Context == null || Context.User == null || Context.User.Identity == null || Context.User.Identity.Name == null)
            {
                throw new GraphQLException(unAuthorizedError);
            }
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            if (user == null)
                throw new GraphQLException(unAuthorizedError);


            var response = await _qLRepository.Get(page, vehicle_type, bike_id);

            if (response == null || response.data == null || response.data.bikes == null)
            {
                return new Models.KovanApiModel.KovanModel
                {
                    data = new Data { bikes = new List<Bike?>() },
                    last_updated = 0,
                    nextPage = false,
                    total_count = 0,
                    ttl = 0,
                };
            }

            return response;
        }


        public async Task<KovanBikeDetailModel> kovanbikedetailmodel([Service] UserManager<User> _userManager, [Service] IHttpContextAccessor _httpContextAccessor, [Service] IKovanBikeDetailModelRepository _qLRepository, string? bike_id)
        {
            var unAuthorizedError = ErrorBuilder.New()
                .SetMessage("UnAuthorized")
                .SetCode("401")
                .Build();

            var Context = _httpContextAccessor.HttpContext;
            if (Context == null || Context.User == null || Context.User.Identity == null || Context.User.Identity.Name == null)
            {
                throw new GraphQLException(unAuthorizedError);
            }
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            if (user == null)
                throw new GraphQLException(unAuthorizedError);

            var response = await _qLRepository.GetById(bike_id);


            if (response == null || response.data == null || response.data.bike == null)
            {
                return new KovanBikeDetailModel
                {
                    data = new Data2 { bike = new Bike() },
                    last_updated = 0,
                    total_count = 0,
                    ttl = 0,
                };
            }
            return response;
        }

    }
}
