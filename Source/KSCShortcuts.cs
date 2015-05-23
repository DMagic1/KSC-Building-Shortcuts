#region license
/*The MIT License (MIT)
KSCShortcuts - Draw the shortcut button UI

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

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

namespace KSCBuildingShortcuts
{

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class KSCShortcuts : MonoBehaviour
	{
		private bool overlay = false;
		private SpaceCenterBuilding[] buildings;

		private int UILayer = 5;
		private GameObject canvas;
		private GameObject eventSystem;
		private GameObject panel;

		private void Start()
		{
			//Add in all of the GameEvent listeners; used to deactivate the UI or block inputs
			GameEvents.onGUIAstronautComplexSpawn.Add(spawn);
			GameEvents.onGUIAdministrationFacilitySpawn.Add(spawn);
			GameEvents.onGUIMissionControlSpawn.Add(spawn);
			GameEvents.onGUIRnDComplexSpawn.Add(spawn);
			GameEvents.onGUIRecoveryDialogSpawn.Add(recoverySpawn);
			GameEvents.onGUILaunchScreenSpawn.Add(launchSpawn);

			GameEvents.onGUIAdministrationFacilityDespawn.Add(deSpawn);
			GameEvents.onGUIAstronautComplexDespawn.Add(deSpawn);
			GameEvents.onGUIRnDComplexDespawn.Add(deSpawn);
			GameEvents.onGUIMissionControlDespawn.Add(deSpawn);
			GameEvents.onGUILaunchScreenDespawn.Add(launchDeSpawn);
			GameEvents.onGUIRecoveryDialogDespawn.Add(recoveryDeSpawn);

			//A not very intelligent way of finding all of the SpaceCenterBuilding instances
			//This basically rips all instances of the type out of Unity
			var bObjects = GameObject.FindObjectsOfType<SpaceCenterBuilding>();
			buildings = new SpaceCenterBuilding[10];

			//Assign buildings to ordered array
			for (int i = 0; i < bObjects.Length; i++)
			{
				if (bObjects[i] == null)
					continue;

				switch (bObjects[i].facilityName)
				{
					case "VAB":
						buildings[0] = bObjects[i];
						break;
					case "SPH":
						buildings[1] = bObjects[i];
						break;
					case "TrackingStation":
						buildings[2] = bObjects[i];
						break;
					case "RnD":
						buildings[3] = bObjects[i];
						break;
					case "LaunchPad":
						buildings[4] = bObjects[i];
						break;
					case "Runway":
						buildings[5] = bObjects[i];
						break;
					case "MissionControl":
						buildings[6] = bObjects[i];
						break;
					case "AstronautComplex":
						buildings[7] = bObjects[i];
						break;
					case "Administration":
						buildings[8] = bObjects[i];
						break;
					case "FlagPole":
						buildings[9] = bObjects[i];
						break;
					default:
						continue;
				}
			}

			UISetup();
		}

		/// <summary>
		/// Watch for keyboard inputs here; 1-10 on the top number row are used to open each building
		/// </summary>
		private void Update()
		{
			if (overlay)
				return;

			for (int i = 0; i < SpaceCenterSetup.keys.Length; i++)
			{
				if (buildings[i] == null)
					continue;

				if (Input.GetKeyDown(SpaceCenterSetup.keys[i]))
				{
					leftClick(buildings[i]);
				}
			}
		}

		/// <summary>
		/// Remove all of the GameEvent listeners
		/// </summary>
		private void OnDestroy()
		{
			GameEvents.onGUIAstronautComplexSpawn.Remove(spawn);
			GameEvents.onGUIAdministrationFacilitySpawn.Remove(spawn);
			GameEvents.onGUIMissionControlSpawn.Remove(spawn);
			GameEvents.onGUIRnDComplexSpawn.Remove(spawn);
			GameEvents.onGUIRecoveryDialogSpawn.Remove(recoverySpawn);
			GameEvents.onGUILaunchScreenSpawn.Remove(launchSpawn);

			GameEvents.onGUIAdministrationFacilityDespawn.Remove(deSpawn);
			GameEvents.onGUIAstronautComplexDespawn.Remove(deSpawn);
			GameEvents.onGUIRnDComplexDespawn.Remove(deSpawn);
			GameEvents.onGUIMissionControlDespawn.Remove(deSpawn);
			GameEvents.onGUILaunchScreenDespawn.Remove(launchDeSpawn);
			GameEvents.onGUIRecoveryDialogDespawn.Remove(recoveryDeSpawn);
		}

		/// <summary>
		/// UI setup method; primarily taken from http://chikkooos.blogspot.jp/2015/03/new-ui-implementation-using-c-scripts.html
		/// </summary>
		private void UISetup()
		{
			canvas = createCanvas(this.transform);

			eventSystem = createEvent(canvas.transform);

			panel = createCanvas(canvas.transform);

			for (int i = 0; i < SpaceCenterSetup.buttons.Length; i++)
			{
				float f = 1f;
				if (i == 6)
					f = 0.85f;
				else if (i == 8)
					f = 0.95f;
				else if (i == 9)
					f = 0.75f;
				SpaceCenterBuilding B = buildings[i];
				createButton(panel.transform, SpaceCenterSetup.buttons[i], SpaceCenterSetup.icons[i], delegate { leftClick(B); }, delegate { rightClick(B); }, B.OnMouseOver, B.OnMouseExit, f);

				string m = i < 9 ? (i + 1).ToString() : "0";
				createText(panel.transform, new Rect(SpaceCenterSetup.buttons[i].x - 9, SpaceCenterSetup.buttons[i].y + 18, 18, 18), m, 24);
			}
		}

		/// <summary>
		/// The UI Canvas is the primary object, which all other UI objects are children of
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		private GameObject createCanvas(Transform parent)
		{
			GameObject co = new GameObject("Canvas");
			co.layer = UILayer;

			RectTransform canvasRect = co.AddComponent<RectTransform>();

			Canvas c = co.AddComponent<Canvas>();
			c.renderMode = RenderMode.ScreenSpaceOverlay;
			c.pixelPerfect = true;

			CanvasScaler cScaler = co.AddComponent<CanvasScaler>();
			cScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

			GraphicRaycaster cRay = co.AddComponent<GraphicRaycaster>();

			co.transform.SetParent(parent);

			return co;
		}

		/// <summary>
		/// The Panel object holds all of the UI controls
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		private GameObject createPanel(Transform parent)
		{
			GameObject p = new GameObject("Panel");

			p.layer = UILayer;

			RectTransform r = p.AddComponent<RectTransform>();
			r.anchorMin = new Vector2(0, 0);
			r.anchorMax = new Vector2(1, 1);
			r.anchoredPosition3D = new Vector3(0, 0, 0);
			r.anchoredPosition = new Vector2(400, 80);
			r.offsetMin = new Vector2(0, 0);
			r.offsetMax = new Vector2(0, 0);
			r.localPosition = new Vector3(0, 0, 0);
			r.sizeDelta = new Vector2(0, 0);
			r.localScale = new Vector3(0.2f, 0.1f, 1);

			CanvasRenderer renderer = p.AddComponent<CanvasRenderer>();

			p.transform.SetParent(parent);

			return p;
		}

		/// <summary>
		/// The Event object is responsible for handling all inputs for the UI system
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		private GameObject createEvent(Transform parent)
		{
			GameObject eso = new GameObject("EventSystem");

			EventSystem es = eso.AddComponent<EventSystem>();
			es.sendNavigationEvents = true;

			StandaloneInputModule inputModule = eso.AddComponent<StandaloneInputModule>();

			eso.transform.SetParent(parent);

			return eso;
		}

		/// <summary>
		/// The button object listens for inputs to trigger various events
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="r">Rect to manually position each button</param>
		/// <param name="t">Button image</param>
		/// <param name="leftAction">Delegate for left-click actions</param>
		/// <param name="rightAction">Delegate for right-click actions</param>
		/// <param name="hoverIn">Delegate for mouse-over action</param>
		/// <param name="hoverOut">Delegate for mouse-out action</param>
		/// <param name="scale">Scales each button based on the size of the icon texture</param>
		/// <returns></returns>
		private GameObject createButton(Transform parent, Rect r, Texture2D t, UnityAction leftAction, UnityAction rightAction, UnityAction hoverIn, UnityAction hoverOut, float scale)
		{
			GameObject button = new GameObject("Button");

			button.layer = UILayer;

			RectTransform RT = button.AddComponent<RectTransform>();
			SetSize(RT, new Vector2(r.width, r.height));
			RT.anchoredPosition3D = new Vector3(0, 0, 0);
			RT.anchoredPosition = new Vector2(r.x, r.y);
			RT.localScale = new Vector3(scale, scale, 1f);
			RT.localPosition.Set(0f, 0f, 0f);

			CanvasRenderer cr = button.AddComponent<CanvasRenderer>();

			Image i = button.AddComponent<Image>();
			i.sprite = UnityEngine.Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));

			EButton b = button.AddComponent<EButton>();
			b.interactable = true;
			b.onClick.AddListener(leftAction);
			b.onRightClick.AddListener(rightAction);
			b.HoverIn.AddListener(hoverIn);
			b.HoverOut.AddListener(hoverOut);

			button.transform.SetParent(parent);

			return button;
		}

		/// <summary>
		/// Text labels
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="r">Rect to manually position each label</param>
		/// <param name="s">Text message</param>
		/// <param name="font">Text font size</param>
		/// <returns></returns>
		private GameObject createText(Transform parent, Rect r, string s, int font)
		{
			GameObject to = new GameObject("Text");

			to.layer = UILayer;

			RectTransform trans = to.AddComponent<RectTransform>();
			SetSize(trans, new Vector2(r.width, r.height));
			trans.anchoredPosition3D = new Vector3(0, 0, 0);
			trans.anchoredPosition = new Vector2(r.x, r.y);
			trans.localScale = new Vector3(1, 1, 1);
			trans.localPosition.Set(0, 0, 0);

			CanvasRenderer render = to.AddComponent<CanvasRenderer>();

			Text t = to.AddComponent<Text>();
			t.supportRichText = true;
			t.text = s;
			t.fontSize = font;
			ScreenMessages SM = (ScreenMessages)GameObject.FindObjectOfType(typeof(ScreenMessages));
			Font d = SM.textStyles[1].font;
			t.font = d;
			t.alignment = TextAnchor.LowerLeft;
			t.horizontalOverflow = HorizontalWrapMode.Overflow;
			t.color = new Color(0, 0.8f, 0);

			to.transform.SetParent(parent);

			return to;
		}

		/// <summary>
		/// Some sort of UI element size calculation...
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="size"></param>
		private static void SetSize(RectTransform trans, Vector2 size)
		{
			Vector2 currSize = trans.rect.size;
			Vector2 sizeDiff = size - currSize;
			trans.offsetMin = trans.offsetMin -
									  new Vector2(sizeDiff.x * trans.pivot.x,
										  sizeDiff.y * trans.pivot.y);
			trans.offsetMax = trans.offsetMax +
									  new Vector2(sizeDiff.x * (1.0f - trans.pivot.x),
										  sizeDiff.y * (1.0f - trans.pivot.y));
		}

		/// <summary>
		/// Triggers the left-click building method
		/// </summary>
		/// <param name="b"></param>
		private void leftClick(SpaceCenterBuilding b)
		{
			if (!overlay && !InputLockManager.IsLocked(ControlTypes.KSC_FACILITIES))
				SpaceCenterSetup.leftClick.Invoke(b, null);
		}

		/// <summary>
		/// Triggers the right-click building method
		/// </summary>
		/// <param name="b"></param>
		private void rightClick(SpaceCenterBuilding b)
		{
			if (!overlay && !InputLockManager.IsLocked(ControlTypes.KSC_FACILITIES))
				SpaceCenterSetup.rightClick.Invoke(b, null);
		}

		/// <summary>
		/// Deactivate or block the UI when a building has been opened
		/// </summary>
		#region GameEventListeners

		private void spawn()
		{
			canvas.SetActive(false);
		}

		private void deSpawn()
		{
			canvas.SetActive(true);
		}

		private void launchSpawn(GameEvents.VesselSpawnInfo d)
		{
			overlay = true;
		}

		private void recoverySpawn(MissionRecoveryDialog d)
		{
			overlay = true;
		}

		private void launchDeSpawn()
		{
			overlay = false;
		}

		private void recoveryDeSpawn(MissionRecoveryDialog d)
		{
			overlay = false;
		}

		#endregion
	}
}
