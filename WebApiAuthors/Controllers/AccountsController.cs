using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAuthors.DTOs;
using WebApiAuthors.Services;
using static System.Net.Mime.MediaTypeNames;

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
        private readonly IDataProtector _dataProtector;
        private readonly HashService _hashService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager">Servicio de Registro de Usuario de Identity</param>
        /// <param name="configuration">Proveedor de Configuración</param>
        /// <param name="signInManager">Servicio de LogIn de Usuario de Identity</param>
        /// <param name="dataProtectionProvider">Servicio de Encriptación de Datos</param>
        /// <param name="hashService">Servicio Generador de Hash</param>
        public AccountsController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            IDataProtectionProvider dataProtectionProvider,
            HashService hashService)
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._signInManager = signInManager;
            this._hashService = hashService;
            _dataProtector = dataProtectionProvider.CreateProtector("valor_unico_y_que_deberia_ser_secreto");
        }

        /// <summary>
        /// Genera 2 Hashs, cada uno a partir de una SAL distinta.
        /// </summary>
        /// <param name="planeText">Se pretende que no sea un string hardcodeado</param>
        /// <returns></returns>
        [HttpGet("hash/{planeText}")]
        public IActionResult GenerateHash(string planeText)
        {
            var result1 = _hashService.Hash(planeText);
            var result2 = _hashService.Hash(planeText);

            return Ok(new
            {
                PlaneText = planeText,
                Hash1 = result1,
                Hash2 = result2
            });
        }

        /// <summary>
        /// Endpoint que muestra el funcionamiento de la Encriptación
        /// </summary>
        /// <returns>Devuelve los valores del texto plano, el encriptado y el desencriptado</returns>
        /// <response code="200">Respuesta Exitosa</response>
        [HttpGet("encrypt")]
        [ProducesResponseType(200)]
        public IActionResult Encrypt()
        {
            var planeText = "Rubia Chinita Loca";
            var encryptedText = _dataProtector.Protect(planeText);
            var decryptedText = _dataProtector.Unprotect(encryptedText);

            return Ok(new
            {
                PlaneText = planeText,
                EncryptedText = encryptedText,
                DecryptedText = decryptedText
            });
        }

        /// <summary>
        /// Endpoint que muestra el funcionamiento de la Encriptación, limitada por tiempo
        /// </summary>
        /// <returns>Devuelve los valores del texto plano, el encriptado y el desencriptado</returns>
        /// <response code="200">Respuesta Exitosa</response>
        [HttpGet("encryptbytime")]
        [ProducesResponseType(200)]
        public IActionResult EncryptByTime()
        {
            // Protector limitado por tiempo
            var toTimeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();


            var planeText = "Rubia Chinita Loca";
            var encryptedText = toTimeLimitedProtector.Protect(planeText, lifetime: TimeSpan.FromSeconds(5));
            Thread.Sleep(4000); // Detiene el hilo que se está ejecutando durante 4 segundos 

            var decryptedText = toTimeLimitedProtector.Unprotect(encryptedText);

            return Ok(new
            {
                PlaneText = planeText,
                EncryptedText = encryptedText,
                DecryptedText = decryptedText
            });
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
                return await BuildToken(credentialsDTO);

            }
            else
            {
                return BadRequest(result.Errors);
            }
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
                return await BuildToken(credentialsDTO);

            }
            else
            {
                // Buenas Prácticas: El mensaje no debe dar información concreta, por seguridad
                // ante usuarios malintencionados.
                return BadRequest("LogIn Incorrectos!");
            }

        }

        /// <summary>
        /// Método para Renovar el Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Porque para renovar el token, se debe tener uno ya asignado.
        public async Task<ActionResult<AuthResponseDTO>> ToRenewToken()
        {
            // email tomado del token
            var emailClaim = HttpContext.User.Claims
                .Where(claim => claim.Type == "email")
                .FirstOrDefault();
            var email = emailClaim?.Value; 

            var userCredentials = new CredentialsDTO()
            {
                Email = email
            };

            return await BuildToken(userCredentials);
        }

        /// <summary>
        /// Método Privado que construye el Token del Usuario, con su Nombre de Usuario y Contraseña
        /// </summary>
        /// <param name="credentialsDTO">Credenciales enviadas por el Usuario</param>
        /// <returns></returns>
        private async Task<AuthResponseDTO> BuildToken(CredentialsDTO credentialsDTO)
        {
            // Claims = Información del usuario emitida por una fuente de confianza.
            var claims = new List<Claim>()
            {
                new Claim("email", credentialsDTO.Email),
                new Claim("lo que yo quiera", "valor asignado")
            };

            // Trae los datos del usuario, dado su Emnail
            var user = await _userManager.FindByEmailAsync(credentialsDTO.Email);

            // Se añade el nuevo Claim al usuario
            var claimsDB = await _userManager.GetClaimsAsync(user);

            // Se fusionan ambas listas de Claims, la que se define y la que se recibe de la DB
            claims.AddRange(claimsDB);


            // Clave para generar el Token, basada en la clave secreta definida en el fichero de configuración
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKeyJwt"]));

            // Codificar la Clave
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Expiración del Token
            var experation = DateTime.UtcNow.AddMinutes(30);

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
        /// Añadir Claims de Admin
        /// </summary>
        /// <param name="adminEditDTO"></param>
        /// <returns></returns>
        [HttpPost("AdminMaker")]
        public async Task<ActionResult> AdminMaker(AdminEditDTO adminEditDTO)
        {
            // Trae los datos del usuario, dado su Emnail
            var user = await _userManager.FindByEmailAsync(adminEditDTO.Email);

            // Se añade el nuevo Claim del usuario a la DB --> Tabla [AspNetUserClaims]
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "1"));

            // Se devuleve respuesta Ok, sin contenido
            return NoContent();
        }

        /// <summary>
        /// Eliminar Claims de Admin
        /// </summary>
        /// <param name="adminEditDTO"></param>
        /// <returns></returns>
        [HttpPost("AdminRemove")]
        public async Task<ActionResult> AdminRemove(AdminEditDTO adminEditDTO)
        {
            // TRae los datos del usuario, dado su Emnail
            var user = await _userManager.FindByEmailAsync(adminEditDTO.Email);

            // Se elimina el Claim del usuario de la DB --> Tabla [AspNetUserClaims]
            await _userManager.RemoveClaimAsync(user, new Claim("IsAdmin", "1"));

            // Se devuleve respuesta OK, sin contenido
            return NoContent();
        }
    }
}
