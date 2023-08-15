using Core.Configuration.Model;

namespace Core.Configuration
{
    public class Preferences : IPreferences
    {
        public SizeWindow accessRemoteWindowSize { get; private set; }

        public SizeWindow clientWindowSize { get; private set; }

        public SizeWindow localPcSize { get; private set; }

        public Preferences()
        {
            DefineLocalSizeWindowSize();
        }

        private void DefineLocalSizeWindowSize()
        {
            localPcSize = new SizeWindow();

            localPcSize.Width = Screen.PrimaryScreen.Bounds.Width;
            localPcSize.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        public void DefineAccessRemoteWindowSize(SizeWindow accessRemoteSize)
        {
            accessRemoteWindowSize = accessRemoteSize;
        }

        public void DefineClientWindowSize(SizeWindow clientSize)
        {
            clientWindowSize = clientSize;
        }



    }
}
