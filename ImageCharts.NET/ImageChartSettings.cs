namespace ImageCharts.NET;

/// <summary>
/// <c>ImageChartSettings</c> can be used to create an <c>ImageCharts</c> instance with custom settings.
/// </summary>
/// <remarks>
/// <para>
/// By default this will use <c>https://image-charts:443/chart</c> with a Timeout of <c>8000 milliseconds</c>
/// to communicate with the image-charts server. These default settings will automatically be used if you create
/// an <c>ImageCharts</c> instance without providing it with an <c>ImageChartSettings</c> object.
/// </para>
/// <para>
/// If you want to connect to a different service you can create your own <c>ImageChartsSettings</c> object.
/// Pass it to <c>ImageCharts</c> via its constructor.
/// </para>
/// </remarks>
public class ImageChartSettings
{
    internal int Timeout { get; }
    private readonly string _host;
    private readonly string _scheme;
    private readonly int _port;
    private readonly string _path;
    
    internal ImageChartSettings()
    {
        Timeout = 8000;
        _host = "image-charts.com";
        _scheme = "https";
        _port = 443;
        _path = "chart";
    }
    
    /// <summary>
    /// Use this to create a custom <c>ImageCharts</c> configuration.
    /// </summary>
    /// <remarks>
    /// You only need this if you want to set custom connection settings.
    /// The default <c>ImageChartSettings</c> will automatically be used when you create <c>ImageCharts</c>
    /// without parameters.
    /// </remarks>
    /// <param name="host">
    /// The target host
    /// </param>
    /// <param name="scheme">
    /// The scheme you want to use, for example <c>https</c>
    /// </param>
    /// <param name="port">
    /// The target port
    /// </param>
    /// <param name="path">
    /// The path to the api
    /// </param>
    /// <param name="timeout">
    /// Set the max time requests can take. Default is 8000 ms.
    /// </param>
    /// <returns>Your custom configuration</returns>
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