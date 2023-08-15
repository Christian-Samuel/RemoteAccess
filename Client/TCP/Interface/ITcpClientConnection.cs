using Core.Logger;
using Core.TCP.Interface;
using static Client.TCP.TcpClientConnection;

namespace Client.TCP.Interface
{

    /// <summary>
    /// Define a interface para a conexão do cliente TCP.
    /// </summary>
    public interface ITcpClientConnection
    {
        public event ConnectionErrorHandler ConnectionError;

        public event ConnectionSuccessHandler ConnectionSuccess;

        /// <summary>
        /// Conecta-se de forma assíncrona a um servidor TCP.
        /// </summary>
        /// <param name="ipServer">O endereço IP do servidor ao qual se conectar.</param>
        /// <param name="port">A porta do servidor ao qual se conectar.</param>
        /// <returns>Uma tarefa que representa a operação de conexão assíncrona. O resultado da tarefa é uma resposta que indica se a conexão foi bem-sucedida ou não.</returns>
        Task ConnectAsync(string ipServer, int port);

        /// <summary>
        /// Fecha a conexão com o servidor TCP.
        /// </summary>
        void Close();
    }
}
