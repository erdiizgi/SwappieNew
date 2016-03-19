# Swappie Documentation

# 0. How to Run

The project can be run using all the scene files but to test all the flow run the Menu.scene. 

# 1. Levels

Every level should include;

 - A Player prefab, it can be attached from the Prefabs folder.
 - An empty game object that wraps the all elements in the game. I named
   it as "world" This is necessary because, after the player fit
   landscape the world is rotating. This wraps makes easy to rotate all
   the visible game elements.
 - There is a swappie camera prefab. It basicly adjusts the background
   to the same color. I didn't want to use the default camera because,
   we can add some features later to camera objects and with swappie
   camera prefab, applying it to all scenes would take less time.
 - There is Tap prefab that activates the tap feature on mobile devices.

# 2. Some important prefabs

 

 - **Bounce Point :** This prefab creates an invisible wall. When the player collides with that, it basicly bouncing.

  

 - **Circle Target:** This prefab makes the player finish the level. if Finishing point is Circle, use it.

 
  

 - **Triangle Target:** This prefab makes the player finish the level. if Finishing point is Triangle, use it.
 - **Square Target:** This prefab makes the player finish the level. if Finishing point is Square, use it.

 

 - **Spiky Landscape:** This prefab is challenge for the player. If player collide with this prefab, the world rotates. If the landscape Spiky or Square, use it.

 - **Danger:** When player collides with this prefab it dies and starts over.

# 3. How to configurate the level flow.

There is a script named Config. You can use to configurate anything in the game using this script. All the necessary prefabs have this script. For example, TriangleTarget prefab accepts only triangle shape. So we are configurating this target as assigning this variable to "Triangle". After that, there is level index. We set this as the next level as integer. So this is enough configuration for the triangle target. 

Variable names are telling what it does. You can add more variable to configure more things. Handling these variables is in the PlayerController.cs script inside the OnTriggerEnter2d function. But this design is purely wrong. Basicly there is one controller that controls everything, but good practices says that every class shouldn't have more than one responsibility.  
