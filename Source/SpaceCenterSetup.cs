#region license
/*The MIT License (MIT)
SpaceCenterSetup - Initialize variables and objects once

Copyright (c) 2015 DMagic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.Reflection;
using UnityEngine;

namespace KSCBuildingShortcuts
{

	/// <summary>
	/// Setup button position, keyboard shortcuts, and button icons at the main menu screen
	/// </summary>
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	class SpaceCenterSetup : MonoBehaviour
	{
		public static Rect[] buttons;
		public static Texture2D[] icons;
		public static KeyCode[] keys;
		public static readonly float size = 38;
		public static MethodInfo leftClick;
		public static MethodInfo rightClick;
		private static bool run;

		private void Start()
		{
			if (run)
				return;

			//Manually positioning each button along the bottom
			Rect r1 = new Rect(432, 55, size, size);
			Rect r2 = new Rect(r1.x + 42, r1.y, size, size);
			Rect r3 = new Rect(r2.x + 42, r1.y, size, size);
			Rect r4 = new Rect(r3.x + 42, r1.y, size, size);
			Rect r5 = new Rect(r4.x + 42, r1.y, size, size);
			Rect r6 = new Rect(r5.x + 42, r1.y, size, size);
			Rect r7 = new Rect(r6.x + 42, r1.y, size, size);
			Rect r8 = new Rect(r7.x + 42, r1.y, size, size);
			Rect r9 = new Rect(r8.x + 42, r1.y, size, size);
			Rect r10 = new Rect(r9.x + 42, r1.y, size, size);

			buttons = new Rect[10] { r1, r2, r3, r4, r5, r6, r7, r8, r9, r10};

			//Assign keyboard shortcuts for each number key on the top row
			keys = new KeyCode[10] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

			//Assign button icon textures, most use R&D node icons
			Texture2D t1 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_advrocketry", false);
			Texture2D t2 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_aerospacetech", false);
			Texture2D t3 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_advunmanned", false);
			Texture2D t4 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_advsciencetech", false);
			Texture2D t5 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_heavierrocketry", false);
			Texture2D t6 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/RDicon_aerospaceTech2", false);
			Texture2D t7 = GameDatabase.Instance.GetTexture("Squad/Contracts/Icons/report", false);
			Texture2D t8 = GameDatabase.Instance.GetTexture("Squad/PartList/SimpleIcons/R&D_node_icon_evatech", false);
			Texture2D t9 = GameDatabase.Instance.GetTexture("Squad/Strategies/Icons/AppreciationCampaign", false);
			Texture2D t10 = GameDatabase.Instance.GetTexture("KSCBuildingShortcuts/Flag_Icon", false);

			icons = new Texture2D[10] { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10 };

			//Some reflection methods to invoke SpaceCenterBuilding methods
			Type t = typeof(SpaceCenterBuilding);
			leftClick = t.GetMethod("OnLeftClick", BindingFlags.NonPublic | BindingFlags.Instance);
			rightClick = t.GetMethod("OnRightClick", BindingFlags.Instance | BindingFlags.NonPublic);

			run = true;
		}

	}
}
