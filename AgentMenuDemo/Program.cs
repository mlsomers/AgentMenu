using AgentMenu;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System;
using System.Threading;

namespace AgentMenuDemo {
  public class Program {
    public static Font fontNinaB;
    public static Menu menu;

    public static void Main() {
      fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);

      menu = new Menu() {
        Title = "Menu Demo",
        Items = new MenuItem[]{
          new MenuItem("Item 1"),
          new Menu("Item 2 (children)", new MenuItem.OnSelected(handleAnyChild)){ 
            Title = "Item 2 Sub menu",
            Items=new MenuItem[]{
              new MenuItem("Hello"),
              new MenuItem("Bye"),
            }
          },
          new MenuItem("Item 3"),
          new MenuItem("Item 4 (log)", new MenuItem.OnSelected(doItem4)),
          new MenuItem("Item 5"),
          new CheckedMenuItem(){Text="Item 6"},
          new MenuItem("Item 7 (custom)", new MenuItem.OnSelected(doCustomStuff)),
          new MenuItem("Item 8 (mini)", new MenuItem.OnSelected(SmallMenu.Toggle)),
        }
      };

      MenuHandler.InitMenu(fontNinaB, menu);
      Thread.Sleep(Timeout.Infinite);
    }

    private static bool handleAnyChild(MenuItem sender) {
      Debug.Print(string.Concat("Child item \"", sender.Text, "\" was selected"));
      return true;
    }

    private static bool doItem4(MenuItem sender) {
      Debug.Print(sender.Text);
      return true;
    }

    #region Custom stuff for menu item 7

    private static bool doCustomStuff(MenuItem sender) {
      MenuHandler.Focus = false; // Enables the events handled below
      MenuHandler.Display.Clear();
      MenuHandler.Display.DrawText("View debug log", MenuHandler.font, Color.White, 5, 5);
      MenuHandler.Display.DrawText("for events", MenuHandler.font, Color.White, 5, 25);
      MenuHandler.Display.DrawText("Menu button", MenuHandler.font, Color.White, 5, 55);
      MenuHandler.Display.DrawText("returns to menu", MenuHandler.font, Color.White, 5, 75);
      MenuHandler.Display.Flush();
      MenuHandler.ButtonPress += MenuHandler_ButtonPress; // *1  Will only fire when menu focus is false!
      return false;
    }

    static void MenuHandler_ButtonPress(Button button) {
      switch (button) {
        case Button.VK_SELECT: Debug.Print("Select"); break;
        case Button.VK_UP: Debug.Print("Up"); break;
        case Button.VK_DOWN: Debug.Print("Down"); break;
        case Button.VK_MENU: Debug.Print("Menu (exit)");
          MenuHandler.Focus = true;
          MenuHandler.Refresh();
          MenuHandler.ButtonPress -= MenuHandler_ButtonPress; // *1  Unsubscribe again, allowing multiple custom handlers
          break;
        default: Debug.Print("Unknown " + button.ToString()); break;
      }
    }

    /* Note that since we only have one custom menuitem, dynamically subscribing and unsubscribing
     * is not really needed, so the rows with the *1 comments can be removed and subscribing could be
     * done in Main() to save some (minimal) overhead. The events will only fire when MenuHandler.Focus
     * is false.
     */

    #endregion
  }
}
