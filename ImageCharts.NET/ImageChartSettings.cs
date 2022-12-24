namespace ImageCharts.NET;

public class ImageChartSettings
{
    internal int Timeout { get; }
    private readonly string _host;
    private readonly string _scheme;
    private readonly int _port;
    private readonly string _path;

    public ImageChartSettings()
    {
        Timeout = 8000;
        _host = "image-charts.com";
        _scheme = "https";
        _port = 443;
        _path = "chart";
    }

    public ImageChartSettings(string host, string scheme, int port, string path, int timeout = 8000)
    {
        Timeout = timeout;
        _host = host;
        _scheme = scheme;
        _port = port;
        _path = path;
    }

    internal Uri GetUri()
    {
        var imageChartUriBuilder = new UriBuilder(_host)
        {
            Scheme = _scheme,
            Port = _port,
            Path = _path
        };

        return imageChartUriBuilder.Uri;
    }
}