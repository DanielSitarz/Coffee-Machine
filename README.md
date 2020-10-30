# Coffee Machine

This repository contains full project of my Coffee Machine implementation made in Unity.

## What it can do?
Like any Coffee Machine it can brew your favorite coffee in matter of seconds.

This machine allows you to brew coffees of 3 sizes (small, medium, big) and 3 strengths (weak, normal, strong).

Based on sensors it displays warnings like: low on water, drip tray full...

You can set your own values for each coffee size and strength.

## How it's done?
In fact, machine is a composition of 1 Coffee Machine, 1 Brew Module, 4 containers (water, coffee, drip, grounds), a display, a couple of sensors and 2 suppliers.

## Scenes
Project consist two example scenes, one with single Coffee Machine and one with 4 of them. Just to show that each machine lives its own life. 

There is a minimal version of working Coffee Machine in Single machine scene.

## How to build my own machine?
To make your own machine start with **CoffeeMachine** and then fill its components. Display, Favorite Coffee and Sensors are not required.

## Features
- Turning On/Off.
- Water/coffee/drip/grounds containers state validation.
- Warnings if water is low, grounds container is full, etc.
- Choosable size & strength of coffee.
- Adjustable containers capacity, water amount per size, water flow rate per strength, coffee amount used by Brew Module.
- Can save current settings as named favorite coffee.
- Save/Load system.
- Fully operational from UI and Custom Editor.

## Components
- Coffee Machine
- Brew Module
- Container
- Display
- Favorite Coffee
- Sensor
- Supplier
- Save Load System
- Color Indicator

## 3rd party tools
- Serialized dictionary - to show some settings inside inspector - https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-lite-110992
- JSON .NET - for saving in BSON - https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347