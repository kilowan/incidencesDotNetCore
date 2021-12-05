using Incidences.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Incidences.Data
{
    public class IncidenceContext : DbContext
    {
        public IncidenceContext(DbContextOptions<IncidenceContext> options) : base(options)
        {
        }

        public DbSet<Credentials> Credentialss { get; set; }
        public DbSet<employee> Employees { get; set; }
        public DbSet<employee_range> EmployeeRanges { get; set; }
        public DbSet<incidence> Incidences { get; set; }
        public DbSet<incidence_piece_log> IncidencePieceLogs { get; set; }
        public DbSet<note_type> NoteTypes { get; set; }
        public DbSet<Notes> Notess { get; set; }
        public DbSet<piece_class> PieceClasss { get; set; }
        public DbSet<piece_type> PieceTypes { get; set; }
        public DbSet<state> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Credentials>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
            });
            modelBuilder.Entity<Credentials>().ToTable("Credentials");
            modelBuilder.Entity<employee>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
                entity.HasOne(emp => emp.EmployeeRange);
                entity.HasOne(emp => emp.Credentials);
                entity.Property(b => b.state)
                    .HasDefaultValue(0);
            });
            modelBuilder.Entity<employee>().ToTable("employee");
            modelBuilder.Entity<employee_range>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
            });
            modelBuilder.Entity<employee_range>().ToTable("employee_range");
            modelBuilder.Entity<incidence>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
                entity.HasOne(inc => inc.owner);
                entity.HasOne(d => d.solver);
                entity.Property(b => b.state)
                    .HasDefaultValue(1);
                entity.HasOne(inc => inc.State);
            });
            modelBuilder.Entity<incidence>().ToTable("incidence");
            modelBuilder.Entity<incidence_piece_log>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
                entity.HasOne(d => d.Piece);
                entity.Property(b => b.status)
                    .HasDefaultValue(0);
            });
            modelBuilder.Entity<incidence_piece_log>().ToTable("incidence_piece_log");
            modelBuilder.Entity<note_type>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
            });
            modelBuilder.Entity<note_type>().ToTable("note_type");
            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
                entity.HasOne(note => note.NoteType);
                entity.HasOne(emp => emp.Employee)
                .WithMany(notes => notes.Notes)
                .HasForeignKey(note => note.employeeId);
                entity.HasOne(note => note.Incidence)
                .WithMany(inc => inc.notes)
                .HasForeignKey(note => note.incidenceId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Notes>().ToTable("Notes");
            modelBuilder.Entity<piece_class>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
                entity.HasOne(pc => pc.PieceType);
                entity.Property(b => b.deleted)
                    .HasDefaultValue(0);
            });
            modelBuilder.Entity<piece_class>().ToTable("piece_class");
            modelBuilder.Entity<piece_type>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
            });
            modelBuilder.Entity<piece_type>().ToTable("piece_type");
            modelBuilder.Entity<state>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedNever();
            });
            modelBuilder.Entity<state>().ToTable("state");
        }
    }
}
