using Microsoft.AspNetCore.WebUtilities;
using Models.BikeModel;
using Models.KovanApiModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://kovan-dummy-api.herokuapp.com/items";
        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<KovanModel> GetKovanData(string? page = "", string? vehicle_type = "", string? bike_id = "")
        {
            var query = new Dictionary<string, string>();

            if (page != null && page != "")
                query.Add("page", page);
            if (bike_id != null && bike_id != "")
                query.Add("bike_id", bike_id);
            if (vehicle_type != null && vehicle_type != "")
                query.Add("vehicle_type", vehicle_type);

            string uri;
            if (query.Count > 0)
                uri = QueryHelpers.AddQueryString(BASE_URL, query);
            else uri = BASE_URL;

            try
            {
                HttpResponseMessage response = await _httpClient.
                                           GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"Error. Status code: " + response.StatusCode);
                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<KovanModel>(resultString);

                var indexes = result.data.bikes.Select((element, index) => element == null ? index : -1).
                    Where(i => i >= 0).
                    ToArray();

                foreach (var index in indexes)
                {
                    result.data.bikes[index] = new Bike();
                };

                return result;
            }
            catch (Exception ex)
            {
                return new KovanModel
                {
                    data = new Data { bikes = new List<Models.BikeModel.Bike?>() },
                    last_updated = 0,
                    nextPage = false,
                    total_count = 0,
                    ttl = 0,
                };
            }

        }


        public async Task<KovanBikeDetailModel> GetKovanBikeDetailData(string? bike_id = "")
        {
            var query = new Dictionary<string, string>();
            if (bike_id != null && bike_id != "")
                query.Add("bike_id", bike_id);

            string uri;
            if (query.Count > 0)
                uri = QueryHelpers.AddQueryString(BASE_URL, query);
            else uri = BASE_URL;

            try
            {
                HttpResponseMessage response = await _httpClient.
                                           GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"Error. Status code: " + response.StatusCode);
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<KovanBikeDetailModel>(resultString);
                return result;
            }
            catch (Exception ex)
            {
                return new KovanBikeDetailModel
                {
                    data = new Data2 { bike = new Bike() },
                    last_updated = 0,
                    total_count = 0,
                    ttl = 0,
                };
            }
        }
    }

}
