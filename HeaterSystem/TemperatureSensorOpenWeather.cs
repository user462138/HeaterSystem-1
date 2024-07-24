using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaterSystem;

public class TemperatureSensorOpenWeather : ITemperatureSensor
{
    public string Url { get; set; } = "http://api.openweathermap.org/data/2.5/weather";
    private readonly string QueryParams = "?q=Antwerp,BE&appid=b1a90ec4d94d84ecf2a3f2bb634b970d&units=metric";

    public double GetTemperature()
    {
        using (var httpClient = new HttpClient())
        {
            var httpRespone = httpClient.GetAsync(Url + QueryParams).GetAwaiter().GetResult();
            var response = httpRespone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<OpenWeather>(response).main.temp;
        }
    }
}
