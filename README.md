# Location Service v0.1

## Overview

The location service enables developers to query [Ordnance Survey AddressBase Plus](https://www.ordnancesurvey.co.uk/business-and-government/products/addressbase-plus.html) data via. a convenient and structured REST API. 

## Debugging using IIS Express
As the system load data object > 2GB into memory the platform target must be x64
IIS express need the following reg key adding to run x64 targets

#### For VS 2013
`reg add HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\12.0\WebProjects /v Use64BitIISExpress /t REG_DWORD /d 1`
<br />
N.B. Use Visual studio vesrion approprite to you install

RESTART IIS


## Versioning

## Endpoints

## Authentication

## Response formats

## Errors

![Build status](https://travis-ci.org/NHSChoices/location-service.svg?branch=master "Build status")


