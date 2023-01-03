using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebApiAuthors.DTOs;

namespace WebApiAuthors.Services
{
    /// <summary>
    /// Servicio para manejo de Hashes
    /// </summary>
    public class HashService
    {
        // Recibe un texto plano y genera una SAL aleatoria, para luego generar el HASH
        public HashResultDTO Hash(string plainText)
        {
            // Define tamaño de propiedad SAL
            var sal = new byte[16];

            // Genera SAL aleatorio
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }
            return Hash(plainText, sal);
        }

        // Genera Hash, dando un texto plano y SAL aleatoria
        public HashResultDTO Hash(string plainText, byte[]  sal)
        {
            var keyDerivation = KeyDerivation.Pbkdf2(password: plainText,
                salt: sal,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(keyDerivation);

            return new HashResultDTO() 
            { 
                Hash = hash,
                Sal = sal
            };
        }
    }
}
