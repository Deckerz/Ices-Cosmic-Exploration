using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using ICE.Scheduler;
using ICE.Ui;
using ICE.IPC;
using ICE.Scheduler.Handlers;

namespace ICE;

public sealed partial class ICE : IDalamudPlugin
{
    public static string Name => "ICE";
    public static Config C => P.Config;

    internal static ICE P = null!;
    private readonly Config Config;

    // Window's that I use, base window to the settings... need these to actually show shit 
    internal WindowSystem windowSystem;
    internal MainWindow mainWindow;
    internal SettingsWindow settingWindow;
    internal OverlayWindow overlayWindow;
    internal DebugWindow debugWindow;

    // Taskmanager from Ecommons
    internal TaskManager TaskManager;

    // Internal IPC's that I use for... well plugins. 
    internal LifestreamIPC Lifestream;
    internal NavmeshIPC Navmesh;
    internal PandoraIPC Pandora;
    internal ArtisanIPC Artisan;
    internal VislandIPC Visland;

    public ICE(IDalamudPluginInterface pi)
    {
        P = this;
        ECommonsMain.Init(pi, P, Module.DalamudReflector, ECommons.Module.ObjectFunctions);

        EzConfig.Migrate<Config>();
        Config = EzConfig.Init<Config>();

        //IPC's that are used
        TaskManager = new();
        Lifestream = new();
        Navmesh = new();
        Pandora = new();
        Artisan = new();
        Visland = new();

        // all the windows
        windowSystem = new();
        mainWindow = new();
        settingWindow = new();
        overlayWindow = new();
        debugWindow = new();

        EzCmd.Add("/icecosmic", OnCommand, """
            Open plugin interface
            - start -> starts the loops
            - stop -> stops the loops
            - clear -> clears all
            """);
        EzCmd.Add("/ice", OnCommand);
        EzCmd.Add("/IceCosmic", OnCommand);
        Init();
        Svc.Framework.Update += Tick;

        TaskManager = new(new(showDebug: true));
        Svc.PluginInterface.UiBuilder.Draw += windowSystem.Draw;
        Svc.PluginInterface.UiBuilder.OpenMainUi += () =>
        {
            mainWindow.IsOpen = true;
        };
        Svc.PluginInterface.UiBuilder.OpenConfigUi += () =>
        {
            settingWindow.IsOpen = true;
        };
        DictionaryCreation();
    }

    private void Init()
    {
        CosmicHelper.Init();
        WeatherForecastHandler.Init();
    }

    private void Tick(object _)
    {
        if (Svc.ClientState.LocalPlayer != null)
        {
            PlayerHandlers.Tick();
            SchedulerMain.Tick();
            WeatherForecastHandler.Tick();
        }
        else
        {
            PlayerHandlers.DisablePlugin();
        }
        GenericManager.Tick();
        TextAdvancedManager.Tick();
        YesAlreadyManager.Tick();
    }

    public void Dispose()
    {
        GenericHelpers.Safe(() => Svc.Framework.Update -= Tick);
        GenericHelpers.Safe(() => Svc.PluginInterface.UiBuilder.Draw -= windowSystem.Draw);
        GenericHelpers.Safe(TextAdvancedManager.UnlockTA);
        GenericHelpers.Safe(YesAlreadyManager.Unlock);
        ECommonsMain.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        var subcommands = args.Split(' ');

        if (subcommands.Length == 0 || args == "")
        {
            mainWindow.IsOpen = !mainWindow.IsOpen;
            return;
        }

        var firstArg = subcommands[0];

        if (firstArg.ToLower() == "d" || firstArg.ToLower() == "debug")
        {
            debugWindow.IsOpen = true;
            return;
        }
        else if (firstArg.ToLower() == "s" || firstArg.ToLower() == "settings")
        {
            settingWindow.IsOpen = true;
            return;
        }
        else if (firstArg.ToLower() == "clear")
        {
            C.Missions.ForEach(x => x.Enabled = false);
        }
        else if (firstArg.ToLower() == "stop")
        {
            SchedulerMain.DisablePlugin();
        }
        else if (firstArg.ToLower() == "start")
        {
            SchedulerMain.EnablePlugin();
        }
    }
}
