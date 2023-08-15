using Core.Logger;
using System.Net.Sockets;
using static Core.TCP.TcpConnection;

namespace Core.TCP.Interface
{
    /// <summary>
    /// Interface for TCP connection operations.
    /// </summary>
    public interface ITcpConnection
    {
        event ReceiveDataHandler ReceiveDataCompletedHandler;

        /// <summary>
        /// Gets or sets the client IP.
        /// </summary>
        string ipClient { get; set; }

        /// <summary>
        /// Gets or sets the server IP.
        /// </summary>
        string ipServer { get; set; }

        Socket socket { get; set; }

        /// <summary>
        /// Asynchronously sends data over the TCP connection.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a <see cref="BaseResponse"/>.</returns>
        Task<BaseResponse> SendAsync(byte[] data);

        /// <summary>
        /// Starts receiving data from the TCP connection.
        /// </summary>
        void StartReceive();
        void InitializeSocketEvent();
    }
}
