using System.Text.Json;
using Core.DataProcessing.Interfaces;
using Core.DataProcessing.Model;
using Core.RealTimeTransmission.Model;
using Core.TCP.Interface;

namespace Core.DataProcessing
{
    public class DataProcessing : IDataProcessing
    {
        public delegate void ProcessingDataHandler(FrameModel response);
        public event ProcessingDataHandler ProcessScreenCompletedHandler;
        public event ProcessingDataHandler ProcessCommandsCompletedHandler;
        public event ProcessingDataHandler ProcessSizeWindowCompletedHandler;

        private readonly ITcpConnection _connection;

        public DataProcessing(ITcpConnection tcpConnection)
        {
            _connection = tcpConnection;
            _connection.ReceiveDataCompletedHandler += OnReceiveData;
        }

        private void OnReceiveData(byte[] response)
        {
            var frame = DeserializeFrameModel(response);

            switch (frame.DataInfo)
            {
                case DataInfo.DetailsPC:
                    ProcessSizeWindowCompletedHandler(frame);
                    break;

                case DataInfo.Commands:
                    ProcessCommandsCompletedHandler(frame);
                    break;

                case DataInfo.Screen:
                    ProcessScreenCompletedHandler(frame);
                    break;
            }
        }

        public byte[] ProcessSendData(FrameModel frame)
        {
            return SerializeFrameModel(frame);

            //_connection.SendAsync(data);
        }

        public byte[] SerializeFrameModel(FrameModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return JsonSerializer.SerializeToUtf8Bytes(model);
        }

        public FrameModel DeserializeFrameModel(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return JsonSerializer.Deserialize<FrameModel>(data);
        }
    }
}
