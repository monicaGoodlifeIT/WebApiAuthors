using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAuthors.DTOs;

namespace WebApiAuthors.Controllers
{
    /// <summary>
    /// Manejo de Registro y LogIn de los Usuarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager">Servicio de Registro de Usuario de Identity</param>
        /// <param name="configuration">Proveedor de Configuración</param>
        /// <param name="signInManager">Servicio de LogIn de Usuario de Identity</param>
        public AccountsController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._signInManager = signInManager;
        }

        /// <summary>
        /// Registro de Nuevo Usuario
        /// </summary>
        /// <param name="credentialsDTO">Credenciales enviadas por el Usuario</param>
        /// <returns>Token y su Caducidad</returns>
        /// <response code="200">Es Correcto</response>
        /// <response code="400">Es Null</response>  
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        [HttpPost("resgister")]
        public async Task<ActionResult<AuthResponseDTO>> ToRegister(CredentialsDTO credentialsDTO)
        {
            // Declarar nuevo Usuario
            var user = new IdentityUser
            {
                UserName = credentialsDTO.Email,
                Email = credentialsDTO.Email,
            };
            // Crear nuevo Usuario
            var result = await _userManager.CreateAsync(user, credentialsDTO.Password);

            // Resultado
            if (result.Succeeded)
            {
                return BuildToken(credentialsDTO);

            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Método Privado que construye el Token del Usuario, con su Nombre de Usuario y Contraseña
        /// </summary>
        /// <param name="credentialsDTO">Credenciales enviadas por el Usuario</param>
        /// <returns></returns>
        private AuthResponseDTO BuildToken(CredentialsDTO credentialsDTO)
        {
            // Claims = Información del usuario emitida por una fuente de confianza.
            var claims = new List<Claim>()
            {
                new Claim("email", credentialsDTO.Email),
                new Claim("lo que yo quiera", "valor asignado")
            };

            // Clave para generar el Token, basada en la clave secreta definida en el fichero de configuración
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKeyJwt"]));

            // Codificar la Clave
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Expiración del Token
            var experation = DateTime.UtcNow.AddYears(1);

            // Generar Token
            var securityToken = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: experation,
                    signingCredentials: creds
                );

            // Respuesta --> Envío del Token generado para el Usuario
            return new AuthResponseDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Experation = experation
            };
        }

        /// <summary>
        /// LogIn de Usuario registrado
        /// </summary>
        /// <param name="credentialsDTO">Credenciales enviadas por el Usuario</param>
        /// <remarks>
        ///   {
        ///     "email": "user@example.com",
        ///     "password": "string"
        ///   }
        /// </remarks>
        /// <returns>Token y su Caducidad</returns>
        /// <response code="200">Es Correcto</response>
        /// <response code="400">Es Null</response>    
        [HttpPost("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<AuthResponseDTO>> ToLogIn(CredentialsDTO credentialsDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(credentialsDTO.Email,
                credentialsDTO.Password,
                isPersistent: false, // Cookie de Autenticación, que no se utiliza en esta API
                lockoutOnFailure: false // Bloqueo de usuario tras varios intentos fallidos
                );

            // Resultado
            if (result.Succeeded)
            {
                return BuildToken(credentialsDTO);

            }
            else
            {
                // Buenas Prácticas: El mensaje no debe dar información concreta, por seguridad
                // ante usuarios malintencionados.
                return BadRequest("LogIn Incorrectos!");
            }

        }
    }
}
