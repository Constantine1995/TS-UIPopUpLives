/***********************************************************
 * Singleton.cs
 * 
 * 
 * 
 **********************************************************/ 
 //using UnityEngine;
public class Singleton
{
    public static float something;

    protected Singleton() { }

    private static Singleton _instance = null;

    public static Singleton Current
    {
        get
        {
            return Singleton._instance == null ? new Singleton() : Singleton._instance;
        }
    }
}
//public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
//{
	//#region INSTANCE PROPERTY

	//private static T currentInstance = null;

	//public static T Instance
	//{
	//	get
	//	{
	//		if (currentInstance == null)
	//		{
	//			currentInstance = (T)GameObject.FindObjectOfType(typeof(T));

	//			if (currentInstance == null)
	//			{
 //                   Debug.LogFormat("<color=blue>Singleton new instance created: </color> <b>{0}</b>", typeof(T));
	//				GameObject singleton = new GameObject(typeof(T).ToString());
	//				currentInstance = singleton.AddComponent<T>();
	//				DontDestroyOnLoad(currentInstance.gameObject);
	//			}
	//		}

	//		return currentInstance;
	//	}
	//}

	//#endregion


	//protected virtual void Awake()
	//{
	//	if (currentInstance != null && currentInstance.GetInstanceID() != this.GetInstanceID())
	//	{
	//		Destroy(this.gameObject);
	//		return;
	//	}
			
	//	currentInstance = gameObject.GetComponent<T>();
	//	DontDestroyOnLoad(currentInstance.gameObject);
	//}

	//protected virtual void OnDestroy()
	//{
	//	if (currentInstance != null)
	//		currentInstance = null;
	//}
//}