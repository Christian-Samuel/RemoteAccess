using System.ComponentModel;

namespace Core.Logger.Enum
{
    public enum ErrorEnum
    {
        None = 0,

        [Description("Erro no socket")]
        SocketException,

        [Description("Argumento inválido")]
        ArgumentException,

        [Description("Objeto descartado")]
        ObjectDisposedException,

        [Description("Operação não suportada")]
        NotSupportedException,

        [Description("Operação inválida")]
        InvalidOperationException,

        [Description("Erro Generico")]
        UnknownError,

        [Description("Limite de tempo atigindo ao esperar conexão")]
        TcpServerConnectionTimeOut,

        [Description("Computador não conectado a internet")]
        NoConnection
    }
}
