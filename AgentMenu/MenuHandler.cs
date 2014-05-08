using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace AgentMenu {
  /// <summary>
  /// The main handling engine for menus, it will hook up events for the navigation buttons and handle them.
  /// When Focus is set to false, the button events can be obtained by subscribing to the ButtonPress event.
  /// </summary>
  public class MenuHandler {

    private static Menu currentMenu;
    public static Bitmap Display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);
    public static int DisplayXoffset = 0;
    public static int DisplayYoffset = 0;
    public static Font font;

    public static int rowHeight=18;
    public static int xOffset = 7;
    public static int yOffset = 2;

    public static bool Focus = false;

    public delegate void OnButtonPress(Button button);

    /// <summary>
    /// Will receive events for buttons while menu Focus is set to False.
    /// Buttons that can be expected are: VK_SELECT, VK_MENU, VK_UP, VK_DOWN
    /// </summary>
    public static event OnButtonPress ButtonPress;

    /// <summary>
    /// Will do some boilerplate work like linking parent references and some general "tweaky" settings
    /// It will also call SetCurrentMenu with the root of the menu-tree
    /// </summary>
    public static void InitMenu(Font font, Menu menu, int rowHeight = 18, int xOffset = 7, int yOffset = 2) {
      MenuHandler.font = font;
      MenuHandler.rowHeight = rowHeight;
      MenuHandler.xOffset = xOffset;
      MenuHandler.yOffset = yOffset;
      menu.Init();
      SetCurrentMenu(menu);
    }

    /// <summary>
    /// Sets the current menu, and renders it to the screen.
    /// </summary>
    public static void SetCurrentMenu(Menu menu) {
      currentMenu = menu;
      Refresh();
      Focus = true;
    }

    /// <summary>
    /// Refreshes the screen
    /// </summary>
    public static void Refresh() {
      currentMenu.Render(Display, font);
      Display.Flush(DisplayXoffset, DisplayYoffset, Display.Width - DisplayXoffset, Display.Height - DisplayYoffset);
    }

    static void btnDown(uint data1, uint data2, DateTime time) {
      if (!Focus) { DoEvent(Button.VK_DOWN); return; }
      currentMenu.selectedIndex++;
      if (currentMenu.Items.Length <= currentMenu.selectedIndex) currentMenu.selectedIndex = currentMenu.Items.Length - 1;
      if (currentMenu.selectedIndex >= 0) Refresh();
    }

    static void btnUp(uint data1, uint data2, DateTime time) {
      if (!Focus) { DoEvent(Button.VK_UP); return; }
      currentMenu.selectedIndex--;
      if (currentMenu.selectedIndex<0) currentMenu.selectedIndex = 0;
      if (currentMenu.selectedIndex >= 0) Refresh();
    }

    static void btnBack(uint data1, uint data2, DateTime time) {
      if (!Focus) { DoEvent(Button.VK_MENU); return; }
      Menu par = currentMenu.Parent as Menu;
      if (ReferenceEquals(par, null)) return;
      SetCurrentMenu(par);
    }

    static void btnSelect(uint data1, uint data2, DateTime time) {
      if (!Focus) { DoEvent(Button.VK_SELECT); return; }
      MenuItem selected = currentMenu.SelectedItem;
      if (ReferenceEquals(selected, null)) return;
      selected.Select();
    }

    #region private housekeeping

    /// <summary>
    /// Subscribes to the navigation button events
    /// </summary>
    static MenuHandler() {
      MapButton(Button.VK_SELECT, btnSelect);
      MapButton(Button.VK_MENU, btnBack);
      MapButton(Button.VK_UP, btnUp);
      MapButton(Button.VK_DOWN, btnDown);
    }

    /// <summary>
    /// Hooks up to a button-press event
    /// </summary>
    private static void MapButton(Button button, NativeEventHandler handler) {
      InterruptPort _button = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(button), true, Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);
      _button.OnInterrupt += handler;
    }

    /// <summary>
    /// Checks for subscribers and executes if needed
    /// </summary>
    private static void DoEvent(Button button) {
      if (!ReferenceEquals(ButtonPress, null)) ButtonPress(button);
    }

    #endregion
  }
}
