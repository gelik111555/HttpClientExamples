class Program
{
    private const string dataUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

    private static void Main(string[] args)
    {
        var client = new HttpClient();

        var result = client.GetAsync(dataUrl).Result;
        var csvStr = result.Content.ReadAsStringAsync().Result;
        Console.WriteLine(csvStr);
    }
}