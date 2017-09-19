using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UnityEditorToolkit {
	public class SnappingMenuItems : Editor {
		static readonly string SnapToOriginMessage = "Snap to Origin";
		static readonly string SnapToGroundMessage = "Snap to Ground";
		static readonly string SnapYToZeroMessage = "Snap Y to Zero";

		// The maximum distance between "lowest" vertices
		const float EPSILON = 0.0001f;

		// TODO: Refactor into helper funtions class
		static bool Approximately(float a, float b, float epsilon) {
			return (a >= (b - epsilon)) && (a <= (b + epsilon));
		}

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
				Undo.RecordObject(snapTransform, SnapToOriginMessage);
				snapTransform.localPosition = Vector3.zero;
			}
			else {
				// Snap transform y to zero
				Undo.RecordObject(snapTransform, SnapYToZeroMessage);
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
			List<Vector3> lowestVertices = new List<Vector3>();
			lowestVertices.Add(snapTransform.TransformPoint(vertices[0]));
			int vertexCount = mesh.vertexCount;

			// Find lowest vertices in world space
			for(int i = 1; i < vertexCount; i++) {
				Vector3 v = snapTransform.TransformPoint(vertices[i]);
				if(Approximately(v.y, lowestVertices[0].y, EPSILON)) {
					lowestVertices.Add(v);
				}
				else if(v.y < lowestVertices[0].y) {
					lowestVertices.Clear();
					lowestVertices.Add(v);
				}
			}

			// Cast ray(s) downwards then snap object to closest surface
			RaycastHit hit;
			float minSnapDistance = Mathf.Infinity;

			// Ignore the current object when raycasting
			int currentLayer = snapObject.layer;
			snapObject.layer = LayerMask.NameToLayer("Ignore Raycast");

			for(int i = 0; i < lowestVertices.Count; i++) {
				if(Physics.Raycast(lowestVertices[i], Vector3.down, out hit)) {
					minSnapDistance = Mathf.Min(hit.distance, minSnapDistance);
				}
			}

			snapObject.layer = currentLayer;

			if(minSnapDistance != Mathf.Infinity) {
				Undo.RecordObject(snapTransform, SnapToGroundMessage);
				snapTransform.Translate(0f, -minSnapDistance, 0f, Space.World);
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
