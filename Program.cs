using System.Data;
using System.Globalization;

class Program
{
    private const string dataUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

    /// <summary>
    /// Метод будет возвращать поток для получения текстовых данных
    /// </summary>
    /// <returns>вернёт поток из которого мы сможем читать данные</returns>
    private static async Task<Stream> GetDataStream()
    {
        var client = new HttpClient();
        // тут будет скачен только заголовок ответа, а тело ответа будет не принятым,
        // останется в буфере сетевой карты
        var response = await client.GetAsync(dataUrl, HttpCompletionOption.ResponseHeadersRead);
        return await response.Content.ReadAsStreamAsync();
    }
    /// <summary>
    ///Построчное чтение данных из буфера с возможностью прервать чтение в любой момент
    ///без необходимости скачивать польностью все данные
    /// </summary>
    /// <returns></returns>
    private static  IEnumerable<string> GetDataLines()
    {
        // запрос к серверу
         using var dataStream =  GetDataStream().Result;
        // обеспечит построчное чтение из потока
        using var dataReader = new StreamReader(dataStream);
        // тут будет считана одна строка и возвращена как результат
        while (!dataReader.EndOfStream)
        {
            var line =  dataReader.ReadLineAsync().Result;
            if (string.IsNullOrWhiteSpace(line)) continue;
            yield return line;
        }
    }

    private static DateTime[] GetDates() => GetDataLines()
        .First()
        .Split(',')
        .Skip(4)
        .Select(x => DateTime.Parse(x, CultureInfo.InvariantCulture))
        .ToArray();
    private static void Main(string[] args)
    {
        //var client = new HttpClient();

        //var result = client.GetAsync(dataUrl).Result;
        //var csvStr = result.Content.ReadAsStringAsync().Result;
        //foreach (var item in GetDataLines())
        //{
        //    Console.WriteLine(item);
        //}
        var dates = GetDates();
        Console.WriteLine(string.Join("\r\n ", dates));
    }
}