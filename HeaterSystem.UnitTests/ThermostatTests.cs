using Moq;

namespace HeaterSystem.UnitTests;

[TestClass]
public class ThermostatTests
{
    private const double Setpoint = 20.0; 
    private const double Offset = 2.0;
    private const double Difference = 0.5;
    private const int MaxFailures = 2;

    private Mock<ITemperatureSensor> temperatureSensorMock = null;
    private Mock<IHeatingElement> heatingElementMock = null;

    private Thermostat thermostat;

    [TestInitialize]
    public void Initialize()
    {
        // Mock the objects used by the thermostat object
        temperatureSensorMock = new Mock<ITemperatureSensor>();
        heatingElementMock = new Mock<IHeatingElement>();

        // Create the test object, set the setpoint and offset
        thermostat = new Thermostat(temperatureSensorMock.Object, heatingElementMock.Object)
        {
            Setpoint = Setpoint,
            Offset = Offset,
            MaxFailures = MaxFailures
        };
    }
    [TestMethod]
    public void WorkWhenTemperatureBetweenBoundariesDoNothing()
    {
        // --- Arrange ---
        // Configure the mock object to get the temperature between boundaries = Setpoint
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint);

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that neither the method Enable nor the method Disable of the heatingElementMock object is called (= Do Nothing)
        heatingElementMock.Verify(x => x.Enable(), Times.Never);
        heatingElementMock.Verify(x => x.Disable(), Times.Never);
    }
    [TestMethod]
    public void WorkWhenTemperatureLessThanLowerBoundaryEnableHeatingElement()
    {
        // --- Arrange ---
        // Configure the mock object to get the temperature less than lower boundary = Setpoint - Offset - Difference
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint - Offset - Difference);

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that neither the method Enable nor the method Disable of the heatingElementMock object is called (= Do Nothing)
        heatingElementMock.Verify(x => x.Enable(), Times.Once);
        heatingElementMock.Verify(x => x.Disable(), Times.Never);
    }
    [TestMethod]
    public void WorkWhenTemperatureEqualsLowerBoundaryDoNothing()
    {
        // --- Arrange ---
        // Configure the mock object to get the temperature equal than lower boundary = Setpoint - Offset
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint - Offset);

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that neither the method Enable nor the method Disable of the heatingElementMock object is called (= Do Nothing)
        heatingElementMock.Verify(x => x.Enable(), Times.Never);
        heatingElementMock.Verify(x => x.Disable(), Times.Never);
    }
    [TestMethod]
    public void WorkWhenTemperatureHigherThanUpperBoundaryDisableHeatingElement()
    {
        // --- Arrange ---
        // Configure the mock object to get the temperature higher than upper boundary = Setpoint + Offset + Difference
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint + Offset + Difference);

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that neither the method Enable nor the method Disable of the heatingElementMock object is called (= Do Nothing)
        heatingElementMock.Verify(x => x.Enable(), Times.Never);
        heatingElementMock.Verify(x => x.Disable(), Times.Once);
    }
    [TestMethod]
    public void WorkWhenTemperatureEqualsUpperBoundaryDoNothing()
    {
        // --- Arrange ---
        // Configure the mock object to get the temperature equal than upper boundary = Setpoint + Offset
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint + Offset);

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that neither the method Enable nor the method Disable of the heatingElementMock object is called (= Do Nothing)
        heatingElementMock.Verify(x => x.Enable(), Times.Never);
        heatingElementMock.Verify(x => x.Disable(), Times.Never);
    }
    [TestMethod]
    public void WorkWhenTemperatureFailsAndNotInsafeModeDoNothing()
    {
        // --- Arrange ---
        // Configure the mock object to get the temperature equal than the setpoint. This will set the status of the Thermostat object to "active" 
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint);
        thermostat.Work();

        // Configure the mock object to getting the temperature throws an exception
        temperatureSensorMock.Setup(x => x.GetTemperature()).Throws<Exception>();

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that neither the method Enable nor the method Disable of the heatingElementMock object is called (= Do Nothing)
        Assert.IsFalse(thermostat.InSafeMode);
        heatingElementMock.Verify(x => x.Enable(), Times.Never);
        heatingElementMock.Verify(x => x.Disable(), Times.Never);
    }
    [TestMethod]
    public void WorkWhenTemperatureFailsAndMaxFailuresInSafeMode()
    {
        // --- Arrange ---
        // Configure the mock object to getting the temperature throws an exception
        temperatureSensorMock.Setup(x => x.GetTemperature()).Throws<Exception>();
        // number of failures = MaxFailures - 1
        for (int i = 1; i < thermostat.MaxFailures; i++)
        {
            thermostat.Work();
        }

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that InSafeMode is on and the method Disable of the heatingElementMock object is called
        Assert.IsTrue(thermostat.InSafeMode);
        heatingElementMock.Verify(x => x.Enable(), Times.Never);
        heatingElementMock.Verify(x => x.Disable(), Times.Once);
    }
    [TestMethod]
    public void WorkWhenInSafeModeAndTemperatureSuccesReset()
    {
        // --- Arrange ---
        // Configure the mock object to getting the temperature throws an exception
        temperatureSensorMock.Setup(x => x.GetTemperature()).Throws<Exception>();
        // number of failures = MaxFailures => InSafeMode
        for (int i = 0; i < thermostat.MaxFailures; i++)
        {
            thermostat.Work();
        }
        // Configure the mock object to get the temperature equal than the setpoint. This will set the status of the Thermostat object to "active" 
        temperatureSensorMock.Setup(x => x.GetTemperature()).Returns(Setpoint);

        // --- Act ---
        thermostat.Work();

        // --- Assert ---
        // Verify that InSafeMode is 
        Assert.IsFalse(thermostat.InSafeMode);
    }
}