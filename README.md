## UniDLL
Editor window to create DLLs from C# code in Unity easily

## How to use
- In Unity, go to `Tools/UniDLL/Dll Creator`  
- Fill the following details 
    - `DLL Name`: the name of the DLL that is to be generated
    - `Source`: list of C# sources. Add a Folder to add all the `*.cs` files inside it recursively or add a `.cs` file to add a single source file  
    - `Defines`: list of compiltation symbols for the DLL
    - `References`: list of references that must be used for compiling.
- Click `Create`
- The build is stored inside the Unity Project's `Assets` folder in a folder called `<DLL Name>-build`. Any `LICENSE` and `README` files from the Unity project root are also copied there. Right-click on this folder to export a `UnityPackage`.

## More
- https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/command-line-building-with-csc-exe
- https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/index

## Contact
[@github](https://www.github.com/adrenak)  
[@www](http://www.vatsalambastha.com)