using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using GHelper.Linux.Helpers;
using GHelper.Linux.I18n;
using GHelper.Linux.Overlay;

namespace GHelper.Linux.UI.Views;

/// <summary>
/// mod: overlay settings, after G-Helper for Windows' OverlayConfig. Every change
/// is saved immediately and pushed to the running overlay.
/// </summary>
public partial class OverlayConfigWindow : Window
{
    private static readonly Color ColorCustom = Color.Parse("#FF8000");

    private readonly (Button Button, OverlayMode Mode, Color Color)[] _modeButtons;
    private readonly (CheckBox Box, string Key)[] _blocks;
    private OverlayMode _mode;
    private bool _updating;

    public OverlayConfigWindow()
    {
        InitializeComponent();

        _modeButtons =
        [
            (buttonLight, OverlayMode.Light, MainWindow.ColorEco),
            (buttonDefault, OverlayMode.Default, MainWindow.ColorStandard),
            (buttonFull, OverlayMode.Full, MainWindow.ColorTurbo),
            (buttonComplete, OverlayMode.Complete, ColorCustom),
        ];
        _blocks =
        [
            (checkFps, "overlay_show_fps"), (checkTemp, "overlay_show_temp"),
            (checkFans, "overlay_show_fans"), (checkChart, "overlay_show_chart"),
            (checkPower, "overlay_show_power"), (checkUsage, "overlay_show_usage"),
            (checkRam, "overlay_show_ram"), (checkLabels, "overlay_names"),
        ];

        ApplyLabels();
        LoadAll();

        checkEnable.IsCheckedChanged += (_, _) =>
        {
            if (!_updating && checkEnable.IsChecked != OverlaySettings.Enabled)
                OverlayControl.SetEnabled(checkEnable.IsChecked == true);
        };
        checkGameOnly.IsCheckedChanged += (_, _) =>
        {
            if (_updating)
                return;
            OverlaySettings.GameOnly = checkGameOnly.IsChecked == true;
            OverlayControl.RefreshSettings();
        };
        foreach (var (button, mode, _) in _modeButtons)
            button.Click += (_, _) => SelectMode(mode);
        foreach (var (box, key) in _blocks)
            box.IsCheckedChanged += (_, _) => BlockChanged(box, key);
        checkBattery.IsCheckedChanged += (_, _) => BatteryChanged();
        sliderScale.ValueChanged += (_, _) => SliderChanged("overlay_scale_percent", sliderScale, labelScale);
        sliderAlpha.ValueChanged += (_, _) => SliderChanged("overlay_alpha", sliderAlpha, labelAlpha);
        comboPosition.SelectionChanged += (_, _) =>
        {
            if (_updating || comboPosition.SelectedIndex < 0)
                return;
            AppConfig.Set("overlay_anchor", comboPosition.SelectedIndex);
            OverlayControl.RefreshSettings();
        };
        buttonCpuColor.Click += (_, _) => PickColor("overlay_color_cpu", OverlaySettings.CpuColor);
        buttonGpuColor.Click += (_, _) => PickColor("overlay_color_gpu", OverlaySettings.GpuColor);
        buttonReset.Click += ButtonReset_Click;
        OverlayControl.EnabledChanged += SyncEnabled;
        Closed += (_, _) => OverlayControl.EnabledChanged -= SyncEnabled;
    }

    private void ApplyLabels()
    {
        Title = Labels.Get("overlay");
        checkEnable.Content = Labels.Get("overlay");
        checkGameOnly.Content = Labels.Get("overlay_game_only");
        labelLight.Text = Labels.Get("overlay_mode_light");
        labelDefault.Text = Labels.Get("overlay_mode_default");
        labelFull.Text = Labels.Get("overlay_mode_full");
        labelComplete.Text = Labels.Get("overlay_mode_complete");
        checkFps.Content = Labels.Get("overlay_fps");
        checkTemp.Content = Labels.Get("overlay_temperatures");
        checkFans.Content = Labels.Get("overlay_fan_speed");
        checkChart.Content = Labels.Get("overlay_chart");
        checkPower.Content = Labels.Get("overlay_power");
        checkUsage.Content = Labels.Get("overlay_load");
        checkRam.Content = Labels.Get("overlay_ram");
        checkBattery.Content = Labels.Get("overlay_battery");
        checkLabels.Content = Labels.Get("overlay_labels");
        labelFpsHint.Text = Labels.Get("overlay_fps_hint");
        labelSizeTitle.Text = Labels.Get("overlay_size");
        labelAlphaTitle.Text = Labels.Get("overlay_transparency");
        labelPositionTitle.Text = Labels.Get("overlay_position");
        labelCpuColor.Text = Labels.Get("overlay_cpu_color");
        labelGpuColor.Text = Labels.Get("overlay_gpu_color");
        labelReset.Text = Labels.Get("overlay_reset");
        labelHotkeys.Text = Labels.Get("overlay_hotkeys");
        comboPosition.ItemsSource = new[]
        {
            Labels.Get("overlay_top_left"), Labels.Get("overlay_top_right"),
            Labels.Get("overlay_bottom_left"), Labels.Get("overlay_bottom_right"),
        };
    }

