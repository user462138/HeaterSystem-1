namespace HeaterSystem.IntegrationTests
{
    public class ThermostatTests
    {
        private ITemperatureSensor temperatureSensor = null;
        private IHeatingElement heatingElement = null;

        private Thermostat thermostat; 
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Fail(); 
        }
    }
}