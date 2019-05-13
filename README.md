# Idle Miner

This is a simulation game with the raw UI. Primary purpose of this project was to implement an expandable code architecture. 

##### Highlights:
  - Unity 2018.2.0f2
  - MVC
  - Dependency Injection
  - Reactive Programing
  - Promises
  - State Machine
  - Generic Popup System
  - Spine2D (Pending)

| Plugins | Description |
| ------ | ------ |
|[Unity3D] | [3D Game Engine.][GE]|
| [Zenject] | [Dependency Injection.][DE] |
| [UniRx] | [Reactive Programming.][RP] |
| [C# Promises] | [Asynchronous coding.][AC] |
| [Spine] | [2D Animation.][SP] |

#### Installation
  - Pull the code.
  - Use Unity 2018.2.0f2 to open it.
  - Check /MainMenu/Potato-Games/Always Start from Startup Scene.
  - Press Play.

#### Hierarchy Overview:
  - All the scenes code is in __\Assets\Scripts\pg\im\view__. Each scene has the respective folders including some extra scenes like Popup and Hud etc.
  - __GamePlay__ scene has all the __magic__.
  - __Bootstrap__ is the starting scene.
  - __*Data__ files\classes are for __Metadata__ objects.
  - __*RemoteData__ files/classes are for __GameState__ objects and I am linking the *Data on loading of GameState.
  - __*RemoteDataModel__ are the Models that contain __RemoteData__ and other reactive properties and collections.
  - __\Assets\Scripts\pg\core__ is __submodule__ that contains just the abstract of some generics that can be shared across the project so I have added it to minimise my work.
  - All the __scenes__ and __respective resources__ are in __\Assets\Resources__
  - Main __Game code__ is in __\Assets\Scripts\pg\im\.
  - (__Models__, __RemoteData__ and __Data__)__s__ are in __\Assets\Scripts\pg\im\model__.
  - All the __Commands__ are in   __\Assets\Scripts\pg\im\command__.

#### Information:
Delete the /Assets/StreamingAssets/GameState.json if you want to run the default gamestate again.
Meta is not balanced. So you will have to manage.


### Development
Want to contribute? Great!
Create PR if you want to change something.


### Todos
 - Add spine animations.
 - Add Managers and Upgrade functionality.
 - Balance and refactor the Meta.
 - Make it reactive to the member wise. Currently it is refreshing the whole Model on update.
 - Write Tests.


**Free Software, Hell Yeah!**

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)


   [Unity3D]: <https://unity3d.com/unity/whats-new/unity-2018.2.0>
   [Zenject]: < https://github.com/svermeulen/Zenject>
   [UniRx]: <https://github.com/neuecc/UniRx>
   [C# Promises]: <https://github.com/Real-Serious-Games/C-Sharp-Promise>
   [Spine]: <http://esotericsoftware.com/>

   [GE]: <https://en.wikipedia.org/wiki/Game_engine>
   [DE]: <https://en.wikipedia.org/wiki/Dependency_injection>
   [RP]: <https://en.wikipedia.org/wiki/Reactive_programming>
   [AC]: <http://www.what-could-possibly-go-wrong.com/promises-for-game-development/#introduction-to-promises>
   [SP]: <http://esotericsoftware.com/blog>

