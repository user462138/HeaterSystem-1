Feature: abnormal working of the thermostat - getting temperature gives an exception

Abnormal working of the Thermostat. We have 3 scenario's:
Scenario 1: The heater is on and number of failures less than maximum and getting temperature fails: Do Nothing
Scenario 2: the heater is on and number of failures equals maximum failures while getting temperature gives exception: thermostat in safe mode
Scenario 3: thermostat in safe mode and getting temperature gives exception: thermostat in normal working

Scenario: 1 - the heater is on and number of failures less than maximum and getting temperature gives exception: Do Nothing
    Given the heater is on
      And number of failures is less than maximum
     When getting the temperature fails
     Then do nothing - heater is on

Scenario: 2 - the heater is on and number of failures equals maximum failures while getting temperature gives exception: thermostat in safe mode
    Given the heater is on
      And number of failures is maximum failures minus one
     When getting the temperature fails
     Then turn heater off
      And set thermostat in safe mode

Scenario: 3 - thermostat in safe mode and getting temperature gives exception: normal working of thermostat
    Given thermostat in safe mode
     When the temperature is less than lower boundary
     Then turn heater on
      And set thermostat in normal mode