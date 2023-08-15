using Core.DataProcessing.Model;
using static Core.DataProcessing.DataProcessing;

namespace Core.DataProcessing.Interfaces
{
    public interface IDataProcessing
    {
        event ProcessingDataHandler ProcessScreenCompletedHandler;
        
        event ProcessingDataHandler ProcessCommandsCompletedHandler;
        
        event ProcessingDataHandler ProcessSizeWindowCompletedHandler;

        FrameModel DeserializeFrameModel(byte[] data);
        
        byte[] ProcessSendData(FrameModel frame);
        
        byte[] SerializeFrameModel(FrameModel model);
    }
}