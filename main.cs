using System;
using Cerf.CWL;

class Program {
  public static void Main (string[] args) {
    View view = new View();
    view.width = 10;
    view.height = 10;
    view.scroll = 0;
    view.lines = new Line[4] {
      new Line(0, 2, "left"),
      new Line(100, -2, "right"),
      new Line(0, 2, "top"),
      new Line(100, -2, "bottom")
    };
    view.elements = new Element[1] {
      new Element("top", "bottom", "left", "right", new Layer[] {new Layer("yeet",new Color(255,255,255),new Background(new Color[0],BackgroundType.Solid),Clip.Default,0)})
    };
    RenderSurface surface = new TestRenderSurface();
    view.Render(ref surface);
  }
}