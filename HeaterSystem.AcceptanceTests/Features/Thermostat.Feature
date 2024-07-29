Feature: Normal working of the Thermostat

Normal working of the Thermostat. We are testing 5(6) features:
Scenario 1a: The heater is off and temperature between Setpoint +/- Offset: Do Nothing
Scenario 1b: The heater is on and temperature between Setpoint +/- Offset: Do Nothing
Scenario 2: temperature less then Setpoint – Offset: Heater On
Scenario 3: temperature equals Setpoint - Offset: Do Nothing
Scenario 4: temperature exceeds Setpoint + Offset: Heater off
Scenario 5: temperature equals Setpoint + Offset: Do Nothing

Scenario: 1a - the heater is off and temperature is between setpoint minus/plus offset - do nothing
    Given the heater is off
     When the temperature is between boundaries
     Then do nothing - heather is off

Scenario: 1b - the heater is on and temperature is between setpoint minus/plus offset - do nothing
    Given the heater is on
     When the temperature is between boundaries
     Then do nothing - heather is on

Scenario: 2 - temperature is less than lower boundary - turn heater on
     When the temperature is less than lower boundary
     Then turn heather on
