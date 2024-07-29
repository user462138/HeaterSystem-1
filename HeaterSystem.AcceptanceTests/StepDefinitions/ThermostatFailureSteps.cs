using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Gherkin.Quick;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HeaterSystem.AcceptanceTests.StepDefinitions;

[FeatureFile("./Features/ThermostatFailure.Feature")] // Annotation that links feature file
public sealed class ThermostatFailureSteps : Feature  // Must inherit from Feature
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

    public ThermostatFailureSteps()
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

    [Given(@"the heater is on")]
    [When(@"the temperature is less than lower boundary")]
    public void SetHeaterOn()
    {
        string queryParam = "?temp=" + (Setpoint - Offset - Difference).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        thermostat.Work();
    }
    [Given(@"thermostat in safe mode")]
    public void SetThermostatInsafeMode()
    {
        string queryParam = "";
        temperatureSensor.Url = $"{UrlMockoonException}{queryParam}";
        for (int i = 0; i < MaxFailures; i++)
        {
            thermostat.Work();
        }
    }
    [When(@"getting the temperature fails")]
    public void GetTemperatureGivesException()
    {
        string queryParam = "";
        temperatureSensor.Url = $"{UrlMockoonException}{queryParam}";
        thermostat.Work();
    }

    [And(@"number of failures is less than maximum")]
    public void ResetNumberOfFailures()
    {
        // Just resetting the failures counter
        string queryParam = "?temp=" + (Setpoint).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        thermostat.Work();
    }

    [And(@"number of failures is maximum failures minus one")]
    public void SetMaximumNumberOfFailuresMinusOne()
    {
        string queryParam = "";
        temperatureSensor.Url = $"{UrlMockoonException}{queryParam}";
        for (int i = 1; i < MaxFailures; i++) 
        {
            thermostat.Work();
        }
    }

    [Then(@"do nothing - heater is on")]
    [Then(@"turn heater on")]
    public void CheckHeaterOn()
    {
        Assert.True(heatingElement.IsEnabled);
    }
    [Then(@"turn heater off")]
    public void CheckHeaterOff()
    {
        Assert.False(heatingElement.IsEnabled);
    }
    [And(@"set thermostat in safe mode")]
    public void CheckThermostatInSafeMode()
    {
        Assert.True(thermostat.InSafeMode);
    }
    [And(@"set thermostat in normal mode")]
    public void CheckThermostatInNormalMode()
    {
        Assert.False(thermostat.InSafeMode);
    }
}


