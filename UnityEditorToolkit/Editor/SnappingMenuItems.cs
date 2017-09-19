using UnityEngine;
using UnityEditor;

namespace UnityEditorToolkit {
	public class SnappingMenuItems : Editor {
		static readonly string SnapToOriginString = "Snap to Origin";
		static readonly string SnapToGroundString = "Snap to Ground";
		static readonly string SnapYToZeroString = "Snap Y to Zero";

		const string SnapToZeroMenuItem = "Toolkit/Snapping/Snap to Zero";
		const string SnapToGroundMenuItem = "Toolkit/Snapping/Snap to Ground";

		[MenuItem(Constants.SnapToZeroItem, false, Constants.SnapToZeroPriority)]
		static void SnapToOrigin() {
			// Get transform data
			Transform snapTransform = Selection.activeTransform;
			if(snapTransform == null) {
				Debug.Log("Cannot snap to origin because" +
					" no gameobject is selected");
				return;
			}

			if(snapTransform.position.y == 0f) {
				// Snap object transform to origin
				Undo.RecordObject(snapTransform, SnapToOriginString);
				snapTransform.localPosition = Vector3.zero;
			}
			else {
				// Snap transform y to zero
				Undo.RecordObject(snapTransform, SnapYToZeroString);
				snapTransform.localPosition = new Vector3(
					snapTransform.localPosition.x,
					0,
					snapTransform.localPosition.z
				);
			}
		}

		[MenuItem(Constants.SnapToZeroItem, true)]
		static bool SelectionNotAtOrigin() {
			return (Selection.activeTransform != null)
				&& (!(Selection.activeTransform.position == Vector3.zero));
		}

		[MenuItem(Constants.SnapToGroundItem, false, Constants.SnapToGroundPriority)]
		static void SnapToGround() {
			// Get current selected gameobject in the scene
			GameObject snapObject = Selection.activeGameObject;
			if(snapObject == null) {
				Debug.Log("Cannot snap to ground because" +
					" no gameobject is selected.");
				return;
			}
			Transform snapTransform = snapObject.transform;

			// Get object mesh if it exists
			MeshFilter filter = snapObject.GetComponent<MeshFilter>();
			if(filter == null) {
				Debug.Log("Cannot snap to ground because" +
					" the selected object has no mesh filter attached.");
				return;
			}
			Mesh mesh = filter.sharedMesh;
			if(mesh == null) {
				Debug.Log("Cannot snap to ground because" +
					" the selected object has no mesh attached.");
				return;
			}

			// Storage of mesh data
			Vector3[] vertices = mesh.vertices;
			Vector3 lowestVertex = snapTransform.TransformPoint(vertices[0]);
			int vertexCount = mesh.vertexCount;

			// Find lowest vertex in world space
			// TODO: Use array of all lowest vertices
			for(int i = 1; i < vertexCount; i++) {
				Vector3 v = snapTransform.TransformPoint(vertices[i]);
				if(v.y < lowestVertex.y) {
					lowestVertex = v;
				}
			}

			// Cast a ray downwards then snap object to closest surface
			RaycastHit hit;
			if(Physics.Raycast(lowestVertex, Vector3.down, out hit)) {
				float snapDistance = hit.point.y - lowestVertex.y;
				Undo.RecordObject(snapTransform, SnapToGroundString);
				snapTransform.Translate(0f, snapDistance, 0f, Space.World);
			}
			else {
				Debug.Log("Cannot snap to ground because" +
					" there is no gameobject with an active collider below" +
					" the selected gameobject's lowest vertex.");
			}
		}

		[MenuItem(Constants.SnapToGroundItem, true)]
		static bool CheckSnapToGround() {
			return Selection.activeTransform != null;
		}
	}
}
