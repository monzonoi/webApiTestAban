using System.Collections.Generic;
using TestAban.Entidades;

namespace TestAban.Services
{
    public interface IClienteService
    {
        IEnumerable<Cliente> GetAll();
        Cliente Get(int id);
        IEnumerable<Cliente> Search(string query);
        void Insert(Cliente cliente);
        void Update(int id, Cliente cliente);
        void Delete(int id);
    }

}
