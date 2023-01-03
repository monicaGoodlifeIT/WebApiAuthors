namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO para Respuesta de Servicio Hash
    /// </summary>
    public class HashResultDTO
    {
        /// <summary>
        /// Valor Hash resultante
        /// </summary>
        public string Hash { get; set; } = null!;

        /// <summary>
        /// Valor aletaorio que se añexa al texto plano
        /// Valor aletaorio que se añexa al texto plano
        /// </summary>
        public byte[] Sal { get; set; } = null!;
    }
}
