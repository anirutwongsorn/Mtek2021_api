using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbBillHeader
    {
        public TbBillHeader()
        {
            TbBillWos = new HashSet<TbBillWo>();
        }

        public string Billcd { get; set; }
        public string Guid { get; set; }
        public int? CusId { get; set; }
        public string Remark { get; set; }
        public string Emp { get; set; }
        public decimal? GAmt { get; set; }
        public decimal? GDiscnt { get; set; }
        public decimal? GTotal { get; set; }
        public int? Tax { get; set; }
        public decimal? Vat { get; set; }
        public string Paidtype { get; set; }
        public string Isactv { get; set; }
        public DateTime? Actv { get; set; }

        public virtual TbCustomer Cus { get; set; }
        public virtual ICollection<TbBillWo> TbBillWos { get; set; }
    }
}
