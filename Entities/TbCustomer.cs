using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbCustomer
    {
        public TbCustomer()
        {
            TbBillHeaders = new HashSet<TbBillHeader>();
        }

        public int CusId { get; set; }
        public string FullName { get; set; }
        public string ShopName { get; set; }
        public string PhoneNo { get; set; }
        public string AddressNo { get; set; }
        public string Postcd { get; set; }
        public string Password { get; set; }
        public bool? Isadmin { get; set; }
        public bool? Isactv { get; set; }
        public DateTime? Lsactv { get; set; }

        public virtual ICollection<TbBillHeader> TbBillHeaders { get; set; }
    }
}
