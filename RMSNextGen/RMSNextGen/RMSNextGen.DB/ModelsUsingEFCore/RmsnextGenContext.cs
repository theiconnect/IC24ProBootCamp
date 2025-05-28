using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RMSNextGen.Models.ModelsUsingEFCore;

public partial class RmsnextGenContext : DbContext
{
    public RmsnextGenContext()
    {
    }

    public RmsnextGenContext(DbContextOptions<RmsnextGenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<CityMaster> CityMasters { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<KrishnaveniStudent> KrishnaveniStudents { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductMaster> ProductMasters { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SaiStudent> SaiStudents { get; set; }

    public virtual DbSet<StateMaster> StateMasters { get; set; }

    public virtual DbSet<StatusMaster> StatusMasters { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockProduct> StockProducts { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Uom> Uoms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VijayStudent> VijayStudents { get; set; }

    public virtual DbSet<YuvaStudent> YuvaStudents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-V6JO2I48\\VENIMSSQLSERVER;Database=RMSNextGen;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.BillingIdPk).HasName("PK__Billing__3CEE25D5AA9B56F7");

            entity.ToTable("Billing");

            entity.Property(e => e.BillingCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.PricePerUnit).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.OrderIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Billing__OrderId__5AEE82B9");

            entity.HasOne(d => d.ProductIdFkNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.ProductIdFk)
                .HasConstraintName("FK__Billing__Product__5CD6CB2B");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Billing__StatusI__5DCAEF64");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Billings)
                .HasForeignKey(d => d.StoreIdFk)
                .HasConstraintName("FK__Billing__StoreId__5BE2A6F2");
        });

        modelBuilder.Entity<CityMaster>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__CityMast__F2D21B76CB2EA08B");

            entity.ToTable("CityMaster");

            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.State).WithMany(p => p.CityMasters)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_State");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerIdPk).HasName("PK__Customer__1633B614FE503221");

            entity.ToTable("Customer");

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Customer__Status__5EBF139D");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeIdPk).HasName("PK__Employee__0EEEF629D95B0471");

            entity.ToTable("Employee");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentAddressLine1)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CurrentAddressLine2)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CurrentCity)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CurrentPinCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CurrentState)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeFirstName)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeLastName)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PermanentAddressLine1)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.PermanentAddressLine2)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.PermanentCity)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.PermanentPinCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.PermanentState)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SalaryCtc)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SalaryCTC");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Employee__Status__60A75C0F");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StoreIdFk)
                .HasConstraintName("FK__Employee__StoreI__5FB337D6");
        });

        modelBuilder.Entity<KrishnaveniStudent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("KrishnaveniStudent");

            entity.Property(e => e.Comments)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Grade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsOwnTransport)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StudentCode ");
            entity.Property(e => e.StudentName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderIdpk).HasName("PK__Orders__1E64062AB61CD838");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.NoOfItems).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.OrderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CustomerIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerIdFk)
                .HasConstraintName("FK__Orders__Customer__628FA481");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Orders__StatusId__6383C8BA");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreIdFk)
                .HasConstraintName("FK__Orders__StoreIdF__619B8048");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryIdPk).HasName("PK__ProductC__FE425FFB08A67746");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.ProductCategoryCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductCategoryName)
                .HasMaxLength(512)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductMaster>(entity =>
        {
            entity.HasKey(e => e.ProductIdPk).HasName("PK__ProductM__99A960D092EDBF76");

            entity.ToTable("ProductMaster");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.PricePerUnit).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ThresholdLimit).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UomidFk).HasColumnName("UOMIdFk");

            entity.HasOne(d => d.ProductCategoryIdFkNavigation).WithMany(p => p.ProductMasters)
                .HasForeignKey(d => d.ProductCategoryIdFk)
                .HasConstraintName("FK__ProductMa__Produ__656C112C");

            entity.HasOne(d => d.UomidFkNavigation).WithMany(p => p.ProductMasters)
                .HasForeignKey(d => d.UomidFk)
                .HasConstraintName("FK__ProductMa__UOMId__6477ECF3");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleIdPk).HasName("PK__Role__CD65EF7AEAC89864");

            entity.ToTable("Role");

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SaiStudent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SaiStudent");

            entity.Property(e => e.Comments)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Grade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsOwnTransport)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StudentCode ");
            entity.Property(e => e.StudentName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StateMaster>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__StateMas__C3BA3B3A8320DB86");

            entity.ToTable("StateMaster");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<StatusMaster>(entity =>
        {
            entity.HasKey(e => e.StatusIdPk).HasName("PK__StatusMa__ADDE8E16A6653352");

            entity.ToTable("StatusMaster");

            entity.Property(e => e.StatusIdPk).ValueGeneratedNever();
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.StatusCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusName)
                .HasMaxLength(256)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockIdPk).HasName("PK__Stock__7AC4CB61F72371A7");

            entity.ToTable("Stock");

            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedComments).IsUnicode(false);
            entity.Property(e => e.ApprovedOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Remarks).IsUnicode(false);
            entity.Property(e => e.StockCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StockInTime).HasColumnType("datetime");
            entity.Property(e => e.VehicleNumber)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Stock__StatusIdF__68487DD7");

            entity.HasOne(d => d.StoreIdFkNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreIdFk)
                .HasConstraintName("FK__Stock__StoreIdFk__66603565");

            entity.HasOne(d => d.SuplierIdFkNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.SuplierIdFk)
                .HasConstraintName("FK__Stock__SuplierId__6754599E");
        });

        modelBuilder.Entity<StockProduct>(entity =>
        {
            entity.HasKey(e => e.StockProductIdPk).HasName("PK__StockPro__8A908633C203CF9D");

            entity.ToTable("StockProduct");

            entity.Property(e => e.AvailableQuantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.PricePerUnit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RecievedQuantity).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ProductIdFkNavigation).WithMany(p => p.StockProducts)
                .HasForeignKey(d => d.ProductIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockProd__Produ__6A30C649");

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.StockProducts)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__StockProd__Statu__6B24EA82");

            entity.HasOne(d => d.StockIdFkNavigation).WithMany(p => p.StockProducts)
                .HasForeignKey(d => d.StockIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockProd__Stock__693CA210");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreIdpk).HasName("PK__Store__58006C66F0C41CD1");

            entity.ToTable("Store");

            entity.Property(e => e.Cin)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("CIN");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Fax)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Gst)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("GST");
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ManagerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StoreCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StoreName)
                .HasMaxLength(512)
                .IsUnicode(false);

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Stores)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Store__StatusIdF__6C190EBB");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierIdPk).HasName("PK__Supplier__909DDADC92EE78F3");

            entity.ToTable("Supplier");

            entity.Property(e => e.Address)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber1)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber2)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("GSTNumber");
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(23)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.StatusIdFkNavigation).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.StatusIdFk)
                .HasConstraintName("FK__Supplier__Status__6D0D32F4");
        });

        modelBuilder.Entity<Uom>(entity =>
        {
            entity.HasKey(e => e.UomidPk).HasName("PK__UOM__CDB0366B19B6404F");

            entity.ToTable("UOM");

            entity.Property(e => e.UomidPk).HasColumnName("UOMIdPk");
            entity.Property(e => e.Uom1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UOM");
            entity.Property(e => e.Uomcode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UOMCode");
            entity.Property(e => e.Uomdescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UOMDescription");

            entity.HasOne(d => d.ProductCategoryIdFkNavigation).WithMany(p => p.Uoms)
                .HasForeignKey(d => d.ProductCategoryIdFk)
                .HasConstraintName("fk_ProductCategoryID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CA7A29BB3");

            entity.Property(e => e.Email)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VijayStudent>(entity =>
        {
            entity.HasKey(e => e.StudentCode).HasName("PK__VijayStu__1FC886053AEE783C");

            entity.ToTable("VijayStudent");

            entity.Property(e => e.StudentCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Comments)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Grade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsOwnTransport)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StudentName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<YuvaStudent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("YuvaStudent");

            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Grade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
