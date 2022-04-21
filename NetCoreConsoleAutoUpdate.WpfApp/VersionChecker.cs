using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NetCoreConsoleAutoUpdate.ConsoleApp;
public class VersionChecker
{
    private const int Second = 1000;
    private string VersionUrl { get; set; }
    public DispatcherTimer Timer { get; set; }
    public VersionChecker(string versionUrl, int intervalMinute)
    {
        VersionUrl = versionUrl;
        AutoUpdater.ShowSkipButton = false;
        AutoUpdater.Mandatory = true;
        AutoUpdater.UpdateMode = Mode.Forced;
        AutoUpdater.ReportErrors = true;
        AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
        AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
        if (intervalMinute <= 0)
            return;
        Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(intervalMinute),
        };

        Timer.Tick += delegate
        {
            CheckUpdate();
        };
        Start();
    }

    public void CheckUpdate()
    {
        Stop();
        AutoUpdater.Start(VersionUrl);//TODO: 開發時期不使用是,移到#if (!DEBUG)內
#if (!DEBUG)
#endif
    }

    private void Start()
    {
        Timer?.Start();
    }

    private void Stop()
    {
        Timer?.Stop();
    }

    private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
    {
        VersionUpdateInfo json = System.Text.Json.JsonSerializer.Deserialize<VersionUpdateInfo>(args.RemoteData);
        args.UpdateInfo = new UpdateInfoEventArgs
        {
            CurrentVersion = json.version,
            Mandatory = new Mandatory
            {
                Value = json.mandatory,
                UpdateMode = Mode.Forced,
                MinimumVersion = json.version
            },
            DownloadURL = json.url
        };

    }

    private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
    {
        if (!args.IsUpdateAvailable)
        {
            Start();
            return;
        }

        try
        {
            MessageBox.Show($@"There is new version {args.CurrentVersion} available. You are using version {
                            args.InstalledVersion
                        }. Do you want to update the application now?", @"Update Available");
            if (AutoUpdater.DownloadUpdate(args))
                Application.Current.Shutdown();
            else
                Start();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, exception.GetType().ToString());
        }
    }
}

public class VersionUpdateInfo
{
    public string version { get; set; }
    public bool mandatory { get; set; }
    public string url { get; set; }
}