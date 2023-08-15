using Core.Logger.Enum;

namespace Core.Logger
{
    public class BaseResponse<T>
    {
        public T? ObjResponse { get; set; }

        public bool IsSuccess { get; set; }

        public ErrorEnum Error { get; set; }
    }

    public class BaseResponse : BaseResponse<object>
    {
        public static explicit operator BaseResponse(BaseResponse<byte[]> response)
        {
            return new BaseResponse
            {
                IsSuccess = response.IsSuccess,
                Error = response.Error
            };
        }
    }
}
