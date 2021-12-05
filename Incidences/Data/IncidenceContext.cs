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
                entity.Property(e => e.id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Credentials>().ToTable("Credentials");
            modelBuilder.Entity<employee>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.HasOne(emp => emp.EmployeeRange)
                .WithOne(er => er.Employee);
                entity.HasOne(emp => emp.Credentials)
                .WithOne(cred => cred.Employee);
            });
            modelBuilder.Entity<employee>().ToTable("employee");
            modelBuilder.Entity<employee_range>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<employee_range>().ToTable("employee_range");
            modelBuilder.Entity<incidence>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.HasOne(inc => inc.owner)
                .WithOne(own => own.emp_inc);
                entity.HasOne(d => d.solver)
                .WithOne(solv => solv.solv_inc);
                entity.HasOne(inc => inc.State)
                .WithOne(st => st.Incidence);
            });
            modelBuilder.Entity<incidence>().ToTable("incidence");
            modelBuilder.Entity<incidence_piece_log>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.HasOne(d => d.Piece)
                .WithOne(pi => pi.ipl);
            });
            modelBuilder.Entity<incidence_piece_log>().ToTable("incidence_piece_log");
            modelBuilder.Entity<note_type>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<note_type>().ToTable("note_type");
            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.HasOne(note => note.NoteType)
                .WithOne(noteType => noteType.Notes);
                entity.HasOne(emp => emp.Employee)
                .WithMany(notes => notes.Notes)
                .HasForeignKey(note => note.employeeId);
                entity.HasOne(note => note.Incidence)
                .WithMany(inc => inc.notes)
                .HasForeignKey(note => note.incidenceId)
                .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Notes>().ToTable("Notes");
            modelBuilder.Entity<piece_class>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.HasOne(pc => pc.PieceType)
                .WithOne(pt => pt.Piece);
            });
            modelBuilder.Entity<piece_class>().ToTable("piece_class");
            modelBuilder.Entity<piece_type>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<piece_type>().ToTable("piece_type");
            modelBuilder.Entity<state>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<state>().ToTable("state");
        }
    }
}
