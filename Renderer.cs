using System;

public class View {
  public int width;
  public int height;
  public float scroll;

  public Line[] lines = new Line[0];
  public Element[] elements = new Element[0];
  public void Render(ref RenderSurface surface) {
    surface.Initialize(width,height);
    double screenRelativePx = MathF.Min((float)width,(float)height) / 10d;
    foreach (Element element in elements) {
      Line top = FindLineByName(element.top);
      Line bottom = FindLineByName(element.bottom);
      Line left = FindLineByName(element.left);
      Line right = FindLineByName(element.right);
      double topPos = ((top.percent * screenRelativePx / 100) * height) + top.offset;
      double bottomPos = ((bottom.percent * screenRelativePx / 100) * height) + bottom.offset;
      double leftPos = ((left.percent * screenRelativePx / 100) * width) + left.offset;
      double rightPos = ((right.percent * screenRelativePx / 100) * width) + right.offset;
      
      foreach (Layer layer in element.layers) {
        Color[] colors = new Color[(int)(rightPos-leftPos)*(int)(bottomPos-topPos)];
        for (int i = 0; i < (int)(rightPos-leftPos)*(int)(bottomPos-topPos); i++) {
          Color color = null;
          switch (layer.bg.bgType) {
            case BackgroundType.Solid:
              color = layer.color;
              break;
          }
          switch (layer.clip) {
            case Clip.Default:
              colors[i] = color;
              break;
          }
        }
        surface.SetPixels((int)MathF.Round((float)leftPos),(int)MathF.Round((float)(topPos - scroll)) ,(int)MathF.Round((float)(rightPos-leftPos)),(int)MathF.Round((float)(bottomPos-topPos)),colors); 
      }
    }
    surface.Done();
  }
  public Line FindLineByName(string name) {
  foreach (Line line in lines) {
      if (line.name == name) {
        return line;
      }
    }
    Console.WriteLine("PROBLEM: UNKNOWN LINE NAME");
    return null;
}
}

public class Line {
  public double percent;
  public int offset; 
  public string name;

  public Line(double percent, int offset, string name) {
    this.percent = percent;
    this.offset = offset;
    this.name = name;
  }
}

public class Element {
  public string top;
  public string bottom;
  public string left;
  public string right;
  public Layer[] layers;

  public Element(string top, string bottom, string left, string right, Layer[] layers) {
    this.top = top;
    this.bottom = bottom;
    this.left = left;
    this.right = right;
    this.layers = layers;
  }
}

public class Layer {
  public string name;
  public Color color;
  public Background bg;
  public Clip clip;
  public int blur; 

  public Layer(string name, Color color, Background bg, Clip clip, int blur) {
    this.name = name;
    this.color = color;
    this.bg = bg;
    this.clip = clip;
    this.blur = blur;
  }
}

public enum Clip {
  Default,
  Text
}

public enum BackgroundType {
  Solid,
  LinearGradient,
  RadialGradient,
  AngularGradient,
  MeshGradient
}

public class Background {
  public Color[] colors;
  public BackgroundType bgType;

  public Background(Color[] colors, BackgroundType bgType) {
    this.colors = colors;
    this.bgType = bgType;
  }
}

public interface RenderSurface {
  public void SetPixels(int x, int y, int w, int h, Color[] cols);
  public void Initialize(int w, int h);
  public void Done();
}

public class TestRenderSurface : RenderSurface {
  public int w;
  public int h;
  public Color[] colors;
  public void SetPixels(int x, int y, int w, int h, Color[] cols) {
    for (int i = 0; i < cols.Length; i++) {
      int placeAtX = x + (i % w);
      int placeAtY = y + (int)MathF.Floor(i / w);
      try {
        colors[placeAtX + (placeAtY * this.w)] = cols[i];
      } catch {

      }
    }
  }
  public void Initialize(int wX, int hX) {
    w = wX;
    h = hX;
    colors = new Color[wX * hX];
    for (int i = 0; i < wX * hX; i++) {
      colors[i] = new Color(0,0,0);
    }
  }

  public void Done() {
    int x = 0;
    for (int i = 0; i < colors.Length; i++) {
      x++;
      ConsoleColor col = ConsoleColor.Black;
      Color c = colors[i];
      if (c.r > 128) {
        col = ConsoleColor.Red;
      } else if (c.g > 128) {
        col = ConsoleColor.Green;
      } else if (c.b > 128) {
        col = ConsoleColor.Blue;
      } else if (c.r + c.g + c.b > 128 * 3) {
        col = ConsoleColor.White;
      }
      Console.BackgroundColor = col;
      Console.ForegroundColor = col;
      Console.Write("0");
      if (x >= w) {
        Console.ResetColor();
        Console.WriteLine();
        x = 0;
      }
    }
    Console.ResetColor();
    Console.WriteLine();
  }
}

public class Color {
  public double r;
  public double g;
  public double b;

  public Color(double r, double g, double b) {
    this.r = r;
    this.g = g;
    this.b = b;
  }
}