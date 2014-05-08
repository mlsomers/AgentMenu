using System;
using Microsoft.SPOT;
using AgentMenu;
using Microsoft.SPOT.Presentation.Media;

namespace AgentMenuDemo {
  public class SmallMenu {
    const int TopSpace = 18;
    private static bool InSmallMenu = false;
    private static ExtendedTimer TimeTimer = new ExtendedTimer(timerCallback, null, ExtendedTimer.TimeEvents.Second);
    private static Font smaller = Resources.GetFont(Resources.FontResources.small);

    private static void timerCallback(object state) {
      if (!InSmallMenu) return;
      DrawTime();
      MenuHandler.Display.Flush(0, 0, Bitmap.MaxWidth, TopSpace);
    }

    private static void DrawTime() {
      MenuHandler.Display.DrawRectangle(Color.White, 1, 0, 0, Bitmap.MaxWidth, TopSpace, 0, 0, Color.White, 0, 0, Color.White, Bitmap.MaxWidth, TopSpace, 255);
      MenuHandler.Display.DrawText(DateTime.Now.ToString("h:mm.ss"), Program.fontNinaB, Color.Black, 23, 2);
    }
    
    internal static bool Toggle(AgentMenu.MenuItem sender) {
      InSmallMenu = !InSmallMenu;
      if (InSmallMenu) {
        MenuHandler.DisplayXoffset = 21;
        MenuHandler.DisplayYoffset = TopSpace+1;
        MenuHandler.rowHeight = 12;
        MenuHandler.yOffset = 0;
        MenuHandler.xOffset = 3;
        MenuHandler.font = smaller;
        DrawBorders(0, TopSpace, 20, Bitmap.MaxHeight - TopSpace);
      } else {
        MenuHandler.DisplayXoffset = 0;
        MenuHandler.DisplayYoffset = 0;
        MenuHandler.rowHeight = 18;
        MenuHandler.yOffset = 2;
        MenuHandler.xOffset = 7;
        MenuHandler.font = Program.fontNinaB;
      }
      MenuHandler.Refresh();
      return true;
    }

    private static void DrawBorders(int x, int y, int w, int h) {
      MenuHandler.Display.Clear();
      MenuHandler.Display.DrawRectangle(Color.White, 0, x, y, w, h, 0, 0, Color.White, x, y, Color.Black, x + w, y + h, 255);
      DrawTime();
      MenuHandler.Display.Flush();
    }
  }
}
