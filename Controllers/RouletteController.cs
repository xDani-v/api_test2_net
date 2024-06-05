using api_test2.Data;
using api_test2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_test2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly RouletteContext _context;

        public RouletteController(RouletteContext context)
        {
            _context = context;
        }

        // Endpoint 1: Devolver un número al azar y un color
        [HttpGet("random")]
        public IActionResult GetRandomNumberAndColor()
        {
            Random rnd = new Random();
            int number = rnd.Next(0, 37);
            string color = number == 0 ? "green" : (number % 2 == 0 ? "red" : "black");

            return Ok(new { Number = number, Color = color });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUserAmount([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Nombre))
                return BadRequest("Datos de usuario inválidos.");

            var existingUser = await _context.Usuarios
            .Where(u => u.Nombre == usuario.Nombre)
            .FirstOrDefaultAsync();


            if (existingUser != null)
            {
                existingUser.Monto += usuario.Monto; // Suma el monto existente con el nuevo monto
                _context.Usuarios.Update(existingUser);
            }
            else
            {
                _context.Usuarios.Add(usuario);
            }

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }




        // GET: api/usuarios/{nombre}/saldo
        [HttpGet("{nombre}/saldo")]
        public async Task<ActionResult<decimal>> GetUsuarioSaldo(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("Nombre de usuario inválido.");
            }

            var usuario = await _context.Usuarios
            .Where(u => u.Nombre == nombre)
            .SingleOrDefaultAsync();

            if (usuario == null)
            {
                // Si el usuario no existe, devolver 0 como saldo
                return Ok(0);
            }

            // Devolver el saldo del usuario
            return Ok(usuario.Monto);
        }

    }
}
