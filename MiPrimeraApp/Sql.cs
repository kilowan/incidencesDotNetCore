using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MiPrimeraApp
{
    public class SQL
    {
        private string stringconnection;
        private IDbConnection connection;
        private IDataReader reader;
        private const string SELECT_PEDIDO = "SELECT * FROM Pedido";
        private const string SELECT_REGISTRO_STOCK = "SELECT * FROM registro_stock";
        private const string SELECT_ARTICULO = "SELECT * FROM Articulo";
        private const string SELECT_CLIENTE = "SELECT * FROM Cliente";
        private const string INSERT_CLIENTE = "INSERT INTO Cliente";
        private const string INSERT_ARTICULO = "INSERT INTO Articulo";
        private const string INSERT_REGISTRO_STOCK = "INSERT INTO registro_stock";
        private const string INSERT_PEDIDO = "INSERT INTO Pedido";
        private const string INSERT_ARTICULO_PEDIDO = "INSERT INTO articulo_pedido";
        private const string UPDATE_ARTICULO = "UPDATE Articulo SET";
        private const string UPDATE_PEDIDO = "UPDATE Pedido SET";
        private const string UPDATE_CLIENTE = "UPDATE Cliente SET";
        private const string DELETE_ARTICULO_PEDIDO = "DELETE FROM articulo_pedido";
        private const string DELETE_PEDIDO = "DELETE FROM Pedido";

        public SQL()
        {
            string user = "test";
            string pass = "123456";

            this.stringconnection = $@"Data Source=localhost\SQLEXPRESS;DataBase=Incidences;Persist Security Info=True;USER ID={user};Password={pass};MultipleActiveResultSets=true;";
            this.connection = new SqlConnection(this.stringconnection);
            this.connection.Open();
            this.get_sql = this.connection.CreateCommand();
        }

        //connection
        public IDbCommand get_sql { get; }

        //check
        public bool check(Articulo articulo)
        {
            return check($@"{SELECT_ARTICULO} WHERE Num_serie = '{articulo.Num_serie}'", string.Empty);
        }
        public bool check(Pedido pedido)
        {
            return check($"{SELECT_PEDIDO} WHERE Id = {pedido.Id}", string.Empty);
        }
        public bool check(Cliente cliente)
        {
            return check($"{SELECT_CLIENTE} WHERE Id = '{cliente.Id}'", string.Empty);
        }

        //get
        public IEnumerable<Articulo> get(IList<Articulo> articulos = null)
        {
            this.get_sql.CommandText = articulos != null ? this.get_sql.CommandText += this.Articulos(articulos) : SELECT_ARTICULO;
            using (IDataReader reader = this.get_sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return new Articulo()
                    {
                        id = (int)reader.GetValue(0),
                        Num_serie = $"{reader.GetValue(1)}",
                        nombre = $"{reader.GetValue(2)}",
                        descripcion = $"{reader.GetValue(3)}",
                        cantidad = (int)reader.GetValue(4)
                    };
                }
            }
        }

        private string Articulos(IList<Articulo> articulos)
        {
            string response = string.Empty;
            foreach (Articulo articulo in articulos)
            {
                response += SELECT_ARTICULO + articulo.Num_serie == null ? $" WHERE Id = {articulo.id}" : $" WHERE Num_serie = '{articulo.Num_serie}'";
            }
            return response;
        }
        public IEnumerable<Cliente> customers(string condition, int borrados)
        {
            this.get_sql.CommandText = $@"
                    {SELECT_CLIENTE} 
                    WHERE (
                            Id = '{condition}' 
                            OR Nombre LIKE '%{condition}%') 
                            OR Apellido1 LIKE '%{condition}%' 
                            OR Apellido2 LIKE '%{condition}%'
                           ) 
                            AND borrado = {borrados};";
            using (IDataReader reader = this.get_sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return new Cliente()
                    {
                        Id = (string)filter(reader.GetValue(0)),
                        nombre = (string)filter(reader.GetValue(1)),
                        apellido1 = (string)filter(reader.GetValue(2)),
                        apellido2 = (string)filter(reader.GetValue(3)),
                        calle = (string)filter(reader.GetValue(4)),
                        numero = (int?)filter(reader.GetValue(5)),
                        puerta = (string)filter(reader.GetValue(6)),
                        CP = (int?)filter(reader.GetValue(7)),
                        Ciudad = (string)filter(reader.GetValue(8)),
                        provincia = (string)filter(reader.GetValue(9)),
                        pais = (string)filter(reader.GetValue(10)),
                        tfno_fijo = (int?)filter(reader.GetValue(11)),
                        tfno_movil = (int?)filter(reader.GetValue(12)),
                        borrado = Convert.ToBoolean(reader.GetValue(13))
                    };
                }
            }
        }
        public IEnumerable<Cliente> customers(string condition)
        {
            this.get_sql.CommandText = $@"{SELECT_CLIENTE} WHERE Id = '{condition}' OR Nombre LIKE '%{condition}%' OR Apellido1 LIKE '%{condition}%' OR Apellido2 LIKE '%{condition}%'";
            using (IDataReader reader = this.get_sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return new Cliente()
                    {
                        Id = (string)filter(reader.GetValue(0)),
                        nombre = (string)filter(reader.GetValue(1)),
                        apellido1 = (string)filter(reader.GetValue(2)),
                        apellido2 = (string)filter(reader.GetValue(3)),
                        calle = (string)filter(reader.GetValue(4)),
                        numero = (int?)filter(reader.GetValue(5)),
                        puerta = (string)filter(reader.GetValue(6)),
                        CP = (int?)filter(reader.GetValue(7)),
                        Ciudad = (string)filter(reader.GetValue(8)),
                        provincia = (string)filter(reader.GetValue(9)),
                        pais = (string)filter(reader.GetValue(10)),
                        tfno_fijo = (int?)filter(reader.GetValue(11)),
                        tfno_movil = (int?)filter(reader.GetValue(12)),
                        borrado = Convert.ToBoolean(reader.GetValue(13))
                    };
                }
            }
        }
        public IEnumerable<Pedido> orders(int id1, int id2)
        {
            this.get_sql.CommandText = $@"{SELECT_PEDIDO} WHERE Id_pedido BETWEEN {id1} AND {id2}";
            this.reader = this.get_sql.ExecuteReader();
            while (this.reader.Read())
            {
                yield return new Pedido()
                {
                    Id = (int)this.reader.GetValue(0),
                    Fecha = (DateTime)this.reader.GetValue(1),
                    estado = (string)this.reader.GetValue(2),
                    customer = new Cliente() { Id = (string)this.reader.GetValue(3) }
                };
            }
            this.reader.Close();
        }
        public IEnumerable<Pedido> orders(int? id = null)
        {
            this.get_sql.CommandText = SELECT_PEDIDO + id != null ? $" WHERE Id = {id}" : "";
            this.reader = this.get_sql.ExecuteReader();
            while (this.reader.Read())
            {
                yield return new Pedido()
                {
                    Id = (int)this.reader.GetValue(0),
                    Fecha = (DateTime)this.reader.GetValue(1),
                    estado = (string)this.reader.GetValue(2),
                    customer = new Cliente() { Id = (string)this.reader.GetValue(3) }
                };
            }
            this.reader.Close();
        }
        public IEnumerable<Articulo> order_products(int Id)
        {
            this.get_sql.CommandText = $@" 
                    SELECT a.* 
                    FROM Articulo a INNER JOIN articulo_pedido ap 
                    ON a.ID = ap.Id_articulo 
                    WHERE ap.Id_pedido = {Id}";
            this.reader = this.get_sql.ExecuteReader();
            while (this.reader.Read())
            {
                yield return new Articulo((string)this.reader.GetValue(1), (string)this.reader.GetValue(2), (string)this.reader.GetValue(3), (int)this.reader.GetValue(4));
            }
            this.reader.Close();
        }
        public IEnumerable<Registro_pedido> get(DateTime Desde, DateTime Hasta)
        {
            this.get_sql.CommandText = $@"{SELECT_REGISTRO_STOCK} WHERE Fecha BETWEEN '{Desde}' AND '{Hasta}'";
            this.reader = this.get_sql.ExecuteReader();
            while (this.reader.Read())
            {
                yield return new Registro_pedido()
                {
                    Id = (int)this.reader.GetValue(0),
                    Id_articulo = this.reader.GetValue(1).ToString(),
                    Tipo_mov = this.reader.GetValue(2).ToString(),
                    Cantidad = (int)this.reader.GetValue(3)
                };
            }
            this.reader.Close();
        }

        //set
        public string set(Cliente client)
        {
            return check($@"{INSERT_CLIENTE} {CheckProperties(client)}", "non_query") ? "Inserción satisfactoria" : "Fallo de inserción";
        }
        public bool set(Articulo articulo)
        {
            return check($@"
                {INSERT_ARTICULO} VALUES ('{articulo.Num_serie}', '{articulo.nombre}', '{articulo.descripcion}', {articulo.cantidad});
                {INSERT_REGISTRO_STOCK} VALUES ('{articulo.Num_serie}', {articulo.cantidad}, '{DateTime.Now}');", "");
        }
        public bool set(Pedido pedido)
        {
            string code = $@"
                {INSERT_PEDIDO} VAUES (GETDATE(), 'Creado', '{pedido.customer.Id}');
                DECLARE @id INT = Scope_Identity();";
            foreach (Articulo articulo in pedido.products)
            {
                code += $@"
                    {UPDATE_ARTICULO} Cantidad = Cantidad - {articulo.cantidad} WHERE Num_serie = '{articulo.Num_serie}';
                    {INSERT_ARTICULO_PEDIDO} VALUES ({articulo.cantidad}, @id, {articulo.id});
                    {INSERT_REGISTRO_STOCK} VALUES ('{articulo.Num_serie}', 'S', {articulo.cantidad}, '{DateTime.Now}');";
            }
            string transaction = $@"
                BEGIN TRY
                    BEGIN TRANSACTION
                        {code}
                    COMMIT
                END TRY
                BEGIN CATCH
                    ROLLBACK
                END CATCH;";
            return check(transaction, "non_query");
        }

        //update
        public bool update(Articulo articulo)
        {
            string insert = $"{UPDATE_ARTICULO} {CheckProperties(articulo, "Num_serie")}";
            return check(insert, "non_query");
        }
        public bool update(Pedido pedido)
        {
            return check($@"{UPDATE_PEDIDO} Estado_pedido = '{pedido.estado}' WHERE Id = {pedido.Id}", "non_query");
        }
        public bool update(Cliente cliente)
        {
            string insert = $"{UPDATE_CLIENTE} {CheckProperties(cliente, "Id")}";
            return check(insert, "non_query");
        }

        //delete
        public void delete(Cliente cliente)
        {
            check($"{UPDATE_CLIENTE} borrado = 1 WHERE Id = '{cliente.Id}'", "non_query");
        }
        public void delete(Pedido pedido)
        {
            check($@"
                    {DELETE_ARTICULO_PEDIDO} WHERE Id_pedido = {pedido.Id};
                    {DELETE_PEDIDO} WHERE Id = '{pedido.Id}';", "non_query");
            foreach (Articulo articulo in pedido.products)
            {
                check($@"
                        {UPDATE_ARTICULO} Cantidad = Cantidad + {articulo.cantidad} WHERE Id = {articulo.id};
                        {INSERT_REGISTRO_STOCK} VALUES ({articulo.id}, {articulo.cantidad}, GETDATE())", "non_query");
            }
        }

        //metodos privados
        private bool check(string commandText, string type)
        {
            this.get_sql.CommandText = commandText;
            return type == "non_query" ? (this.get_sql.ExecuteNonQuery() > 0 ? true : false) : (this.get_sql.ExecuteScalar() != null ? true : false);
        }

        private string CheckProperties<T>(T objeto, string condition)
        {
            IList<string> strings = new List<string>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            string conditions = string.Empty;
            foreach (var item in properties)
            {
                string value;
                value = PropertyType(item, objeto);
                //checks fields
                if (item.GetValue(objeto) != null)
                {
                    if (!string.Equals(item.Name, condition))
                    {
                        strings.Add($"{item.Name} = {value}");
                    }
                    //checks condition
                    else
                    {
                        conditions = $"WHERE {item.Name} = {value}";
                    }
                }
            }
            return $"{string.Join(", ", strings)} {conditions}";
        }
        private string CheckProperties<T>(T objeto)
        {
            IList<string> fields = new List<string>();
            IList<string> values = new List<string>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var item in properties)
            {
                if (item.GetValue(objeto) != null)
                {
                    fields.Add($"{item.Name}");
                    values.Add($"{PropertyType(item, objeto)}");
                }
            }
            return $"({string.Join(", ", fields)}) VALUES ({string.Join(", ", values)})";
        }
        private object filter<T>(T thing)
        {
            if (!Convert.IsDBNull(thing))
            {
                return thing;
            }
            else
            {
                return null;
            }
        }

        private string PropertyType(PropertyInfo item, object objeto)
        {
            string value;
            if (item.PropertyType == typeof(string))
            {
                value = $"'{item.GetValue(objeto)}'";
            }
            else if (item.PropertyType == typeof(bool))
            {
                value = $"{((bool)item.GetValue(objeto) ? 1 : 0)}";
            }
            else
            {
                value = $"{item.GetValue(objeto)}";
            }
            return value;
        }

        //Aux Classes
        public class Registro_pedido
        {
            public int Id { get; set; }
            public string Tipo_mov { get; set; }
            public Articulo articulo { get; set; }
            public string Id_articulo { get; set; }
            public DateTime Fecha { get; set; }
            public int Cantidad { get; set; }
        }
    }
}
