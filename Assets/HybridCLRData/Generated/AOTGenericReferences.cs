public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	//System.Action`1<System.Byte>
	//System.Action`1<System.Object>
	//System.Action`1<System.Single>
	//System.Collections.Generic.Dictionary`2<System.Object,System.Object>
	//System.Collections.Generic.Dictionary`2<System.Single,System.Object>
	//System.Collections.Generic.Dictionary`2/Enumerator<System.Object,System.Object>
	//System.Collections.Generic.IEnumerator`1<System.Object>
	//System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>
	//System.Collections.Generic.List`1<System.Object>
	//System.Collections.Generic.List`1<ThreadTransitionComponent/DelayedQueueItem>
	//System.Collections.Generic.List`1<ThreadTransitionComponent/NoDelayedQueueItem>
	//System.Collections.Generic.List`1<UnityEngine.CombineInstance>
	//System.Collections.Generic.List`1<System.Single>
	//System.Collections.Generic.List`1<UnityEngine.EventSystems.RaycastResult>
	//System.Collections.Generic.List`1/Enumerator<System.Object>
	//System.Func`1<System.Byte>
	//System.Func`1<System.Single>
	//System.Func`2<ThreadTransitionComponent/DelayedQueueItem,System.Byte>
	//System.Func`2<System.Object,System.Byte>
	//System.Nullable`1<System.Byte>
	//System.Predicate`1<System.Object>
	//UnityEngine.Events.UnityAction`1<System.Byte>
	//UnityEngine.Events.UnityEvent`1<System.Object>
	//UnityEngine.Events.UnityEvent`1<System.Byte>
	// }}

	public void RefMethods()
	{
		// System.Object[] System.Array::Empty<System.Object>()
		// System.Collections.Generic.IEnumerable`1<ThreadTransitionComponent/DelayedQueueItem> System.Linq.Enumerable::Where<ThreadTransitionComponent/DelayedQueueItem>(System.Collections.Generic.IEnumerable`1<ThreadTransitionComponent/DelayedQueueItem>,System.Func`2<ThreadTransitionComponent/DelayedQueueItem,System.Boolean>)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Test.InitBGPanel/<>c/<<Start>b__1_0>d>(System.Runtime.CompilerServices.TaskAwaiter&,Test.InitBGPanel/<>c/<<Start>b__1_0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder::Start<Test.InitBGPanel/<>c/<<Start>b__1_0>d>(Test.InitBGPanel/<>c/<<Start>b__1_0>d&)
		// System.Object System.Threading.Interlocked::CompareExchange<System.Object>(System.Object&,System.Object,System.Object)
		// System.Object UnityEngine.AndroidJavaObject::Call<System.Object>(System.String,System.Object[])
		// System.Object UnityEngine.AndroidJavaObject::CallStatic<System.Object>(System.String,System.Object[])
		// System.Object UnityEngine.Component::GetComponent<System.Object>()
		// System.Object UnityEngine.Component::GetComponentInChildren<System.Object>()
		// System.Object UnityEngine.Component::GetComponentInParent<System.Object>()
		// System.Object[] UnityEngine.Component::GetComponents<System.Object>()
		// System.Boolean UnityEngine.EventSystems.ExecuteEvents::Execute<System.Object>(UnityEngine.GameObject,UnityEngine.EventSystems.BaseEventData,UnityEngine.EventSystems.ExecuteEvents/EventFunction`1<System.Object>)
		// System.Object UnityEngine.GameObject::AddComponent<System.Object>()
		// System.Object UnityEngine.GameObject::GetComponent<System.Object>()
		// System.Object UnityEngine.GameObject::GetComponentInChildren<System.Object>(System.Boolean)
		// System.Object[] UnityEngine.GameObject::GetComponentsInChildren<System.Object>(System.Boolean)
		// System.Object UnityEngine.Object::FindObjectOfType<System.Object>()
		// System.Object[] UnityEngine.Object::FindObjectsOfType<System.Object>()
		// System.Object UnityEngine.Object::Instantiate<System.Object>(System.Object)
		// System.Object UnityEngine.Object::Instantiate<System.Object>(System.Object,UnityEngine.Transform)
		// System.Object UnityEngine.Resources::Load<System.Object>(System.String)
		// YooAsset.AssetOperationHandle YooAsset.YooAssets::LoadAssetAsync<System.Object>(System.String)
	}
}