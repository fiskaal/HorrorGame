****************************
* Audio Room by Dataram_57 *
*			v1			   *
****************************

*** Description ***

There are tons of moments where it is better to describe the sound by the space area than 1 single point in the space.
And even if we use 1 single point in the space, there will be some areas that will require clones of it.
AudioRooms comes with a simple solution...
We describe the space area, on which our AudioSource will be moving.
This will give an effect of being inside the party, where the music seems to be everywhere.
By that AudioRoom gives lots of usages, especially with things like Rains, Forest noises, Ambients, Drones, etc...

*** Instruction ***

0. Add an AudioRoom class to your GameObject. (Avaliable in this package: AudioRoomBox AudioRoomSphere AudioRoomPoint)
1. Create an area where you want to hear your sound at 0 distance.
2. Give it a group name.
3. Create/Use GameObject with AudioSource component
4. To this object add AudioRoomFollow component, and set his group name to the group name of area/structure that you want to use.

*If you want, you can change the code of AudioRoomFollow/AudioRoomPool script to optimize unused features, or add something new.
*Or you can create your own AudioRoom via Vector3 AudioRoomArea.Locate(Vector3) which returns the closest point;

*** Classes & Interface ***

AudioRoomRep:
-AudioRoom						Main static class
-AudioRoomArea					Interface for AudioRooms.
-AudioRoomStructure				Class that AudioRoom uses to locate AudioRooms with the same group name.

AudioRoomRep.Rooms:
-AudioRoomBox					AudioRoom, of box shape (Vector3 pos, Vector3 size)
-AudioRoomPoint					Simple Point (Vector3 pos)
-AudioRoomSphere				AudioRoom, of sphere shape (Vector3 pos, float radius)

AudioRoomRep.RoomsEditor: ("editor")
-AudioRoomBoxEditor
-AudioRoomPointEditor
-AudioRoomSphereEditor

AudioRoomRep.Optional:
-AudioRoomFollow				Script that changes the position of the transform to the closest point of the specific AudioRoomStructure to Listener.
-AudioRoomPool					The script that assigns a new position to each target, selected hierarchically from the sorted table of closest points, where each is created from specific AudioRoomArea to Listener.

*Example:
 -ExampleShowcaseMovement		Simple player movement

*** Assets from the Example ***

https://freesound.org/people/JarredGibb/sounds/243728/
https://freesound.org/people/FlatHill/sounds/237729/
https://freesound.org/people/XHALE303/sounds/535481/

*** Contact ***

Website: http://dataram57.com/contact/
Twitter: @Dataram_57

*** Donate ***

http://dataram57.com/donate

*** PS ***
sorry for my English