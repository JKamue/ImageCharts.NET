using System.Collections.Specialized;

namespace ImageCharts.NET;

public class ImageChart
{
    private readonly ImageChartSettings _settings;
    private readonly Dictionary<string, string> _parameters = new();

    public ImageChart()
    {
        _settings = new ImageChartSettings();
    }
    
    public ImageChart(ImageChartSettings settings)
    {
        _settings = settings;
    }

    public string toURL()
    {
        var queryParameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
        
        foreach (var parameterKeyValuePair in _parameters)
            queryParameters.Add(
                parameterKeyValuePair.Key,
                parameterKeyValuePair.Value);

        return $"{_settings.GetUri()}?{queryParameters}";
    }

    public async Task<byte[]> toBufferAsync()
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromMilliseconds(_settings.Timeout);
        client.DefaultRequestHeaders.Add("User-Agent", "JKamue/ImageCharts.NET");

        var parametersAsKeyValuePair = _parameters.ToList();
        var formContent = new FormUrlEncodedContent(parametersAsKeyValuePair);
        
        var result = await client.PostAsync(_settings.GetUri(), formContent);
        result.EnsureSuccessStatusCode();

        return await result.Content.ReadAsByteArrayAsync();
    }

    public async Task toFileAsync(string path)
    {
        var imageAsBuffer = await toBufferAsync();
        await File.WriteAllBytesAsync(path, imageAsBuffer);
    }

    public async Task<string> toDataURIAsync()
    {
        var fileFormat = _parameters.ContainsKey("chan")
            ? "gif"
            : "png";
        
        var imageAsBuffer = await toBufferAsync();
        var encodedImage = Convert.ToBase64String(imageAsBuffer);
        
        return $"data:image/{fileFormat};base64,{encodedImage}";
    }

    public byte[] toBuffer() => toBufferAsync().Result;

    public void toFile(string path) => toFileAsync(path).Wait();

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