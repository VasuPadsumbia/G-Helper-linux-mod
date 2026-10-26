using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace GHelper.Linux.Overlay;

/// <summary>
/// mod: borderless, always-on-top, non-activating window that hosts
/// <see cref="OverlayView"/>. On X11 (and XWayland, which is how the app runs
/// on GNOME / KDE Wayland) its input region is cleared with the XShape
/// extension, so clicks pass through to the game underneath.
/// </summary>
public sealed class OverlayWindow : Window
{
    public OverlayView View { get; } = new();

    public OverlayWindow()
    {
        Title = "G-Helper Overlay";
        WindowDecorations = WindowDecorations.None;
        Topmost = true;
        ShowInTaskbar = false;
        ShowActivated = false;
        Focusable = false;
        CanResize = false;
        SizeToContent = SizeToContent.WidthAndHeight;
        Background = Brushes.Transparent;
        TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
        Content = View;

        Opened += (_, _) =>
        {
            MakeClickThrough();
            PlaceInCorner();
        };
        View.LayoutUpdated += (_, _) => PlaceInCorner();
    }

    /// <summary>Move to the configured corner of the primary screen.</summary>
    public void PlaceInCorner()
    {
        var screen = Screens.Primary ?? Screens.All.FirstOrDefault();
        if (screen == null)
            return;
        var area = screen.Bounds;
        double scaling = screen.Scaling;
        int w = (int)Math.Ceiling(Bounds.Width * scaling);
        int h = (int)Math.Ceiling(Bounds.Height * scaling);
        int ox = (int)(OverlaySettings.OffsetX * scaling);
        int oy = (int)(OverlaySettings.OffsetY * scaling);
        int anchor = OverlaySettings.Anchor;
        int x = (anchor & 1) != 0 ? area.Right - w - ox : area.X + ox;
        int y = (anchor & 2) != 0 ? area.Bottom - h - oy : area.Y + oy;
        var target = new PixelPoint(x, y);
        if (Position != target)
            Position = target;
    }

    private void MakeClickThrough()
    {
        var handle = TryGetPlatformHandle();
        if (handle == null || handle.HandleDescriptor != "XID")
        {
            Helpers.Logger.WriteLine($"Overlay: click-through needs an X11 window (got {handle?.HandleDescriptor ?? "none"})");
            return;
        }

        nint display = nint.Zero;
        try
        {
            display = XOpenDisplay(null);
            if (display == nint.Zero)
                return;
            // Empty input shape: the window receives no pointer events.
            XShapeCombineRectangles(display, handle.Handle, ShapeInput, 0, 0, nint.Zero, 0, ShapeSet, Unsorted);
            XFlush(display);
            Helpers.Logger.WriteLine("Overlay: click-through enabled (XShape input region cleared)");
        }
        catch (Exception ex)
        {
            Helpers.Logger.WriteLine($"Overlay: click-through failed: {ex.Message}");
        }
        finally
        {
            if (display != nint.Zero)
                XCloseDisplay(display);
        }
    }

    private const int ShapeInput = 2;
    private const int ShapeSet = 0;
    private const int Unsorted = 0;

    [DllImport("libX11.so.6")]
    private static extern nint XOpenDisplay(string? name);

    [DllImport("libX11.so.6")]
    private static extern int XCloseDisplay(nint display);

    [DllImport("libX11.so.6")]
    private static extern int XFlush(nint display);

    [DllImport("libXext.so.6")]
    private static extern void XShapeCombineRectangles(nint display, nint window, int destKind,
        int xOff, int yOff, nint rectangles, int count, int op, int ordering);
}
