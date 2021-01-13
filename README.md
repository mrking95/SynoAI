# SynoAI
A Synology Surveillance Station notification system utilising DeepStack AI, inspired by Christopher Adams' [sssAI](https://github.com/Christofo/sssAI) implementation.

The aim of the solution is to reduce the noise generated by Synology Surveillance Station's motion detection by routing all motion events via a [Deepstack](https://deepstack.cc/) docker image to look for particular objects, e.g. people.

While sssAI is a great solution, it is hamstrung by the Synology notification system to send motion alerts. Due to the delay between fetching the snapshot, processing the image using the AI and requesting the alert, it means that the image attached to the Synology notification is sometimes 5-10 seconds after the motion alert was originally triggered.

SynoAI aims to solve this problem by side-stepping the Synology notifications entirely by allowing other notification systems to be used.

## Buy Me A Coffee! :coffee:

I made this application mostly for myself in order to improve upon Christopher Adams' original idea and don't expect anything in return. However, if you find it useful and would like to buy me a coffee, feel free to do it at [__Buy me a coffee! :coffee:__](https://buymeacoff.ee/djdd87). This is entirely optional, but would be appreciated! Or even better, help supported this project by contributing changes such as expanding the supported notification systems (or even AIs).

## Features
* Triggered via an Action Rule from Synology Surveillance Station
* Uses an AI for object/person detection
* Produces an output image with highlighted objects using the original image at the point of motion detection
* Sends notification(s) at the point of notification with the processed image attached.

## Supported AIs
* [Deepstack](https://deepstack.cc/)

## Supported Notifications
* [Pushbullet](https://www.pushbullet.com/)
* HomeAssistant (TODO)
* Webhooks (TODO)

## Configuration
The configuration instructions below are primarily aimed at running SynoAI in a docker container on DSM (Synology's operating system). Docker will be required anyway as Deepstack is assumed to be setup inside a Docker container. It is entirely possible to run SynoAI on a webserver instead, or to install it on a Docker instance that's not running on your Synology NAS, however that is outside the scope of these instructions. Additionally, the configuration of the third party notification systems (e.g. generating a Pushbullet API Key) is outside the scope of these instructions and can be found on the respective applications help guides.

The top level steps are:
* Setup the Deepstack Docker image on DSM
* Setup the SynoAI image on DSM
* Add Action Rules to Synology Surveillance Station's motion alerts in order to trigger the SynoAI API.

### Configure Deepstack
TODO

### Configure SynoAI
The following instructions explain how to set up the SynoAI image using the Docker app built into DSM. For docker-compose, see the example file in the src, or in the documentation below.
* Create a folder called synoai (this will contain your Captures directory and appsettings.json)
* Put your appsettings.json file in the folder
* Create a folder called Captures 
* Open Docker in DSM
* Download the SynoAI:latest image by either;
  * Searching the registry for djdd87/SynoAI
  * Going to the image tab and;
    * Add > Add from URL
    * Enter https://hub.docker.com/r/djdd87/synoai
* Run the image
* Enter a name for the image, e.g. synoai
* Edit the advanced settings
* Enable auto restarts
* On the volumes tab;
   * Add a file mapping from your appsettings.json to /app/appsettings.json
   * Add a folder mapping from your captures directory to /app/Captures (optional)
* On the port settings tab;
   * Enter a port mapping to port 80, e.g. 8080

### Create Action Rules
TODO

## Docker
SynoAI can be installed as a docker image, which is [available from DockerHub](https://hub.docker.com/r/djdd87/synoai).

### Docker Configuration
The image can be pulled using the Docker cli by calling:
```
docker pull djdd87/synoai:latest
```
To run the image a volume must be specified to map your appsettings.json file. Additionally a port needs mapping to port 80 in order to trigger the API. Optionally, the Captures directory can also be mapped to easily expose all the images output from SynoAI.

```
docker run 
  -v /path/appsettings.json:/app/appsettings.json 
  -v /path/captures:/app/Captures 
  -p 8080:80 djdd87/synoai:latest
```

### Docker-Compose
```yaml
version: '3.4'

services:
  synoai:
    image: djdd87/synoai:latest
    build:
      context: .
      dockerfile: ./Dockerfile
    ports:
      - "8080:80"
    volumes:
      - /docker/synoai/captures:/app/Captures
      - /docker/synoai/appsettings.json:/app/appsettings.json
```

## Example appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },

  "Url": "http://192.168.0.0:5000",
  "User": "SynologyUser",
  "Password": "SynologyPassword",

  "AI": {
    "Type": "DeepStack",
    "Url": "http://10.0.0.10:83",
    "MinSizeX": 100,
    "MinSizeY": 100
  },

  "Notifier": {
    "Type": "Pushbullet",
    "ApiKey": "0.123456789"
  },

  "Cameras": [
    {
      "Name": "Driveway",
      "Types": [ "Person", "Car" ],
      "Threshold": 45
    },
    {
      "Name": "BackDoor",
      "Types": [ "Person" ],
      "Threshold": 30
    }
  ]
}
```
