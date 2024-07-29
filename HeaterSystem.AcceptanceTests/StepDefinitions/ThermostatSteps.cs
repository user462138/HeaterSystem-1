using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Gherkin.Quick;

namespace HeaterSystem.AcceptanceTests.StepDefinitions;

[FeatureFile("./Features/Thermostat.Feature")] // Annotation that links feature file
public sealed class ThermostatSteps : Feature  // Must inherit from Feature
{
    // Setup

    private const double Setpoint = 20.0;
    private const double Offset = 2.0;
    private const double Difference = 0.5;
    private const int MaxFailures = 2;

    private const string UrlMockoon = "http://localhost:3000/data/2.5/weather";
    private const string UrlMockoonException = "http://localhost:3000/data/2.5/weather/exception";

    private readonly IHeatingElement heatingElement = null;
    private readonly ITemperatureSensor temperatureSensor = null;
    private readonly Thermostat thermostat;

    public ThermostatSteps()
    {
        temperatureSensor = new TemperatureSensorOpenWeather();
        heatingElement = new HeatingElementStub();
        thermostat = new Thermostat(temperatureSensor, heatingElement)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };
    }

    // Step implementations, possible attributes are Given, When, Then, And

    [Given(@"the heater is off")]
    public void SetHeaterOn()
    {
        string queryParam = "?temp=" + (Setpoint + Offset + Difference).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        thermostat.Work();
    }

    [When(@"the temperature is between boundaries")]
    public void SetTemperatureBetweenBoundaries()
    {
        string queryParam = "?temp=" + (Setpoint).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        thermostat.Work();
    }

    [Then(@"do nothing - heather is off")]
    public void CheckHeaterOff()
    {
        Assert.False(heatingElement.IsEnabled);
    }
}
