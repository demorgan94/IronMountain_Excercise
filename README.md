# Iron Mountain Excercise

This excersise was made as a test to get a job at Iron Mountain

## Setup Info

To setup this project you have to clone the repo, navigate inside the IronMountain_Exercise folder at "project level" and do `dotnet restore` to be sure all NuGet packages are installed correctly, also yo have to navigate to "ClientApp" folder, and run `npm install` to get all packages needed for Angular app.

Also you have to install all NuGet packages and run `dotnet ef database update` with dotnet cli to create local DB. (The project have a Local MSSQL Server connection string).

## Info
This proyect is a web-based build to upload images, store them on database (as byte string), it will give you a ".meta" file that contains the Identifier, Creation Date and Image Name.
When you upload the images and get the ".meta" file then you can upload the ".meta" file, the project will read the file, get the images Identifiers inside and get the correct images from database and return a ".zip" file with the images you uploaded at step one, basically is a "Cloud Storage" app.