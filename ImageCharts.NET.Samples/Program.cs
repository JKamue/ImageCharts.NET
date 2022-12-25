using ImageCharts.NET;
using static System.Console;

var myFirstChart = new ImageChart()  
    .chbr("10") // equivalent to chbr=10
    .chd("t:10,40,60,80,30,20") // equivalent to chd=t:10,40,60,80,30,20
    .chf("b0,lg,0,fdb45c,0,ed7e30,1") // ... chf=b0,lg,0,fdb45c,0,ed7e30,1
    .chs("700x125") // chs=700x125
    .cht("bvs") // cht=bvs
    .chxt("y,x"); // chxt=y,x
    

// Generates a URL leading to the chart.
// No web request necessary but limited by max URL length
var urlToImage = myFirstChart.toURL();

// Stores the chart in the local file system with name "myChart.png"
await myFirstChart.toFileAsync("myChart.png");

// Creates a base64 string containing the Chart
var base64EncodedChart = await myFirstChart.toDataURIAsync();

// Returns a byte array containing the Chart
var chartInBuffer = await myFirstChart.toBufferAsync();

WriteLine($"Url Example: {urlToImage}");
WriteLine($"File Example: myChart.png in {AppContext.BaseDirectory}");