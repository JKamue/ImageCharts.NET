using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ImageCharts.NET;

/// <summary>
/// <c>ImageCharts</c> can be used to create different types of charts.
/// For more information on the various commands visit <c>https://documentation.image-charts.com/</c>
/// </summary>
public class ImageChart
{
    private readonly ImageChartSettings _settings;
    private readonly Dictionary<string, string> _parameters = new();

    /// <summary>
    /// Creates an <c>ImageCharts</c> instance.
    /// </summary>
    public ImageChart()
    {
        _settings = new ImageChartSettings();
    }
    
    /// <summary>
    /// Creates an <c>ImageCharts</c> instance with custom settings.
    /// If you want to use the default settings call <c>ImageCharts</c> with an empty constructor instead.
    /// </summary>
    public ImageChart(ImageChartSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Creates an Image-Charts API url.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note that this is not recommended for large datasets.
    /// Otherwise the URL might go above the max URL length of various programs.
    /// You should use
    /// <see cref="toBufferAsync"/>,
    /// <see cref="toFileAsync"/> or
    /// <see cref="toDataURIAsync"/>
    /// instead. These use POST Requests for requests up to 300 Kilobytes long.
    /// </para>
    /// </remarks>
    public string toURL()
    {
        var queryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
        
        foreach (var parameterKeyValuePair in _parameters)
            queryParameters.Add(
                parameterKeyValuePair.Key,
                parameterKeyValuePair.Value);

        return $"{_settings.GetUri()}?{queryParameters}";
    }

    /// <summary>
    /// Asynchronously creates a buffer containing the chart
    /// </summary>
    public async Task<byte[]> toBufferAsync()
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromMilliseconds(_settings.Timeout);
        client.DefaultRequestHeaders.Add("User-Agent", "JKamue/ImageCharts.NET");

        var parametersAsKeyValuePair = _parameters.ToList();
        var formContent = new FormUrlEncodedContent(parametersAsKeyValuePair);
        
        var result = await client.PostAsync(_settings.GetUri(), formContent);
        if (result.IsSuccessStatusCode)
            return await result.Content.ReadAsByteArrayAsync();

        var errorMessage = GenerateErrorMessage(result);
        throw new ImageChartException(errorMessage);
    }

    private string GenerateErrorMessage(HttpResponseMessage result)
    {
        var defaultErrorMessage = 
            "Chart generation failed with HTTP response code " +
            $"\"{(int) result.StatusCode}: {result.StatusCode}\". " +
            "No further information provided";
        
        IEnumerable<string>? values;
        if (!result.Headers.TryGetValues("x-ic-error-validation", out values))
            return defaultErrorMessage;
        
        var enumerable = values as string[] ?? values.ToArray();
        if (!enumerable.Any())
            return defaultErrorMessage;
        
        var imageChartErrorListAsJson = enumerable.First();
        var imageChartErrorList = JsonNode.Parse(imageChartErrorListAsJson);

        if (imageChartErrorList is null)
            return defaultErrorMessage;
        
        var errorList = new List<string?>();
        foreach (var imageChartErrorNode in imageChartErrorList.AsArray())
        {
            if (imageChartErrorNode is not JsonObject imageChartErrorObject)
                continue;

            if (!imageChartErrorObject.ContainsKey("message"))
                continue;

            if (imageChartErrorObject["message"] is null)
                continue;

            var message = imageChartErrorObject["message"];
            
            if (message is null)
                continue;
            
            if (!message.AsValue().TryGetValue<string>(out var errorMessage))
                continue;
            
            if (string.IsNullOrWhiteSpace(errorMessage))
                continue;
            
            errorList.Add(errorMessage);
        }
        
        return errorList.Count == 0 
            ? defaultErrorMessage 
            : string.Join(" and ", errorList);
    }

    /// <summary>
    /// Asynchronously generates the chart and stores it at a custom location
    /// </summary>
    /// <param name="path">
    /// The path and name to store the file at
    /// </param>
    public async Task toFileAsync(string path)
    {
        var imageAsBuffer = await toBufferAsync();
        await File.WriteAllBytesAsync(path, imageAsBuffer);
    }

