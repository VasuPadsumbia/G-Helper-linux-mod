using Avalonia.Threading;

namespace GHelper.Linux.Overlay;

/// <summary>
/// mod: owns the overlay window and its one-second refresh, like
/// G-Helper for Windows' Program.hardwareOverlay. All public members are
/// called on the UI thread.
/// </summary>
public static class OverlayControl
{
    private static OverlayWindow? _window;
    private static DispatcherTimer? _timer;
    private static bool _sampling;
    private static int _gameCheckTick;
    private static bool _gameRunning;

    /// <summary>Raised when the overlay is switched on or off (tray menu, buttons).</summary>
    public static event Action? EnabledChanged;

    /// <summary>Start the overlay if it was on when the app last ran.</summary>
    public static void RestoreOnStart()
    {
        if (OverlaySettings.Enabled)
            Start();
    }

    public static void Toggle() => SetEnabled(!OverlaySettings.Enabled);

    public static void SetEnabled(bool enabled)
    {
        OverlaySettings.Enabled = enabled;
        if (enabled)
            Start();
        else
            Stop();
        EnabledChanged?.Invoke();
    }

    /// <summary>Re-read settings after the settings window changed something.</summary>
    public static void RefreshSettings()
    {
        _window?.View.ReloadSettings();
        _window?.PlaceInCorner();
        _gameCheckTick = 0;
    }

    private static void Start()
    {
        if (_timer != null)
            return;
        _gameCheckTick = 0;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();
        Tick();
        Helpers.Logger.WriteLine("Overlay: started");
    }

    private static void Stop()
    {
        _timer?.Stop();
        _timer = null;
        _window?.Close();
        _window = null;
        Helpers.Logger.WriteLine("Overlay: stopped");
    }

    private static void Tick()
    {
        if (_sampling)
            return;
        _sampling = true;
        bool checkGame = OverlaySettings.GameOnly && _gameCheckTick-- <= 0;
        if (checkGame)
            _gameCheckTick = 5; // scanning /proc every 5 s is plenty

        Task.Run(() =>
        {
            var sample = OverlaySampler.Read();
            bool game = checkGame ? OverlaySampler.IsGameRunning() : _gameRunning;
            Dispatcher.UIThread.Post(() =>
            {
                _sampling = false;
                if (_timer == null)
                    return;
                _gameRunning = game;
                bool visible = !OverlaySettings.GameOnly || _gameRunning;
                if (!visible)
                {
                    _window?.Hide();
                    return;
                }
                if (_window == null)
                    _window = new OverlayWindow();
                _window.View.Update(sample);
                if (!_window.IsVisible)
                    _window.Show();
            });
        });
    }
}
