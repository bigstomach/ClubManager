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
        public virtual DbSet<JoinClubs> JoinClubs { get; set; }
        public virtual DbSet<ParticipateActivity> ParticipateActivity { get; set; }
        public virtual DbSet<Specifications> Specifications { get; set; }
        public virtual DbSet<Sponsorships> Sponsorships { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:DefaultSchema", "BIGSTOMACH");

            modelBuilder.Entity<Activities>(entity =>
            {
                entity.HasKey(e => e.ActivityId);

                entity.HasIndex(e => e.ActivityId)
                    .HasName("PK_Activities")
                    .IsUnique();

                entity.HasIndex(e => e.ClubId)
                    .HasName("IX_Activity_Club");

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.Cost).HasColumnType("NUMBER(10,2)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Fund).HasColumnType("NUMBER(10,2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Suggestion).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Time).HasColumnType("DATE");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Act_User");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_Activity_Club");
            });

            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Admin");

                entity.HasIndex(e => e.UserId)
                    .HasName("PK_Admin")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Administrators)
                    .HasForeignKey<Administrators>(d => d.UserId)
                    .HasConstraintName("FK_Admin_User");
            });

            modelBuilder.Entity<Announcements>(entity =>
            {
                entity.HasKey(e => new { e.ClubId, e.AnnouncementId })
                    .HasName("PK_Announce");

                entity.HasIndex(e => new { e.AnnouncementId, e.ClubId })
                    .HasName("PK_Announce")
                    .IsUnique();

                entity.Property(e => e.AnnouncementId).ValueGeneratedOnAdd();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Time).HasColumnType("DATE");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Announcements)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_Announce_Club");
            });

            modelBuilder.Entity<Clubs>(entity =>
            {
                entity.HasKey(e => e.ClubId);

                entity.HasIndex(e => e.ClubId)
                    .HasName("PK_Clubs")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Club_UserId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.EstablishmentDate).HasColumnType("DATE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Type).HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Clubs)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Club_StuId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Clubs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Club_UserId");
            });

            modelBuilder.Entity<JoinClubs>(entity =>
            {
                entity.HasKey(e => new { e.ClubId, e.StudentId })
                    .HasName("PK_StuClub");

                entity.HasIndex(e => new { e.StudentId, e.ClubId })
                    .HasName("PK_StuClub")
                    .IsUnique();

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.Payed).HasColumnType("NUMBER(2)");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql(@"0
");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.JoinClubs)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_StuClub_Club");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.JoinClubs)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_StuClub_Stu");
            });

            modelBuilder.Entity<ParticipateActivity>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ActivityId });

                entity.HasIndex(e => new { e.ActivityId, e.StudentId })
                    .HasName("PK_ParticipateActivity")
                    .IsUnique();

                entity.Property(e => e.ApplyDate).HasColumnType("DATE");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql(@"0
");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ParticipateActivity)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("FK_ParAct_Act");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ParticipateActivity)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_ParAct_Stu");
            });

            modelBuilder.Entity<Specifications>(entity =>
            {
                entity.HasKey(e => e.SpecificationId)
                    .HasName("PK_Spec");

                entity.HasIndex(e => e.SpecificationId)
                    .HasName("PK_Spec")
                    .IsUnique();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Date).HasColumnType("DATE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Specifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Spec_Admin");
            });

            modelBuilder.Entity<Sponsorships>(entity =>
            {
                entity.HasKey(e => e.SponsorshipId)
                    .HasName("PK_Sponsorship");

                entity.HasIndex(e => e.ClubId)
                    .HasName("IX_Spon_Club");

                entity.HasIndex(e => e.SponsorshipId)
                    .HasName("PK_Sponsorship")
                    .IsUnique();

                entity.Property(e => e.Amount).HasColumnType("NUMBER(10,2)");

                entity.Property(e => e.ApplyTime).HasColumnType("DATE");

                entity.Property(e => e.Requirement).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Sponsor)
                    .IsRequired()
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Suggestion).HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Sponsorships)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Sponsor_User");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Sponsorships)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_Spon_Club");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.HasIndex(e => e.StudentId)
                    .HasName("PK_Students")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("STU_USER_UQ");

                entity.Property(e => e.Grade).HasColumnType("NUMBER(2)");

                entity.Property(e => e.Major).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Name).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Number).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Phone).HasColumnType("NVARCHAR2(2000)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.UserId)
                    .HasName("PK_Users")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("USERNAME_UQ")
                    .IsUnique();

                entity.Property(e => e.Password).HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.UserName).HasColumnType("NVARCHAR2(2000)");
            });

            modelBuilder.HasSequence("ISEQ$$_94058");

            modelBuilder.HasSequence("ISEQ$$_94076");

            modelBuilder.HasSequence("ISEQ$$_94083");

            modelBuilder.HasSequence("ISEQ$$_94094");

            modelBuilder.HasSequence("ISEQ$$_94097");

            modelBuilder.HasSequence("ISEQ$$_94106");

            modelBuilder.HasSequence("ISEQ$$_94109");

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
