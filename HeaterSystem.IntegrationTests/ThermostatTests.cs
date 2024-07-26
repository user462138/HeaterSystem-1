using System.Globalization;

using HeaterSystem;

namespace HeaterSystem.IntegrationTests;

public class ThermostatTests
{
    private const double Setpoint = 20.0;
    private const double Offset = 2.0;
    private const double Difference = 0.5;
    private const int MaxFailures = 2;
    private const string UrlMockoon = "http://localhost:3000/data/2.5/weather";
    private const string UrlMockoonException = "http://localhost:3000/data/2.5/weather/exception";

    private ITemperatureSensor temperatureSensor = null;
    private IHeatingElement heatingElement = null;

    private Thermostat thermostat;

    [SetUp]
    public void Setup()
    {
        temperatureSensor = new TemperatureSensorOpenWeather();
        heatingElement = new HeatingElementStub();
        // Create the test object, set the setpoint and offset and MaxFailures
        thermostat = new Thermostat(temperatureSensor, heatingElement)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };
    }

    [Test]
    public void WhenHeatingElementEnabledAndTemperatureBetweenBoundariesDoNothing()
    {
        // Arrange
        // instantiate the temperaturesensor object but it will be using Mockoon (url) - first set the heaterelement object active
        string queryParam = "?temp=" + (Setpoint - Offset - Difference).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        // Create the test object, set the setpoint and offset and MaxFailures
        thermostat = new Thermostat(temperatureSensor, heatingElement)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };
        thermostat.Work();
        Assert.IsTrue(heatingElement.IsEnabled);

        // temperature between boundaries => do nothing
        queryParam = "?temp=" + Setpoint.ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";

        // Act
        thermostat.Work();

        // Assert
        Assert.IsTrue(heatingElement.IsEnabled);
    }
    [Test]
    public void WhenHeatingElementDisabledAndTemperatureBetweenBoundariesDoNothing()
    {
        // Arrange
        // instantiate the temperaturesensor object but it will be using Mockoon (url) - first set the heaterelement object active
        string queryParam = "?temp=" + (Setpoint + Offset + Difference).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        // Create the test object, set the setpoint and offset and MaxFailures
        thermostat = new Thermostat(temperatureSensor, heatingElement)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };
        thermostat.Work();
        Assert.IsFalse(heatingElement.IsEnabled);

        // temperature between boundaries => do nothing
        queryParam = "?temp=" + Setpoint.ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";

        // Act
        thermostat.Work();

        // Assert
        Assert.IsFalse(heatingElement.IsEnabled);
    }
    [Test]
    public void WhenTemperatureLessThenLowerBoundaryHeaterElementEnabled()
    {
        // Arrange
        // temperature between boundaries => do nothing
        string queryParam = "?temp=" + (Setpoint - Offset - Difference).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        // Create the test object, set the setpoint and offset and MaxFailures
        thermostat = new Thermostat(temperatureSensor, heatingElement)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };

        // Act
        thermostat.Work();

        // Assert
        Assert.IsTrue(heatingElement.IsEnabled);
    }
    [Test]
    public void WhenTemperatureHigherThenUpperBoundaryHeaterElementDisabled()
    {
        // Arrange
        // temperature between boundaries => do nothing
        string queryParam = "?temp=" + (Setpoint + Offset + Difference).ToString(CultureInfo.InvariantCulture);
        temperatureSensor.Url = $"{UrlMockoon}{queryParam}";
        // Create the test object, set the setpoint and offset and MaxFailures
        thermostat = new Thermostat(temperatureSensor, heatingElement)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };

        // Act
        thermostat.Work();

        // Assert
        Assert.IsFalse(heatingElement.IsEnabled);
    }
}
