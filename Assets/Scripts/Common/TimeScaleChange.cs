using UnityEngine;

public class TimeScaleChange : MonoBehaviour
{

	#region -- 資源參考區 --

	public float timeScale;

	#endregion
	
    #region -- 初始化/運作 --

	private void Update()
	{

		Time.timeScale = timeScale;
		
	}

	#endregion
	
}