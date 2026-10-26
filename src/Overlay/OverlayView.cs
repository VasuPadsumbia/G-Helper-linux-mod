using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace GHelper.Linux.Overlay;

/// <summary>
/// mod: draws the overlay as a small table, after G-Helper for Windows'
/// HardwareOverlay: a GPU row and a CPU row in their own colours, then optional
/// fan, power chart, watts, load and memory columns, and a battery row.
/// </summary>
public sealed class OverlayView : Control
{
    private const double BaseFontSize = 13;
    private const double BasePad = 6;
    private const double BaseGap = 8;
    private const double BaseChartWidth = 46;
    private const int ChartPoints = 30;
    private const double CornerRadius = 3;

    private static readonly Typeface Mono = new(new FontFamily("DejaVu Sans Mono, Liberation Mono, Noto Sans Mono, monospace"));
    private static readonly IBrush BatteryBrush = new SolidColorBrush(Color.FromRgb(235, 235, 235));

    private readonly Queue<double> _cpuHistory = new();
    private readonly Queue<double> _gpuHistory = new();

    private OverlaySample? _sample;
    private OverlayMode _mode;
    private double _scale = 1;
    private IBrush _cpuBrush = new SolidColorBrush(OverlaySettings.DefaultCpuColor);
    private IBrush _gpuBrush = new SolidColorBrush(OverlaySettings.DefaultGpuColor);
    private IBrush _bgBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
    private bool _showTemp, _showFans, _showChart, _showPower, _showUsage, _showRam, _showNames;
    private int _batteryState;

    public OverlayView() => ReloadSettings();

    public void ReloadSettings()
    {
        _mode = OverlaySettings.Mode;
        _scale = OverlaySettings.ScalePercent / 100.0;
        _cpuBrush = new SolidColorBrush(OverlaySettings.CpuColor);
        _gpuBrush = new SolidColorBrush(OverlaySettings.GpuColor);
        _bgBrush = new SolidColorBrush(Color.FromArgb((byte)OverlaySettings.Alpha, 0, 0, 0));
        _showTemp = OverlaySettings.BlockShown("overlay_show_temp", _mode);
        _showFans = OverlaySettings.BlockShown("overlay_show_fans", _mode);
        _showChart = OverlaySettings.BlockShown("overlay_show_chart", _mode);
        _showPower = OverlaySettings.BlockShown("overlay_show_power", _mode);
        _showUsage = OverlaySettings.BlockShown("overlay_show_usage", _mode);
        _showRam = OverlaySettings.BlockShown("overlay_show_ram", _mode);
        _showNames = OverlaySettings.BlockShown("overlay_names", _mode);
        _batteryState = OverlaySettings.BatteryState(_mode);
        InvalidateMeasure();
        InvalidateVisual();
    }

    public void Update(OverlaySample sample)
    {
        _sample = sample;
        Push(_cpuHistory, sample.CpuWatts ?? 0);
        Push(_gpuHistory, sample.GpuWatts ?? 0);
        InvalidateMeasure();
        InvalidateVisual();
    }

    private static void Push(Queue<double> q, double v)
    {
        q.Enqueue(v);
        while (q.Count > ChartPoints)
            q.Dequeue();
    }

    // Layout model: rows of cells; each cell is text or a chart.

    private sealed record Cell(string? Text, Queue<double>? Chart = null, double Bar = -1);

    private List<(IBrush Brush, List<Cell> Cells)> BuildRows()
    {
        var rows = new List<(IBrush, List<Cell>)>();
        var s = _sample;
        if (s == null)
            return rows;

        bool gpuPresent = s.GpuName != null || s.GpuTemp > 0;
        if (gpuPresent)
            rows.Add((_gpuBrush, BuildDeviceRow(_showNames ? s.GpuName ?? "GPU" : "GPU",
                s.GpuTemp, s.GpuFanRpm, _gpuHistory, s.GpuWatts, s.GpuLoad, ram: null)));
        rows.Add((_cpuBrush, BuildDeviceRow(_showNames ? s.CpuName : "CPU",
            s.CpuTemp, s.CpuFanRpm, _cpuHistory, s.CpuWatts, s.CpuLoad, ram: s)));

        bool showBattery = _batteryState == 1 || (_batteryState == -1 && s.OnBattery);
        if (showBattery && s.BatteryPercent >= 0)
        {
            double w = s.OnBattery ? -Math.Abs(s.BatteryWatts) : Math.Abs(s.BatteryWatts);
            rows.Add((BatteryBrush, new List<Cell>
            {
                new("BAT"),
                new($"{s.BatteryPercent}%"),
                new(w.ToString("+0.0;-0.0;0.0", CultureInfo.InvariantCulture) + "W"),
            }));
        }
        return rows;
    }