    private void LoadAll()
    {
        _updating = true;
        checkEnable.IsChecked = OverlaySettings.Enabled;
        checkGameOnly.IsChecked = OverlaySettings.GameOnly;
        _mode = OverlaySettings.Mode;
        sliderScale.Value = OverlaySettings.ScalePercent;
        labelScale.Text = $"{OverlaySettings.ScalePercent}%";
        sliderAlpha.Value = OverlaySettings.Alpha;
        labelAlpha.Text = AlphaText(OverlaySettings.Alpha);
        comboPosition.SelectedIndex = OverlaySettings.Anchor;
        swatchCpu.Background = new SolidColorBrush(OverlaySettings.CpuColor);
        swatchGpu.Background = new SolidColorBrush(OverlaySettings.GpuColor);
        _updating = false;
        ShowBlocks();
    }

    private void SyncEnabled()
    {
        _updating = true;
        checkEnable.IsChecked = OverlaySettings.Enabled;
        _updating = false;
    }

    private void ShowBlocks()
    {
        _updating = true;
        foreach (var (button, mode, color) in _modeButtons)
            MainWindow.SetButtonActive(button, mode == _mode, color);
        foreach (var (box, key) in _blocks)
            box.IsChecked = OverlaySettings.BlockShown(key, _mode);
        // FPS needs MangoHud in games; the native overlay can't measure it.
        checkFps.IsChecked = false;
        int battery = OverlaySettings.BatteryState(_mode);
        checkBattery.IsChecked = battery < 0 ? null : battery == 1;
        _updating = false;
    }

    private void SelectMode(OverlayMode mode)
    {
        _mode = mode;
        OverlaySettings.Mode = mode;
        ShowBlocks();
        OverlayControl.RefreshSettings();
    }

    private void BlockChanged(CheckBox box, string key)
    {
        if (_updating)
            return;
        AppConfig.Set(OverlaySettings.ModeKey(key, (int)_mode), box.IsChecked == true ? 1 : 0);
        OverlayControl.RefreshSettings();
    }

    private void BatteryChanged()
    {
        if (_updating)
            return;
        int state = checkBattery.IsChecked switch { null => -1, true => 1, false => 0 };
        AppConfig.Set(OverlaySettings.ModeKey("overlay_show_battery", (int)_mode), state);
        OverlayControl.RefreshSettings();
    }

    private void SliderChanged(string key, Slider slider, TextBlock label)
    {
        int v = (int)Math.Round(slider.Value);
        label.Text = key == "overlay_alpha" ? AlphaText(v) : $"{v}%";
        if (_updating)
            return;
        AppConfig.Set(key, v);
        OverlayControl.RefreshSettings();
    }

    private static string AlphaText(int alpha) => $"{Math.Round(alpha * 100.0 / 255)}%";

    private void PickColor(string key, Color initial)
    {
        ColorPicker.Show(this, initial.R, initial.G, initial.B, (r, g, b) =>
        {
            AppConfig.Set(key, OverlaySettings.ToHex(Color.FromRgb(r, g, b)));
            swatchCpu.Background = new SolidColorBrush(OverlaySettings.CpuColor);
            swatchGpu.Background = new SolidColorBrush(OverlaySettings.GpuColor);
            OverlayControl.RefreshSettings();
        });
    }

    private void ButtonReset_Click(object? sender, RoutedEventArgs e)
    {
        OverlaySettings.Reset();
        LoadAll();
        OverlayControl.RefreshSettings();
    }
}
