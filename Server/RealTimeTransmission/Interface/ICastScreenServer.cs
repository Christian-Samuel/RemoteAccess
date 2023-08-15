using static Server.RealTimeTransmission.CastScreenServer;

namespace Server.RealTimeTransmission.Interface
{
    public interface ICastScreenServer
    {
        event FrameReceiveCompletedHandler FrameReceiveCompleted;
    }
}