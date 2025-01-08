using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleChange : MonoBehaviour
{

	#region -- 資源參考區 --

	public float timeScale;

	#endregion
	
	#region -- 變數參考區 --

	#endregion
	
    #region -- 初始化/運作 --

	private void Awake()
	{
		
	}

	private void Update()
	{

		Time.timeScale = timeScale;
		
	}

	#endregion
	
	#region -- 方法參考區 --

	#endregion
	
}