using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mtek_api.Data;
using mtek_api.Entities;
using MtekApi.Interfaces;

namespace MtekApi.Services
{
   public class PosService : IPosService
   {
      private CultureInfo en_US = new CultureInfo("en-US");
      private readonly DatabaseContext dbContext;

      private readonly ICustomerService ICus;

      public PosService(DatabaseContext dbContext, ICustomerService customerService)
      {
         this.dbContext = dbContext;
         this.ICus = customerService;
      }

      public async Task<int> PostSale(List<PosSaleDto> productSale)
      {
         string billCd = DateTime.Now.ToString("yyMMdd", en_US);
         var billHeader = new TbBillHeader();
         var billWo = new List<TbBillWo>();
         //billHeader = productSale[0].Adapt<TbBillHeader>();
         billHeader = fromBillMainSaleDto(productSale[0]);
         billWo = productSale.Select(fromBillWoSaleDto).ToList();

         //======= Check Qty must more than 0 ===========
         productSale = productSale.Where(p => p.Qty > 0).ToList();
         if (productSale.Count == 0)
         {
            return 0;
         }

         //=======Looking BillCd==========
         var _guid = productSale[0].BillCd;
         var _lookingGuid = await dbContext.TbBillHeaders.Where(p => p.Guid == _guid).FirstOrDefaultAsync();
         if (_lookingGuid == null)
         {
            int _lookingBill = (await dbContext.TbBillHeaders.CountAsync()) + 1;
            billCd = productSale[0].billtype + billCd + "-" + _lookingBill.ToString();

            billHeader.Billcd = billCd;
            billHeader.Guid = _guid;
            billWo.ForEach(p => p.Billcd = billCd);

            dbContext.TbBillHeaders.Add(billHeader);
            dbContext.TbBillWos.AddRange(billWo);
         }
         else
         {
            billCd = _lookingGuid.Billcd;
            billWo.ForEach(p => p.Billcd = billCd);
            dbContext.TbBillWos.AddRange(billWo);
         }
         return await dbContext.SaveChangesAsync();
      }

      public async Task<List<SaleReportDto>> GetSaleReport(DateTime dateFrom, DateTime dateTo)
      {
         var saleModel = new List<SaleReportDto>();
         saleModel = (await dbContext.TbBillHeaders
                     .Where(p => p.Actv.Value.Date >= dateFrom.Date && p.Actv.Value.Date <= dateTo.Date)
                     .Include(c => c.Cus)
                     .Include(p => p.TbBillWos)
                     .Include(p => p.TbBillWos)
                     .ToListAsync()).Select(SaleReportDto.FromBillHeader).ToList();
         return saleModel;
      }

      public async Task<List<SaleReportDto>> GetSaleReportByCus(int cusid, string pcd)
      {
         var saleModel = new List<SaleReportDto>();
         saleModel = (await dbContext.TbBillHeaders
                       .Where(p => p.CusId == cusid)
                       .Include(p => p.Cus)
                       .Include(p => p.TbBillWos.Where(p => p.Pcd == pcd)).ToListAsync()).Select(SaleReportDto.FromBillHeader).ToList();
         return saleModel;

      }

      public async Task<List<SaleSummaryDateDto>> GetSaleSummaryByDate()
      {
         var _dFrom = DateTime.Now.AddDays(-15).Date;
         var _dTo = DateTime.Now.Date;
         var _saleModel = new List<SaleSummaryDateDto>();
         var _sumBill = await dbContext.TbBillHeaders.Where(p => p.Actv.Value.Date >= _dFrom && p.Actv.Value.Date <= _dTo).ToListAsync();
         var _dataShaping = (from c in _sumBill
                             group c by c.Actv.Value.Date into cg
                             select new
                             {
                                SaleDate = cg.FirstOrDefault().Actv.Value.Date,
                                Amount = (double)cg.Sum(p => p.GAmt),
                                Discount = (double)cg.Sum(p => p.GDiscnt),
                                Total = (double)cg.Sum(p => p.GTotal),
                             }).OrderBy(p => p.SaleDate).ToList();
         double _difVal = 0;
         foreach (var item in _dataShaping)
         {
            if (_difVal == 0) { _difVal = item.Total; }
            _difVal = item.Total - _difVal;
            _saleModel.Add(new SaleSummaryDateDto
            {
               SaleDate = item.SaleDate,
               Amount = item.Amount,
               Discount = item.Discount,
               Total = item.Total,
               DifValue = _difVal
            });
            _difVal = item.Total;
         }
         return _saleModel;
      }

