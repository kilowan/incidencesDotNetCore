using Incidences.Data.Models;
using System.Linq;

namespace Incidences.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IncidenceContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Credentials.
            if (context.Credentials.Any())
            {
                return;   // DB has been seeded
            }

            var credentials = new Credentials[]
            {
                new Credentials{username="12345678Z",password="HASHBYTES('MD5', '1234')",employeeId=1},
                new Credentials{username="12345679W",password="HASHBYTES('MD5', '1234')",employeeId=2},
                new Credentials{username="11111111Z",password="HASHBYTES('MD5', '1234')",employeeId=3},
                new Credentials{username="12345678A",password="HASHBYTES('MD5', '1234')",employeeId=4},
                new Credentials{username="12345678S",password="HASHBYTES('MD5', '1234')",employeeId=5},
                new Credentials{username="12345678C",password="HASHBYTES('MD5', '1234')",employeeId=6},
                new Credentials{username="12345678B",password="HASHBYTES('MD5', '1234')",employeeId=7},
            };

            foreach (Credentials c in credentials)
            {
                context.Credentials.Add(c);
            }

            context.SaveChanges();

            var employeeType = new employee_range[]
{
                new employee_range{name="Employee", id=1},
                new employee_range{name="Technician", id=2},
                new employee_range{name="Admin", id=3},
            };

            foreach (employee_range er in employeeType)
            {
                context.EmployeeRange.Add(er);
            }

            var employees = new employee[]
            {
                new employee{dni="12345678Z", name="Jose Javier", surname1="Valero", surname2="Fuentes", typeId=2},
                new employee{dni="12345679Z", name="Juan Francisco", surname1="Navarro", surname2="Ramiro", typeId=2},
                new employee{dni="11111111Z", name="Jose", surname1="admin", surname2="istrador", typeId=3},
                new employee{dni="12345678A", name="Jose", surname1="jackson", surname2="arzapalo", typeId=1},
                new employee{dni="12345678S", name="Jose Antonio", surname1="Lidon", surname2="Ferrer", typeId=1},
                new employee{dni="12345678C", name="Samuel", surname1="Garcia", surname2="Sanchez", typeId=1},
                new employee{dni="12345678B", name="jessie", surname1="deep", surname2=null, typeId=1}
            };
            foreach (employee e in employees)
            {
                context.Employee.Add(e);
            }

            context.SaveChanges();

            var PieceTypes = new piece_type[]
            {
                new piece_type{id=1, name="Interno", description="Componentes relativos al interior de la torre (fuente, placa base, procesador memoria principal, memorias secundarias, etc.)"},
                new piece_type{id=1, name="Externo", description="Componentes periféricos (monitor, impresora, teclado, ratón, etc.)"},
                new piece_type{id=1, name="Otros", description="Componentes adicionales (pendrive, pincho wifi, cables eléctricos, regletas, pilas, etc.)"},
            };
            foreach (piece_type pt in PieceTypes)
            {
                context.PieceType.Add(pt);
            }

            context.SaveChanges();

            var Pieces = new piece_class[]
            {
                new piece_class{id=1, name="RAM", typeId=1, deleted=0},
                new piece_class{id=2, name="HDD o SSD", typeId=1, deleted=0},
                new piece_class{id=3, name="Placa base", typeId=1, deleted=0},
                new piece_class{id=4, name="Tarjeta Gráfica (GPU)", typeId=1, deleted=0},
                new piece_class{id=5, name="Ratón", typeId=2, deleted=0},
                new piece_class{id=6, name="Teclado", typeId=2, deleted=0},
                new piece_class{id=7, name="Impresora", typeId=2, deleted=0},
                new piece_class{id=8, name="Cámara", typeId=2, deleted=0},
                new piece_class{id=9, name="Otros", typeId=3, deleted=0},
            };

            foreach (piece_class pc in Pieces)
            {
                context.PieceClass.Add(pc);
            }

            context.SaveChanges();

            var NoteTypes = new note_type[]
            {
                new note_type{id=1, name="ownerNote"},
                new note_type{id=2, name="solverNote"},
            };

            foreach (note_type nt in NoteTypes)
            {
                context.NoteType.Add(nt);
            }

            context.SaveChanges();

            var States = new state[]
            {
                new state{id=1, name="Nuevo"},
                new state{id=2, name="En curso"},
                new state{id=3, name="Cerrado"},
            };

            foreach (state st in States)
            {
                context.State.Add(st);
            }

            context.SaveChanges();
        }
    }
}
