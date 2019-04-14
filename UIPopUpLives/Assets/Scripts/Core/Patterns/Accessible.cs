/***********************************************************
 * Accessible.cs
 * 
 * 
 * 
 **********************************************************/ 
using UnityEngine;

public class Accessible<T> : MonoBehaviour where T : MonoBehaviour
{
	#region INSTANCE PROPERTY

	private static T currentInstance = null;

	public static T Current
	{
		get
		{
			if (currentInstance == null)
			{
				currentInstance = (T)GameObject.FindObjectOfType(typeof(T));

				if (currentInstance == null)
				{
					GameObject singleton = new GameObject(typeof(T).ToString());
					currentInstance = singleton.AddComponent<T>();
                }
			}

			return currentInstance;
		}
	}

    public static bool IsInstanceExists
    {
        get
        {
            return currentInstance != null;
        }
    }
    #endregion


    protected bool isOtherInstanceExists
    {
        get
        {
            return (currentInstance != null && currentInstance.GetInstanceID() != this.GetInstanceID());
        }
    }

    virtual protected void Awake()
    {
        if (isOtherInstanceExists)
        {
            Destroy(this.gameObject);
            return;
        }
        if (currentInstance == null)
        {
            currentInstance = this as T;
        }
    }

    void OnDestroy()
	{
		currentInstance = null;
	}
}