namespace Server.TCP.Status
{
    public enum TcpServerStatus
    {
        /// <summary>
        /// The server is disconnected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The server is waiting for a connection.
        /// </summary>
        WaitingForConnection,

        /// <summary>
        /// The server is connected.
        /// </summary>
        Connected
    }
}

