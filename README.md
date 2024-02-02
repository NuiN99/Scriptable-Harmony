# Scriptable Harmony

How to add to a Unity project

1. Have git installed (if you just installed it, restart your computer)
2. Open the Unity Package Manager under Window/Package Manager
3. Select the '+' in the top left, and then 'Add package from git URL'
4. Paste the URL found below into the field and select 'Apply'

### Package URL: 
```
https://github.com/NuiN99/Scriptable-Harmony-Package.git
```

--- WIP READ ME ---

Includes a custom Sound System, Scriptable Variables, Runtime Sets, Object Pooled Particle Spawner, an Extensions library, and more

I use all of these combined in all of my projects, and am constantly updating it

It has a lot of customization, and many useful editor windows and tools included

# Core System
Based on Ryan Hipples talk, which includes:

### THE IMPORTANT ONES
Scriptable Variable - A ScriptableObject that contain a variable, with OnChange events. Because it's an asset, its value is scene independant
Runtime Set - A ScriptableObject that contains objects during runtime, like a list of enemies, which automatically add and remove themselves OnEnable and OnDisable

### STILL GOOD ONES
Runtime Single - The same as above, but only one object
Scriptable List - A list of variables, with OnChange events
Scriptable Dictionary - A dictionary with OnChange events (WIP)

# Sound System
A complete system for playing sounds, automatically object pooled

It has 2 main objects:
SoundSO - Contains a list of AudioClips, and has AudioSource options, as well as randomization like Volume and Pitch
SoundPlayer - Basically acts as an AudioMixer, and all SoundSO's need to reference one. It controls the volume and pitch of all sounds currently playing (No AudioMixer required)

SoundSO Automatically chooses from a random clip in the list if there are multiple
Syntax:
[SerializeField] SoundSO sound;
sound.Play();
sound.PlaySpatial(Vector3 position, Transform parent = null)

# Particle Spawner
It just keeps a dictionary of already used prefabs, and keeps and Object Pool for each prefab. It calculates how long the particle system will last for, and then releases it to its pool

# NExtensions
A bunch of extension methods, utility, editor tools



