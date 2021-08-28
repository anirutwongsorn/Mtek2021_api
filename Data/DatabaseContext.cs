using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using mtek_api.Entities;

#nullable disable

namespace mtek_api.Data
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbBanner> TbBanners { get; set; }
        public virtual DbSet<TbBillHeader> TbBillHeaders { get; set; }
        public virtual DbSet<TbBillWo> TbBillWos { get; set; }
        public virtual DbSet<TbCustomer> TbCustomers { get; set; }
        public virtual DbSet<TbExpense> TbExpenses { get; set; }
        public virtual DbSet<TbProduct> TbProducts { get; set; }
        public virtual DbSet<TbProductBranch> TbProductBranches { get; set; }
        public virtual DbSet<TbProductGroup> TbProductGroups { get; set; }
        public virtual DbSet<TbThaiLocation> TbThaiLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_100_CI_AS");

            modelBuilder.Entity<TbBanner>(entity =>
            {
                entity.ToTable("TbBanner");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(150)
                    .HasColumnName("imgUrl");
            });

            modelBuilder.Entity<TbBillHeader>(entity =>
            {
                entity.HasKey(e => e.Billcd)
                    .HasName("PK_MASTER_BILLHEADER");

                entity.ToTable("TbBillHeader");

                entity.Property(e => e.Billcd)
                    .HasMaxLength(150)
                    .HasColumnName("BILLCD");

                entity.Property(e => e.Actv)
                    .HasColumnType("datetime")
                    .HasColumnName("ACTV")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CusId)
                    .HasColumnName("CUS_ID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Emp)
                    .HasMaxLength(150)
                    .HasColumnName("EMP");

                entity.Property(e => e.GAmt)
                    .HasColumnType("money")
                    .HasColumnName("G_AMT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GDiscnt)
                    .HasColumnType("money")
                    .HasColumnName("G_DISCNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GTotal)
                    .HasColumnType("money")
                    .HasColumnName("G_TOTAL")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Guid)
                    .HasMaxLength(150)
                    .HasColumnName("GUID");

                entity.Property(e => e.Isactv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISACTV")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.Paidtype)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PAIDTYPE")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Remark)
                    .HasMaxLength(250)
                    .HasColumnName("REMARK");

                entity.Property(e => e.Tax)
                    .HasColumnName("TAX")
                    .HasDefaultValueSql("((7))");

                entity.Property(e => e.Vat)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("VAT");

                entity.HasOne(d => d.Cus)
                    .WithMany(p => p.TbBillHeaders)
                    .HasForeignKey(d => d.CusId)
                    .HasConstraintName("FK_TbBillHeader_TbCustomer");
            });

            modelBuilder.Entity<TbBillWo>(entity =>
            {
                entity.ToTable("TbBillWo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amt)
                    .HasColumnType("money")
                    .HasColumnName("AMT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Billcd)
                    .HasMaxLength(150)
                    .HasColumnName("BILLCD");

                entity.Property(e => e.Discount)
                    .HasColumnType("money")
                    .HasColumnName("DISCOUNT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Pcd)
                    .HasMaxLength(50)
                    .HasColumnName("PCD");

                entity.Property(e => e.Prcs)
                    .HasColumnType("money")
                    .HasColumnName("PRCS")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Qty)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("QTY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Uom)
                    .HasMaxLength(50)
                    .HasColumnName("UOM");

                entity.HasOne(d => d.BillcdNavigation)
                    .WithMany(p => p.TbBillWos)
                    .HasForeignKey(d => d.Billcd)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TbBillWo_TbBillHeader1");

                entity.HasOne(d => d.PcdNavigation)
                    .WithMany(p => p.TbBillWos)
                    .HasForeignKey(d => d.Pcd)
                    .HasConstraintName("FK_TbBillWo_TbProduct");
            });

            modelBuilder.Entity<TbCustomer>(entity =>
            {
                entity.HasKey(e => e.CusId);

                entity.ToTable("TbCustomer");

                entity.Property(e => e.CusId).HasColumnName("CUS_ID");

                entity.Property(e => e.AddressNo)
                    .HasMaxLength(250)
                    .HasColumnName("ADDRESS_NO")
                    .HasDefaultValueSql("(N'ไม่ระบุ')")
                    .UseCollation("Thai_100_CS_AS");

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .HasColumnName("FULL_NAME")
                    .UseCollation("Thai_100_CS_AS");

                entity.Property(e => e.Isactv)
                    .HasColumnName("ISACTV")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Isadmin)
                    .HasColumnName("ISADMIN")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lsactv)
                    .HasColumnType("datetime")
                    .HasColumnName("LSACTV")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NO")
                    .UseCollation("Thai_100_CS_AS");

                entity.Property(e => e.Postcd)
                    .HasMaxLength(10)
                    .HasColumnName("POSTCD")
                    .HasDefaultValueSql("(N'ไม่ระบุ')")
                    .UseCollation("Thai_100_CS_AS");

                entity.Property(e => e.ShopName)
                    .HasMaxLength(250)
                    .HasColumnName("SHOP_NAME");
            });

            modelBuilder.Entity<TbExpense>(entity =>
            {
                entity.HasKey(e => e.ExpId)
                    .HasName("PK_POS_EXPENDS");

                entity.ToTable("TbExpense");

                entity.Property(e => e.ExpId).HasColumnName("EXP_ID");

                entity.Property(e => e.EmpName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("EMP_NAME");

                entity.Property(e => e.ExpCd)
                    .HasMaxLength(50)
                    .HasColumnName("EXP_CD");

                entity.Property(e => e.ExpDesc)
                    .HasMaxLength(150)
                    .HasColumnName("EXP_DESC");

                entity.Property(e => e.ExpDt)
                    .HasColumnType("date")
                    .HasColumnName("EXP_DT");

                entity.Property(e => e.ExpMoney)
                    .HasColumnType("money")
                    .HasColumnName("EXP_MONEY");

                entity.Property(e => e.ExpNote)
                    .HasMaxLength(150)
                    .HasColumnName("EXP_NOTE");

                entity.Property(e => e.Isactv).HasColumnName("ISACTV");
            });

            modelBuilder.Entity<TbProduct>(entity =>
            {
                entity.HasKey(e => e.Pcd);

                entity.ToTable("TbProduct");

                entity.Property(e => e.Pcd)
                    .HasMaxLength(50)
                    .HasColumnName("PCD");

                entity.Property(e => e.Gpcd)
                    .HasColumnName("GPCD")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ImgPath)
                    .HasMaxLength(150)
                    .HasColumnName("IMG_PATH");

                entity.Property(e => e.Lsactv)
                    .HasColumnType("datetime")
                    .HasColumnName("LSACTV")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Minstk)
                    .HasColumnName("MINSTK")
                    .HasDefaultValueSql("((20))");

                entity.Property(e => e.Pdesc)
                    .HasMaxLength(250)
                    .HasColumnName("PDESC");

                entity.Property(e => e.PrcCost)
                    .HasColumnType("money")
                    .HasColumnName("PRC_COST")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PrcSale)
                    .HasColumnType("money")
                    .HasColumnName("PRC_SALE")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Stock)
                    .HasColumnName("STOCK")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Uom)
                    .HasMaxLength(50)
                    .HasColumnName("UOM")
                    .HasDefaultValueSql("(N'อัน')");

                entity.HasOne(d => d.GpcdNavigation)
                    .WithMany(p => p.TbProducts)
                    .HasForeignKey(d => d.Gpcd)
                    .HasConstraintName("FK_TbProduct_TbProductGroup");
            });

            modelBuilder.Entity<TbProductBranch>(entity =>
            {
                entity.ToTable("TbProductBranch");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Blqty)
                    .HasColumnName("BLQTY")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CusId).HasColumnName("CUS_ID");

                entity.Property(e => e.Lastv)
                    .HasColumnType("datetime")
                    .HasColumnName("LASTV")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Pcd)
                    .HasMaxLength(50)
                    .HasColumnName("PCD")
                    .UseCollation("Thai_100_CS_AS");
            });

            modelBuilder.Entity<TbProductGroup>(entity =>
            {
                entity.HasKey(e => e.Gpcd);

                entity.ToTable("TbProductGroup");

                entity.Property(e => e.Gpcd).HasColumnName("GPCD");

                entity.Property(e => e.Gpdesc)
                    .HasMaxLength(150)
                    .HasColumnName("GPDESC");
            });

            modelBuilder.Entity<TbThaiLocation>(entity =>
            {
                entity.HasKey(e => e.Loccd);

                entity.ToTable("TbThaiLocation");

                entity.Property(e => e.Loccd)
                    .ValueGeneratedNever()
                    .HasColumnName("LOCCD");

                entity.Property(e => e.District)
                    .HasMaxLength(250)
                    .HasColumnName("DISTRICT")
                    .UseCollation("Thai_100_CS_AS");

                entity.Property(e => e.Postcd).HasColumnName("POSTCD");

                entity.Property(e => e.Province)
                    .HasMaxLength(250)
                    .HasColumnName("PROVINCE")
                    .UseCollation("Thai_100_CS_AS");

                entity.Property(e => e.Subdistinct)
                    .HasMaxLength(250)
                    .HasColumnName("SUBDISTINCT")
                    .UseCollation("Thai_100_CS_AS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
