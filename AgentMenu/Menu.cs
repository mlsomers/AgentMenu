using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;

namespace AgentMenu {

  /// <summary>
  /// A menu contains a number of menu items. A Title is optional.
  /// </summary>
  public class Menu:MenuItem {

    public Menu():base() {}

    public Menu(OnSelected selectEvent) : base(selectEvent) {}

    public Menu(string text) : base(text) {}

    public Menu(string text, OnSelected childSelectEvent): base(text) {
      ChildSelected += childSelectEvent;
    }

    /// <summary>
    /// Set a title so the user will know the context of this menu
    /// Leave empty for no title
    /// </summary>
    public string Title;

    /// <summary>
    /// List of menu items
    /// </summary>
    public MenuItem[] Items = new MenuItem[0];

    internal int selectedIndex = 0;

    /// <summary>
    /// Returns the currently selected item
    /// </summary>
    public MenuItem SelectedItem {
      get { 
        if(selectedIndex>=0 && selectedIndex<Items.Length) return Items[selectedIndex];
        return null;
      }
    }

    /// <summary>
    /// Handles the selection of a child menuitem, should return false if the event should not handle menu navigation
    /// </summary>
    public event OnSelected ChildSelected;

    internal bool DoChildSelected(MenuItem selected) {
      if (!ReferenceEquals(ChildSelected, null)) return ChildSelected(selected);
      return true;
    }

    /// <summary>
    /// Hooks up parent references recursively
    /// </summary>
    public void Init(Menu parent = null) {
      this.Parent = parent;
      for (int t = Items.Length - 1; t >= 0; t--) {
        Items[t].Parent = this;
        Menu sub = Items[t].SubMenu;
        if (!ReferenceEquals(sub, null)) sub.Init(this);
      }
    }

    /// <summary>
    /// Draws the menu on the screen
    /// </summary>
    /// <param name="display">The display bitmap</param>
    /// <param name="font">A font</param>
    public virtual void Render(Bitmap display, Font font) {
      display.Clear();
      int maxH = Bitmap.MaxHeight;
      int top = 0;
      bool drawTitle = (!ReferenceEquals(Title, null) && (Title != string.Empty));

      if (drawTitle) {
        maxH -= MenuHandler.rowHeight;
        top = MenuHandler.rowHeight;
      }

      int ymid = top + (maxH / 2) - (MenuHandler.rowHeight / 2);

      if(Items.Length>0){
        // Draw selected item
        Items[selectedIndex].Render(display, font, ymid);
        //display.DrawText(Items[selectedIndex].Text, font, Color.White, MenuHandler.xOffset, ymid + MenuHandler.yOffset);

        // Draw items above
        int i = selectedIndex - 1;
        int yPos = ymid - MenuHandler.rowHeight;
        while (i >= 0 && yPos > 0) {
          Items[i].Render(display, font, yPos);
          i--;
          yPos -= MenuHandler.rowHeight;
        }

        // Draw items below
        i = selectedIndex + 1;
        yPos = ymid + MenuHandler.rowHeight;
        while (i < Items.Length && yPos < Bitmap.MaxHeight) {
          Items[i].Render(display, font, yPos);
          i++;
          yPos += MenuHandler.rowHeight;
        }
      }

      DrawCaret(display, ymid);

      if(drawTitle) DrawTitle(Title, display, font);
      display.Flush();
    }

    /// <summary>
    /// Draws the selection cursor or indicator, override this to make an arrow or some other fancy effect
    /// Note that this will be drawn AFTER the menu-item has been drawn.
    /// </summary>
    public virtual void DrawCaret(Bitmap display, int yOffset) {
      display.DrawRectangle(Color.White, 0, 0, yOffset, Bitmap.MaxWidth, MenuHandler.rowHeight, 3, 3, Color.White, 0, 0, Color.White, Bitmap.MaxWidth, MenuHandler.rowHeight, 0);
    }

    /// <summary>
    /// Draws the titlebar at the top of the screen
    /// </summary>
    /// <remarks>Made it static so it can be called for different screens as well, but maybe virtual would be better in some cases</remarks>
    public static void DrawTitle(string title, Bitmap display, Font font) {
      display.DrawRectangle(Color.White, 1, 0, 0, Bitmap.MaxWidth, MenuHandler.rowHeight, 0, 0, Color.White, 0, 0, Color.White, Bitmap.MaxWidth, MenuHandler.rowHeight, 255);
      display.DrawText(title, font, Color.Black, MenuHandler.xOffset, MenuHandler.yOffset);
    }

    /// <summary>
    /// When debugging, this will show more info than only the type
    /// </summary>
    public override string ToString() {
      return string.Concat(base.ToString(), " [", Items.Length, "] ", Title);
    }
  }

  
}
