using Models.BikeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.KovanApiModel
{
    public class KovanBikeDetailModel : GenericModel
    {
        public long last_updated { get; set; }
        public int ttl { get; set; }
        public Data2 data { get; set; }
        public int total_count { get; set; }
    }
    public class Data2
    {
        public Bike bike { get; set; }
    }
}
