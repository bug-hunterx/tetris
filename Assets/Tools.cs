using UnityEngine;

public class Tools {
	
	public static bool IsInLayerMask(GameObject obj, LayerMask mask){
		return ((mask.value & (1 << obj.layer)) > 0);
	}
}
