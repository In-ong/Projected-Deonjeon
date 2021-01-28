//===============================================================================
using UnityEngine;
//===============================================================================
public class FSMSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	//----------------------------------------
	private static T _instance;	
	private static object _lock = new object();
	//----------------------------------------
	public static T Instance
	{
		get
		{
			lock(_lock)
			{
				if (_instance == null)
				{
					_instance = (T) FindObjectOfType(typeof(T));
					
					if ( FindObjectsOfType(typeof(T)).Length > 1 )
					{
						Debug.LogError("--- FSMSingleton error ---");
						return _instance;
					
					}//	if ( FindObjectsOfType(typeof(T)).Length > 1 )
					
					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) "+ typeof(T).ToString();
						singleton.hideFlags = HideFlags.HideAndDontSave;

					}//	if (_instance == null)
					else
						Debug.LogError("--- FSMSingleton already exists ---");

				}//	if (_instance == null)
				
				return _instance;

			}//	lock(_lock)

		}//	get
	
	}//	public static T Instance
	//----------------------------------------

}//	public class FSMSingleton<T> : MonoBehaviour where T : MonoBehaviour
//===============================================================================