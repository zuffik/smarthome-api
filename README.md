# SmartHome API

Service that manages more devices than you can think (but not yet :D). This readme contains necessary 
information that can be forgotten.

## Supported devices

### Heaters

 * **Comet Blue**: Bluetooth and GATT


## Runtime

### Linux

In order to use [GATT](https://www.bluetooth.com/specifications/gatt/generic-attributes-overview) 
with [BlueZ](http://www.bluez.org/) library, it's necessary to enable bluetooth experimental
features (`bluetoothd -E`).
