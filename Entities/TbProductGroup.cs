using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbProductGroup
    {
        public TbProductGroup()
        {
            TbProducts = new HashSet<TbProduct>();
        }

        public int Gpcd { get; set; }
        public string Gpdesc { get; set; }

        public virtual ICollection<TbProduct> TbProducts { get; set; }
    }
}
