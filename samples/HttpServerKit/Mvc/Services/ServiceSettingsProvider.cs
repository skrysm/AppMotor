namespace AppMotor.HttpServerKit.Samples.Mvc.Services;

public sealed class ServiceSettingsProvider
{
    public int HttpsPort { get; }

    public ServiceSettingsProvider(int httpsPort)
    {
        this.HttpsPort = httpsPort;
    }
}