    private List<Cell> BuildDeviceRow(string label, int temp, int fanRpm, Queue<double> history,
        double? watts, int load, OverlaySample? ram)
    {
        var cells = new List<Cell>();
        string head = label + (_mode == OverlayMode.Light ? ":" : "");
        if (_showTemp)
            head += " " + (temp > 0 ? $"{temp}°" : "--");
        cells.Add(new Cell(head));

        if (_showFans)
            cells.Add(new Cell(fanRpm > 0 ? $"{fanRpm}" : "0"));
        if (_showChart)
            cells.Add(new Cell(null, history));
        if (_showPower)
            cells.Add(new Cell(watts.HasValue ? watts.Value.ToString("0.0", CultureInfo.InvariantCulture) + "W" : "--"));
        if (_showUsage)
            cells.Add(new Cell(load >= 0 ? $"{load}%" : "--", Bar: load >= 0 ? load / 100.0 : 0));
        if (_showRam)
            cells.Add(ram != null
                ? new Cell(ram.RamUsedGb.ToString("0.0", CultureInfo.InvariantCulture) + "GB", Bar: ram.RamPercent / 100.0)
                : new Cell(""));
        return cells;
    }

    private FormattedText Text(string text, IBrush brush) =>
        new(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Mono, BaseFontSize * _scale, brush);

    private (List<(IBrush Brush, List<Cell> Cells)> rows, double[] colWidths, double rowHeight) Measure()
    {
        var rows = BuildRows();
        int cols = rows.Count == 0 ? 0 : rows.Max(r => r.Cells.Count);
        var widths = new double[cols];
        double rowHeight = Text("0", Brushes.White).Height;
        double barExtra = (BaseGap / 2 + 4) * _scale;
        foreach (var (brush, cells) in rows)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                var c = cells[i];
                double w = c.Chart != null ? BaseChartWidth * _scale
                    : Text(c.Text ?? "", brush).Width + (c.Bar >= 0 ? barExtra : 0);
                widths[i] = Math.Max(widths[i], w);
            }
        }
        return (rows, widths, rowHeight);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var (rows, widths, rowHeight) = Measure();
        if (rows.Count == 0)
            return new Size(1, 1);
        double pad = BasePad * _scale;
        double width = pad * 2 + widths.Sum() + BaseGap * _scale * Math.Max(0, widths.Length - 1);
        double height = pad * 2 + rowHeight * rows.Count;
        return new Size(Math.Ceiling(width), Math.Ceiling(height));
    }

    public override void Render(DrawingContext ctx)
    {
        var (rows, widths, rowHeight) = Measure();
        if (rows.Count == 0)
            return;

        ctx.DrawRectangle(_bgBrush, null, new Rect(Bounds.Size), CornerRadius * _scale, CornerRadius * _scale);

        double pad = BasePad * _scale;
        double gap = BaseGap * _scale;
        double y = pad;
        foreach (var (brush, cells) in rows)
        {
            double x = pad;
            for (int i = 0; i < cells.Count; i++)
            {
                var c = cells[i];
                if (c.Chart != null)
                    DrawChart(ctx, c.Chart, brush, new Rect(x, y + 2 * _scale, widths[i], rowHeight - 4 * _scale));
                else
                {
                    var t = Text(c.Text ?? "", brush);
                    ctx.DrawText(t, new Point(x, y));
                    if (c.Bar >= 0)
                        DrawBar(ctx, brush, c.Bar, new Rect(x + widths[i] - 4 * _scale, y + 2 * _scale, 4 * _scale, rowHeight - 4 * _scale));
                }
                x += widths[i] + gap;
            }
            y += rowHeight;
        }
    }

    private static void DrawBar(DrawingContext ctx, IBrush brush, double fraction, Rect r)
    {
        ctx.DrawRectangle(new SolidColorBrush(Color.FromArgb(70, 255, 255, 255)), null, r);
        double h = r.Height * Math.Clamp(fraction, 0, 1);
        ctx.DrawRectangle(brush, null, new Rect(r.X, r.Bottom - h, r.Width, h));
    }

    private static void DrawChart(DrawingContext ctx, Queue<double> history, IBrush brush, Rect r)
    {
        if (history.Count < 2)
            return;
        var values = history.ToArray();
        double max = Math.Max(1, values.Max());
        double step = r.Width / (ChartPoints - 1);
        double x0 = r.Right - step * (values.Length - 1);

        var line = new StreamGeometry();
        var fill = new StreamGeometry();
        using (var g = line.Open())
        using (var f = fill.Open())
        {
            f.BeginFigure(new Point(x0, r.Bottom), true);
            for (int i = 0; i < values.Length; i++)
            {
                var p = new Point(x0 + step * i, r.Bottom - r.Height * values[i] / max);
                if (i == 0)
                    g.BeginFigure(p, false);
                else
                    g.LineTo(p);
                f.LineTo(p);
            }
            f.LineTo(new Point(r.Right, r.Bottom));
            f.EndFigure(true);
            g.EndFigure(false);
        }

        // Fill = base colour at 1/3 brightness, alpha 128, as on Windows.
        var c = (brush as ISolidColorBrush)?.Color ?? Colors.White;
        ctx.DrawGeometry(new SolidColorBrush(Color.FromArgb(128, (byte)(c.R / 3), (byte)(c.G / 3), (byte)(c.B / 3))), null, fill);
        ctx.DrawGeometry(null, new Pen(brush, 1.5), line);
    }
}
