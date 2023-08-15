namespace Core.Logger.Interface
{
    /// <summary>
    /// Interface para registrar mensagens em um arquivo de log.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Escreve a mensagem no arquivo de log.
        /// </summary>
        /// <param name="message">A mensagem a ser registrada no log.</param>
        void WriteToLogFile(string message);

        /// <summary>
        /// Fecha o arquivo de log e libera os recursos associados.
        /// </summary>
        void CloseLogFile();
    }
}
