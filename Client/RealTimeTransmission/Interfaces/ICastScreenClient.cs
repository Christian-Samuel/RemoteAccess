using static Client.RealTimeTransmission.CastScreenClient;

namespace Client.RealTimeTransmission.Interfaces
{
    /// <summary>
    /// Representa um cliente que captura e transmite screenshots da tela em tempo real.
    /// </summary>
    public interface ICastScreenClient
    {
        /// <summary>
        /// Captura a tela atual e dispara o evento <see cref="ScreenshotTaken"/>.
        /// </summary>
        /// <param name="quality">Qualidade da imagem JPEG capturada (0 a 100).</param>
        void CaptureScreen(int quality);
    }
}
