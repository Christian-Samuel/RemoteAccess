using Client.RealTimeTransmission.Interfaces;
using Client.TCP.Interface;
using Core.DataProcessing.Interfaces;
using Core.DataProcessing.Model;
using Core.RealTimeTransmission.Model;
using Core.TCP.Interface;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Drawing.Imaging;

namespace Client.RealTimeTransmission
{
    public class CastScreenClient : ICastScreenClient
    {
        private readonly IDataProcessing _dataProcessing;
        private readonly ITcpConnection _connection;
        private Bitmap cursorBitmap;

        public CastScreenClient(IDataProcessing dataProcessing, ITcpConnection clientConnection)
        {
            _dataProcessing = dataProcessing;
            _connection = clientConnection;
            GetCursorIcon();
        }

        public void CaptureScreen(int quality)
        {
            var screenSize = Screen.PrimaryScreen.Bounds;

            using (var screenshot = new Bitmap(screenSize.Width, screenSize.Height))
            {
                using (var graphics = Graphics.FromImage(screenshot))
                {
                    System.Drawing.Point cursorPosition = Cursor.Position;

                    graphics.CopyFromScreen(screenSize.X, screenSize.Y, 0, 0, screenSize.Size, CopyPixelOperation.SourceCopy);
                    graphics.DrawImage(cursorBitmap, cursorPosition.X, cursorPosition.Y);
                }

                var frame = new FrameModel();
                    
                frame.Screen = ConvertToJpeg(screenshot, quality);
                frame.DataInfo = DataInfo.Screen;

                var data = _dataProcessing.ProcessSendData(frame);
                _connection.SendAsync(data);
            }
        }

        private void GetCursorIcon()
        {
            var cursor = Cursors.Default;
            var icon = Icon.FromHandle(cursor.Handle);
            cursorBitmap = icon.ToBitmap();
        }

        private byte[] ConvertToJpeg(Bitmap sourceBitmap, int quality)
        {
            using var imageSharp = SixLabors.ImageSharp.Image.Load(ConvertBitmapToByteArray(sourceBitmap));

            var options = new JpegEncoder
            {
                Quality = quality

            };

            using var ms = new MemoryStream();

            imageSharp.SaveAsJpeg(ms, options);

            return ms.ToArray();
        }

        public static byte[] ConvertBitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                return stream.ToArray();
            }
        }
    }
}
