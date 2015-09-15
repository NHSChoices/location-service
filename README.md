# Location Service v0.1 ![Build status](https://travis-ci.org/NHSChoices/location-service.svg?branch=master "Build status")

## Overview

The location service enables developers to query [Ordnance Survey AddressBase Plus](https://www.ordnancesurvey.co.uk/business-and-government/products/addressbase-plus.html) data via a convenient and structured REST API. 

## Environments

The service is hosted on Azure as an API application.

#### Staging

https://locationservicesstaging.azure-api.net/  
https://locationservicesstaging.portal.azure-api.net/

#### Production

https://locationservices.azure-api.net/  
https://locationservices.portal.azure-api.net/

## Authentication

Authentication is controlled using an [Azure API key](https://azure.microsoft.com/en-gb/documentation/articles/api-management-get-started/). Developers can request a key from the [developer portal](https://locationservices.portal.azure-api.net/) by clicking the 'sign up' button. Once approved, your subscription key needs to be passed as a request header named `Ocp-Apim-Subscription-Key`.

## Versioning

As the application is currently on its initial version, versioning will be added once required.

## Endpoints

#### /location/search/{query}

##### Description
Provides free text searching for locations. Results are grouped by postcode.

##### Documentation
https://locationservices.portal.azure-api.net/docs/services/55b8d653aa93b505cc139726/operations/55c9f98faa93b50c8c6c728e

#### /location/{id} [GET]

##### Description
Returns an atomic location by provided Id. Id numbers are Base64 encoded and navigable via the URL's  within the `Next` field returned as part of a `/location/search`.

##### Documentation
https://locationservices.portal.azure-api.net/docs/services/55b8d653aa93b505cc139726/operations/55bb5343aa93b50bc84ea774

#### /info [GET]

##### Description

##### Documentation
https://locationservices.portal.azure-api.net/docs/services/55b8d653aa93b505cc139726/operations/55c9ff00aa93b50c8c6c728f

## Response formats

#### Navigation

The location service supports [HATEOAS](http://martinfowler.com/articles/richardsonMaturityModel.html#level3) in the form of `Next` fields being returned in each payload where applicable.

#### Errors

## Build

#### Continuous Integration

The CI is currently managed by [Travis:CI](https://travis-ci.org/NHSChoices/location-service).

#### Continuous Deployment

Currently, when a PR passes review and is merged into the [master branch](https://github.com/NHSChoices/location-service/tree/master) it is automatically deployed to staging via an Azure

## Debugging using IIS Express
As the system loads a data object > 2GB into memory the platform target must be x64.
IIS express need the following registration key adding to run x64 targets.

#### For VS 2013
`reg add HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\12.0\WebProjects /v Use64BitIISExpress /t REG_DWORD /d 1`  
N.B. Use the Visual studio version appropriate to your installed version

RESTART IIS

