# EDForceFeedback
Elite Dangerous Force Feedback with a Microsoft Force Feedback 2 Joystick (MSFFB2)

### Description
EDForceFeedback.exe is a console program that runs during a Elite Dangerous session.  It watches the ED log files and responds to game events by playing a force feedback editor (.ffe) file.  It is only configured to work with a [Microsoft Sidewinder Force Feedback 2](https://en.wikipedia.org/wiki/Microsoft_SideWinder#Force_Feedback_2) joystick. 

### Usage
1. Connect your MSFFB2 joystick and run the EDForceFeedback.exe program.  
2. Start Elite Dangerous 
3. In-game events like deploy/retract hardpoints and deploy/retract landing gear will send the configured forces to the joystick.

### Configuration

The `settings.json` file contains the events and the forces that will be played when an event occurs.  Each event can have a different force played for the on or off state.

	{
		"Event": "Status.Docked:True",
		"ForceFile": "Dock.ffe",
		"Duration": 2000
	}

###### Event
The `"Event"` Field<br/>
The [EliteAPI](https://github.com/EliteAPI/EliteAPI) StatusEvent name and state to respond to.<br/>
The format is:  Status.*StatusEventName*:*[True or False]*

###### ForceFile
The `"ForceFile"` Field<br/>
The name of the force file (.ffe) to play when this event is detected.  The force files can be found under the .\Forces folder.  There is a force file editor included under the .\FFUtils folder.  See the *Creating And Editing* forces section for more information.

###### Duration
The `"Duration"` Field<br/>
This is how long the force will be played.  The value is in milliseconds (1 Second = 1000 milliseconds).  The forces will be stopped after this amount of time even if the .ffe file is configured to play longer.  

##### Additional Events
The `settings.json` [json](https://www.json.org/) file only has a few of the status events defined.  Additional status events are provided by the [EliteAPI](https://github.com/EliteAPI/EliteAPI) and can be added to the `StatusEvents` array in the settings file.  During game play, the console window will output the names of the events that were detected.  You can use these names to add additional forces.

### Creating and Editing Forces
On startup all .ffe files in the .\Forces folder will be loaded.  To create new forces use `fedit.exe` and save the file in the .\Forces folder.

###### .\FFUtils
These are Microsoft utilities to edit and configure force feedback devices. They may get removed from this location.  They can be found in various locations on the internet. They were part of the DirectX DirectInput developer packages.

	csFeedback.exe
	fedit.exe
	FFConst.exe

###### fedit.exe
This utility allows you to build your own custom forces.  For an example, open the Cargo.ffe file.  The forces can overlay and played before, after, and simultaneously to create complex movement and effects.<br/>

Enjoy! - Bob (CMDR Axe_)
