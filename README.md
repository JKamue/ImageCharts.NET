# ImageCharts.NET
## About

This library provides an easy to use implementation of the [Image-Charts API](https://documentation.image-charts.com/). 

It can be used to effortlessly create charts or URLs to charts in the backend headlessly.  

## Special Features

I am aware of the official [image-charts/c-sharp](https://github.com/image-charts/c-sharp) library. I created this custom implementation of the API because I think the official library lacks not only a license but also two very important features:

 1. POST Requests
By default image charts have all relevant settings to generate the chart encoded in the URL. This leads to issues with URL length when visualizing larger datasets. That is why the Image-Charts Api [provides the option](https://github.com/image-charts/c-sharp) to create charts through POST requests. This feature is sadly not implemented in the original [image-charts/c-sharp](https://github.com/image-charts/c-sharp) library. ImageCharts.NET on the other hand has built in support for POST requests, allowing for charts up to 300 Kilobytes long.

 2. Asynchronous Usage
Being a headless chart generation API Image-Charts will often be used in the backend to server charts to users through emails, websited or interactive chat bots. The original [image-charts/c-sharp](https://github.com/image-charts/c-sharp) library does only offer synchronous usage that will for example block a chatbots other conversations while generating a chart. ImageCharts.NET has been built with asynchronous usage in mind.

Switching from [image-charts/c-sharp](https://github.com/image-charts/c-sharp) to ImageCharts.NET is an easy stressfree process. Methods have been named identically on purpose. There are barely any breaking changes and you can seamlessly transition and start using POST Requests asynchronously.

## Limitations
Since I do not own a subscription to image charts I was not able to implement the subscription authentification and the features connected to it.

## Quickstart
If you are familiar with Image-Charts or chart generation starting out will be easy.

### Create a chart layout
I recommend using the [official chart designer](https://www.image-charts.com/#all-features) to play around and set the layout for your chart.
Make sure to set `API Endpoint` to `/chart`.
Once you have a layout you are happy with you can put it into code by using the tag names as methods and passing their value as a string in the parameters.

### Add ImageCharts.NET to your project
You can download ImageCharts.NET on [nuget](https://www.nuget.org/packages/ImageCharts.NET).

### Put your chart into code
These are the values used in the chart designer to create a basic rounded bar chart.
```
chbr=10
chd=t:10,40,60,80,30,20
chf=b0,lg,0,fdb45c,0,ed7e30,1
chs=700x125
cht=bvs
chxt=y,x
```

To put it into code you have to create an image chart object and pass each tag through their respective methods:

```csharp
var myFirstChart = new ImageChart()  
 .chbr("10") // equivalent to chbr=10
 .chd("t:10,40,60,80,30,20") // equivalent to chd=t:10,40,60,80,30,20
 .chf("b0,lg,0,fdb45c,0,ed7e30,1") // ... chf=b0,lg,0,fdb45c,0,ed7e30,1
 .chs("700x125") // chs=700x125
 .cht("bvs") // cht=bvs
 .chxt("y,x"); // chxt=y,x
```

The created chart can be used in various ways:
```csharp
// Generates a URL leading to the chart.
// No web request necessary but limited by max URL length
var urlToImage = myFirstChart.toURL();

// Stores the chart in the local file system with name "myChart.png"
await myFirstChart.toFileAsync("myChart.png");

// Creates a base64 string containing the Chart
var base64EncodedChart = await myFirstChart.toDataURIAsync();

// Returns a byte array containing the Chart
var chartInBuffer = await myFirstChart.toBufferAsync();
```

The urlToImage variable for example will contain the path to generate the specified chart, visualized below.

`https://image-charts.com/chart?chbr=10&chd=t%3a10%2c40%2c60%2c80%2c30%2c20&chf=b0%2clg%2c0%2cfdb45c%2c0%2ced7e30%2c1&chs=700x125&cht=bvs&chxt=y%2cx`

<p align="center">
  <img src="https://image-charts.com/chart?chbr=10&chd=t%3a10%2c40%2c60%2c80%2c30%2c20&chf=b0%2clg%2c0%2cfdb45c%2c0%2ced7e30%2c1&chs=700x125&cht=bvs&chxt=y%2cx">
</p>
