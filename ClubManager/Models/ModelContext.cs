using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClubManager
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activities> Activities { get; set; }
        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<Announcements> Announcements { get; set; }
        public virtual DbSet<Clubs> Clubs { get; set; }
        public virtual DbSet<JoinClub> JoinClub { get; set; }
        public virtual DbSet<Managers> Managers { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<ParticipateActivity> ParticipateActivity { get; set; }
        public virtual DbSet<Sponsorships> Sponsorships { get; set; }
        public virtual DbSet<StudentMeta> StudentMeta { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:DefaultSchema", "BIGSTOMACH");

            modelBuilder.Entity<Activities>(entity =>
            {
                entity.HasKey(e => e.ActivityId)
                    .HasName("SYS_C0011331");

                entity.HasIndex(e => e.ActivityId)
                    .HasName("SYS_C0011331")
                    .IsUnique();

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.Budget)
                    .HasColumnType("NUMBER(10,2)")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.EventTime).HasColumnType("DATE");

                entity.Property(e => e.IsPublic)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Suggestion).HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("SYS_C0011333");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0011332");
            });

            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("SYS_C0011319");

                entity.HasIndex(e => e.AdminId)
                    .HasName("SYS_C0011319")
                    .IsUnique();

                entity.Property(e => e.AdminId).ValueGeneratedNever();

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(20)");

                entity.HasOne(d => d.Admin)
                    .WithOne(p => p.Administrators)
                    .HasForeignKey<Administrators>(d => d.AdminId)
                    .HasConstraintName("SYS_C0011320");
            });

            modelBuilder.Entity<Announcements>(entity =>
            {
                entity.HasKey(e => e.AnnouncementId)
                    .HasName("SYS_C0011357");

                entity.HasIndex(e => e.AnnouncementId)
                    .HasName("SYS_C0011357")
                    .IsUnique();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Time).HasColumnType("DATE");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Announcements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0011358");
            });

            modelBuilder.Entity<Clubs>(entity =>
            {
                entity.HasKey(e => e.ClubId)
                    .HasName("SYS_C0011315");

                entity.HasIndex(e => e.ClubId)
                    .HasName("SYS_C0011315")
                    .IsUnique();

                entity.Property(e => e.ClubId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("CLOB");

                entity.Property(e => e.EstablishmentDate).HasColumnType("DATE");

                entity.Property(e => e.Logo).HasColumnType("CLOB");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Type).HasColumnType("NUMBER(9)");

                entity.HasOne(d => d.Club)
                    .WithOne(p => p.Clubs)
                    .HasForeignKey<Clubs>(d => d.ClubId)
                    .HasConstraintName("SYS_C0011316");
            });

            modelBuilder.Entity<JoinClub>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ClubId })
                    .HasName("SYS_C0011369");

                entity.HasIndex(e => new { e.ClubId, e.StudentId })
                    .HasName("SYS_C0011369")
                    .IsUnique();

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.ApplyReason).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.JoinClub)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("SYS_C0011370");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.JoinClub)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("SYS_C0011371");
            });

            modelBuilder.Entity<Managers>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ClubId })
                    .HasName("SYS_C0011362");

                entity.HasIndex(e => new { e.ClubId, e.StudentId })
                    .HasName("SYS_C0011362")
                    .IsUnique();

                entity.Property(e => e.Term).HasColumnType("NUMBER(9)");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("SYS_C0011363");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("SYS_C0011364");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("SYS_C0011350");

                entity.HasIndex(e => e.MessageId)
                    .HasName("SYS_C0011350")
                    .IsUnique();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Read)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Time).HasColumnType("DATE");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("SYS_C0011351");
            });

            modelBuilder.Entity<ParticipateActivity>(entity =>
            {
                entity.HasKey(e => new { e.ActivityId, e.StudentId })
                    .HasName("SYS_C0011376");

                entity.HasIndex(e => new { e.StudentId, e.ActivityId })
                    .HasName("SYS_C0011376")
                    .IsUnique();

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.ApplyReason).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ParticipateActivity)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("SYS_C0011378");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ParticipateActivity)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("SYS_C0011377");
            });

            modelBuilder.Entity<Sponsorships>(entity =>
            {
                entity.HasKey(e => e.SponsorshipId)
                    .HasName("SYS_C0011341");

                entity.HasIndex(e => e.SponsorshipId)
                    .HasName("SYS_C0011341")
                    .IsUnique();

                entity.Property(e => e.Amount).HasColumnType("NUMBER(10,2)");

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.Requirement)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Sponsor)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Suggestion).HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Sponsorships)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("SYS_C0011343");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Sponsorships)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SYS_C0011342");
            });

            modelBuilder.Entity<StudentMeta>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("SYS_C0011306");

                entity.HasIndex(e => e.Number)
                    .HasName("SYS_C0011306")
                    .IsUnique();

                entity.Property(e => e.Number)
                    .HasColumnType("NUMBER(7)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Grade).HasColumnType("NUMBER(9)");

                entity.Property(e => e.Major)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(20)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(20)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("1 ");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StudentId)
                    .HasName("SYS_C0011308");

                entity.HasIndex(e => e.StudentId)
                    .HasName("SYS_C0011308")
                    .IsUnique();

                entity.Property(e => e.StudentId).ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("DATE");

                entity.Property(e => e.Mail).HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.Number).HasColumnType("NUMBER(7)");

                entity.Property(e => e.Phone).HasColumnType("NVARCHAR2(20)");

                entity.Property(e => e.Signature).HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.NumberNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Number)
                    .HasConstraintName("SYS_C0011310");

                entity.HasOne(d => d.Student)
                    .WithOne(p => p.Students)
                    .HasForeignKey<Students>(d => d.StudentId)
                    .HasConstraintName("SYS_C0011309");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("SYS_C0011299");

                entity.HasIndex(e => e.UserId)
                    .HasName("SYS_C0011299")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("SYS_C0011300")
                    .IsUnique();

                entity.Property(e => e.ImgUrl)
                    .HasColumnType("NVARCHAR2(2000)")
                    .HasDefaultValueSql("('https://tongji4m3.oss-cn-beijing.aliyuncs.com/OIP.jpg')");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(20)");
            });

            modelBuilder.HasSequence("ISEQ$$_108853");

            modelBuilder.HasSequence("ISEQ$$_108867");

            modelBuilder.HasSequence("ISEQ$$_108870");

            modelBuilder.HasSequence("ISEQ$$_108873");

            modelBuilder.HasSequence("ISEQ$$_108876");

            modelBuilder.HasSequence("ISEQ$$_108885");

            modelBuilder.HasSequence("ISEQ$$_108899");

            modelBuilder.HasSequence("ISEQ$$_108902");

            modelBuilder.HasSequence("ISEQ$$_108905");

            modelBuilder.HasSequence("ISEQ$$_108908");

            modelBuilder.HasSequence("ISEQ$$_97657");

            modelBuilder.HasSequence("ISEQ$$_97660");

            modelBuilder.HasSequence("ISEQ$$_97672");

            modelBuilder.HasSequence("ISEQ$$_98221");

            modelBuilder.HasSequence("ISEQ$$_98366");

            modelBuilder.HasSequence("ISEQ$$_98369");

            modelBuilder.HasSequence("ISEQ$$_98372");

            modelBuilder.HasSequence("ISEQ$$_98376");

            modelBuilder.HasSequence("ISEQ$$_98379");
        }
    }
}
