using System;
using Microsoft.SPOT;
using AgentMenu;
using Microsoft.SPOT.Presentation.Media;

namespace AgentMenuDemo {

  /// <summary>
  /// Checked menu item, to demonstrate how to create custom menu items
  /// </summary>
  public class CheckedMenuItem : MenuItem {

    #region Constructors

    public CheckedMenuItem():base() { }

    public CheckedMenuItem(OnSelected selectEvent): base(selectEvent) { }

    public CheckedMenuItem(string text) : base(text) { }

    public CheckedMenuItem(string text, OnSelected selectEvent) : base(text, selectEvent) { }

    #endregion

    public bool Checked;

    public override void Render(Bitmap display, Font font, int yPos) {
      int size = MenuHandler.rowHeight - 4;
      int xoffset = MenuHandler.xOffset + MenuHandler.DisplayXoffset;
      int width = Bitmap.MaxWidth - xoffset;
      int top = yPos + MenuHandler.DisplayYoffset;

      display.DrawRectangle(Color.White, 1, xoffset, top + 2, size, size, 0, 0,
        Color.White, 0, 0, Color.White, Bitmap.MaxWidth, Bitmap.MaxHeight, (ushort)(Checked ? 255 : 0));
      display.DrawText(Text, font, Color.White, xoffset + MenuHandler.rowHeight, top + MenuHandler.yOffset);

      // fancy checkbox fill effect for checked items
      if (Checked) {
        display.DrawRectangle(Color.Black, 1, xoffset + 1, top + 3, size - 2, size - 2, 0, 0,
          Color.Black, xoffset + 1, top + 3, Color.White, xoffset + 1 + (size / 2), top + 3 + (size / 2), 255);
      }
    }

    public override void Select() {
      Checked = !Checked; // Toggle the checkbox
      MenuHandler.Refresh();
      base.Select();
    }
  }
}
