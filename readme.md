## JustRetryPlus

Based on JustRetry from https://thunderstore.io/c/repo/p/nickklmao/JustRetry/

this mod skips the round end and heals all players to full health when retrying a level.

## Changelog
26.03.2025 - Added a more stable, non wait based healing that fires on the round start
I couldn't at first find it but nickklmao had already implemented a round start event that I could use to heal the players. 

## Building and finding the .dlls
1. Clone the repository
2. Open the solution in Visual Studio, Rider or VSCode
3. Go to Steam, find R.E.P.O, right click, properties, local files, browse local files
4. go to the REPO_Data\Managed folder
5. COPY the following dlls to the JustRetryPlus/libs folder:
    - Assembly-CSharp.dll < required, the others are optional but may be of use when modifiying this mod)
    - Facepunch.Steamworks.Win64.dll < optional >
    - Newtonsoft.Json.dll < optional >
    - Photon3Unity3D.dll < optional >
    - PhotonChat.dll < optional >
    - PhotonRealtime.dll < optional >
    - PhotonUnityNetworking.dll < optional >
    - PhotonUnityNetworking.Utilities.dll < optional >
    - PhotonVoice.dll < optional >
    - PhotonVoice.API.dll < optional >
    - PhotonVoice.PUN.dll < optional >
    - Unity.TextMeshPro.dll < optional >
    - UnityEngine.UI.dll < optional >
6. Build the solution
7. Go to Thunderstore, Select R.E.P.O, go to a profile, "Settings" and "Import local mod"
8. Select the .dll from the JustRetryPlus/bin/Debug folder