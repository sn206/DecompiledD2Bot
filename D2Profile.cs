// Decompiled with JetBrains decompiler
// Type: D2Bot.D2Profile
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace D2Bot
{
  public class D2Profile : ProfileBase
  {
    [JsonIgnore]
    private bool m_scheduleEnable;

    public string Account { get; set; }

    public string Password { get; set; }

    public string Character { get; set; }

    public string GameName { get; set; }

    public string GamePass { get; set; }

    public string D2Path { get; set; }

    public string Realm { get; set; }

    public string Mode { get; set; }

    public string Difficulty { get; set; }

    public string Parameters { get; set; }

    public string Entry { get; set; }

    public string Location { get; set; }

    public string KeyList { get; set; }

    public string Schedule { get; set; }

    public int GameCount { get; set; }

    public int Runs { get; set; }

    public int Chickens { get; set; }

    public int Deaths { get; set; }

    public int Crashes { get; set; }

    public int Restarts { get; set; }

    public int RunsPerKey { get; set; }

    public int KeyRuns { get; set; }

    public string InfoTag { get; set; }

    public bool Visible { get; set; }

    public bool BlockRD { get; set; }

    public bool ScheduleEnable
    {
      get
      {
        return this.m_scheduleEnable;
      }
      set
      {
        if (!value && this.m_scheduleEnable != value)
          this.Stop();
        this.m_scheduleEnable = value;
      }
    }

    [JsonIgnore]
    public string GameExe
    {
      get
      {
        return Path.GetFileName(this.D2Path);
      }
    }

    [JsonIgnore]
    public CDKey CurrentKey { get; set; }

    [JsonIgnore]
    public string Key
    {
      get
      {
        if (this.CurrentKey != null)
          return this.CurrentKey.Name;
        return "";
      }
    }

    [JsonIgnore]
    public int HeartAttack { get; set; }

    [JsonIgnore]
    public int Crashed { get; set; }

    [JsonIgnore]
    public int NoResponse { get; set; }

    [JsonIgnore]
    public bool Error { get; set; }

    [JsonIgnore]
    public Process D2Process { get; set; }

    [JsonIgnore]
    public IntPtr MainWindowHandle { get; set; }

    [JsonIgnore]
    public Thread Client { get; set; }

    [JsonIgnore]
    public bool IRCEvent { get; set; }

    public override ProfileType Type
    {
      get
      {
        return ProfileType.D2;
      }
    }

    public bool IncrementKey()
    {
      D2Bot.KeyList keyList = Program.GM.GetKeyList(this.KeyList);
      if (keyList == null)
        return false;
      CDKey currentKey = this.CurrentKey;
      try
      {
        this.CurrentKey = keyList.GetAndIncrement();
        currentKey?.Set(false);
      }
      catch (Exception ex)
      {
        if (ex.ToString().ToLower().Contains("all keys"))
        {
          currentKey?.Set(false);
          int num = (int) MessageBox.Show("All keys in this keylist are in use or on hold. Please add more keys or resume paused keys.");
        }
        else
        {
          int num1 = (int) MessageBox.Show(ex.ToString());
        }
        return false;
      }
      return true;
    }

    internal override void Start(int delay)
    {
      if (!this.StatusLock.TryEnterReadLock(10))
        return;
      if (this.Status != Status.Starting)
      {
        this.StatusLock.ExitReadLock();
      }
      else
      {
        this.StatusLock.ExitReadLock();
        if (delay > 0)
          Thread.Sleep(delay);
        if (this.KeyList != null && this.KeyList.Length > 0 && (this.CurrentKey == null && !this.IncrementKey()))
        {
          this.Stop();
        }
        else
        {
          try
          {
            Program.LaunchClient(this);
            this.StatusLock.EnterWriteLock();
            this.Status = Status.Run;
            this.StatusLock.ExitWriteLock();
          }
          catch (ThreadAbortException ex)
          {
          }
          catch (Exception ex)
          {
            PrintMessage pm = new PrintMessage(ex.Message, "", "", 0);
            Program.GM.ConsolePrint(pm, (ProfileBase) this);
            this.StopClient();
          }
        }
      }
    }

    public void StopClient()
    {
      for (int index = 0; this.Status == Status.Starting && index != 25; ++index)
        Thread.Sleep(100);
      if (Thread.CurrentThread != this.Handle && this.Handle != null && this.Handle.IsAlive)
        this.Handle.Abort();
      if (this.D2Process != null)
      {
        if (this.D2Process != null && this.D2Process != null && !this.D2Process.HasExited)
        {
          this.D2Process.CloseMainWindow();
          for (int index = 0; index < 10 && (this.D2Process != null && !this.D2Process.HasExited); ++index)
            Thread.Sleep(100);
          try
          {
            this.D2Process.Kill();
          }
          catch
          {
          }
        }
        this.D2Process = (Process) null;
        this.HeartAttack = 0;
        this.Crashed = 0;
      }
      ProfileBase profileBase;
      Program.Runtime.TryRemove(this.MainWindowHandle, out profileBase);
      this.StatusLock.EnterWriteLock();
      this.Status = Status.Stop;
      this.StatusLock.ExitWriteLock();
    }

    public override void Stop()
    {
      Program.GM.Queue[this.MainWindowHandle.ToInt32() % 10].Add(new Worker(IntPtr.Zero, (string) null, Main.ProfileAction.Stop, this));
    }

    public void StopThread()
    {
      this.StatusLock.EnterUpgradeableReadLock();
      if (this.Status == Status.Busy || this.Status == Status.Stop)
      {
        this.StatusLock.ExitUpgradeableReadLock();
      }
      else
      {
        this.StatusLock.EnterWriteLock();
        this.Status = Status.Busy;
        this.StatusLock.ExitWriteLock();
        this.StatusLock.ExitUpgradeableReadLock();
        this.StopClient();
      }
    }

    public void Restart(bool increment = false)
    {
      this.StatusLock.EnterUpgradeableReadLock();
      if (this.Status == Status.Busy)
      {
        this.StatusLock.ExitUpgradeableReadLock();
      }
      else
      {
        this.StatusLock.EnterWriteLock();
        this.Status = Status.Busy;
        this.StatusLock.ExitWriteLock();
        this.StatusLock.ExitUpgradeableReadLock();
        new Thread((ThreadStart) (() => this.RestartClient(increment)))
        {
          IsBackground = true
        }.Start();
      }
    }

    private void RestartClient(bool increment)
    {
      this.StopClient();
      if (increment)
      {
        this.KeyRuns = 0;
        if (!this.IncrementKey())
        {
          this.Stop();
          return;
        }
        Program.GM.UpdateMPQ(this);
        Program.GM.UpdateRestarts(this);
      }
      this.StatusLock.EnterWriteLock();
      this.Status = Status.Starting;
      this.StatusLock.ExitWriteLock();
      this.Start(0);
      this.StatusLock.EnterWriteLock();
      this.Status = Status.Run;
      this.StatusLock.ExitWriteLock();
    }

    public void HeartBeat()
    {
      this.HeartAttack = 0;
    }

    public void NextKey()
    {
      if (this.KeyList.Length == 0 || Program.GM.GetKeyList(this.KeyList).CDKeys.Count <= 0)
        return;
      this.IncrementKey();
      Program.GM.UpdateMPQ(this);
    }

    public void ReleaseKey()
    {
      if (this.CurrentKey == null)
        return;
      this.CurrentKey.Set(false);
      this.CurrentKey = (CDKey) null;
      Program.GM.UpdateMPQ(this);
    }

    public override ProfileBase DeepCopy()
    {
      M0 m0 = JsonConvert.DeserializeObject<D2Profile>(JsonConvert.SerializeObject((object) this));
      ((D2Profile) m0).CurrentKey = (CDKey) null;
      ((ProfileBase) m0).Status = Status.Stop;
      ((ProfileBase) m0).State = "";
      ((D2Profile) m0).D2Process = (Process) null;
      return (ProfileBase) m0;
    }

    public void ShowWindow()
    {
      Program.GM.Queue[this.MainWindowHandle.ToInt32() % 10].Add(new Worker(IntPtr.Zero, (string) null, Main.ProfileAction.Show, this));
    }

    public void ShowWindowThread()
    {
      IntPtr mainWindowHandle;
      if (this.D2Process == null || (mainWindowHandle = this.D2Process.MainWindowHandle) == IntPtr.Zero)
        return;
      if (this.Visible)
      {
        if (this.Location == "")
        {
          MessageHelper.ShowWindow(this.D2Process.MainWindowHandle.ToInt32(), 1);
        }
        else
        {
          try
          {
            int X = int.Parse(this.Location.Split(',')[0].Trim());
            int Y = int.Parse(this.Location.Split(',')[1].Trim());
            int num = 0;
            while (num < 20 && this.Status != Status.Run)
            {
              ++num;
              Thread.Sleep(50);
            }
            Thread.Sleep(100);
            MessageHelper.ShowWindow(mainWindowHandle.ToInt32(), 1);
            MessageHelper.SetWindowPos(mainWindowHandle, IntPtr.Zero, X, Y, 10, 10, 65);
          }
          catch
          {
            MessageHelper.ShowWindow(mainWindowHandle.ToInt32(), 1);
          }
        }
      }
      else
        MessageHelper.ShowWindow(this.D2Process.MainWindowHandle.ToInt32(), 6);
    }

    public void HideWindow()
    {
      Program.GM.Queue[this.MainWindowHandle.ToInt32() % 10].Add(new Worker(IntPtr.Zero, (string) null, Main.ProfileAction.Hide, this));
    }

    public void HideWindowThread()
    {
      if (this.D2Process == null)
        return;
      IntPtr mainWindowHandle;
      if ((mainWindowHandle = this.D2Process.MainWindowHandle) == IntPtr.Zero)
        return;
      try
      {
        if (!MessageHelper.IsIconic(mainWindowHandle))
          MessageHelper.ShowWindow(mainWindowHandle.ToInt32(), 6);
        MessageHelper.ShowWindow(mainWindowHandle.ToInt32(), 0);
      }
      catch
      {
      }
    }

    public override void ShowEditor(bool init)
    {
      D2ProfileEditor d2ProfileEditor = new D2ProfileEditor()
      {
        ProfileToEdit = this,
        IsNew = init
      };
      d2ProfileEditor.Show();
      d2ProfileEditor.DiabloPath.Select(d2ProfileEditor.DiabloPath.Text.Length, 0);
      d2ProfileEditor.EntryScript.Select(d2ProfileEditor.EntryScript.Text.Length, 0);
    }
  }
}
