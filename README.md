# LookCaresAPI - ASP.NET Web API


### API List

- Login

	 [POST]/api/auth/Login

	 body: { username, password }


- Get Clients all

	[GET]/api/Clients


- Get Client by key 

	[GET]/api/Clients/{:ClientKey}


- Get ClientLocations by Client

	[GET]/api/ClientLocations/ByClientKey/{:ClientKey}


- Get ClientLocation by key

	[GET]/api/ClientLocations/{:ClientLocationKey}


- Find Frame by serial number

	[GET]/api/Frames/{:SerialNumber}


- Add Fabric

	 [POST]/api/Frames/Fabric

	 body: { ClientKey, ClientLocationKey, FrameKey, Height, Width, Extrusion, FileName }


- Remove Fabric by key

	[DELETE]/api/Frames/Fabric/{:FabricKey}


- Get InStore Locations

	[GET]/api/StoreLocations