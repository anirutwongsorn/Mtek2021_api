using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbCusCommission
    {
        public int Id { get; set; }
        public int? CusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Com { get; set; }
    }
}
