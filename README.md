Just some notes for the talk

deploy code for each web app like the following

- dotnet publish ./src/UserGroupLEDs.Web -r linux-arm64 -p:PublishTrimmed=true --self-contained -o ./deploy/web

need to get pipeline finished to deploy code out to pi
- usergroupleds.web
- usergroupleds.alexa
- raspberrypi.yarp


might need to run "export PATH="$PATH:/home/vscode/.dotnet/tools" when starting up
if tye is not installed, run "dotnet tool install -g Microsoft.Tye --version "0.10.0-alpha.21420.1""