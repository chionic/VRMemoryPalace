# MemoryPalaceVR

Final year project based on creating a platform for developers to use Memory Palace spaces for testing in Virtual Reality.

# How to get the Application Up and Running
1. Download Unity Hub (https://unity3d.com/get-unity/download).
2. From Unity Hub, create a Unity account and download Unity 2019.2.0f1
3. Download Steam (https://store.steampowered.com/about/) and create an account
4. Clone this project onto a git directory on your computer: 'git clone https://gitlab.cs.nuim.ie/u180336/memorypalacevr.git'
5. From the Unity Hub, navigate to the folder where you cloned the project and import the project folder 'MemoryPalaceApartment'
6. Lauch steam and open SteamVR
7. Open the project by double clicking on it in Unity Hub
8. Once the project is opened, navigate to the 'Complete Floor Plan A' scene within the project panel in the Unity editor and open it.
9. Press the play button near the top of the Unity editor screen, with SteamVR and the VR headset running, to preview the project

Note: This project is meant to be used with a VR headset and much of the functionality will not work without one. 
A version with limited functionality can be played on a desktop with a keyboard and mouse by following the instructions below.

# How to run the application with LIMITED functionality on a regular PC
1. Open the unity project by following the steps above.
2. Navigate to the 'Complete Floor Plan A' scene within the project panel in the Unity editor and open it.
3. Select the '[CameraRig] Variant' game object in the project hierarchy panel.
4. In the inspector panel, deactivate the game object by unticking the box to the left of the object name
5. In the search bar in the project panel, type in 'Player'. A blue arrow object should appear.
6. Drag the blue arrow object into the game window and place it somewhere within the environment.
7. Press the play button. The game window should allow you to navigate through the house using w/a/s/d/e/q and the right mouse button.

This version of the project for a regular PC does not display the controllers or text tablet, the menu does not function and neither does teleporting or object deletion.
Object interaction is not facilitated by default but can be added in by dragging an object with the 'Ob_' prefix from the 'menuModels' folder into the game window, 
removing all the script components from the object and replacing them with the 'throwable' script (which also automatically adds the interactable script).
This needs to be done for every object you want to interact with.

# How to Create a Menu of Custom Objects
1. Open the unity project and navigate into the Assets folder
2. Create a new folder and call it whatever you like
3. In the folder create new folders with the names of the different submenus you want
4. In each submenu folder import the model assets associated with that submenu and their relevant material files (if separate)^
5. Resize the model assets to where they would fit into the menu (select the asset, go to import settings in the inspector, change the 'scale factor attribute' then select apply changes).
5.1 (optional) if you want more exact object collisions with other objects (with a potential loss in terms of performance) click the 'Generate Colliders' box for the imported assets then select 'mesh' as the default collider type in the next step.
6. Navigate to Window -> CreateJSON
7. A window should pop up allowing you to specify the name of the json file, the path to the folder created in step 2, and the default collider type. The collider type states what shape the collider should be for the objects (which affects how they bump into other objects), the suggested default is 'box'.
7.1 (optional) navigate to the 'JSON_Files' folder and open the json file you just created, edit the file as you wish (see explanation of file below) then save changes.
8. Navigate to Window -> MakeMenu
9. A window should pop up allowing you to specify the name of the JSON file you want to use and the filepath to said json files as well as a toggle for whether you want to create menu and game objects or not. Tick the toggle box if this is your first time generating the menu or you have changed the asset files in some way.
10. Allow some time for the script to run. When it is finished, a new menu object should have appeared in the scene hierarchy.

^*In the case of .obj files, the asset should be imported, then the material file, and then right click on the asset and select 'reimport asset' for the material to show up on the model.*

###Menu file variables
assetPath: the path to the top level menu folder from the unity Assets folder
BaseFolder: where the assetPath is on your computer
menuName: the name the menu will have once created in the form of 'menu_NameGiven'
Menu: a list of all the submenus.
name: the name of the submenu (taken from the folder name in unity by default)
id: a unique identifier, positive integer
isSubmenu: boolean true/false, states whether the menu is a submenu or not
topLevelMenuRep: the name of the object representing the submenu in the first menu.
Values: a list of the different objects in the submenu
objectName: the name of the object
objectPath: the path to the object from the top level menu folder
defaultSize: how big the object is when it is spawned from the menu (default 1)
colliderType: the shape of the collider around the object - box, sphere, capsule or mesh

####Custom Errors when creating the Menu and how to resolve them
"""Warning: object bounds are too large to fit inside the menu for the mesh "meshName". Rescale model to fit properly into menu.""" -> the mesh you are trying to create an object from is not scaled correctly. Find the mesh in the hierarchy and rescale it.
"""The game object model 2(Clone) does not have a renderer attached to it, manually add a collider to the object.""" ->


# How to add Custom Text to the User Tablet
1. Open the unity project (see steps above)
2. Navigate to Assets/JSON_Files and open sampleText
3. Edit the sampleText file with the text you want to add and save it

###Custom Text Variables
id: a positive integer between 0 and the number of different text sets, used to randomly choose the text set. The id should be unique.
runOrder: a positive integer or -1. Sets in what order the text sets should run, 0 runs first, then 1, then 2 etc. -1 sets the text set to run randomly after the specified ordered sets have run.
loop: boolean true/false, sets if the text set should repeat from the start when it is finished or if the script should move on to the next text set.
Values: an array of strings that will show up one after the other on the user tablet once the text set is selected.