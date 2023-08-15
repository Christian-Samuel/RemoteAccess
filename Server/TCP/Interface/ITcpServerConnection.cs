using Core.TCP;
using Core.TCP.Interface;
using static Server.TCP.TcpServerConnection;

namespace Server.TCP.Interface
{
    /// <summary>
    /// Defines an interface for the TcpServerConnection class.
    /// </summary>
    public interface ITcpServerConnection
    {
        /// <summary>
        /// Event triggered when the connection state changes.
        /// </summary>
        event ConnectionChangedHandler StartServerConnectionChanged;

        /// <summary>
        /// Event triggered when the connection state changes.
        /// </summary>
        event ConnectionChangedHandler WaitClientConnectionChanged;

        /// <summary>
        /// Asynchronously starts the server.
        /// Binds the socket to the local IP address and starts listening.
        /// Triggers the ConnectionChanged event with the local IP address if initialization is successful,
        /// or with an error if an exception occurs.
        /// </summary>
        void StartServerAsync();

        /// <summary>
        /// Asynchronously waits for a connection to the server.
        /// Accepts connections on the socket and triggers the ConnectionChanged event when a connection is established or if an error occurs.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task WaitForConnectionAsync();
    }

}
