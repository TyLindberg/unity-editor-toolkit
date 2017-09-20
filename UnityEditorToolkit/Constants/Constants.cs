namespace UnityEditorToolkit {
	public static class Constants {
		//////////////////////////////
		//   Menu Item Components   //
		//////////////////////////////

		private const string BaseMenuPath = "Toolkit/";
		private const string SnappingMenuPath = "";
		private const string ShadingModeMenuPath = "Shading Mode/";
		private const string SceneRotationMenuPath = "Scene Rotation/";


		//////////////////////////////
		//  Menu Item Key Bindings  //
		//////////////////////////////

		// _ = raw key press
		// % = Control(Windows)/Command(Mac)
		// # = Shift
		// & = Alt

		// Snapping
		private const string SnapToZeroKey = "_.";
		private const string SnapToGroundKey = "_g";

		// Shading Mode
		private const string ShadedModeKey = "_7";
		private const string WireframeModeKey = "_8";
		private const string ShadedWireframeModeKey = "_9";

		// Scene Rotation
		private const string RightViewKey = "_3";
		private const string LeftViewKey = "#3";
		private const string TopViewKey = "_4";
		private const string BottomViewKey = "#4";
		private const string FrontViewKey = "_5";
		private const string BackViewKey = "#5";


		//////////////////////////////
		//    Menu Item Strings     //
		//////////////////////////////

		// Snapping
		public const string SnapToZeroItem = BaseMenuPath + SnappingMenuPath +
			"Snap to Zero " + SnapToZeroKey;
		public const string SnapToGroundItem = BaseMenuPath + SnappingMenuPath +
			"Snap to Ground " + SnapToGroundKey;

		// Shading Mode
		public const string ShadedModeItem = BaseMenuPath + ShadingModeMenuPath +
			"Shaded " + ShadedModeKey;
		public const string WireframeModeItem = BaseMenuPath + ShadingModeMenuPath +
			"Wireframe " + WireframeModeKey;
		public const string ShadedWireframeModeItem = BaseMenuPath + ShadingModeMenuPath +
			"Shaded Wireframe " + ShadedWireframeModeKey;

		// Scene Rotation
		public const string RightViewItem = BaseMenuPath + SceneRotationMenuPath +
			"Right View " + RightViewKey;
		public const string LeftViewItem = BaseMenuPath + SceneRotationMenuPath +
			"Left View " + LeftViewKey;
		public const string TopViewItem = BaseMenuPath + SceneRotationMenuPath +
			"Top View " + TopViewKey;
		public const string BottomViewItem = BaseMenuPath + SceneRotationMenuPath +
			"Bottom View " + BottomViewKey;
		public const string FrontViewItem = BaseMenuPath + SceneRotationMenuPath +
			"Front View " + FrontViewKey;
		public const string BackViewItem = BaseMenuPath + SceneRotationMenuPath +
			"Back View " + BackViewKey;


		//////////////////////////////
		//   Menu Item Priorities   //
		//////////////////////////////

		// Snapping
		public const int SnapToZeroPriority = 1;
		public const int SnapToGroundPriority = SnapToZeroPriority + 1;

		// Shading Mode
		public const int ShadedModePriority = 50;
		public const int WireframeModePriority = ShadedModePriority + 1;
		public const int ShadedWireframeModePriority = WireframeModePriority + 1;

		// Scene Rotation
		public const int RightViewPriority = 55;
		public const int LeftViewPriority = RightViewPriority + 1;
		public const int TopViewPriority = LeftViewPriority + 1;
		public const int BottomViewPriority = TopViewPriority + 1;
		public const int FrontViewPriority = BottomViewPriority + 1;
		public const int BackViewPriority = FrontViewPriority + 1;
	}
}
