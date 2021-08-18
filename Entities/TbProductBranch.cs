using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbProductBranch
    {
        public int Id { get; set; }
        public int? CusId { get; set; }
        public string Pcd { get; set; }
        public int? Blqty { get; set; }
        public DateTime? Lastv { get; set; }
    }
}
