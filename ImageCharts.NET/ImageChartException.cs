namespace ImageCharts.NET;

public class ImageChartException : Exception
{
    internal ImageChartException()
    {
    }

    internal ImageChartException(string message)
        : base(message)
    {
    }

    internal ImageChartException(string message, Exception inner)
        : base(message, inner)
    {
    }
}