using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TestAban.Entidades;
using TestAban.Services;

namespace TestAban.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        // GET api/clientes
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            try { 
            var clientes = _clienteService.GetAll();

            _logger.LogInformation("Se obtuvieron todos los clientes desde el controlador con éxito");

            return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes desde el controlador");
                return StatusCode(500, "Ocurrió un error al obtener clientes.");
            }
        }

        // GET api/clientes/5
        [HttpGet("{id}")]
        public ActionResult<Cliente> Get(int id)
        {
            try { 
            var cliente = _clienteService.Get(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el cliente con Id {id} desde el controlador");
                return StatusCode(500, "Ocurrió un error al obtener el cliente.");
            }
        }

        // GET api/clientes/search?query=John
        [HttpGet("search")]
        public ActionResult<IEnumerable<Cliente>> Search(string query)
        {
            try
            {
                var clientes = _clienteService.Search(query);
                _logger.LogInformation($"Se obtuvo el filtro de busqueda  {query} desde el controlador con éxito");

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el filtro  busqueda con  {query} desde el controlador");
                return StatusCode(500, "Ocurrió un error al obtener el cliente.");
            }
        }

        // POST api/clientes
        [HttpPost]
        public ActionResult Post([FromBody] Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Datos del cliente no válidos. No se puede crear el cliente.");

                    return BadRequest(ModelState);
                }

                _clienteService.Insert(cliente);
                _logger.LogInformation("Cliente insertado correctamente: {ClienteId}", cliente.Id);

                return CreatedAtAction("Get", new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cliente: {ClienteId}", cliente.Id);
                return BadRequest("Error al insertar el cliente.");
            }

        }


        // PUT api/clientes/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null || id != cliente.Id)
            {
                _logger.LogWarning("Datos del cliente no válidos. No se puede actualizar el cliente.");

                return BadRequest();
            }

            var existingCliente = _clienteService.Get(id);
            if (existingCliente == null)
            {
                _logger.LogError($"Error al actualizar el cliente con Id {id}.");
                return NotFound();
            }

            _clienteService.Update(id, cliente);
            _logger.LogInformation($"Cliente con Id {id} actualizado con éxito.");

            return NoContent();
        }

        // DELETE api/clientes/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cliente = _clienteService.Get(id);
                if (cliente == null)
                {
                    _logger.LogWarning($"Cliente con Id {id} no encontrado. No se puede eliminar.");

                    return NotFound();
                }
                _clienteService.Delete(id);
                _logger.LogInformation($"Cliente con Id {id} eliminado con éxito.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al intentar eliminar el cliente con Id {id}.");
                return StatusCode(500, "Ocurrió un error al eliminar el cliente.");
            }
        }
    }

}