    /// <summary>
    /// Asynchronously generates the chart and returns it as a base64 encoded string
    /// </summary>
    public async Task<string> toDataURIAsync()
    {
        var fileFormat = _parameters.ContainsKey("chan")
            ? "gif"
            : "png";
        
        var imageAsBuffer = await toBufferAsync();
        var encodedImage = Convert.ToBase64String(imageAsBuffer);
        
        return $"data:image/{fileFormat};base64,{encodedImage}";
    }
    
    /// <summary>
    /// Creates a buffer containing the chart
    /// </summary>
    public byte[] toBuffer() => toBufferAsync().Result;

    /// <summary>
    /// Generates the chart and stores it at a custom location
    /// </summary>
    /// <param name="path">
    /// The path and name to store the file at
    /// </param>
    public void toFile(string path) => toFileAsync(path).Wait();

    /// <summary>
    /// Generates the chart and returns it as a base64 encoded string
    /// </summary>
    public string toDataURI() => toDataURIAsync().Result;
    
    private ImageChart AddKey(string key, string value)
    {
        if (_parameters.ContainsKey(key))
            throw new InvalidOperationException($"The key {key} has already been set with value \"{_parameters[key]}\"");
        
        _parameters.Add(key, value);
        return this;
    }

    public ImageChart cht(string cht) => AddKey("cht", cht);
    public ImageChart chd(string chd) => AddKey("chd", chd);
    public ImageChart chds(string chds) => AddKey("chds", chds);
    public ImageChart choe(string choe) => AddKey("choe", choe);
    public ImageChart chld(string chld) => AddKey("chld", chld);
    public ImageChart chxr(string chxr) => AddKey("chxr", chxr);
    public ImageChart chof(string chof) => AddKey("chof", chof);
    public ImageChart chs(string chs) => AddKey("chs", chs);
    public ImageChart chdl(string chdl) => AddKey("chdl", chdl);
    public ImageChart chdls(string chdls) => AddKey("chdls", chdls);
    public ImageChart chg(string chg) => AddKey("chg", chg);
    public ImageChart chco(string chco) => AddKey("chco", chco);
    public ImageChart chtt(string chtt) => AddKey("chtt", chtt);
    public ImageChart chts(string chts) => AddKey("chts", chts);
    public ImageChart chxt(string chxt) => AddKey("chxt", chxt);
    public ImageChart chxl(string chxl) => AddKey("chxl", chxl);
    public ImageChart chxs(string chxs) => AddKey("chxs", chxs);
    public ImageChart chm(string chm) => AddKey("chm", chm);
    public ImageChart chls(string chls) => AddKey("chls", chls);
    public ImageChart chl(string chl) => AddKey("chl", chl);
    public ImageChart chlps(string chlps) => AddKey("chlps", chlps);
    public ImageChart chma(string chma) => AddKey("chma", chma);
    public ImageChart chdlp(string chdlp) => AddKey("chdlp", chdlp);
    public ImageChart chf(string chf) => AddKey("chf", chf);
    public ImageChart chbr(string chbr) => AddKey("chbr", chbr);
    public ImageChart chan(string chan) => AddKey("chan", chan);
    public ImageChart chli(string chli) => AddKey("chli", chli);
    public ImageChart icac(string icac) => AddKey("icac", icac);
    public ImageChart ichm(string ichm) => AddKey("ichm", ichm);
    public ImageChart icff(string icff) => AddKey("icff", icff);
    public ImageChart icfs(string icfs) => AddKey("icfs", icfs);
    public ImageChart iclocale(string iclocale) => AddKey("iclocale", iclocale);
    public ImageChart icretina(string icretina) => AddKey("icretina", icretina);
    public ImageChart icqrb(string icqrb) => AddKey("icqrb", icqrb);
    public ImageChart icqrf(string icqrf) => AddKey("icqrf", icqrf);

}