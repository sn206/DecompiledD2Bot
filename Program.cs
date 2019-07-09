// Decompiled with JetBrains decompiler
// Type: D2Bot.Program
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using D2Bot.Properties;
using D2BSItemlog;
using Microsoft.Win32;
using Newtonsoft.Json;
using PInvoke;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace D2Bot
{
  public static class Program
  {
    public static string VER = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    public static bool GA = false;
    public static ConcurrentDictionary<string, PatchData> PatchList = new ConcurrentDictionary<string, PatchData>();
    public static ConcurrentDictionary<string, string> DataCache = new ConcurrentDictionary<string, string>();
    public static ConcurrentDictionary<string, ProfileBase> ProfileList = new ConcurrentDictionary<string, ProfileBase>();
    public static ConcurrentDictionary<IntPtr, ProfileBase> Runtime = new ConcurrentDictionary<IntPtr, ProfileBase>();
    public static List<ProfileBase> Viewable = new List<ProfileBase>();
    public static ConcurrentDictionary<string, Schedule> Schedules = new ConcurrentDictionary<string, Schedule>();
    public static ConcurrentDictionary<string, KeyList> KeyLists = new ConcurrentDictionary<string, KeyList>();
    public static Dictionary<string, DateTime> RDTime = new Dictionary<string, DateTime>();
    public static Dictionary<string, Image> ItemBG = new Dictionary<string, Image>();
    public static HashSet<string> HoldKeyList = new HashSet<string>();
    public static HashSet<string> Versions = new HashSet<string>();
    public static GoogleTracker ReportError = new GoogleTracker("UA-101039229-1", Program.GetProductId());
    public static WebConfig WC = (WebConfig) null;
    public static WebServer WS = (WebServer) null;
    public static string BOT_LIB = "";
    private static readonly Regex _whitespace = new Regex("\\s+");
    public static Main GM;
    public static bool UPDATE;
    public static string CurrentProfile;
    public static IntPtr Handle;
    public static D2Tooltip D2Tool;
    public static ListViewItem Previous;
    public static Item CurrentItem;
    public const string EXCEPTIONS = "\\logs\\exceptions.log";
    public const string KEYINFO = "\\logs\\keyinfo.log";
    public const string D2BS_INI = "\\d2bs\\d2bs.ini";
    public const string D2BS_DLL = "\\d2bs\\D2BS.dll";
    public const string CDKEYS = "\\data\\cdkeys.json";
    public const string PATCH = "\\data\\patch.json";
    public const string PROFILE = "\\data\\profile.json";
    public const string SCHEDULES = "\\data\\schedules.json";
    public const string SERVER = "\\data\\server.json";

    [STAThread]
    private static void Main()
    {
      if (!Program.AdminRelauncher())
        return;
      string[] directories = Directory.GetDirectories(Application.StartupPath + "\\d2bs\\", "*bot");
      if (directories.Length != 0)
      {
        Program.BOT_LIB = directories[0];
      }
      else
      {
        int num1 = (int) MessageBox.Show("D2BS Folder is missing botting library! Add a botting lib!");
      }
      Program.UPDATE = Update.CheckForUpdate();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      int num2 = (int) Kernel32.SetErrorMode(Kernel32.ErrorModes.SEM_NOGPFAULTERRORBOX);
      Program.GM = new Main();
      Program.GM.LoadList();
      try
      {
        Program.LoadProfile("\\data\\profile.json");
        if (Settings.Default.Start_Server)
        {
          WebEvents.InitEvents();
          if (Program.WC != null)
          {
            Program.WS = new WebServer(Application.StartupPath + Program.WC.path, Program.WC.ip, Program.WC.port);
            Program.WS.Start();
          }
        }
        if (Program.IsValidURL("http://google-analytics.com"))
          Program.GA = true;
        Application.Run((Form) Program.GM);
      }
      catch (Exception ex)
      {
        Program.LogCrash(ex, "", true);
        Program.GM.Main_Close((object) null, (FormClosingEventArgs) null);
        Application.Exit();
      }
      finally
      {
        if (Program.WC != null && Program.WS != null)
          Program.WS.Stop();
      }
    }

    private static bool AdminRelauncher()
    {
      if (Program.IsRunAsAdmin())
        return true;
      ProcessStartInfo startInfo = new ProcessStartInfo()
      {
        UseShellExecute = true,
        WorkingDirectory = Environment.CurrentDirectory,
        FileName = Assembly.GetEntryAssembly().CodeBase,
        Verb = "runas"
      };
      try
      {
        Process.Start(startInfo);
      }
      catch (Exception ex)
      {
        Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
      }
      return false;
    }

    private static bool IsRunAsAdmin()
    {
      return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }

    private static bool IsValidURL(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.Timeout = 5000;
      httpWebRequest.Method = "HEAD";
      try
      {
        using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
          return response.StatusCode == HttpStatusCode.OK;
      }
      catch (WebException ex)
      {
        return false;
      }
    }

    private static string GetProductId()
    {
      return (!Environment.Is64BitOperatingSystem ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32) : RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)).OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion").GetValue("ProductId").ToString().GetHashCode().ToString();
    }

    public static void LogCrash(Exception e, string comment = "", bool show = true)
    {
      string str = e.ToString() + "\r\n" + comment;
      try
      {
        if (!System.IO.File.Exists(Application.StartupPath + "\\logs\\exceptions.log"))
          System.IO.File.Create(Application.StartupPath + "\\logs\\exceptions.log").Close();
        if (show)
        {
          int num = (int) MessageBox.Show("Please view your exceptions.log file. D2Bot# will close now :(", "An error has occured!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        System.IO.File.AppendAllText(Application.StartupPath + "\\logs\\exceptions.log", DateTime.UtcNow.ToShortDateString() + " " + DateTime.UtcNow.ToShortTimeString() + "\r\n" + str + "\r\n");
        Program.ReportError.trackException(Program._whitespace.Replace(e.ToString(), "+"), true);
      }
      catch
      {
      }
    }

    public static void LoadProfile(string file = "\\data\\profile.json")
    {
      Program.CurrentProfile = Application.StartupPath + file;
      if (!System.IO.File.Exists(Application.StartupPath + "\\d2bs\\d2bs.ini"))
        throw new Exception("D2BS Folder is missing file: d2bs.ini");
      if (!System.IO.File.Exists(Application.StartupPath + "\\logs\\keyinfo.log"))
        System.IO.File.Create(Application.StartupPath + "\\logs\\keyinfo.log").Close();
      if (!System.IO.File.Exists(Application.StartupPath + file))
        System.IO.File.Create(Application.StartupPath + file).Close();
      if (!System.IO.File.Exists(Application.StartupPath + "\\data\\schedules.json"))
        System.IO.File.Create(Application.StartupPath + "\\data\\schedules.json").Close();
      if (!System.IO.File.Exists(Application.StartupPath + "\\data\\server.json"))
        System.IO.File.Create(Application.StartupPath + "\\data\\server.json").Close();
      if (!System.IO.File.Exists(Application.StartupPath + "\\data\\cdkeys.json"))
        System.IO.File.Create(Application.StartupPath + "\\data\\cdkeys.json").Close();
      if (!System.IO.File.Exists(Application.StartupPath + "\\data\\patch.json"))
        System.IO.File.Create(Application.StartupPath + "\\data\\patch.json").Close();
      string[] strArray1 = System.IO.File.ReadAllLines(Application.StartupPath + "\\data\\patch.json");
      string[] strArray2 = System.IO.File.ReadAllLines(Application.StartupPath + file);
      string[] strArray3 = System.IO.File.ReadAllLines(Application.StartupPath + "\\data\\cdkeys.json");
      string[] strArray4 = System.IO.File.ReadAllLines(Application.StartupPath + "\\data\\schedules.json");
      string str = System.IO.File.ReadAllText(Application.StartupPath + "\\data\\server.json");
      JsonSerializerSettings serializerSettings1 = new JsonSerializerSettings();
      serializerSettings1.set_CheckAdditionalContent(false);
      serializerSettings1.set_MissingMemberHandling((MissingMemberHandling) 0);
      JsonSerializerSettings serializerSettings2 = serializerSettings1;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (!strArray1[index].StartsWith("//"))
        {
          PatchData patchData = (PatchData) JsonConvert.DeserializeObject<PatchData>(strArray1[index], serializerSettings2);
          Program.PatchList.TryAdd(patchData.Name + patchData.Version, patchData);
          if (!Program.Versions.Contains(patchData.Version))
          {
            Program.Versions.Add(patchData.Version);
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem1.CheckOnClick = true;
            toolStripMenuItem1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMenuItem1.Name = patchData.Version;
            toolStripMenuItem1.Size = new Size(152, 22);
            toolStripMenuItem1.Text = patchData.Version;
            toolStripMenuItem1.TextImageRelation = TextImageRelation.Overlay;
            ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
            toolStripMenuItem2.Click += new EventHandler(Program.GM.VersionHandler);
            Program.GM.versionMenuItem.DropDownItems.Add((ToolStripItem) toolStripMenuItem2);
            if (Settings.Default.D2_Version.Equals(patchData.Version))
              toolStripMenuItem2.Checked = true;
          }
        }
      }
      for (int index = 0; index < strArray2.Length; ++index)
      {
        TypeFinder typeFinder = (TypeFinder) JsonConvert.DeserializeObject<TypeFinder>(strArray2[index], serializerSettings2);
        ProfileBase profileBase;
        try
        {
          profileBase = typeFinder.Type != ProfileType.D2 || strArray2[index].Contains("\"IsIRC\":true") ? (ProfileBase) JsonConvert.DeserializeObject<IRCProfile>(strArray2[index], serializerSettings2) : (ProfileBase) JsonConvert.DeserializeObject<D2Profile>(strArray2[index], serializerSettings2);
        }
        catch (Exception ex)
        {
          string comment = "Profile: " + strArray2[index];
          Program.LogCrash(ex, comment, false);
          continue;
        }
        if (profileBase != null)
        {
          if (string.IsNullOrEmpty(profileBase.Group))
            profileBase.Group = "default";
          profileBase.Add(false);
        }
      }
      for (int index = 0; index < strArray3.Length; ++index)
      {
        KeyList keyList;
        try
        {
          keyList = (KeyList) JsonConvert.DeserializeObject<KeyList>(strArray3[index]);
        }
        catch (Exception ex)
        {
          string comment = "CDKey: " + strArray3[index];
          Program.LogCrash(ex, comment, false);
          continue;
        }
        Program.KeyLists.TryAdd(keyList.Name.ToLower(), keyList);
      }
      for (int index = 0; index < strArray4.Length; ++index)
      {
        Schedule schedule;
        try
        {
          schedule = (Schedule) JsonConvert.DeserializeObject<Schedule>(strArray4[index]);
        }
        catch (Exception ex)
        {
          string comment = "Schedule: " + strArray4[index];
          Program.LogCrash(ex, comment, false);
          continue;
        }
        Program.Schedules.TryAdd(schedule.Name.ToLower(), schedule);
      }
      if (str.Length <= 0)
        return;
      Program.WC = (WebConfig) JsonConvert.DeserializeObject<WebConfig>(str);
    }

    public static void SendCopyData(IntPtr hWnd, string msg, IntPtr id, int delay = 0, int retry = 1)
    {
      new Thread((ThreadStart) (() => Program.SendTimeout(hWnd, msg, id, delay, retry)))
      {
        IsBackground = true
      }.Start();
    }

    public static void ShoutGlobal(string msg, IntPtr id)
    {
      foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.ProfileList.Values)
      {
        if (profileBase.Type == ProfileType.D2)
        {
          D2Profile d2Profile = (D2Profile) profileBase;
          if (d2Profile.D2Process != null && d2Profile.D2Process != null)
            Program.SendCopyData(d2Profile.MainWindowHandle, msg, id, 0, 1);
        }
      }
    }

    public static void IRCEventMsg(string msg, IntPtr id)
    {
      foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.ProfileList.Values)
      {
        if (profileBase.Type == ProfileType.D2)
        {
          D2Profile d2Profile = (D2Profile) profileBase;
          if (d2Profile.D2Process != null && d2Profile.D2Process != null && d2Profile.IRCEvent)
            Program.SendCopyData(d2Profile.MainWindowHandle, msg, id, 0, 1);
        }
      }
    }

    public static bool SendTimeout(IntPtr hWnd, string msg, IntPtr id, int delay = 0, int retry = 1)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(msg + "\0");
      MessageHelper.COPYDATASTRUCT lParam = new MessageHelper.COPYDATASTRUCT()
      {
        dwData = id,
        cbData = bytes.Length,
        lpData = Marshal.AllocHGlobal(bytes.Length)
      };
      Marshal.Copy(bytes, 0, lParam.lpData, bytes.Length);
      for (int index = 0; index < retry; ++index)
      {
        if (delay > 0)
          Thread.Sleep(delay);
        IntPtr result;
        MessageHelper.SendMessageTimeout(hWnd, 74U, IntPtr.Zero, ref lParam, MessageHelper.SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG, 1000U, out result);
        if (result != IntPtr.Zero)
          return true;
      }
      return false;
    }

    public static bool CanRenameItem(string oldName, string newName)
    {
      oldName = oldName.Trim();
      newName = newName.Trim();
      return oldName.Equals(newName, StringComparison.OrdinalIgnoreCase) || Program.ProfileList.ContainsKey(oldName.ToLower()) && !Program.ProfileList.ContainsKey(newName.ToLower());
    }

    public static bool RenameItem(string oldName, string newName)
    {
      oldName = oldName.Trim();
      newName = newName.Trim();
      if (!Program.CanRenameItem(oldName, newName))
        return false;
      if (oldName.Equals(newName, StringComparison.OrdinalIgnoreCase))
        return true;
      ProfileBase profileBase;
      if (!Program.ProfileList.TryRemove(oldName.ToLower(), out profileBase))
        return false;
      profileBase.Name = newName;
      Program.ProfileList.TryAdd(newName.ToLower(), profileBase);
      return true;
    }

    public static void SaveProfiles()
    {
      if (Program.ProfileList.Count == 0)
        return;
      string[] strArray = new string[Program.ProfileList.Count];
      string[] contents = new string[Program.ProfileList.Count + 1];
      string str1 = System.IO.File.ReadAllText(Application.StartupPath + "\\d2bs\\d2bs.ini");
      contents[0] = str1.Substring(0, str1.IndexOf("; gateway=") + 10);
      int index = 0;
      int num = 0;
      Program.GM.objectLock.WaitOne();
      foreach (ProfileBase profileBase in Program.GM.objectProfileList.get_Objects())
      {
        if (profileBase != null)
        {
          strArray[index] = JsonConvert.SerializeObject((object) profileBase);
          ++index;
          if (profileBase.Type != ProfileType.IRC)
          {
            D2Profile d2Profile = (D2Profile) profileBase;
            string str2 = "";
            if (d2Profile.Difficulty.ToLower().Equals("highest"))
              str2 = "3";
            else if (d2Profile.Difficulty[1] == 'e')
              str2 = "2";
            else if (d2Profile.Difficulty[1] == 'i')
              str2 = "1";
            else if (d2Profile.Difficulty[1] == 'o')
              str2 = "0";
            contents[num + 1] = "[" + d2Profile.Name + "]\r\nMode=" + d2Profile.Mode + "\r\nUsername=" + d2Profile.Account + "\r\nPassword=" + d2Profile.Password + "\r\ngateway=" + d2Profile.Realm + "\r\ncharacter=" + d2Profile.Character + "\r\nScriptPath=" + Path.GetFileName(Program.BOT_LIB) + "\r\nDefaultGameScript=default.dbj\r\nDefaultStarterScript=" + Path.GetFileName(d2Profile.Entry) + "\r\nspdifficulty=" + str2 + "\r\n";
            ++num;
          }
        }
      }
      Program.GM.objectLock.ReleaseMutex();
      System.IO.File.WriteAllText(Program.CurrentProfile, string.Join("\n", strArray));
      System.IO.File.WriteAllLines(Application.StartupPath + "\\d2bs\\d2bs.ini", contents, Encoding.Unicode);
    }

    public static void SaveKeyLists(string k = null)
    {
      string[] strArray = new string[Program.KeyLists.Count];
      int index = 0;
      foreach (KeyList keyList in (IEnumerable<KeyList>) Program.KeyLists.Values)
      {
        strArray[index] = JsonConvert.SerializeObject((object) keyList);
        ++index;
      }
      System.IO.File.WriteAllText(Application.StartupPath + "\\data\\cdkeys.json", string.Join("\n", strArray));
    }

    public static void SaveSchedules(string s = null)
    {
      string[] strArray = new string[Program.Schedules.Count];
      int index = 0;
      foreach (Schedule schedule in (IEnumerable<Schedule>) Program.Schedules.Values)
      {
        strArray[index] = JsonConvert.SerializeObject((object) schedule);
        ++index;
      }
      System.IO.File.WriteAllText(Application.StartupPath + "\\data\\schedules.json", string.Join("\n", strArray));
    }

    public static void SaveAll()
    {
      Program.SaveProfiles();
      Program.SaveKeyLists((string) null);
      Program.SaveSchedules((string) null);
    }

    public static void RemoveFromLog(string profile, string key)
    {
      string[] strArray = System.IO.File.ReadAllLines(Application.StartupPath + "\\logs\\keyinfo.log");
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) strArray);
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (stringList[index].Contains(profile) && stringList[index].Contains(key))
        {
          stringList.Remove(stringList[index]);
          --index;
        }
      }
      System.IO.File.WriteAllText(Application.StartupPath + "\\logs\\keyinfo.log", string.Join("\n", stringList.ToArray()));
    }

    public static void ClearLog()
    {
      System.IO.File.Delete(Application.StartupPath + "\\logs\\keyinfo.log");
      System.IO.File.Create(Application.StartupPath + "\\logs\\keyinfo.log");
    }

    private static IEnumerable<string> Split(string str, int maxChunkSize)
    {
      for (int i = 0; i < str.Length; i += maxChunkSize)
        yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
    }

    public static void LaunchClient(D2Profile client)
    {
      if (!System.IO.File.Exists(client.D2Path))
        throw new Exception("Invalid Diablo II path!");
      foreach (string file in Directory.GetFiles(Path.GetDirectoryName(client.D2Path), "*.dat*", SearchOption.AllDirectories))
      {
        try
        {
          System.IO.File.Delete(file);
        }
        catch
        {
        }
      }
      CDKey currentKey = client.CurrentKey;
      string str1 = "";
      if (currentKey != null && currentKey.Classic != null && currentKey.Classic.Length > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" -d2c \"" + currentKey.Classic + "\"");
        stringBuilder.Append(" -d2x \"" + currentKey.Expansion + "\"");
        str1 = stringBuilder.ToString();
      }
      else if (currentKey != null)
        str1 = currentKey.Name;
      if (currentKey != null)
        Program.GM.UpdateMPQ(client);
      string parameters = client.Parameters;
      string str2 = " -handle \"" + Program.Handle.ToString() + "\" -cachefix -multi -title \"" + client.Name + "\" " + parameters;
      if (!str2.Contains(" -L"))
        str2 = " -profile \"" + client.Name + "\"" + str2;
      if (str1 != null && str1.ToLower().Contains("mpq"))
        str2 = " -mpq \"" + str1 + "\"" + str2;
      else if (str1.Length > 0)
        str2 = str1 + str2;
      ProcessStartInfo startInfo = new ProcessStartInfo(client.D2Path)
      {
        Arguments = str2,
        UseShellExecute = false,
        WorkingDirectory = Path.GetDirectoryName(client.D2Path)
      };
      client.D2Process = new Process()
      {
        StartInfo = startInfo
      };
      bool flag1 = false;
      if (System.IO.File.Exists(Application.StartupPath + "\\D2BS\\D2M.dll"))
        flag1 = true;
      client.D2Process = Kernel32.StartSuspended(client.D2Process, startInfo);
      try
      {
        if (flag1)
        {
          Kernel32.LoadRemoteLibrary(client.D2Process, (object) (Application.StartupPath + "\\D2BS\\D2M.dll"));
          flag1 = true;
        }
        else
          Kernel32.LoadRemoteLibrary(client.D2Process, (object) (Application.StartupPath + "\\d2bs\\D2BS.dll"));
      }
      catch (Exception ex)
      {
        Program.GM.ConsolePrint(new PrintMessage("D2M.dll failed to load", "", "", 0), (ProfileBase) client);
        Program.CrashReport(client);
      }
      PatchData patchData1;
      bool flag2 = Program.PatchList.TryGetValue("hidewin0" + Settings.Default.D2_Version, out patchData1);
      PatchData patchData2;
      bool flag3 = Program.PatchList.TryGetValue("hidewin1" + Settings.Default.D2_Version, out patchData2);
      PatchData patchData3;
      bool flag4 = Program.PatchList.TryGetValue("hidewin2" + Settings.Default.D2_Version, out patchData3);
      Patch patch1 = (Patch) null;
      Patch patch2 = (Patch) null;
      Patch patch3 = (Patch) null;
      if (!client.Visible)
      {
        if (flag2)
        {
          patch1 = new Patch(patchData1.Module, patchData1.Offset, patchData1.Data);
          patch1.Install(client.D2Process);
        }
        if (flag3)
        {
          patch2 = new Patch(patchData2.Module, patchData2.Offset, patchData2.Data);
          patch2.Install(client.D2Process);
        }
        if (flag4)
        {
          patch3 = new Patch(patchData3.Module, patchData3.Offset, patchData3.Data);
          patch3.Install(client.D2Process);
        }
      }
      PatchData patchData4;
      if (client.BlockRD && Program.PatchList.TryGetValue("rdblock" + Settings.Default.D2_Version, out patchData4))
        new Patch(patchData4.Module, patchData4.Offset, patchData4.Data).Install(client.D2Process);
      Program.InstallPatches(client.D2Process);
      Kernel32.Resume(client.D2Process);
      client.D2Process.WaitForInputIdle();
      if (Program.Runtime.ContainsKey(client.MainWindowHandle))
      {
        ProfileBase profileBase;
        Program.Runtime.TryRemove(client.MainWindowHandle, out profileBase);
      }
      client.MainWindowHandle = client.D2Process.MainWindowHandle;
      if (flag1)
      {
        try
        {
          Kernel32.LoadRemoteLibrary(client.D2Process, (object) (Application.StartupPath + "\\d2bs\\D2BS.dll"));
        }
        catch
        {
          Program.GM.ConsolePrint(new PrintMessage("D2BS.dll failed to load", "", "", 0), (ProfileBase) client);
          Program.CrashReport(client);
        }
      }
      if (!client.Visible)
      {
        if (Settings.Default.Start_Hidden)
        {
          client.HideWindow();
        }
        else
        {
          IntPtr result;
          MessageHelper.SendMessageTimeout(client.MainWindowHandle, 28U, (IntPtr) 0, IntPtr.Zero, MessageHelper.SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG, 250U, out result);
        }
      }
      client.Crashed = 0;
      try
      {
        if (patch1 != null && patch1.IsInstalled() && !client.D2Process.HasExited)
          patch1.Remove(client.D2Process);
        if (patch2 != null && patch2.IsInstalled() && !client.D2Process.HasExited)
          patch2.Remove(client.D2Process);
        if (patch3 != null && patch3.IsInstalled() && !client.D2Process.HasExited)
          patch3.Remove(client.D2Process);
        IntPtr result;
        MessageHelper.SendMessageTimeout(client.MainWindowHandle, 28U, (IntPtr) 0, IntPtr.Zero, MessageHelper.SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG, 250U, out result);
      }
      catch
      {
      }
      Program.Runtime.TryAdd(client.MainWindowHandle, (ProfileBase) client);
      if (!client.Visible)
        return;
      client.ShowWindow();
    }

    private static void InstallPatches(Process p)
    {
      foreach (string key in (IEnumerable<string>) Program.PatchList.Keys)
      {
        PatchData patchData;
        if (!key.StartsWith("hidewin") && !key.StartsWith("rdblock") && Program.PatchList.TryGetValue(key, out patchData))
          new Patch(patchData.Module, patchData.Offset, patchData.Data).Install(p);
      }
    }

    public static void CrashReport(D2Profile profile)
    {
      ++profile.Crashed;
      profile.Stop();
      if (profile.Crashed >= 6)
        return;
      PrintMessage pm = new PrintMessage("Window crashed on load! Restarting.", "", "", 0);
      Program.GM.ConsolePrint(pm, (ProfileBase) profile);
      profile.Restart(false);
    }
  }
}
