using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWebAPI.Models;

public partial class RscContext : DbContext
{
    public RscContext()
    {
    }

    public RscContext(DbContextOptions<RscContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customer1> Customers1 { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<ProductMaster> ProductMasters { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-V6JO2I48\\VENIMSSQLSERVER;Database=Rsc;User Id=sa;Password=123; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.BillIdPk).HasName("PK__Billing__CC0E2A1381E1D998");

            entity.ToTable("Billing");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerIdPk).HasName("PK__Customer__A4AE64D8F12B6A81");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.CustomerCode, "UQ__Customer__066785215FEBB248").IsUnique();

            entity.HasIndex(e => e.ContactNumber, "UQ__Customer__570665C6D95FB2C0").IsUnique();

            entity.HasIndex(e => e.CustomerCode, "Unique_customerCode").IsUnique();

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer1>(entity =>
        {
            entity.HasKey(e => e.CustomerIdPk).HasName("PK__Customer__1633B614F15E487B");

            entity.ToTable("Customers");

            entity.HasIndex(e => e.CustomerCode, "UQ__Customer__066785218E37D965").IsUnique();

            entity.HasIndex(e => e.ContactNumber, "UQ__Customer__570665C6EEB0C667").IsUnique();

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeIdPk).HasName("PK__Employee__7AD04F114FF3AF2A");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.EmployeeCode, "UQ__Employee__1F64254830D6007A").IsUnique();

            entity.HasIndex(e => e.ContactNumber, "UQ__Employee__570665C63785ED46").IsUnique();

            entity.HasIndex(e => e.ContactNumber, "Unique_contactnumber").IsUnique();

            entity.HasIndex(e => e.EmployeeCode, "Unique_employeeCode").IsUnique();

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Salary)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("salary");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StoreIdFk)
                .HasConstraintName("FK__Employee__StoreI__4222D4EF");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderIdPk).HasName("PK__Orders__C3905BCFCCB44875");

            entity.HasIndex(e => e.OrderCode, "UQ__Orders__999B5229F8A546F1").IsUnique();

            entity.HasIndex(e => e.OrderCode, "Unique_orderCode").IsUnique();

            entity.Property(e => e.OrderCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.CustomerIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Customer__4F47C5E3");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Employee__503BEA1C");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__StoreIdF__51300E55");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.OrderProductIdPk).HasName("PK__OrderPro__29B019C216ABD751");

            entity.ToTable("OrderProduct");

            entity.Property(e => e.Amount)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PriceUnit)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Quantity)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderIdFk)
                .HasConstraintName("OrderId");

            entity.HasOne(d => d.ProductIdFkNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductIdFk)
                .HasConstraintName("ProductId");
        });

        modelBuilder.Entity<ProductMaster>(entity =>
        {
            entity.HasKey(e => e.ProductIdPk).HasName("PK__ProductM__99A960D0DBCF47CE");

            entity.ToTable("ProductMaster");

            entity.Property(e => e.PricePerUnit).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockIdPk).HasName("PK__Stock__2C83A9C210904A25");

            entity.ToTable("Stock");

            entity.Property(e => e.QuantityAvailable).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.ProductIdFkNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductIdFk)
                .HasConstraintName("ProductIdFK");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreIdFk)
                .HasConstraintName("FK__Stock__StoreId__03F0984C");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreIdPk).HasName("PK__stores_t__3B82F101A6E2521C");

            entity.HasIndex(e => e.StoreCode, "UQ__stores_t__02A384F863EFB77F").IsUnique();

            entity.HasIndex(e => e.StoreCode, "Unique_storeCode").IsUnique();

            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StoreCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StoreContactNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StoreName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
