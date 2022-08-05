using HotChocolate.Types;
using Models.BikeModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.KovanApiModel
{
    public class KovanModel : GenericModel
    {
        public long last_updated { get; set; } = -1;
        public int ttl { get; set; } = -1;
        public Data data { get; set; } = new Data();
        public int total_count { get; set; } = -1;
        public bool nextPage { get; set; } = false;
    }

    public class Data
    {
        //[HotChocolate.Types.UsePaging(IncludeTotalCount = true, DefaultPageSize = 3)]
        [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseSorting]
        [UseFiltering]

        public List<Bike> bikes { get; set; } = new List<Bike>();
    }
}
