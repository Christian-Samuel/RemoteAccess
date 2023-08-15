using Core.Configuration.Model;
using Core.RealTimeTransmission.Model;

namespace Core.DataProcessing.Model
{
    [Serializable]
    public class FrameModel
    {
        public DataInfo DataInfo { get; set; }

        public byte[] Screen { get; set; }

        public MouseController MouseController { get; set; }

        public SizeWindow SizeWindow { get; set; }
    }
}
