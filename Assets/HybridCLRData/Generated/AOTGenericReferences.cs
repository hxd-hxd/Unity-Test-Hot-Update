using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Framework.Local.dll",
		"Framework.Runtime.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// }}

	public void RefMethods()
	{
		// object Framework.ExtendUtility.FindOf<object>(UnityEngine.Transform,string)
		// Framework.IAssetOperation Framework.IResourcesHandler.LoadAssetAsync<object>(string)
		// Framework.IAssetOperation Framework.ResourcesManager.LoadAssetAsync<object>(string)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Test.InitBGPanel.<>c.<<Start>b__1_0>d>(System.Runtime.CompilerServices.TaskAwaiter&,Test.InitBGPanel.<>c.<<Start>b__1_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Test.InitBGPanel.<>c.<<Start>b__1_0>d>(Test.InitBGPanel.<>c.<<Start>b__1_0>d&)
		// object UnityEngine.Component.GetComponent<object>()
	}
}