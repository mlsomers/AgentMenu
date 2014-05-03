using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;

namespace AgentMenu {
  /// <summary>
  /// Selectable Menu Item
  /// </summary>
  public class MenuItem {

    public MenuItem():base() { }

    public MenuItem(OnSelected selectEvent) : base() {
      Selected += selectEvent;
    }

    public MenuItem(string text) : base() {
      Text = text;
    }

    public MenuItem(string text, OnSelected selectEvent): this(selectEvent) {
      Text = text;
    }

    /// <summary>
    /// Will draw the menu-item.
    /// When overriding, be sure to keep yPos and MenuHandler.rowHeight in mind (prevent drawing over other stuff)
    /// </summary>
    public virtual void Render(Bitmap display, Font font, int yPos) {
      display.DrawText(Text, font, Color.White, MenuHandler.xOffset, yPos + MenuHandler.yOffset);
    }

    /// <summary>
    /// Handles the selection of a menuitem, should return false if the event should not bubble up farther
    /// </summary>
    /// <param name="sender">The menuitem that was selected</param>
    /// <returns>True to show a submenu</returns>
    public delegate bool OnSelected(MenuItem sender);

    /// <summary>
    /// Handles the selection of a menuitem, should return false if the event should not bubble up farther
    /// </summary>
    public event OnSelected Selected;

    /// <summary>
    /// The parent of this menu, Null if this is a root menu
    /// </summary>
    public Menu Parent;

    /// <summary>
    /// This may be a bit confusing, but it just checks if this is a Menu (which inherits from MenuItem) and if so, will cast itself to Menu and return that.
    /// If not a menu it returns null.
    /// </summary>
    public Menu SubMenu {
      get {
        Menu subMenu = this as Menu;
        return subMenu;
      }
    }

    /// <summary>
    /// The menu item's text as seen in the menu. On a root menu, this field will not show up (the Title will)
    /// </summary>
    public string Text;

    /// <summary>
    /// This will call events hooked up to the menu item, it's parent and handle navigation (when applicable). 
    /// Note that any event can prevent following handling by returning false.
    /// </summary>
    public virtual void Select() {
      // First call Selected on the menu item itself
      if (!ReferenceEquals(Selected, null)) if (!Selected(this)) return; // skip rest false

      // Call Selected on the menu that contains this item
      if (!ReferenceEquals(Parent, null)) if (!Parent.DoChildSelected(this)) return; // skip submenu if false

      // Default behaviur: show submenu
      Menu subMenu = SubMenu;
      if (!ReferenceEquals(subMenu, null)) {
        MenuHandler.SetCurrentMenu(subMenu);
      }
    }

    /// <summary>
    /// Shows some extra info when debugging
    /// </summary>
    public override string ToString() {
      return string.Concat(base.ToString(), " ", Text);
    }
  }
}
