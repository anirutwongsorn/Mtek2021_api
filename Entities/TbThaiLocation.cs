using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbThaiLocation
    {
        public int Loccd { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Subdistinct { get; set; }
        public int? Postcd { get; set; }
    }
}
