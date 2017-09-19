namespace UnityEditorToolkit {
	public static class Constants {
		//////////////////////////////
		//   Menu Item Components   //
		//////////////////////////////

		private const string BaseMenuPath = "Toolkit/";
		private const string SnappingMenuPath = "Snapping/";
		private const string ShadingModeMenuPath = "Shading Mode/";


		//////////////////////////////
		//  Menu Item Key Bindings  //
		//////////////////////////////

		// Snapping
		private const string SnapToZeroKey = "_.";
		private const string SnapToGroundKey = "_g";

		// Shading Mode
		private const string ShadedModeKey = "_3";
		private const string WireframeModeKey = "_4";
		private const string ShadedWireframeModeKey = "_5";


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
	}
}
