using System;
using Microsoft.SPOT;
using AgentMenu;
using Microsoft.SPOT.Presentation.Media;

namespace AgentMenuDemo {

  /// <summary>
  /// Checked menu item, to demonstrate how to create custom menu items
  /// </summary>
  public class CheckedMenuItem : MenuItem {

    public bool Checked;

    public override void Render(Bitmap display, Font font, int yPos) {
      int size = MenuHandler.rowHeight - 4;
      display.DrawRectangle(Color.White, 1, MenuHandler.xOffset, yPos + 2, size, size, 0, 0,
        Color.White, 0, 0, Color.White, Bitmap.MaxWidth, Bitmap.MaxHeight, (ushort)(Checked ? 255 : 0));
      display.DrawText(Text, font, Color.White, MenuHandler.xOffset + MenuHandler.rowHeight, yPos + MenuHandler.yOffset);

      // Uncomment to add a fancy checkbox fill effect for checked items, uses more resources though
      //if (Checked) {
      //  display.DrawRectangle(Color.Black, 1, MenuHandler.xOffset + 1, yPos + 3, size - 2, size - 2, 0, 0,
      //    Color.Black, MenuHandler.xOffset + 1, yPos + 3, Color.White, MenuHandler.xOffset + 1 + (size / 2), yPos + 3 + (size / 2), 255);
      //}
    }

    public override void Select() {
      Checked = !Checked; // Toggle the checkbox
      MenuHandler.Refresh();
      base.Select();
    }
  }
}
