Weather App

1. Getting started:
Clone this repository to your local machine:
git clone https://github.com/NikhaelS/WeatherApp.git

2. Navigate to the project directory:
cd WeatherApp

3. Install necessary dependancies:
dotnet restore

4. Build and run the application:
dotnet build --project WeatherApp
dotnet run --project WeatherApp

5. Use Postman to easily access the endpoints. Below is a list of the available endpoints:
Get current weather:
http://localhost:5052/api/weather?latitude={yourLatitude}&longitude={yourLongitude}

Convert Temperature:
http://localhost:5052/api/weather/convert?temperature={temperature}&fromUnit={originalTemperatureUnit}&toUnit={newTemperatureUnit}

Calculate Average Temperature:
http://localhost:5052/api/weather/AverageTemperature?latitude={yourLatitude}&longitude={yourLongitude}

Calculate highest and lowest temperature:
http://localhost:5052/api/weather/highLowTemp?latitude={yourLatitude}&longitude={yourLongitude}

Please note, you might need to add the folder to Windows Security Excluded list to run the project guide for this below:
https://support.microsoft.com/en-us/windows/add-an-exclusion-to-windows-security-811816c0-4dfd-af4a-47e4-c301afe13b26#:~:text=Go%20to%20Start%20%3E%20Settings%20%3E%20Update,%2C%20file%20types%2C%20or%20process.
