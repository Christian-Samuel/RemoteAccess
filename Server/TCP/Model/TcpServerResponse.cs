using Core.Logger.Enum;
using Server.TCP.Status;

namespace Server.TCP.Model
{
    public class TcpServerResponse<T>
    {
        public T? ObjResponse { get; set; }

        public ErrorEnum Error { get; set; }

        public TcpServerStatus Status { get; set; }

        public bool IsSuccess { get; set; }
    }

    public class TcpServerResponse : TcpServerResponse<object>
    {
        public static explicit operator TcpServerResponse(TcpServerResponse<string> response)
        {
            return new TcpServerResponse
            {
                IsSuccess = response.IsSuccess,
                Error = response.Error,
                Status = response.Status,
            };
        }
    }
}
