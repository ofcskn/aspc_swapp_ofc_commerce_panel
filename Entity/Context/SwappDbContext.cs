using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Entity.Models;

namespace Entity.Context
{
    public partial class SwappDbContext : DbContext
    {
        public SwappDbContext()
        {
        }

        public SwappDbContext(DbContextOptions<SwappDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<AdminResetPassword> AdminResetPassword { get; set; }
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Calendar> Calendar { get; set; }
        public virtual DbSet<CargoCompany> CargoCompany { get; set; }
        public virtual DbSet<CargoProcess> CargoProcess { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ConfirmationEmail> ConfirmationEmail { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Current> Current { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<EmailAttachments> EmailAttachments { get; set; }
        public virtual DbSet<EmailStatus> EmailStatus { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<MemberSetting> MemberSetting { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageRooms> MessageRooms { get; set; }
        public virtual DbSet<MessageUserRooms> MessageUserRooms { get; set; }
        public virtual DbSet<Newsletter> Newsletter { get; set; }
        public virtual DbSet<NewsletterMail> NewsletterMail { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationMail> NotificationMail { get; set; }
        public virtual DbSet<NotificationType> NotificationType { get; set; }
        public virtual DbSet<NotificationUser> NotificationUser { get; set; }
        public virtual DbSet<PageAnalysis> PageAnalysis { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCargo> ProductCargo { get; set; }
        public virtual DbSet<ProductColor> ProductColor { get; set; }
        public virtual DbSet<ProductComment> ProductComment { get; set; }
        public virtual DbSet<ProductDescription> ProductDescription { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductRating> ProductRating { get; set; }
        public virtual DbSet<ProductSize> ProductSize { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<SaleProcess> SaleProcess { get; set; }
        public virtual DbSet<Search> Search { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<SpecificSetting> SpecificSetting { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<TimeLine> TimeLine { get; set; }
        public virtual DbSet<TimeLineEkstra> TimeLineEkstra { get; set; }
        public virtual DbSet<ToDoList> ToDoList { get; set; }
        public virtual DbSet<ToDoListGroup> ToDoListGroup { get; set; }
        public virtual DbSet<ToDoListUser> ToDoListUser { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<ViewMessageAdmin> ViewMessageAdmin { get; set; }
        public virtual DbSet<ViewMessageAll> ViewMessageAll { get; set; }
        public virtual DbSet<ViewMessageCurrent> ViewMessageCurrent { get; set; }
        public virtual DbSet<ViewMessageStaff> ViewMessageStaff { get; set; }
        public virtual DbSet<ViewMessageUser> ViewMessageUser { get; set; }
        public virtual DbSet<ViewUserAll> ViewUserAll { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-AATLPISC;Database=Swapp;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Adress).HasMaxLength(300);

                entity.Property(e => e.Birthday).HasColumnType("smalldatetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValueSql("(N'default.jpg')");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Phone).HasMaxLength(64);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<AdminResetPassword>(entity =>
            {
                entity.Property(e => e.ExpireTime).HasColumnType("datetime");

                entity.Property(e => e.Ip).HasMaxLength(500);

                entity.Property(e => e.ResetDate).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.AnnouncerRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.SubTitle)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Permalink)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.SubTitle).HasMaxLength(64);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.Property(e => e.BackgroundColor)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.BorderColor)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Subject).HasMaxLength(64);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserRole).HasMaxLength(64);
            });

            modelBuilder.Entity<CargoCompany>(entity =>
            {
                entity.Property(e => e.Image).HasMaxLength(64);

                entity.Property(e => e.Title).HasMaxLength(64);

                entity.Property(e => e.WebSite).HasMaxLength(64);
            });

            modelBuilder.Entity<CargoProcess>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.ProductCargo)
                    .WithMany(p => p.CargoProcess)
                    .HasForeignKey(d => d.ProductCargoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CargoProcess_ProductCargo");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<ConfirmationEmail>(entity =>
            {
                entity.Property(e => e.AdminEmail)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.ConfirmationCode)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Current>(entity =>
            {
                entity.Property(e => e.City).HasMaxLength(64);

                entity.Property(e => e.Image).HasMaxLength(128);

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("(N'::1')");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.No).HasMaxLength(64);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.RegisterIp).HasMaxLength(128);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.Property(e => e.DraftDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiverRole).HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderName).HasMaxLength(256);

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Subject).HasMaxLength(128);
            });

            modelBuilder.Entity<EmailAttachments>(entity =>
            {
                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.Size).HasMaxLength(64);

                entity.HasOne(d => d.Mail)
                    .WithMany(p => p.EmailAttachments)
                    .HasForeignKey(d => d.MailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailAttachments_Email");
            });

            modelBuilder.Entity<EmailStatus>(entity =>
            {
                entity.Property(e => e.FavouriteDate).HasColumnType("datetime");

                entity.Property(e => e.JunkDate).HasColumnType("datetime");

                entity.Property(e => e.PermanentDate).HasColumnType("datetime");

                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.TrashDate).HasColumnType("datetime");

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Email)
                    .WithMany(p => p.EmailStatus)
                    .HasForeignKey(d => d.EmailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmailStatus_Email");
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SerialNumber).HasMaxLength(64);

                entity.Property(e => e.TaxAdministration)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Current)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.CurrentId)
                    .HasConstraintName("FK_Invoice_Current");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Staff");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetail)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetail_Invoice");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.ShortTitle)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.FileNames)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Folder).HasMaxLength(128);

                entity.Property(e => e.Path).HasMaxLength(100);

                entity.Property(e => e.Project).HasMaxLength(16);

                entity.Property(e => e.Title).HasMaxLength(128);
            });

            modelBuilder.Entity<MemberSetting>(entity =>
            {
                entity.Property(e => e.MemberRole).HasMaxLength(128);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.ActionName).HasMaxLength(64);

                entity.Property(e => e.ControllerName).HasMaxLength(64);

                entity.Property(e => e.FontAwesomeClass)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasDefaultValueSql("(N'far fa-circle')");

                entity.Property(e => e.Lang)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasDefaultValueSql("(N'tr-TR')");

                entity.Property(e => e.Role).HasMaxLength(64);

                entity.Property(e => e.RouteString).HasMaxLength(256);

                entity.Property(e => e.RouteValue).HasMaxLength(64);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Message1)
                    .IsRequired()
                    .HasColumnName("Message");

                entity.Property(e => e.ReceiverRole).HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<MessageRooms>(entity =>
            {
                entity.Property(e => e.AdminRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.GroupFor).HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Newsletter>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.UnsubscribeDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NewsletterMail>(entity =>
            {
                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(512);

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.NotType)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.NotTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_NotificationType");
            });

            modelBuilder.Entity<NotificationMail>(entity =>
            {
                entity.Property(e => e.SendDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.Property(e => e.GeneralType).HasMaxLength(64);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<NotificationUser>(entity =>
            {
                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<PageAnalysis>(entity =>
            {
                entity.Property(e => e.Browser).HasMaxLength(512);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EntryDate).HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Lang).HasMaxLength(64);

                entity.Property(e => e.Page)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Stock).HasMaxLength(64);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<ProductCargo>(entity =>
            {
                entity.Property(e => e.CargoChaseLink)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.CargoNo).HasMaxLength(32);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.QrCode).IsRequired();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.CargoCompany)
                    .WithMany(p => p.ProductCargo)
                    .HasForeignKey(d => d.CargoCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCargo_CargoCompany");

                entity.HasOne(d => d.Current)
                    .WithMany(p => p.ProductCargo)
                    .HasForeignKey(d => d.CurrentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCargo_Current");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCargo)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCargo_Product");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.Property(e => e.ColorClass).HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductColor)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductColor_Product");
            });

            modelBuilder.Entity<ProductComment>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductComment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductComment_Product");
            });

            modelBuilder.Entity<ProductDescription>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDescription)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductDescription_Product");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImage_Product");
            });

            modelBuilder.Entity<ProductRating>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RatingScore)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductRating)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductRating_Product");
            });

            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.Property(e => e.ShortSize)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSize)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductSize_Product");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Link).HasMaxLength(128);

                entity.Property(e => e.SubTitle).HasMaxLength(64);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<SaleProcess>(entity =>
            {
                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Total)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.SaleProcess)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_SaleProcess_Invoice");
            });

            modelBuilder.Entity<Search>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Permalink)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Lang)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.SettingJsonData).IsRequired();
            });

            modelBuilder.Entity<SpecificSetting>(entity =>
            {
                entity.Property(e => e.JsonData)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.MemberRole)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(64);

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("(N'::1')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Password).HasMaxLength(256);

                entity.Property(e => e.Phone).HasMaxLength(32);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Staff_Department");
            });

            modelBuilder.Entity<TimeLine>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Lang).HasMaxLength(8);

                entity.Property(e => e.MemberRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.TlekstraId).HasColumnName("TLEkstraId");

                entity.HasOne(d => d.Tlekstra)
                    .WithMany(p => p.TimeLine)
                    .HasForeignKey(d => d.TlekstraId)
                    .HasConstraintName("FK_TimeLine_TimeLineEkstra1");
            });

            modelBuilder.Entity<TimeLineEkstra>(entity =>
            {
                entity.Property(e => e.ColorCode)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.IconClass)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<ToDoList>(entity =>
            {
                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Goal).IsRequired();

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ToDoList)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_ToDoList_ToDoListGroup");
            });

            modelBuilder.Entity<ToDoListGroup>(entity =>
            {
                entity.Property(e => e.AdminRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Color)
                    .HasMaxLength(64)
                    .HasDefaultValueSql("(N'#000')");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icon).HasMaxLength(64);

                entity.Property(e => e.Image).HasMaxLength(128);

                entity.Property(e => e.ShortDescription).HasMaxLength(256);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.TitleColor)
                    .HasMaxLength(64)
                    .HasDefaultValueSql("(N'#fff')");
            });

            modelBuilder.Entity<ToDoListUser>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ToDoListUser)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ToDoListUser_ToDoListGroup");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<ViewMessageAdmin>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewMessageAdmin");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<ViewMessageAll>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewMessageAll");

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ViewMessageCurrent>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewMessageCurrent");

                entity.Property(e => e.Image).HasMaxLength(128);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ViewMessageStaff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewMessageStaff");

                entity.Property(e => e.Email).HasMaxLength(64);

                entity.Property(e => e.Image).HasMaxLength(64);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname).HasMaxLength(128);

                entity.Property(e => e.UserName).HasMaxLength(64);
            });

            modelBuilder.Entity<ViewMessageUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewMessageUser");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SenderRole)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<ViewUserAll>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewUserAll");

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.Mail).HasMaxLength(64);

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Surname).HasMaxLength(128);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
