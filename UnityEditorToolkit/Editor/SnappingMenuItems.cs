using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UnityEditorToolkit {
	public class SnappingMenuItems : Editor {
		// Whether or not to use raycasting to calculate snapping to ground
		public static bool usePhysicsSnapToGround = false;
		// Only relevant for render texture snap to ground
		public static float maxSnapToGroundDistance = 100;
		// Use to make values in editor prettier (no scientific notation)
		public static bool isSnapTruncated = true;
		public static uint truncateDecimalPlaces = 6;

		static readonly string SnapToOriginMessage = "Snap to Origin";
		static readonly string SnapToGroundMessage = "Snap to Ground";
		static readonly string SnapYToZeroMessage = "Snap Y to Zero";

		// The maximum distance between "lowest" vertices
		const float EPSILON = 0.0001f;

		// TODO: Refactor into helper funtions class
		static bool Approximately(float a, float b, float epsilon) {
			return (a >= (b - epsilon)) && (a <= (b + epsilon));
		}

		static float Truncate(float val, uint numDecimalPlaces) {
			return Mathf.Round(val * (10f * numDecimalPlaces)) / (10f * numDecimalPlaces);
		}

		static void ConfigureSnapToGroundCamera(Camera cam, RenderTexture renderTex) {
			cam.enabled = false; // Needs to be disabled to manually render
			cam.orthographic = true;
			cam.aspect = 1;
			cam.orthographicSize = 0.01f;
			cam.farClipPlane = maxSnapToGroundDistance;
			cam.nearClipPlane = 0.00001f;
			cam.targetTexture = renderTex;
			cam.clearFlags = CameraClearFlags.SolidColor;
			cam.backgroundColor = new Color(1, 0, 0);
			cam.transform.rotation = Quaternion.Euler(90, 0, 0);
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
			bool hasMesh = false;
			Mesh mesh = null;
			MeshFilter filter = snapObject.GetComponent<MeshFilter>();
			if(filter != null) {
				mesh = filter.sharedMesh;
				if(mesh != null && mesh.vertexCount > 0) {
					hasMesh = true;
				}
			}

			List<Vector3> lowestVertices = new List<Vector3>();

			if(hasMesh) {
				// Storage of mesh data
				Vector3[] vertices = mesh.vertices;
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
			}
			else {
				lowestVertices.Add(snapTransform.position);
			}

			float minSnapDistance = Mathf.Infinity;
			if(usePhysicsSnapToGround) {
				// Cast ray(s) downwards then snap object to closest surface
				RaycastHit hit;

				// Ignore the current object when raycasting
				int currentLayer = snapObject.layer;
				snapObject.layer = LayerMask.NameToLayer("Ignore Raycast");

				for(int i = 0; i < lowestVertices.Count; i++) {
					if(Physics.Raycast(lowestVertices[i], Vector3.down, out hit)) {
						minSnapDistance = Mathf.Min(hit.distance, minSnapDistance);
					}
				}

				snapObject.layer = currentLayer;
			}
			else {
				// Create a 1x1 pixel render texture to render out a orthographic camera
				// pointing directly below the object. This will function as an inefficient
				// and less precise raycast that allows snapping to ground to work on
				// meshes without colliders
				RenderTexture depthTexture = RenderTexture.GetTemporary(1, 1, 32);
				Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
				Shader depthShader = Shader.Find("Unity Editor Toolkit/Depth to Color");
				GameObject camObject = new GameObject();
				Camera depthCamera = camObject.AddComponent<Camera>();

				depthTexture.format = RenderTextureFormat.ARGBFloat;
				ConfigureSnapToGroundCamera(depthCamera, depthTexture);

				// Need to set this to active in order to use ReadPixels later
				RenderTexture.active = depthTexture;

				for(int i = 0; i < lowestVertices.Count; i++) {
					depthCamera.transform.position = lowestVertices[i];
					depthCamera.RenderWithShader(depthShader, "");
					tex.ReadPixels(new Rect(0, 0, 1, 1), 0, 0);
					Color[] colors = tex.GetPixels();

					if(colors.Length != 1) {
						Debug.LogError("GetPixels returned an array of colors of length: " +
							colors.Length + ". Expected an array of length 1.");
						return;
					}

					float snapDistance = depthCamera.nearClipPlane +
						colors[0].r * (depthCamera.farClipPlane - depthCamera.nearClipPlane);

					// Nothing was hit
					if(colors[0] == depthCamera.backgroundColor) {
						snapDistance = Mathf.Infinity;
					}

					minSnapDistance = Mathf.Min(minSnapDistance, snapDistance);
				}

				depthCamera.targetTexture = null;
				RenderTexture.active = null;
				RenderTexture.ReleaseTemporary(depthTexture);
				DestroyImmediate(camObject);
			}

			if(minSnapDistance != Mathf.Infinity) {
				Undo.RecordObject(snapTransform, SnapToGroundMessage);
				snapTransform.Translate(0f, -minSnapDistance, 0f, Space.World);
				if(isSnapTruncated) {
					float truncatedY = Truncate(snapTransform.position.y, truncateDecimalPlaces);
					snapTransform.position = new Vector3(
						snapTransform.position.x,
						truncatedY,
						snapTransform.position.z
					);
				}
			}
			else {
				if(usePhysicsSnapToGround) {
					Debug.Log("Cannot snap to ground because" +
						" there is no gameobject with an active collider below" +
						" the selected gameobject's lowest vertices.");
				}
				else {
					Debug.Log("Cannot snap to ground because no rendered mesh" +
						" components were found beneath the selected gameobject's" +
						" lowest vertices.");
				}
			}
		}

		[MenuItem(Constants.SnapToGroundItem, true)]
		static bool CheckSnapToGround() {
			return Selection.activeTransform != null;
		}
	}
}
