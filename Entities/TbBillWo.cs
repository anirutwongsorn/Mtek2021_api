using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbBillWo
    {
        public int Id { get; set; }
        public string Billcd { get; set; }
        public string Pcd { get; set; }
        public string Uom { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Prcs { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Amt { get; set; }

        public virtual TbBillHeader BillcdNavigation { get; set; }
        public virtual TbProduct PcdNavigation { get; set; }
    }
}
