using Core.Configuration.Model;

namespace Core.Configuration
{
    public interface IPreferences
    {
        SizeWindow accessRemoteWindowSize { get; }
        SizeWindow clientWindowSize { get; }
        SizeWindow localPcSize { get; }
    }
}
