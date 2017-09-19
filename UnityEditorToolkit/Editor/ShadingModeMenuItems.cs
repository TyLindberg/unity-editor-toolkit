using UnityEngine;
using UnityEditor;

namespace UnityEditorToolkit {
	public class ShadingModeMenuItems : Editor {

		static readonly string ShadedModeMessage = "Set to Shaded";
		static readonly string WireframeModeMessage = "Set to Wireframe";
		static readonly string ShadedWireframeModeMessage = "Set to Shaded Wireframe";

		static string ShadingModeToMessage(DrawCameraMode mode) {
			switch(mode) {
			case DrawCameraMode.Textured:
				return ShadedModeMessage;
			case DrawCameraMode.Wireframe:
				return WireframeModeMessage;
			case DrawCameraMode.TexturedWire:
				return ShadedWireframeModeMessage;
			default:
				Debug.LogError(mode + " is an unsupported DrawCameraMode");
				break;
			}

			return "";
		}

		static SceneView GetSceneView() {
			return SceneView.lastActiveSceneView;
		}

		static void SetSceneRenderMode(DrawCameraMode mode) {
			SceneView currentScene = GetSceneView();
			if(currentScene.renderMode != mode) {
				Undo.RecordObject(currentScene, ShadingModeToMessage(mode));
				currentScene.renderMode = mode;
				currentScene.Repaint();
			}
		}

		[MenuItem(Constants.ShadedModeItem, false, Constants.ShadedModePriority)]
		static void SetShadedMode() {
			SetSceneRenderMode(DrawCameraMode.Textured);
		}

		[MenuItem(Constants.ShadedModeItem, true)]
		static bool CheckShadedMode() {
			return GetSceneView() != null;
		}

		[MenuItem(Constants.WireframeModeItem, false, Constants.WireframeModePriority)]
		static void SetWireframeMode() {
			SetSceneRenderMode(DrawCameraMode.Wireframe);
		}

		[MenuItem(Constants.WireframeModeItem, true)]
		static bool CheckWireframeMode() {
			return GetSceneView() != null;
		}

		[MenuItem(Constants.ShadedWireframeModeItem, false, Constants.ShadedWireframeModePriority)]
		static void SetShadedWireframeMode() {
			SetSceneRenderMode(DrawCameraMode.TexturedWire);
		}

		[MenuItem(Constants.ShadedWireframeModeItem, true)]
		static bool CheckShadedWireframeMode() {
			return GetSceneView() != null;
		}
	}
}
