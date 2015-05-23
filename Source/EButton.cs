#region license
/*The MIT License (MIT)
EButton - An extension of the UnityEngine.UI.Button class

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

using UnityEngine.UI;

namespace KSCBuildingShortcuts
{
	/// <summary>
	/// An extension of the UI.Button class with a few added functions
	/// </summary>
	class EButton : Button
	{
		private EButton.ButtonClickedEvent right_click = new ButtonClickedEvent();
		private EButton.ButtonClickedEvent middle_click = new ButtonClickedEvent();
		private EButton.ButtonClickedEvent hoverIn = new ButtonClickedEvent();
		private EButton.ButtonClickedEvent hoverOut = new ButtonClickedEvent();

		/// <summary>
		/// Activates a right-click method
		/// </summary>
		public EButton.ButtonClickedEvent onRightClick
		{
			get { return right_click; }
			set { right_click = value; }
		}

		/// <summary>
		/// Activates a middle-click method
		/// </summary>
		public EButton.ButtonClickedEvent onMiddleClick
		{
			get { return middle_click; }
			set { middle_click = value; }
		}

		/// <summary>
		/// Activates a method when the mouse moves over the button
		/// </summary>
		public EButton.ButtonClickedEvent HoverIn
		{
			get { return hoverIn; }
			set { hoverIn = value; }
		}

		/// <summary>
		/// Activates a method when the mouse moves out of a button
		/// </summary>
		public EButton.ButtonClickedEvent HoverOut
		{
			get { return hoverOut; }
			set { hoverOut = value; }
		}

		/// <summary>
		/// Overrides the base Selectable class' OnPointerEnter method
		/// </summary>
		/// <param name="eventData"></param>
		public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);

			hoverIn.Invoke();
		}

		/// <summary>
		/// Overrides the base Selectable class' OnPointerExit method
		/// </summary>
		/// <param name="eventData"></param>
		public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
		{
			base.OnPointerExit(eventData);

			hoverOut.Invoke();
		}

		/// <summary>
		/// Overrides the base Button class' OnPointerClick method; listens for three different mouse buttons instead of just left-clicks
		/// </summary>
		/// <param name="eventData"></param>
		public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case UnityEngine.EventSystems.PointerEventData.InputButton.Left:
					base.OnPointerClick(eventData);
					break;
				case UnityEngine.EventSystems.PointerEventData.InputButton.Right:
					right_click.Invoke();
					break;
				case UnityEngine.EventSystems.PointerEventData.InputButton.Middle:
					middle_click.Invoke();
					break;
			}
		}
	}
}