      public async Task<List<SaleSummaryDateDto>> GetSaleSummaryByDateByEmployee(DateTime dateFrom, DateTime dateTo)
      {
         var _saleModel = new List<SaleSummaryDateDto>();
         var _sumBill = await dbContext.TbBillHeaders.Where(p => p.Actv.Value.Date >= dateFrom && p.Actv.Value.Date <= dateTo).ToListAsync();
         var _dataShaping = (from c in _sumBill
                             group c by c.Emp into cg
                             select new
                             {
                                SaleDate = dateFrom.Date,
                                Employee = cg.FirstOrDefault().Emp,
                                Amount = (double)cg.Sum(p => p.GAmt),
                                Discount = (double)cg.Sum(p => p.GDiscnt),
                                Total = (double)cg.Sum(p => p.GTotal),
                                BillCount = cg.ToList().Count
                             }).OrderBy(p => p.SaleDate).ToList();
         double _difVal = 0;
         foreach (var item in _dataShaping)
         {
            if (_difVal == 0) { _difVal = item.Total; }
            _difVal = item.Total - _difVal;
            _saleModel.Add(new SaleSummaryDateDto
            {
               SaleDate = item.SaleDate,
               Employee = item.Employee,
               Amount = item.Amount,
               Discount = item.Discount,
               Total = item.Total,
               DifValue = _difVal,
               BillCount = item.BillCount
            });
            _difVal = item.Total;
         }
         return _saleModel;
      }

      public async Task<List<SaleReportDescDtos>> GetSaleSummaryByBillCd(string billCd)
      {
         return (await dbContext.TbBillHeaders.Where(p => p.Billcd == billCd).Include(p => p.TbBillWos).ToListAsync()).Select(SaleReportDescDtos.FromTbBillMain).ToList();
      }

      public async Task<List<SaleReportDescDtos>> GetSaleSummaryByBillCd(DateTime dateFrom, DateTime dateTo)
      {
         return (await dbContext.TbBillHeaders.Where(p => p.Actv.Value.Date >= dateFrom.Date && p.Actv.Value.Date <= dateTo.Date).Include(p => p.TbBillWos).ToListAsync()).Select(SaleReportDescDtos.FromTbBillMain).ToList();
      }

      private TbBillWo fromBillWoSaleDto(PosSaleDto posSale)
      {
         return new TbBillWo
         {
            Pcd = posSale.Pcd,
            //Pdesc = posSale.Pdesc,
            Uom = posSale.Uom,
            Qty = posSale.Qty,
            Prcs = posSale.Prcs,
            Discount = posSale.Discount,
            Amt = posSale.Amount,
            //ImgPath = posSale.ImgPath
         };
      }

      private TbBillHeader fromBillMainSaleDto(PosSaleDto posSale)
      {
         return new TbBillHeader
         {
            CusId = posSale.CusId,
            // Idcard = posSale.Idcard,
            // Cusname = posSale.Cusname,
            // Phoneno = posSale.Phoneno,
            Emp = posSale.Emp,
            GAmt = posSale.GAmnt,
            GDiscnt = posSale.GDiscnt,
            GTotal = posSale.GAmnt - posSale.GDiscnt,
            Tax = 7,
            Vat = posSale.Vat,
            Paidtype = "N",
            Isactv = "Y",
            Actv = DateTime.Now,
            Remark = posSale.Remark
         };
      }
   }

}