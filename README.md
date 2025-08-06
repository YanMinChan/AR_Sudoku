# AR_Sudoku
 sudoku game developed for HoloLens 2
 
![VideoDemo](./Report%20Resources/VideoDemo.gif)

### Built with
[![Unity][Unity]][Unity-url]
[![C#][CSharp]][Csharp-url]
[![DotNet][DotNet]][Dotnet-url]

## Getting Started

### Requirements
- [Unity 2022.3 LTS][Unity-url] 
- [MRTK3][MRTK-url]
- [Newtonsoft][Newtonsoft-url]
- [Microsoft Visual Studio][vs-url]

### Installation (Unity)
Below is the steps on how to install and set up the application in Unity.

1. Clone the repo
    ```sh
    git clone https://github.com/YanMinChan/AR_Sudoku.git
    ```

2. Change git remote url to avoid accidental pushes to base project
   ```sh
   git remote set-url origin github_username/repo_name
   git remote -v # confirm the changes
   ```

3. Install the sudoku puzzle from the following link and manually move it to the `Assets/StreamingAssets`. [[Link][Sudoku-url]]

4. Set up the OpenXRproject with MRTK configurations. [[Link][OpenXRconfig-url]]

### Deployment (HoloLens 2)
Deployment can be done by following the steps in the following links.
1. Build the project in the Unity Editor. [[Link][BuildDeploy-url]]
2. Using Visual Studio to deploy and debug. [[Link][VSDeploy-url]]

## Features
- Core sudoku gameplay (validate the number filled in by player)
- Undo action
- Pause game
- Restart game
- Elapsed time timer
- Local leaderboard

## References
- Digits: 
    - [https://assetstore.unity.com/packages/3d/props/modular-low-poly-letters-and-icons-296956#description](https://assetstore.unity.com/packages/3d/props/modular-low-poly-letters-and-icons-296956#description)

- Sfx: 
    - [https://pixabay.com/sound-effects/error-08-206492/](https://pixabay.com/sound-effects/error-08-206492/)
    - [https://pixabay.com/sound-effects/level-up-05-326133/](https://pixabay.com/sound-effects/level-up-05-326133/)
    - [https://pixabay.com/sound-effects/stick-hitting-a-dreadlock-small-thud-83297/](https://pixabay.com/sound-effects/stick-hitting-a-dreadlock-small-thud-83297/)
    - [https://pixabay.com/sound-effects/new-level-04-152480/](https://pixabay.com/sound-effects/new-level-04-152480/)

- Puzzle:
    - [https://www.kaggle.com/datasets/bryanpark/sudoku][Sudoku-url]

- Keyboard: 
    - [https://github.com/LocalJoost/MRTK3TouchableNonNativeKeyboard](https://github.com/LocalJoost/MRTK3TouchableNonNativeKeyboard)

<!--Mark down links and images-->
[Unity]: https://img.shields.io/badge/unity-100000?style=for-the-badge&logo=unity&logoColor=white
[Unity-url]: https://unity.com/
[CSharp]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white
[CSharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
[Dotnet]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[Dotnet-url]: https://dotnet.microsoft.com/en-us/
[MRTK-url]: https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk3-overview/
[Newtonsoft-url]: https://www.newtonsoft.com/json
[vs-url]: https://visualstudio.microsoft.com/
[OpenXRconfig-url]: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/new-openxr-project-with-mrtk
[BuildDeploy-url]: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/build-and-deploy-to-hololens
[VSDeploy-url]: https://learn.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2
[Sudoku-url]: https://www.kaggle.com/datasets/bryanpark/sudoku
