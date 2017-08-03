using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	// Update is called once per frame
	public static Transform FindDeepChild(this Transform aParent, string aName)
	{
		foreach(Transform child in aParent)
		{
			if(child.name == aName )
				return child;
			Transform result = child.FindDeepChild(aName);
			if (result != null)
				return result;
		}
		return null;
	}

}
