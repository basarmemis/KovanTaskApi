using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BikeModel
{
    public class Bike
    {
        public string bike_id { get; set; } = "null";
        public double lat { get; set; } = 0;
        public double lon { get; set; } = 0;
        public bool is_reserved { get; set; } = false;
        public bool is_disabled { get; set; } = false;
        public string vehicle_type { get; set; } = "null";
        public int total_bookings { get; set; } = 0;
        public string android { get; set; } = "null";
        public string ios { get; set; } = "null";
    }
}
