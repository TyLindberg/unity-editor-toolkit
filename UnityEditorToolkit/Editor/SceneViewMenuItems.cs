using UnityEngine;
using UnityEditor;

namespace UnityEditorToolkit {
	public class SceneViewMenuItems : Editor {

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

		// Shading Mode

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

		// Scene Controls

		static bool CanRotateScene(SceneView view) {
			return view != null && !view.isRotationLocked;
		}

		static void RotateSceneView(SceneView view, Quaternion rot) {
			if(CanRotateScene(view)) {
				view.LookAt(view.pivot, rot);
			}
		}

		[MenuItem(Constants.RightViewItem, false, Constants.RightViewPriority)]
		static void SetRightView() {
			RotateSceneView(GetSceneView(), Quaternion.Euler(0, -90, 0));
		}

		[MenuItem(Constants.RightViewItem, true)]
		static bool CheckRightViewItem() {
			return CanRotateScene(GetSceneView());
		}

		[MenuItem(Constants.LeftViewItem, false, Constants.LeftViewPriority)]
		static void SetLeftView() {
			RotateSceneView(GetSceneView(), Quaternion.Euler(0, 90, 0));
		}

		[MenuItem(Constants.LeftViewItem, true)]
		static bool CheckLeftViewItem() {
			return CanRotateScene(GetSceneView());
		}

		[MenuItem(Constants.TopViewItem, false, Constants.TopViewPriority)]
		static void SetTopView() {
			RotateSceneView(GetSceneView(), Quaternion.Euler(90, 0, 0));
		}

		[MenuItem(Constants.TopViewItem, true)]
		static bool CheckTopViewItem() {
			return CanRotateScene(GetSceneView());
		}

		[MenuItem(Constants.BottomViewItem, false, Constants.BottomViewPriority)]
		static void SetBottomView() {
			RotateSceneView(GetSceneView(), Quaternion.Euler(-90, 0, 0));
		}

		[MenuItem(Constants.BottomViewItem, true)]
		static bool CheckBottomViewItem() {
			return CanRotateScene(GetSceneView());
		}

		[MenuItem(Constants.FrontViewItem, false, Constants.FrontViewPriority)]
		static void SetFrontView() {
			RotateSceneView(GetSceneView(), Quaternion.Euler(0, 180, 0));
		}

		[MenuItem(Constants.FrontViewItem, true)]
		static bool CheckFrontViewItem() {
			return CanRotateScene(GetSceneView());
		}

		[MenuItem(Constants.BackViewItem, false, Constants.BackViewPriority)]
		static void SetBackView() {
			RotateSceneView(GetSceneView(), Quaternion.Euler(0, 0, 0));
		}

		[MenuItem(Constants.BackViewItem, true)]
		static bool CheckBackViewItem() {
			return CanRotateScene(GetSceneView());
		}
	}
}
