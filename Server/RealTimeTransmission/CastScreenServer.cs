using Core.DataProcessing.Interfaces;
using Core.DataProcessing.Model;
using Server.RealTimeTransmission.Interface;
using System.IO;
using System.Windows.Media.Imaging;

namespace Server.RealTimeTransmission
{
    public class CastScreenServer : ICastScreenServer
    {
        public delegate void FrameReceiveCompletedHandler(BitmapImage response);
        public event FrameReceiveCompletedHandler FrameReceiveCompleted;

        private readonly IDataProcessing _dataProcessing;

        public CastScreenServer(IDataProcessing dataProcessing)
        {
            _dataProcessing = dataProcessing;

            _dataProcessing.ProcessScreenCompletedHandler += OnProcessScreenCompleted;
        }

        private void OnProcessScreenCompleted(FrameModel frame)
        {
            var bitmapImage = SetImage(frame.Screen);

            FrameReceiveCompleted?.Invoke(bitmapImage);
        }

        private BitmapImage SetImage(byte[] imageData)
        {
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;

                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = null;
                bitmap.StreamSource = mem;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
        }

    }
}
