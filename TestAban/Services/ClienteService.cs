using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using TestAban.Entidades;

namespace TestAban.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IDbConnection _dbConnection;

        public ClienteService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Cliente> GetAll()
        {
            try { 
            var sql = "SELECT * FROM Clientes";
            return _dbConnection.Query<Cliente>(sql);
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 1) // Error de tabla inexistente
                {
                    CreateDatabaseAndTable();
                }

                throw new System.Exception("Base de datos inexistente, se procedio a su creacion");
            }
        }

        public Cliente Get(int id)
        {
            try
            {
            var sql = "SELECT * FROM Clientes WHERE Id = @Id";
            return _dbConnection.QueryFirstOrDefault<Cliente>(sql, new { Id = id });
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 1) // Error de tabla inexistente
                {
                    CreateDatabaseAndTable();
                }

                throw new System.Exception("Base de datos inexistente, se procedio a su creacion");
            }
        }

        public IEnumerable<Cliente> Search(string query)
        {
            try { 
            var sql = "SELECT * FROM Clientes WHERE Nombres LIKE @Query OR Apellidos LIKE @Query";
            query = $"%{query}%"; // Añade comodines para coincidencias parciales
            return _dbConnection.Query<Cliente>(sql, new { Query = query });
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 1) // Error de tabla inexistente
                {
                    CreateDatabaseAndTable();
                }

                throw new System.Exception("Base de datos inexistente, se procedio a su creacion");
            }
        }

        public void Insert(Cliente cliente)
        {          
            try 
            {
                var sql = @"INSERT INTO Clientes (Nombres, Apellidos, FechaDeNacimiento, CUIT, Domicilio, Celular, Email)
                    VALUES (@Nombres, @Apellidos, @FechaDeNacimiento, @CUIT, @Domicilio, @Celular, @Email)";
                _dbConnection.Execute(sql, cliente);

            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 1) // Error de tabla inexistente
                {                 
                    CreateDatabaseAndTable();
                }         

               throw new System.Exception("Base de datos inexistente, se procedio a su creacion");
            }

        }

        public void Update(int id, Cliente cliente)
        {
            try { 
            var sql = @"UPDATE Clientes
                    SET Nombres = @Nombres, Apellidos = @Apellidos, FechaDeNacimiento = @FechaDeNacimiento,
                        CUIT = @CUIT, Domicilio = @Domicilio, Celular = @Celular, Email = @Email
                    WHERE Id = @Id";
            cliente.Id = id; // Asegúrate de que el objeto cliente tenga el ID correcto
            _dbConnection.Execute(sql, cliente);
            }
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 1) // Error de tabla inexistente
                {
                    CreateDatabaseAndTable();
                }

                throw new System.Exception("Base de datos inexistente, se procedio a su creacion");
            }
        }

        public void Delete(int id)
        {
            try
            {
                var sql = "DELETE FROM Clientes WHERE Id = @Id";
                _dbConnection.Execute(sql, new { Id = id });
            }             
            catch (SqliteException ex)
            {
                if (ex.SqliteErrorCode == 1) // Error de tabla inexistente
                {                 
                    CreateDatabaseAndTable();
    }         

               throw new System.Exception("Base de datos inexistente, se procedio a su creacion");
            }

        }


        #region metodos privados
        private void CreateDatabaseAndTable()
        {
            using (var connection = _dbConnection)
            {
                connection.Open();

                // connection.Execute("CREATE DATABASE IF NOT EXISTS testAbandb");

                // connection.ChangeDatabase("testAbandb.db");

                connection.Execute(@"CREATE TABLE IF NOT EXISTS Clientes (
                Id INTEGER PRIMARY KEY,
                Nombres VARCHAR(255) NOT NULL,
                Apellidos VARCHAR(255) NOT NULL,
                FechaDeNacimiento DATE,
                CUIT VARCHAR(20),
                Domicilio VARCHAR(255),
                Celular VARCHAR(15),
                Email VARCHAR(255)
            )");
            }
        }

        #endregion
    }

}
