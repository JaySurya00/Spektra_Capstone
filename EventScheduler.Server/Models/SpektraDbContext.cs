using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventScheduler.Server.Models;

public partial class SpektraDbContext : DbContext
{
    public SpektraDbContext()
    {
    }

    public SpektraDbContext(DbContextOptions<SpektraDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersDatum> UsersData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-95U5QAI\\SQLEXPRESS;Database=SpektraDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__events__3213E83F79818469");

            entity.ToTable("events");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Capacity)
                .HasDefaultValue(1000)
                .HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Detail)
                .IsUnicode(false)
                .HasColumnName("detail");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.EventCategories)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("event_categories");
            entity.Property(e => e.EventType)
                .IsUnicode(false)
                .HasColumnName("event_type");
            entity.Property(e => e.ImgUrl)
                .IsUnicode(false)
                .HasColumnName("img_url");
            entity.Property(e => e.Location)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Organizer)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("organizer");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.TicketCost)
                .HasDefaultValue(0)
                .HasColumnName("ticket_cost");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.OrganizerNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Organizer)
                .HasConstraintName("fk_event_organizer");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tickets__3213E83F20556AF3");

            entity.ToTable("tickets");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Owner)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("owner");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_event");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Owner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_owner");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__users__AB6E6165432B1CAC");

            entity.ToTable("users");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        modelBuilder.Entity<UsersDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("usersData");

            entity.HasIndex(e => e.TicketPurchased, "UQ__usersDat__F1094A128D62DD5E").IsUnique();

            entity.Property(e => e.TicketPurchased).HasColumnName("ticket_purchased");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");

            entity.HasOne(d => d.TicketPurchasedNavigation).WithOne()
                .HasForeignKey<UsersDatum>(d => d.TicketPurchased)
                .HasConstraintName("fk_usersData_ticket");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_usersData_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
