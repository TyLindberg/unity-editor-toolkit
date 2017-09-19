using UnityEngine;
using UnityEditor;

namespace UnityEditorToolkit {
	public class ShadingModeMenuItems : Editor {

		static SceneView GetSceneView() {
			return SceneView.lastActiveSceneView;
		}

		static void SetSceneRenderMode(DrawCameraMode mode) {
			SceneView currentScene = GetSceneView();
			currentScene.renderMode = mode;
			currentScene.Repaint();
		}

		[MenuItem(Constants.ShadedModeItem)]
		static void SetShadedMode() {
			SetSceneRenderMode(DrawCameraMode.Textured);
		}

		[MenuItem(Constants.ShadedModeItem, true)]
		static bool CheckShadedMode() {
			return GetSceneView() != null;
		}

		[MenuItem(Constants.WireframeModeItem)]
		static void SetWireframeMode() {
			SetSceneRenderMode(DrawCameraMode.Wireframe);
		}

		[MenuItem(Constants.WireframeModeItem, true)]
		static bool CheckWireframeMode() {
			return GetSceneView() != null;
		}

		[MenuItem(Constants.ShadedWireframeModeItem)]
		static void SetShadedWireframeMode() {
			SetSceneRenderMode(DrawCameraMode.TexturedWire);
		}

		[MenuItem(Constants.ShadedWireframeModeItem, true)]
		static bool CheckShadedWireframeMode() {
			return GetSceneView() != null;
		}
	}
}
