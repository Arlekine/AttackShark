
USING THE SCRIPTS
The package contains two scripts - Shoal and ShoalManager.  ShoalManager is the spawner, and needs to be attached to an empty GameObject. The fish will spawn around
this GameObject. Drag your terrain object into the Terrain field in the inspector, drag your fish prefab into the Fish Prefab field (the fish prefab must have the 
Shoal script attached to it in order to work properly), set the water level, the spawn limits (the distance from the spawner game object in which the fish will spawn),
the swim limits (the y limit is not important, as it will be determined by the terrain level and water level) and set Num Fish to be the number of fish you want spawned.
The terrain height value is for the y-position of the terrain. By default it is 0, but if you adjust the terrain height you need to adjust this value.
Restrict to surface will keep fish at the water surface. The surface offset value can be adjusted to ensure that the fish stays at the correct height and doesn't breach the surface.
The min and max speeds should be greater than zero, or the fish won't move.
You can ignore the other fields, as they will be controlled by the script.  You can also ignore most of the settings in the Shoal script, as they are also controlled within the script,
with the exception of the size value. This determines the length of the raycasts for obstacle detection.  You can view the swimming demo in scene view to see the raycasts, and 
adjust the size value accordingly.


