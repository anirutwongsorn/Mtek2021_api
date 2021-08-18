using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbProduct
    {
        public TbProduct()
        {
            TbBillWos = new HashSet<TbBillWo>();
        }

        public string Pcd { get; set; }
        public string Pdesc { get; set; }
        public string Uom { get; set; }
        public int? Gpcd { get; set; }
        public decimal? PrcCost { get; set; }
        public decimal? PrcSale { get; set; }
        public int? Stock { get; set; }
        public int? Minstk { get; set; }
        public string ImgPath { get; set; }
        public DateTime? Lsactv { get; set; }

        public virtual TbProductGroup GpcdNavigation { get; set; }
        public virtual ICollection<TbBillWo> TbBillWos { get; set; }
    }
}
