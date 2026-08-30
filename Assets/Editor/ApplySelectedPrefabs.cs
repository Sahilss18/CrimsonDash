using UnityEditor;
using UnityEngine;


public class ApplySelectedPrefabs : EditorWindow
{
	public delegate void ApplyOrRevert (GameObject instanceRoot);

	[MenuItem ("Tools/Apply all selected prefabs %#a")]
	static void ApplyPrefabs ()
	{
		SearchPrefabConnections (ApplyToSelectedPrefabs);
	}

	[MenuItem ("Tools/Revert all selected prefabs %#r")]
	static void ResetPrefabs ()
	{
		SearchPrefabConnections (RevertToSelectedPrefabs);
	}

	//Look for connections
	static void SearchPrefabConnections (ApplyOrRevert applyOrRevert)
	{
		GameObject[] selection = Selection.gameObjects;

		if (selection.Length > 0) {
			int updatedCount = 0;
			//Iterate through all the selected gameobjects
			foreach (GameObject go in selection) {
				if (!PrefabUtility.IsPartOfPrefabInstance (go)) {
					continue;
				}

				GameObject instanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot (go);
				if (instanceRoot == null)
				{
					continue;
				}

				if (applyOrRevert != null) {
					updatedCount++;
					applyOrRevert (instanceRoot);
				}
			}
			Debug.Log (updatedCount + " prefab" + (updatedCount > 1 ? "s" : "") + " updated");
		}
	}

	//Apply
	static void ApplyToSelectedPrefabs (GameObject instanceRoot)
	{
		PrefabUtility.ApplyPrefabInstance (instanceRoot, InteractionMode.UserAction);
	}

	//Revert
	static void RevertToSelectedPrefabs (GameObject instanceRoot)
	{
		PrefabUtility.RevertPrefabInstance (instanceRoot, InteractionMode.UserAction);
	}


}
