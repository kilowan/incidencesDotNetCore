using Incidences.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incidences.Data
{
    public class IncidenceContext : DbContext
    {
        public IncidenceContext(DbContextOptions<IncidenceContext> options) : base(options)
        {
        }

        public DbSet<Credentials> Credentials { get; set; }
        public DbSet<employee> Employee { get; set; }
        public DbSet<employee_range> EmployeeRange { get; set; }
        public DbSet<incidence> Incidence { get; set; }
        public DbSet<incidence_piece_log> IncidencePieceLog { get; set; }
        public DbSet<note_type> NoteType { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<piece_class> PieceClass { get; set; }
        public DbSet<piece_type> PieceType { get; set; }
        public DbSet<state> State { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Credentials>().ToTable("Credentials");
            modelBuilder.Entity<employee>().ToTable("employee");
            modelBuilder.Entity<employee_range>().ToTable("employee_range");
            modelBuilder.Entity<incidence>().ToTable("incidence");
            modelBuilder.Entity<incidence_piece_log>().ToTable("incidence_piece_log");
            modelBuilder.Entity<note_type>().ToTable("note_type");
            modelBuilder.Entity<Notes>().ToTable("Notes");
            modelBuilder.Entity<piece_class>().ToTable("piece_class");
            modelBuilder.Entity<piece_type>().ToTable("piece_type");
            modelBuilder.Entity<state>().ToTable("state");
        }
    }
}
