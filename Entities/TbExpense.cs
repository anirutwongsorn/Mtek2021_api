using System;
using System.Collections.Generic;

#nullable disable

namespace mtek_api.Entities
{
    public partial class TbExpense
    {
        public int ExpId { get; set; }
        public string ExpCd { get; set; }
        public string ExpDesc { get; set; }
        public DateTime? ExpDt { get; set; }
        public string ExpNote { get; set; }
        public decimal? ExpMoney { get; set; }
        public string EmpName { get; set; }
        public bool? Isactv { get; set; }
    }
}
