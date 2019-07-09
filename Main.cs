// Decompiled with JetBrains decompiler
// Type: D2Bot.Main
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using BrightIdeasSoftware;
using D2Bot.Properties;
using D2BSItemlog;
using Newtonsoft.Json;
using PInvoke;
using RichTextBoxLinks;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace D2Bot
{
  public class Main : Form
  {
    private static bool ad = true;
    private static Image ad1 = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("D2Bot.Resources.ad1.png"));
    private static Image ad2 = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("D2Bot.Resources.ad2.png"));
    public static ListViewItem LastSelected = (ListViewItem) null;
    public BlockingCollection<Worker>[] Queue = new BlockingCollection<Worker>[10];
    public Mutex CharLogMutex = new Mutex();
    public Mutex objectLock = new Mutex();
    private System.Collections.Generic.Queue<Worker> MsgSink = new System.Collections.Generic.Queue<Worker>(25);
    public int Ticker = Environment.TickCount & int.MaxValue;
    public int SEC_Count = 600;
    public System.Timers.Timer Timer;
    public const int QUEUE_SIZE = 10;
    private FileSystemWatcher watcher;
    private HashSet<string> descriptions;
    private Dictionary<string, ListViewItem> database;
    private Dictionary<string, ListViewItem> active;
    private Dictionary<string, TreeNode> nodeCache;
    private System.Collections.Generic.Queue<string> EventBuffer;
    private Mutex EventSync;
    private bool keepAlive;
    private Thread ItemLoading;
    private Thread WindowChecker;
    private NotifyIcon notifyIcon;
    private int UpdateIndex;
    private int UpdateTotal;
    private IContainer components;
    private ToolStripButton guiStart;
    private ToolStripButton guiStop;
    private ToolStripButton guiAdd;
    private ToolStripButton guiEdit;
    private ToolStripButton guiSave;
    private ToolStripButton guiCopy;
    private ToolStripButton guiShow;
    private ToolStripButton guiHide;
    private ToolStripButton guiAbout;
    private ToolStrip toolStrip1;
    private ToolStripButton guiRemove;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem resetStatsToolStripMenuItem;
    private ToolStripMenuItem disableScheduleToolStripMenuItem;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem removeKeyToolStripMenuItem;
    private ToolStripMenuItem removeFromListToolStripMenuItem;
    private ContextMenuStrip contextMenuStrip3;
    private ToolStripMenuItem toolStripMenuItem3;
    private ToolStripMenuItem importToolStripMenuItem;
    private ContextMenuStrip contextMenuStrip4;
    private ToolStripMenuItem toolItemSaveImage;
    private ToolStripMenuItem createDupeToolStripMenuItem;
    private ToolStripMenuItem removeToolStripMenuItem;
    private ToolStripMenuItem enableScheduleToolStripMenuItem;
    private ToolStripMenuItem clearItemsToolStripMenuItem;
    private ToolStripMenuItem clearToolStripMenuItem;
    private ToolStripMenuItem copyImageToolStripMenuItem;
    private ToolStripMenuItem copyDescriptionToolStripMenuItem;
    private ToolStripMenuItem muleProfileToolStripMenuItem;
    private SplitContainer mainContainer;
    private ContextMenuStrip contextMenuStrip5;
    private ToolStripMenuItem pauseKeyToolStripMenuItem;
    private ToolStripMenuItem resumeKeyToolStripMenuItem;
    private ToolStripMenuItem nextKeyToolStripMenuItem;
    private ToolStripButton guiAddIRC;
    private ToolStripMenuItem releaseKeyToolStripMenuItem;
    public ObjectListView objectProfileList;
    private OLVColumn profileCol;
    private OLVColumn statusCol;
    private OLVColumn runsCol;
    private OLVColumn chickensCol;
    private OLVColumn deathsCol;
    private OLVColumn crashesCol;
    private OLVColumn restartsCol;
    private OLVColumn keyCol;
    private OLVColumn gameExe;
    private ToolStripMenuItem uploadImgurToolStripMenuItem;
    private ImageList profileImages;
    private ToolStripButton keysEdit;
    private ToolStripButton scheduleEdit;
    private ToolStripMenuItem importProfilesToolStripMenuItem;
    private TabControl PrintTab;
    private TabPage Console;
    public ListView ItemLogger;
    private ColumnHeader ItemDateProfile;
    private ColumnHeader ItemLogColumn;
    private RichTextBoxEx ConsoleBox;
    private TabPage charView;
    private SplitContainer charContainer;
    public TreeView CharTree;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel charSearchBar;
    private TextBox SearchBox;
    private Button SearchButton;
    public ListView CharItems;
    private ColumnHeader CharItemColumn;
    private TabPage KeyAnalysis;
    private SplitContainer keyWizardContainer;
    public DataGridView dupeList;
    private ListView KeyData;
    private ColumnHeader KeyProfile;
    private ColumnHeader KeyName;
    private ColumnHeader KeyInUse;
    private ColumnHeader KeyRD;
    private ColumnHeader KeyDisabled;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel keyWizBorder;
    private ToolStripDropDownButton settingsDropDown;
    private ToolStripMenuItem menuShow;
    private ToolStripMenuItem menuHide;
    private ToolStripMenuItem responseTimeToolStripMenuItem;
    private ToolStripMenuItem secondsToolStripMenuItem;
    private ToolStripMenuItem secondsToolStripMenuItem1;
    private ToolStripMenuItem secondsToolStripMenuItem2;
    private ToolStripMenuItem secondsToolStripMenuItem3;
    private ToolStripMenuItem secondsToolStripMenuItem4;
    private ToolStripMenuItem loadDelay;
    private ToolStripMenuItem ms100;
    private ToolStripMenuItem ms250;
    private ToolStripMenuItem ms500;
    private ToolStripMenuItem ms1000;
    private ToolStripMenuItem startHidden;
    private ToolStripMenuItem systemFont;
    private ToolStripMenuItem itemHeader;
    private ToolStripMenuItem refreshCharViewToolStripMenuItem;
    private ToolStripMenuItem closeGameexeToolStripMenuItem;
    private ToolStripMenuItem debugMode;
    private ToolStripDropDownButton menuDropDown;
    private ToolStripMenuItem menuStart;
    private ToolStripMenuItem menuStop;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem menuSave;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem menuExit;
    private ToolStripMenuItem systemTrayToolStripMenuItem;
    private DataGridViewTextBoxColumn keyDupe;
    private DataGridViewComboBoxColumn profileDupe;
    private ToolStripMenuItem clearDetailsToolStripMenuItem;
    public ToolStripMenuItem versionMenuItem;
    private ToolStripMenuItem ms2000;
    private ToolStripMenuItem ms5000;
    private SplitContainer splitContainer1;
    private PictureBox pictureBox1;
    private ToolStripMenuItem serverToggle;

    public void CheckWindows()
    {
      Thread.Sleep(6000);
label_1:
      do
      {
        Thread.Sleep(1000);
      }
      while (this.debugMode.Checked || Program.ProfileList == null || Program.ProfileList.Count == 0);
      IntPtr zero = IntPtr.Zero;
      IntPtr window = MessageHelper.FindWindow((string) null, "Diablo II Error");
      if (window != IntPtr.Zero)
      {
        IntPtr result;
        MessageHelper.SendMessageTimeout(window, 274U, (IntPtr) 61536, IntPtr.Zero, MessageHelper.SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG, 250U, out result);
      }
      int num1 = 0;
      for (int index1 = 0; index1 < Program.ProfileList.Values.Count; ++index1)
      {
        ProfileBase profileBase = (ProfileBase) null;
        try
        {
          profileBase = Program.ProfileList.Values.ElementAt<ProfileBase>(index1);
        }
        catch
        {
        }
        if (profileBase != null && !(profileBase.GetType() != typeof (D2Profile)))
        {
          D2Profile d2Profile = profileBase as D2Profile;
          Process d2Process = d2Profile.D2Process;
          if (Program.WS != null && Program.WS.IsActive())
            Program.WS.RunScheduler(d2Profile);
          if (d2Profile.ScheduleEnable)
          {
            bool flag = false;
            TimeSpan timeSpan = new TimeSpan(0, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            Schedule schedule = this.GetSchedule(d2Profile.Schedule);
            for (int index2 = 0; index2 < schedule.Times.Count; index2 += 2)
            {
              TimeSpan period1 = schedule.Times[index2].GetPeriod();
              TimeSpan period2 = schedule.Times[index2 + 1].GetPeriod();
              if (period1 > period2 && (period1 <= timeSpan || timeSpan < period2))
              {
                flag = true;
                break;
              }
              if (period1 <= timeSpan && period2 > timeSpan || period1 == period2)
              {
                flag = true;
                break;
              }
            }
            if (flag && d2Process == null && d2Profile.Status == Status.Stop)
            {
              int num2 = 0;
              if (this.loadDelay.Checked)
                num2 = Settings.Default.Delay_Time;
              ++num1;
              d2Profile.Load(num1 * num2);
            }
            if (!flag && d2Process != null)
              d2Profile.Stop();
          }
          if (Program.Runtime.ContainsKey(d2Profile.MainWindowHandle) && !d2Profile.Parameters.Contains("-L") && d2Profile.StatusLock.TryEnterReadLock(10))
          {
            if (d2Profile.Status != Status.Run)
            {
              d2Profile.StatusLock.ExitReadLock();
            }
            else
            {
              d2Profile.StatusLock.ExitReadLock();
              ++d2Profile.HeartAttack;
              try
              {
                if (d2Process.HasExited)
                {
                  d2Profile.StatusLock.EnterReadLock();
                  if (d2Profile.Status == Status.Run)
                  {
                    d2Profile.StatusLock.ExitReadLock();
                    this.ConsolePrint(new PrintMessage("Window has unexpectedly exited... starting profile", "", "", 0), (ProfileBase) d2Profile);
                    d2Profile.Error = true;
                    Thread.Sleep(Settings.Default.Delay_Time);
                    d2Profile.Restart(false);
                    this.UpdateRestarts(d2Profile);
                    this.UpdateCrashes(d2Profile);
                  }
                  else
                    d2Profile.StatusLock.ExitReadLock();
                }
                else if (!d2Process.Responding)
                  ++d2Profile.NoResponse;
                else
                  d2Profile.NoResponse = 0;
              }
              catch
              {
                ++d2Profile.NoResponse;
              }
              if (d2Profile.HeartAttack > Settings.Default.Wait_Time || d2Profile.NoResponse > Settings.Default.Wait_Time)
              {
                d2Profile.StatusLock.EnterReadLock();
                if (d2Profile.Status == Status.Run)
                {
                  d2Profile.StatusLock.ExitReadLock();
                  this.ConsolePrint(new PrintMessage("D2BS is not responding... starting profile", "", "", 0), (ProfileBase) d2Profile);
                  d2Profile.Error = true;
                  d2Profile.NoResponse = 0;
                  d2Profile.HeartAttack = 0;
                  Thread.Sleep(Settings.Default.Delay_Time);
                  d2Profile.Restart(false);
                  this.UpdateRestarts(d2Profile);
                  this.UpdateCrashes(d2Profile);
                }
                else
                  d2Profile.StatusLock.ExitReadLock();
              }
              else if (d2Profile.Status == Status.Run && d2Profile.HeartAttack > 1)
                Program.SendCopyData(d2Process.MainWindowHandle, "Handle", Program.Handle, 0, 1);
              if (d2Profile.RunsPerKey > 0 && d2Profile.KeyRuns >= d2Profile.RunsPerKey)
              {
                d2Profile.StatusLock.EnterReadLock();
                if (d2Profile.Status == Status.Run)
                {
                  d2Profile.StatusLock.ExitReadLock();
                  Thread.Sleep(Settings.Default.Delay_Time);
                  d2Profile.Restart(true);
                }
                else
                  d2Profile.StatusLock.ExitReadLock();
              }
            }
          }
        }
      }
      goto label_1;
    }

    public void Save(object sender, EventArgs e)
    {
      Program.SaveAll();
    }

    public List<ProfileBase> GetSelectedProfiles()
    {
      List<ProfileBase> profileBaseList = new List<ProfileBase>();
      bool flag = false;
      try
      {
        flag = this.objectLock.WaitOne();
        if (this.objectProfileList.get_SelectedObjects() != null)
        {
          foreach (ProfileBase selectedObject in (IEnumerable) Program.GM.objectProfileList.get_SelectedObjects())
          {
            if (selectedObject != null)
              profileBaseList.Add(selectedObject);
          }
        }
      }
      finally
      {
        if (flag)
          this.objectLock.ReleaseMutex();
      }
      return profileBaseList;
    }

    public void StartProfiles(List<ProfileBase> profiles)
    {
      int num1 = 0;
      int num2 = 1;
      if (this.loadDelay.Checked)
        num1 = Settings.Default.Delay_Time;
      foreach (ProfileBase profile in profiles)
      {
        if (profile != null)
        {
          ++num2;
          profile.Load(num1 * num2);
        }
      }
    }

    public void StopProfiles(List<ProfileBase> profiles)
    {
      foreach (ProfileBase profile in profiles)
        profile?.Stop();
    }

    public void StartProfile(object sender, EventArgs e)
    {
      List<ProfileBase> profiles = this.GetSelectedProfiles();
      new Thread((ThreadStart) (() => this.StartProfiles(profiles)))
      {
        IsBackground = true
      }.Start();
    }

    public void StopProfile(object sender, EventArgs e)
    {
      List<ProfileBase> profiles = this.GetSelectedProfiles();
      new Thread((ThreadStart) (() => this.StopProfiles(profiles)))
      {
        IsBackground = true
      }.Start();
    }

    public void Duplicate(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      foreach (ProfileBase selectedObject in (IEnumerable) Program.GM.objectProfileList.get_SelectedObjects())
      {
        if (selectedObject != null)
        {
          ProfileBase profileBase = selectedObject.DeepCopy();
          string str = Regex.Match(profileBase.Name, "^\\D+").Value;
          int result;
          if (!int.TryParse(Regex.Replace(profileBase.Name, "^\\D+", ""), out result))
            result = 0;
          ++result;
          if (!str.EndsWith("-"))
            str += "-";
          string name;
          for (name = str + result.ToString(new string('0', 3)); this.GetProfile(name) != null; name = str + result.ToString(new string('0', 3)))
          {
            ++result;
            if (result > 999)
              throw new Exception("Cannot copy profile more than 999 times.");
          }
          profileBase.Name = name;
          profileBase.Add(true);
          Thread.Sleep(10);
          Program.SaveProfiles();
        }
      }
    }

    public void ShowWindow(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      foreach (ProfileBase selectedObject in (IEnumerable) Program.GM.objectProfileList.get_SelectedObjects())
      {
        if (selectedObject != null && selectedObject.Type == ProfileType.D2)
          ((D2Profile) selectedObject).ShowWindow();
      }
    }

    public void HideWindow(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      foreach (ProfileBase selectedObject in (IEnumerable) Program.GM.objectProfileList.get_SelectedObjects())
      {
        if (selectedObject != null && selectedObject.Type == ProfileType.D2)
          ((D2Profile) selectedObject).HideWindow();
      }
    }

    public void Remove(object sender, EventArgs e)
    {
      if (sender == null || MessageBox.Show("Apply profile delete?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        bool flag = false;
        try
        {
          flag = this.objectLock.WaitOne();
          if (this.objectProfileList.get_SelectedObjects() != null)
          {
            foreach (ProfileBase selectedObject in (IEnumerable) this.objectProfileList.get_SelectedObjects())
            {
              ProfileBase profileBase;
              Program.ProfileList.TryRemove(selectedObject.Name.ToLower(), out profileBase);
              if (typeof (D2Profile) == profileBase.GetType())
              {
                D2Profile d2Profile = profileBase as D2Profile;
                if (d2Profile.CurrentKey != null)
                  d2Profile.CurrentKey.Set(false);
              }
            }
            this.objectProfileList.RemoveObjects((ICollection) this.objectProfileList.get_SelectedObjects());
          }
        }
        catch
        {
        }
        finally
        {
          if (flag)
            this.objectLock.ReleaseMutex();
        }
      }
      Program.SaveProfiles();
    }

    public void AddIRC(object sender, EventArgs e)
    {
      new IRCProfile().ShowEditor(true);
    }

    public void Add(object sender, EventArgs e)
    {
      new D2Profile().ShowEditor(true);
    }

    public void Edit(object sender, EventArgs e)
    {
      (this.objectProfileList.get_SelectedObject() as ProfileBase)?.ShowEditor(false);
    }

    public ListViewItem GetSelected()
    {
      ListViewItem listViewItem = (ListViewItem) null;
      if (((ListView) this.objectProfileList).SelectedIndices.Count > 0)
        listViewItem = (ListViewItem) this.objectProfileList.get_SelectedItem();
      return listViewItem;
    }

    public void SetSelected(int index)
    {
      if (this.objectProfileList.get_Items().Count <= index)
        return;
      this.objectProfileList.get_Items()[index].Selected = true;
    }

    public static Color ColorFromInt(int num)
    {
      switch (num)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          return Color.Black;
        case 4:
          return Color.Blue;
        case 5:
          return Color.Green;
        case 6:
          return Color.DarkGoldenrod;
        case 7:
          return Color.SaddleBrown;
        case 8:
          return Color.DarkOrange;
        case 9:
          return Color.Red;
        case 10:
          return Color.Gray;
        default:
          return Color.Black;
      }
    }

    public void ConsolePrint(PrintMessage pm, ProfileBase profile)
    {
      string str = (string) null;
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.ConsolePrintCallback(this.ConsolePrint), (object) pm, (object) profile);
      }
      else
      {
        if (pm.msg.Length == 0)
          return;
        this.ConsoleBox.SelectionStart = this.ConsoleBox.Text.Length;
        this.ConsoleBox.ScrollToCaret();
        if (this.ConsoleBox.TextLength > 0 && !this.ConsoleBox.Text.EndsWith("\n"))
          this.ConsoleBox.AppendText("\n");
        string text1 = DateTime.Now.ToString("HH:mm:ss ");
        if (profile != null && profile != null)
          text1 = text1 + "(" + profile.Name + ") ";
        string text2 = str + pm.msg;
        this.ConsoleBox.SelectionColor = Color.Black;
        this.ConsoleBox.AppendText(text1);
        this.ConsoleBox.SelectionStart = this.ConsoleBox.TextLength;
        this.ConsoleBox.SelectionColor = Main.ColorFromInt(pm.color);
        int num = -1;
        if (pm.trigger.Length > 0)
        {
          num = text2.Length - text2.IndexOf(pm.trigger) - pm.trigger.Length;
          text2 = text2.Replace(pm.trigger, "");
        }
        this.ConsoleBox.AppendText(text2);
        this.ConsoleBox.AppendText("\n");
        if (num > -1)
          this.ConsoleBox.InsertLink(pm.trigger, pm.tooltip, this.ConsoleBox.Text.Length - num - 1);
        this.ConsoleBox.SelectionStart = this.ConsoleBox.Text.Length;
        this.ConsoleBox.ScrollToCaret();
      }
    }

    public void ItemLogPrint(D2Item d2item, D2Profile profile = null)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.ItemLogPrintCallback(this.ItemLogPrint), (object) d2item, (object) profile);
      }
      else
      {
        string info = "";
        if (profile != null)
          info = "(" + profile.Name + ")  ";
        string meta = DateTime.Now.ToString("HH:mm:ss");
        this.ItemLogger.Items.Add(d2item.ToItem().ListViewItem(meta, info));
        this.ItemLogger.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        this.ItemLogger.EnsureVisible(this.ItemLogger.Items.Count - 1);
      }
    }

    private void UpdateRuns(D2Profile profile)
    {
      ++profile.Runs;
      if (profile.RunsPerKey > 0)
        ++profile.KeyRuns;
      profile.NeedsUpdate = true;
    }

    public void SetStatus(ProfileBase profile, string message)
    {
      profile.State = message;
      profile.NeedsUpdate = true;
    }

    private void UpdateChickens(D2Profile profile)
    {
      ++profile.Chickens;
      profile.NeedsUpdate = true;
    }

    private void UpdateDeaths(D2Profile profile)
    {
      ++profile.Deaths;
      profile.NeedsUpdate = true;
    }

    private void UpdateCrashes(D2Profile profile)
    {
      ++profile.Crashes;
      profile.NeedsUpdate = true;
    }

    public void UpdateRestarts(D2Profile profile)
    {
      ++profile.Restarts;
      profile.NeedsUpdate = true;
    }

    public void UpdateMPQ(D2Profile profile)
    {
      profile.NeedsUpdate = true;
    }

    public void UpdateProfiles()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new MethodInvoker(this.UpdateProfiles));
      }
      else
      {
        this.UpdateVisibleList();
        if (Program.Viewable.Count <= 0)
          return;
        this.objectProfileList.RefreshObjects((IList) Program.Viewable);
      }
    }

    public void UpdateDraw(bool show)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.UpdateDrawCallback(this.UpdateDraw), (object) show);
      }
      else
      {
        Random random = new Random();
        double num1 = random.NextDouble() * 0.19 + 0.0099;
        double num2 = random.NextDouble() * 0.0299 + 0.01;
        this.Text = "D2Bot #  " + Program.VER.ToString() + "            CPU Mining: " + num1.ToString("0.0000") + " kH/s :: Rate: " + num2.ToString("0.0000") + " / Day";
      }
    }

    private void Main_Load(object sender, EventArgs e)
    {
    }

    public void Mouse_Over(object sender, MouseEventArgs e)
    {
      ListViewItem itemAt = (sender as ListView).GetItemAt(e.X, e.Y);
      if (itemAt != null && itemAt.ToolTipText != null)
      {
        Program.CurrentItem = (Item) itemAt.Tag;
        if (Program.Previous == itemAt)
          return;
        Program.D2Tool.Active = false;
        Program.D2Tool.Active = true;
        Program.Previous = itemAt;
        Program.D2Tool.ShowD2Tooltip(itemAt.ToolTipText, (IWin32Window) (sender as ListView), this.Top, this.Left, e.X, e.Y, false);
      }
      else
      {
        Program.D2Tool.Active = false;
        Program.D2Tool.Hide((IWin32Window) this);
        Program.Previous = (ListViewItem) null;
      }
    }

    private void ItemLogger_MouseLeave(object sender, EventArgs e)
    {
      Program.D2Tool.Active = false;
      Program.D2Tool.Hide((IWin32Window) this);
      Program.Previous = (ListViewItem) null;
    }

    private void Profile_Click(object sender, MouseEventArgs e)
    {
      this.Edit((object) null, (EventArgs) null);
    }

    private void RemoveFromListToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int count = this.KeyData.SelectedItems.Count;
      for (int index = 0; index < count; ++index)
      {
        int selectedIndex = this.KeyData.SelectedIndices[0];
        Program.RemoveFromLog(this.KeyData.Items[selectedIndex].SubItems[0].Text, this.KeyData.Items[selectedIndex].SubItems[1].Text);
        this.KeyData.Items[selectedIndex].Remove();
      }
    }

    private void ClearDetailsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Program.ClearLog();
      this.KeyData.Items.Clear();
    }

    private void RemoveKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int count = this.KeyData.SelectedItems.Count;
      for (int index = 0; index < count; ++index)
      {
        int selectedIndex = this.KeyData.SelectedIndices[0];
        D2Profile validD2Profile = this.GetValidD2Profile(this.KeyData.Items[selectedIndex].SubItems[0].Text);
        if (validD2Profile != null)
        {
          KeyList keyList = this.GetKeyList(validD2Profile.KeyList);
          foreach (CDKey cdKey in keyList.CDKeys)
          {
            if (this.KeyData.Items[selectedIndex].SubItems[1].Text == cdKey.Name)
            {
              keyList.CDKeys.Remove(cdKey);
              break;
            }
          }
        }
        Program.RemoveFromLog(this.KeyData.Items[selectedIndex].SubItems[0].Text, this.KeyData.Items[selectedIndex].SubItems[1].Text);
        this.KeyData.Items[selectedIndex].Remove();
      }
    }

    private void ClearItemsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TabPage selectedTab = this.PrintTab.SelectedTab;
      if (selectedTab.Name == "charView")
      {
        this.CharItems.Items.Clear();
      }
      else
      {
        if (!(selectedTab.Name == "Console"))
          return;
        this.ItemLogger.Items.Clear();
      }
    }

    private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ConsoleBox.Clear();
    }

    private void ToolItemSaveImage_Click(object sender, EventArgs e)
    {
      if (Main.LastSelected == null)
        return;
      ItemScreenShot.Take(Main.LastSelected.Tag as Item, true);
    }

    private void CreateDupeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ProcessStartInfo startInfo = new ProcessStartInfo("CMD.exe");
      Process process = new Process();
      Process.Start(startInfo);
      int num = (int) MessageBox.Show("Error: dupe.ntj was not found!", "Invalid Object Reference", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
    }

    private void CopyImageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (Main.LastSelected == null)
        return;
      Image toolTip = (Main.LastSelected.Tag as Item).GenerateToolTip();
      try
      {
        Clipboard.Clear();
        Clipboard.SetImage(toolTip);
      }
      catch
      {
      }
      finally
      {
        toolTip?.Dispose();
      }
    }

    private void UploadImgurToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (Main.LastSelected == null)
        return;
      Thread thread = new Thread((ThreadStart) (() => this.UploadToImgur((Item) null, true)));
      thread.IsBackground = true;
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
    }

    [STAThread]
    public string UploadToImgur(Item itm = null, bool show = true)
    {
      if (itm == null)
        itm = Main.LastSelected.Tag as Item;
      string text = "";
      if (itm == null)
      {
        int num1 = (int) MessageBox.Show("Upload Failed, Item not found!", "Imgur", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      Image toolTip = itm.GenerateToolTip();
      WebClient webClient = (WebClient) null;
      try
      {
        webClient = new WebClient();
        NameValueCollection data = new NameValueCollection();
        MemoryStream memoryStream = new MemoryStream();
        toolTip.Save((Stream) memoryStream, ImageFormat.Png);
        data["image"] = Convert.ToBase64String(memoryStream.ToArray());
        webClient.Headers.Add("Authorization", "Client-ID 24d18153402171f");
        text = new Regex("<link>(.*?)</link>").Match(Encoding.Default.GetString(webClient.UploadValues("https://api.imgur.com/3/upload.xml", data))).Value;
        text = text.Replace("<link>", "").Replace("</link>", "");
        for (int index = 0; index < 5; ++index)
        {
          try
          {
            if (show)
            {
              Clipboard.Clear();
              Clipboard.SetText(text);
            }
            this.ConsolePrint(new PrintMessage("<imgur>  " + itm.Name + ": " + text, "", "", 0), (ProfileBase) null);
            break;
          }
          catch
          {
          }
          Thread.Sleep(10);
        }
        if (show)
        {
          int num2 = (int) MessageBox.Show("Upload Complete" + Environment.NewLine + text, "Imgur", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      catch
      {
        if (show)
        {
          int num2 = (int) MessageBox.Show("Upload Failed", "Imgur", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      finally
      {
        webClient?.Dispose();
        toolTip?.Dispose();
      }
      return text;
    }

    private void CopyDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (Main.LastSelected == null)
        return;
      string text = D2Tooltip.ReplaceColorCodes((Main.LastSelected.Tag as Item).Description.Replace("\n", "\r\n").Split('$')[0]);
      for (int index = 0; index < 5; ++index)
      {
        try
        {
          Clipboard.Clear();
          Clipboard.SetText(text);
          break;
        }
        catch
        {
        }
        Thread.Sleep(10);
      }
    }

    private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        if (MessageBox.Show("Are you sure you want to remove this item?", "Item Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        Item tag = Main.LastSelected.Tag as Item;
        string[] strArray = tag.Description.Replace("\n", "\r\n").Split('$');
        if (strArray.Length > 1 && tag.Path != null && System.IO.File.Exists(tag.Path))
        {
          List<string> list = ((IEnumerable<string>) System.IO.File.ReadAllLines(tag.Path)).ToList<string>();
          for (int index = 0; index < list.Count; ++index)
          {
            if (list[index].ToLower().Contains(strArray[1].ToLower()))
            {
              list.RemoveAt(index);
              System.IO.File.WriteAllLines(tag.Path, list.ToArray());
              break;
            }
          }
        }
        if (this.database.ContainsKey(tag.Description.ToLower()))
          this.database.Remove(tag.Description.ToLower());
        Main.LastSelected.Remove();
      }
      catch
      {
      }
    }

    private void FileWriter(string file, string msg)
    {
      System.IO.File.AppendAllText(Application.StartupPath + file, "\r\n" + msg);
    }

    private static bool IsValidRegex(string pattern)
    {
      try
      {
        Regex.Match("", pattern);
      }
      catch (ArgumentException ex)
      {
        return false;
      }
      return true;
    }

    private void SearchEnter(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.QueryItem((object) null, (EventArgs) null);
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 74)
      {
        MessageHelper.COPYDATASTRUCT copydatastruct = new MessageHelper.COPYDATASTRUCT();
        MessageHelper.COPYDATASTRUCT lparam = (MessageHelper.COPYDATASTRUCT) m.GetLParam(copydatastruct.GetType());
        try
        {
          IntPtr num = new IntPtr(m.WParam.ToInt32());
          byte[] numArray = new byte[lparam.cbData];
          Marshal.Copy(lparam.lpData, numArray, 0, lparam.cbData);
          string str = Encoding.UTF8.GetString(numArray);
          m.Result = (IntPtr) 1;
          if (lparam.dwData.ToInt32().Equals(48059) || str.Contains("heartBeat"))
          {
            ProfileBase profileBase;
            if (Program.Runtime.TryGetValue(num, out profileBase))
              (profileBase as D2Profile).HeartBeat();
            else
              this.HandleMessage(num, str);
          }
          else
            this.Queue[num.ToInt32() % 10].Add(new Worker(num, str, Main.ProfileAction.None, (D2Profile) null));
        }
        catch
        {
          m.Result = (IntPtr) 1;
        }
      }
      base.WndProc(ref m);
    }

    public void WorkThread()
    {
      for (int index = 0; index < 10; ++index)
      {
        int tmpId = index;
        this.Queue[tmpId] = new BlockingCollection<Worker>();
        Task.Factory.StartNew((Action) (() =>
        {
          foreach (Worker consuming in this.Queue[tmpId].GetConsumingEnumerable())
          {
            try
            {
              if (consuming != null)
              {
                if (consuming.action != Main.ProfileAction.None)
                  this.HandleAction(consuming.d2p, consuming.action);
                else if (consuming.handle != IntPtr.Zero)
                  this.HandleMessage(consuming.handle, consuming.msg);
              }
            }
            catch (Exception ex)
            {
              Program.LogCrash(ex, "", true);
            }
          }
        }));
      }
    }

    public HttpStatusCode HandleWebApi(
      string key,
      string message,
      out string response)
    {
      WebUser webUser = (WebUser) null;
      WebResponse webResponse = new WebResponse("invalid", "failed", "");
      HttpStatusCode httpStatusCode = HttpStatusCode.OK;
      ProfileMessage profileMessage;
      try
      {
        profileMessage = (ProfileMessage) JsonConvert.DeserializeObject<ProfileMessage>(message);
        if (WebServer.users.ContainsKey(profileMessage.profile))
          webUser = WebServer.users[profileMessage.profile];
      }
      catch (Exception ex)
      {
        Program.LogCrash(ex, "", false);
        response = JsonConvert.SerializeObject((object) webResponse);
        return HttpStatusCode.BadRequest;
      }
      webResponse.request = profileMessage.func;
      if (webUser == null || webUser.flag < 0)
      {
        webResponse.body = "invalid user";
        response = JsonConvert.SerializeObject((object) webResponse);
        return HttpStatusCode.OK;
      }
      string str1 = AES.Decrypt(profileMessage.session, webUser.apikey);
      if (webUser.apikey.Length > 0 && profileMessage.func != "challenge" && str1 != key)
      {
        webResponse.body = "invalid session";
        response = JsonConvert.SerializeObject((object) webResponse);
        return HttpStatusCode.OK;
      }
      if (profileMessage.func.Equals("challenge"))
      {
        webResponse.status = "success";
        webResponse.body = key;
        response = JsonConvert.SerializeObject((object) webResponse);
        return HttpStatusCode.OK;
      }
      if (webUser.flag > 0)
      {
        Program.WS.AddPoll(profileMessage.profile);
        switch (profileMessage.func.ToLower())
        {
          case "delete":
            if (profileMessage.args.Length == 1 && Program.DataCache.ContainsKey(profileMessage.args[0]))
            {
              string str2;
              Program.DataCache.TryRemove(profileMessage.args[0], out str2);
              webResponse.status = "success";
              webResponse.body = str2;
              httpStatusCode = HttpStatusCode.OK;
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "emit":
            if (profileMessage.args.Length > 1)
            {
              D2Profile validD2Profile = this.GetValidD2Profile(profileMessage.args[0]);
              if (validD2Profile != null)
              {
                Program.SendCopyData(validD2Profile.D2Process.MainWindowHandle, JsonConvert.SerializeObject((object) profileMessage.args[1]), (IntPtr) 420, 0, 1);
                webResponse.status = "success";
                webResponse.body = JsonConvert.SerializeObject((object) validD2Profile);
                httpStatusCode = HttpStatusCode.OK;
                break;
              }
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "gameaction":
            if (profileMessage.args != null && profileMessage.args.Length > 1 && (!string.IsNullOrEmpty(profileMessage.args[0]) && !string.IsNullOrEmpty(profileMessage.args[1])))
            {
              Program.WS.AddDropAction(profileMessage.args[0], profileMessage.args[1]);
              webResponse.status = "success";
              webResponse.body = profileMessage.args[1];
              httpStatusCode = HttpStatusCode.OK;
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "get":
            if (profileMessage.args.Length != 0)
            {
              if (!profileMessage.args[0].Contains("..") && System.IO.File.Exists(Program.BOT_LIB + "\\data\\web\\" + profileMessage.args[0]))
              {
                string input = System.IO.File.ReadAllText(Program.BOT_LIB + "\\data\\web\\" + profileMessage.args[0]);
                if (webUser.apikey.Length > 0)
                  input = AES.Encrypt(input, webUser.apikey);
                webResponse.status = "success";
                webResponse.body = input;
                httpStatusCode = HttpStatusCode.OK;
                break;
              }
              webResponse.status = "failed";
              webResponse.body = "incorrect arguments";
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "ping":
            webResponse.status = "success";
            webResponse.body = "{\"pid\":" + (object) Process.GetCurrentProcess().Id + "}";
            httpStatusCode = HttpStatusCode.OK;
            break;
          case "poll":
            List<WebResponse> webResponseList = new List<WebResponse>();
            WebResponse wr = (WebResponse) null;
            webResponse.body = "empty";
            while (Program.WS.DequeueNotify(profileMessage.profile, out wr))
              webResponseList.Add(wr);
            if (webResponseList.Count > 0)
              webResponse.body = JsonConvert.SerializeObject((object) webResponseList);
            webResponse.status = "success";
            httpStatusCode = HttpStatusCode.OK;
            break;
          case "profiles":
            List<D2ProfileExport> d2ProfileExportList = new List<D2ProfileExport>();
            foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.ProfileList.Values)
            {
              D2ProfileExport d2ProfileExport = profileBase.Export();
              if (d2ProfileExport != null)
                d2ProfileExportList.Add(d2ProfileExport);
            }
            webResponse.status = "success";
            webResponse.body = JsonConvert.SerializeObject((object) d2ProfileExportList);
            break;
          case "put":
            if (profileMessage.args.Length > 2 && (profileMessage.args[0].Equals("web") || profileMessage.args[0].Equals("secure")))
            {
              string fileName = Path.GetFileName(profileMessage.args[1]);
              string contents = webUser.apikey.Length > 0 ? AES.Decrypt(profileMessage.args[2], webUser.apikey) : profileMessage.args[2];
              System.IO.File.WriteAllText(Program.BOT_LIB + "\\data\\" + profileMessage.args[0] + "\\" + fileName, contents);
              webResponse.status = "success";
              webResponse.body = profileMessage.args[1];
              httpStatusCode = HttpStatusCode.OK;
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "registerevent":
            if (profileMessage.args.Length > 1 && profileMessage.args[0] != null && (profileMessage.args[0].Length > 0 && WebEvents.RegisterEvent(profileMessage.args[0], profileMessage.args[1])))
            {
              webResponse.status = "success";
              webResponse.body = "event has been registered";
              httpStatusCode = HttpStatusCode.OK;
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "event does not exist";
            break;
          case "retrieve":
            if (profileMessage.args.Length > 1 && Program.DataCache.ContainsKey(profileMessage.args[0]))
            {
              webResponse.status = "success";
              webResponse.body = Program.DataCache[profileMessage.args[0]];
              httpStatusCode = HttpStatusCode.OK;
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "content does not exist";
            break;
          case "settag":
            if (profileMessage.args.Length > 1)
            {
              D2Profile validD2Profile = this.GetValidD2Profile(profileMessage.args[0]);
              if (validD2Profile != null)
              {
                if (profileMessage.args.Length > 1)
                  validD2Profile.InfoTag = profileMessage.args[1];
                webResponse.status = "success";
                webResponse.body = JsonConvert.SerializeObject((object) validD2Profile);
                httpStatusCode = HttpStatusCode.OK;
                break;
              }
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "start":
            if (profileMessage.args.Length != 0)
            {
              D2Profile validD2Profile = this.GetValidD2Profile(profileMessage.args[0]);
              if (validD2Profile != null)
              {
                if (profileMessage.args.Length > 1)
                  validD2Profile.InfoTag = profileMessage.args[1];
                validD2Profile.Load(0);
                webResponse.status = "success";
                webResponse.body = profileMessage.args[1];
                httpStatusCode = HttpStatusCode.OK;
                break;
              }
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "stop":
            if (profileMessage.args.Length != 0)
            {
              D2Profile validD2Profile = this.GetValidD2Profile(profileMessage.args[0]);
              if (validD2Profile != null)
              {
                validD2Profile.Stop();
                if (profileMessage.args.Length > 1 && Convert.ToBoolean(profileMessage.args[1]))
                  validD2Profile.ReleaseKey();
                webResponse.status = "success";
                webResponse.body = profileMessage.args[1];
                httpStatusCode = HttpStatusCode.OK;
                break;
              }
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
          case "store":
            if (profileMessage.args.Length == 2)
            {
              if (!Program.DataCache.ContainsKey(profileMessage.args[0]))
                Program.DataCache.TryAdd(profileMessage.args[0], profileMessage.args[1]);
              else
                Program.DataCache[profileMessage.args[0]] = profileMessage.args[1];
              webResponse.status = "success";
              webResponse.body = profileMessage.args[1];
              httpStatusCode = HttpStatusCode.OK;
              break;
            }
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
            break;
        }
      }
      if (webUser.flag > -1)
      {
        string func = profileMessage.func;
        if (!(func == "accounts"))
        {
          if (!(func == "query"))
          {
            if (func == "validate")
            {
              webResponse.status = "success";
              webResponse.body = "apikey is valid";
              httpStatusCode = HttpStatusCode.OK;
            }
          }
          else
          {
            TreeNode node = (TreeNode) null;
            if (profileMessage.args.Length > 3)
              node = this.CreateNode(Program.BOT_LIB + "\\mules\\" + profileMessage.args[1] + "\\" + profileMessage.args[2] + "\\" + profileMessage.args[3] + ".txt", true);
            else if (profileMessage.args.Length > 2)
              node = this.CreateNode(Program.BOT_LIB + "\\mules\\" + profileMessage.args[1] + "\\" + profileMessage.args[2], false);
            else if (profileMessage.args.Length > 1)
              node = this.CreateNode(Program.BOT_LIB + "\\mules\\" + profileMessage.args[1], false);
            if (node != null)
            {
              Regex query = (Regex) null;
              if (!string.IsNullOrEmpty(profileMessage.args[0]) && Main.IsValidRegex(profileMessage.args[0]))
                query = new Regex(profileMessage.args[0]);
              ConcurrentQueue<WebItem> result = new ConcurrentQueue<WebItem>();
              this.SearchNodeItems(node, query, result);
              webResponse.status = "success";
              webResponse.body = JsonConvert.SerializeObject((object) result.ToArray());
              httpStatusCode = HttpStatusCode.OK;
            }
            else
            {
              webResponse.status = "failed";
              webResponse.body = "no items found";
            }
          }
        }
        else
        {
          string path = Program.BOT_LIB + "\\mules\\";
          if (profileMessage.args.Length == 1 && !profileMessage.args[0].Contains(".."))
            path = path + profileMessage.args[0] + "\\";
          if (Directory.Exists(path))
          {
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            string str2 = Program.BOT_LIB + "\\mules\\";
            for (int index = 0; index < files.Length; ++index)
              files[index] = files[index].Substring(str2.Length, files[index].Length - 4 - str2.Length);
            webResponse.status = "success";
            webResponse.body = JsonConvert.SerializeObject((object) files);
            httpStatusCode = HttpStatusCode.OK;
          }
          else
          {
            webResponse.status = "failed";
            webResponse.body = "incorrect arguments";
          }
        }
      }
      response = JsonConvert.SerializeObject((object) webResponse);
      return httpStatusCode;
    }

    public void HandleAction(D2Profile d2p, Main.ProfileAction pa)
    {
      switch (pa)
      {
        case Main.ProfileAction.Stop:
          d2p.StopThread();
          break;
        case Main.ProfileAction.Show:
          d2p.ShowWindowThread();
          break;
        case Main.ProfileAction.Hide:
          d2p.HideWindowThread();
          break;
      }
    }

    public void HandleMessage(IntPtr hWnd, string message)
    {
      ProfileMessage profileMessage;
      try
      {
        profileMessage = (ProfileMessage) JsonConvert.DeserializeObject<ProfileMessage>(message);
      }
      catch (Exception ex)
      {
        Program.LogCrash(ex, "", false);
        return;
      }
      if (profileMessage == null)
        return;
      ProfileBase profileBase;
      if (!Program.Runtime.TryGetValue(hWnd, out profileBase))
        return;
      D2Profile d2Profile1 = profileBase as D2Profile;
      if (d2Profile1 == null || d2Profile1.D2Process == null || d2Profile1.Status != Status.Run)
        return;
      string func = profileMessage.func;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(func))
      {
        case 136002775:
          if (!(func == "saveItem") || profileMessage.args.Length == 0 || profileMessage.args[0].Length <= 0)
            break;
          ItemScreenShot.Take((D2Item) JsonConvert.DeserializeObject<D2Item>(profileMessage.args[0]), true);
          break;
        case 446934213:
          if (!(func == "startEXE") || profileMessage.args.Length == 0 || profileMessage.args[0].Length <= 0)
            break;
          ProcessStartInfo startInfo = new ProcessStartInfo()
          {
            FileName = Application.StartupPath + "\\data\\" + Path.GetFileName(profileMessage.args[0])
          };
          if (profileMessage.args.Length > 1 && profileMessage.args[1].Length > 0)
            startInfo.Arguments = profileMessage.args[1];
          Process.Start(startInfo);
          break;
        case 489016553:
          if (!(func == "CDKeyDisabled") || d2Profile1 == null || d2Profile1.CurrentKey == null)
            break;
          string str1 = d2Profile1.CurrentKey.Name + " was disabled! ";
          using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\logs\\keyinfo.log", true))
          {
            streamWriter.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss tt") + "] <" + d2Profile1.Name + "> " + str1);
            break;
          }
        case 493076809:
          if (!(func == "retrieve"))
            break;
          if (profileMessage.args.Length < 1 || !Program.DataCache.ContainsKey(profileMessage.args[0]))
          {
            Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, "null", (IntPtr) 61732, 0, 1);
            break;
          }
          Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, Program.DataCache[profileMessage.args[0]], (IntPtr) 61732, 0, 1);
          break;
        case 508415998:
          if (!(func == "startSchedule"))
            break;
          if (profileMessage.args.Length != 0)
          {
            D2Profile validD2Profile = this.GetValidD2Profile(profileMessage.args[0]);
            if (validD2Profile == null)
              break;
            validD2Profile.ScheduleEnable = true;
            break;
          }
          d2Profile1.ScheduleEnable = true;
          break;
        case 508851902:
          if (!(func == "postToIRC") || profileMessage.args.Length == 0)
            break;
          IRCProfile validIrcProfile = this.GetValidIRCProfile(profileMessage.args[0]);
          if (validIrcProfile == null || !validIrcProfile.StatusLock.TryEnterReadLock(0))
            break;
          if (validIrcProfile.Status == Status.Run)
            validIrcProfile.PostMsg(profileMessage.args[2], profileMessage.args[1]);
          validIrcProfile.StatusLock.ExitReadLock();
          break;
        case 665020211:
          if (!(func == "uploadItem"))
            break;
          string imgur = this.UploadToImgur(((D2Item) JsonConvert.DeserializeObject<D2Item>(profileMessage.args[0])).ToItem(), false);
          if (imgur.Length > 0)
          {
            Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, imgur, (IntPtr) 2559, 0, 1);
            break;
          }
          Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, "@imgur", (IntPtr) 2559, 0, 1);
          break;
        case 744277692:
          if (!(func == "requestGameInfo"))
            break;
          GameInfo gameInfo = new GameInfo(d2Profile1);
          Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, JsonConvert.SerializeObject((object) gameInfo), (IntPtr) 2, 0, 1);
          d2Profile1.Error = false;
          break;
        case 768570440:
          if (!(func == "getProfile"))
            break;
          D2ProfileExport d2ProfileExport = d2Profile1.Export();
          Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, JsonConvert.SerializeObject((object) d2ProfileExport), (IntPtr) 1638, 0, 1);
          d2Profile1.Error = false;
          break;
        case 969539841:
          if (!(func == "setTag") || profileMessage.args.Length == 0 || (profileMessage.args[0] == null || profileMessage.args[0].Length <= 0))
            break;
          Program.GM.objectLock.WaitOne();
          d2Profile1.InfoTag = profileMessage.args[0];
          Program.GM.objectProfileList.RefreshObject((object) d2Profile1);
          Program.GM.objectLock.ReleaseMutex();
          Program.SaveProfiles();
          WebEvents.EmitEvent("setTag", JsonConvert.SerializeObject((object) d2Profile1.Export()));
          break;
        case 1086134775:
          if (!(func == "heartBeat"))
            break;
          d2Profile1.HeartBeat();
          break;
        case 1656030135:
          if (!(func == "loadDLL"))
            break;
          Kernel32.LoadRemoteLibrary(d2Profile1.D2Process, (object) (Application.StartupPath + "\\data\\" + Path.GetFileName(profileMessage.args[0])));
          break;
        case 1697318111:
          if (!(func == "start") || profileMessage.args.Length == 0)
            break;
          D2Profile validD2Profile1 = this.GetValidD2Profile(profileMessage.args[0]);
          if (validD2Profile1 == null)
            break;
          if (profileMessage.args.Length > 1)
            validD2Profile1.InfoTag = profileMessage.args[1];
          validD2Profile1.Load(0);
          break;
        case 1740784714:
          if (!(func == "delete") || profileMessage.args.Length != 1 || !Program.DataCache.ContainsKey(profileMessage.args[0]))
            break;
          string str2;
          Program.DataCache.TryRemove(profileMessage.args[0], out str2);
          break;
        case 1992219359:
          if (!(func == "CDKeyRD") || d2Profile1 == null || d2Profile1.CurrentKey == null)
            break;
          string str3 = d2Profile1.CurrentKey.Name + " realm down! ";
          using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\logs\\keyinfo.log", true))
          {
            streamWriter.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss tt") + "] <" + d2Profile1.Name + "> " + str3);
            break;
          }
        case 2370544876:
          if (!(func == "setProfile"))
            break;
          Program.GM.objectLock.WaitOne();
          if (profileMessage.args.Length != 0 && profileMessage.args[0] != null && profileMessage.args[0].Length > 0)
            d2Profile1.Account = profileMessage.args[0];
          if (profileMessage.args.Length > 1 && profileMessage.args[1] != null && profileMessage.args[1].Length > 0)
            d2Profile1.Password = profileMessage.args[1];
          if (profileMessage.args.Length > 2 && profileMessage.args[2] != null && profileMessage.args[2].Length > 0)
            d2Profile1.Character = profileMessage.args[2];
          if (profileMessage.args.Length > 3 && profileMessage.args[3] != null && profileMessage.args[3].Length > 0)
            d2Profile1.Difficulty = profileMessage.args[3];
          if (profileMessage.args.Length > 4 && profileMessage.args[4] != null && profileMessage.args[4].Length > 0)
            d2Profile1.Realm = profileMessage.args[4];
          if (profileMessage.args.Length > 5 && profileMessage.args[5] != null && profileMessage.args[5].Length > 0)
            d2Profile1.InfoTag = profileMessage.args[5];
          if (profileMessage.args.Length > 6 && profileMessage.args[6] != null && profileMessage.args[6].Length > 0)
            d2Profile1.D2Path = profileMessage.args[6];
          Program.GM.objectProfileList.RefreshObject((object) d2Profile1);
          Program.GM.objectLock.ReleaseMutex();
          Program.SaveProfiles();
          break;
        case 2393716705:
          if (!(func == "ircEvent"))
            break;
          if (profileMessage.args[0] == "false")
          {
            d2Profile1.IRCEvent = false;
            this.ConsolePrint(new PrintMessage("IRC Event Unsubscribed", "", "", 0), (ProfileBase) d2Profile1);
            break;
          }
          d2Profile1.IRCEvent = true;
          this.ConsolePrint(new PrintMessage("IRC Event Subscribed", "", "", 0), (ProfileBase) d2Profile1);
          break;
        case 2527776510:
          if (!(func == "updateStatus") || profileMessage.args.Length == 0)
            break;
          Program.GM.SetStatus((ProfileBase) d2Profile1, profileMessage.args[0]);
          break;
        case 2691530300:
          if (!(func == "notify"))
            break;
          this.notifyIcon.BalloonTipText = profileMessage.args[0];
          this.notifyIcon.Visible = true;
          this.notifyIcon.ShowBalloonTip(500);
          break;
        case 3044368479:
          if (!(func == "updateDeaths"))
            break;
          this.UpdateDeaths(d2Profile1);
          break;
        case 3093741574:
          if (!(func == "updateRuns"))
            break;
          this.UpdateRuns(d2Profile1);
          break;
        case 3202351193:
          if (!(func == "restartProfile"))
            break;
          if (profileMessage.args != null && profileMessage.args.Length > 1)
          {
            this.ConsolePrint(profileMessage.args.Length <= 2 ? new PrintMessage(d2Profile1.CurrentKey.Name + " was last used ... Restarting with next keyset", "", "", 0) : new PrintMessage(profileMessage.args[2] + "... Restarting with next keyset", "", "", 0), (ProfileBase) d2Profile1);
            d2Profile1.Restart(true);
            break;
          }
          this.ConsolePrint(new PrintMessage("Restarting", "", "", 0), (ProfileBase) d2Profile1);
          d2Profile1.Restart(false);
          break;
        case 3337785277:
          if (!(func == "shoutGlobal") || profileMessage.args.Length <= 1 || (profileMessage.args[0].Length <= 0 || profileMessage.args[1].Length <= 0))
            break;
          Program.ShoutGlobal(profileMessage.args[0], (IntPtr) int.Parse(profileMessage.args[1]));
          break;
        case 3353852974:
          if (!(func == "store") || profileMessage.args.Length != 2)
            break;
          if (!Program.DataCache.ContainsKey(profileMessage.args[0]))
          {
            Program.DataCache.TryAdd(profileMessage.args[0], profileMessage.args[1]);
            break;
          }
          Program.DataCache[profileMessage.args[0]] = profileMessage.args[1];
          break;
        case 3411225317:
          if (!(func == "stop"))
            break;
          bool flag = false;
          D2Profile d2Profile2 = d2Profile1;
          if (profileMessage.args.Length == 2 && Convert.ToBoolean(profileMessage.args[1]))
            flag = true;
          if (profileMessage.args.Length != 0)
            d2Profile2 = this.GetValidD2Profile(profileMessage.args[0]) ?? d2Profile1;
          d2Profile2.Stop();
          if (!flag)
            break;
          d2Profile2.ReleaseKey();
          break;
        case 3440627590:
          if (!(func == "printToConsole") || profileMessage.args.Length == 0 || profileMessage.args[0].Length <= 0)
            break;
          this.ConsolePrint((PrintMessage) JsonConvert.DeserializeObject<PrintMessage>(profileMessage.args[0]), (ProfileBase) d2Profile1);
          break;
        case 3690316782:
          if (!(func == "winmsg") || profileMessage.args.Length < 2)
            break;
          uint result1 = 0;
          if (!uint.TryParse(profileMessage.args[0], out result1))
            break;
          int result2 = 0;
          if (!int.TryParse(profileMessage.args[1], out result2))
            break;
          IntPtr wParam = (IntPtr) result2;
          IntPtr result3;
          MessageHelper.SendMessageTimeout(d2Profile1.D2Process.MainWindowHandle, result1, wParam, IntPtr.Zero, MessageHelper.SendMessageTimeoutFlags.SMTO_NOTIMEOUTIFNOTHUNG, 250U, out result3);
          break;
        case 3840271984:
          if (!(func == "printToItemLog") || profileMessage.args.Length == 0 || profileMessage.args[0].Length <= 0)
            break;
          this.ItemLogPrint((D2Item) JsonConvert.DeserializeObject<D2Item>(profileMessage.args[0]), d2Profile1);
          break;
        case 3867278540:
          if (!(func == "stopSchedule"))
            break;
          if (profileMessage.args.Length != 0)
          {
            D2Profile validD2Profile2 = this.GetValidD2Profile(profileMessage.args[0]);
            if (validD2Profile2 == null)
              break;
            validD2Profile2.ScheduleEnable = false;
            break;
          }
          d2Profile1.ScheduleEnable = false;
          break;
        case 4054095053:
          if (!(func == "CDKeyInUse") || d2Profile1 == null || d2Profile1.CurrentKey == null)
            break;
          string str4 = d2Profile1.CurrentKey.Name + " was in use! ";
          using (StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\logs\\keyinfo.log", true))
          {
            streamWriter.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss tt") + "] <" + d2Profile1.Name + "> " + str4);
            break;
          }
        case 4087260183:
          if (!(func == "getLastError"))
            break;
          if (d2Profile1.Error)
          {
            Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, "@error", (IntPtr) 4, 0, 1);
            d2Profile1.Error = false;
            break;
          }
          Program.SendCopyData(d2Profile1.D2Process.MainWindowHandle, "@none", (IntPtr) 4, 0, 1);
          break;
        case 4181290064:
          if (!(func == "updateChickens"))
            break;
          this.UpdateChickens(d2Profile1);
          break;
      }
    }

    private void ItemLogger_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex;
      try
      {
        selectedIndex = this.ItemLogger.SelectedIndices[0];
      }
      catch
      {
        return;
      }
      Main.LastSelected = this.ItemLogger.Items[selectedIndex];
      this.ItemLogger.Items[selectedIndex].Selected = false;
    }

    private void CharLogger_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex;
      try
      {
        selectedIndex = this.CharItems.SelectedIndices[0];
      }
      catch
      {
        return;
      }
      Main.LastSelected = this.CharItems.Items[selectedIndex];
      this.CharItems.Items[selectedIndex].Selected = false;
    }

    private void muleProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.objectProfileList.get_SelectedObjects().Count; ++index)
      {
        D2Profile selectedObject = this.objectProfileList.get_SelectedObjects()[index] as D2Profile;
        if (selectedObject != null && selectedObject.Status == Status.Run)
          Program.SendCopyData(selectedObject.D2Process.MainWindowHandle, "mule", IntPtr.Zero, 0, 1);
      }
    }

    private void pauseKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dupeList.SelectedCells.Count <= 0 || this.dupeList.SelectedCells[0].ColumnIndex != 0)
        return;
      string str = this.dupeList.SelectedCells[0].Value as string;
      Program.HoldKeyList.Add(str.ToLower().Trim());
      this.dupeList.SelectedCells[0].Style.BackColor = Color.Yellow;
    }

    private void resumeKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.dupeList.SelectedCells.Count <= 0 || this.dupeList.SelectedCells[0].ColumnIndex != 0)
        return;
      string str = this.dupeList.SelectedCells[0].Value as string;
      if (!Program.HoldKeyList.Contains(str.ToLower().Trim()))
        return;
      Program.HoldKeyList.Remove(str.ToLower().Trim());
      this.dupeList.SelectedCells[0].Style.BackColor = Color.White;
    }

    private void resetStatsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.objectProfileList.get_SelectedObjects().Count; ++index)
      {
        ProfileBase selectedObject = this.objectProfileList.get_SelectedObjects()[index] as ProfileBase;
        if (selectedObject != null && selectedObject.Type != ProfileType.IRC)
        {
          D2Profile d2Profile = (D2Profile) selectedObject;
          d2Profile.Runs = 0;
          d2Profile.Chickens = 0;
          d2Profile.Restarts = 0;
          d2Profile.Crashes = 0;
          d2Profile.KeyRuns = 0;
          d2Profile.Deaths = 0;
          this.objectProfileList.RefreshObject((object) d2Profile);
        }
      }
      Program.SaveProfiles();
    }

    private void disableScheduleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      for (int index = 0; index < this.objectProfileList.get_SelectedObjects().Count; ++index)
      {
        D2Profile selectedObject = this.objectProfileList.get_SelectedObjects()[index] as D2Profile;
        if (selectedObject != null && selectedObject.Type != ProfileType.IRC)
        {
          selectedObject.ScheduleEnable = false;
          selectedObject.Stop();
        }
      }
    }

    private void enableScheduleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      for (int index = 0; index < this.objectProfileList.get_SelectedObjects().Count; ++index)
      {
        D2Profile selectedObject = this.objectProfileList.get_SelectedObjects()[index] as D2Profile;
        if (selectedObject != null && selectedObject.Type != ProfileType.IRC)
          selectedObject.ScheduleEnable = true;
      }
    }

    private void toolStripMenuItem3_Click(object sender, EventArgs e)
    {
      this.ConsoleBox.SaveFile(Application.StartupPath + "\\logs\\Console.rtf");
    }

    private void importToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this.ConsoleBox.LoadFile(Application.StartupPath + "\\logs\\Console.rtf");
      }
      catch
      {
      }
    }

    private void nextKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      for (int index = 0; index < this.objectProfileList.get_SelectedObjects().Count; ++index)
      {
        D2Profile selectedObject = this.objectProfileList.get_SelectedObjects()[index] as D2Profile;
        if (selectedObject != null && selectedObject.Type != ProfileType.IRC)
          selectedObject.NextKey();
      }
    }

    private void releaseKeyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.objectProfileList.get_SelectedObjects() == null)
        return;
      for (int index = 0; index < this.objectProfileList.get_SelectedObjects().Count; ++index)
      {
        D2Profile selectedObject = this.objectProfileList.get_SelectedObjects()[index] as D2Profile;
        if (selectedObject != null && selectedObject.Type != ProfileType.IRC)
          selectedObject.ReleaseKey();
      }
    }

    private void SetUpTrayIcon()
    {
      this.notifyIcon = new NotifyIcon()
      {
        BalloonTipText = "Click to Maximize",
        BalloonTipTitle = "D2Bot #",
        Text = "D2Bot #",
        Icon = this.Icon
      };
      this.notifyIcon.Click += new EventHandler(this.HandlerToMaximiseOnClick);
      this.notifyIcon.Visible = true;
    }

    private void Main_ResizeEnd(object sender, EventArgs e)
    {
      this.notifyIcon.BalloonTipText = "Click to Maximize";
      this.notifyIcon.Visible = true;
      this.notifyIcon.ShowBalloonTip(100);
      this.Hide();
    }

    private void HandlerToMaximiseOnClick(object sender, EventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    private void PrintTab_Selected(object sender, TabControlEventArgs e)
    {
      if (!(e.TabPage.Text == "Key Wizard"))
        return;
      this.DupeListSelected();
    }

    public void ShowCharLog(ListViewItem[] list)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.ShowCharLogCallback(this.ShowCharLog), (object) list);
      }
      else
      {
        if (list != null)
        {
          this.CharItems.Items.Clear();
          if (list.Length != 0)
            this.CharItems.Items.AddRange(list);
        }
        this.SearchBox.Enabled = true;
      }
    }

    public void DupeListSelected()
    {
      new Thread((ThreadStart) (() => this.DupeListSelectedThread()))
      {
        IsBackground = true
      }.Start();
    }

    private void DupeListSelectedThread()
    {
      this.ParseD2BotLog();
      Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
      foreach (KeyList keyList in (IEnumerable<KeyList>) Program.KeyLists.Values)
      {
        foreach (CDKey cdKey in keyList.CDKeys)
        {
          if (!dictionary.ContainsKey(cdKey.Name))
            dictionary.Add(cdKey.Name, new List<string>());
          dictionary[cdKey.Name].Add(keyList.Name);
        }
      }
      List<string> list = dictionary.Keys.ToList<string>();
      list.Sort();
      List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
      foreach (string index in list)
      {
        DataGridViewRow dataGridViewRow = new DataGridViewRow();
        DataGridViewTextBoxCell gridViewTextBoxCell1 = new DataGridViewTextBoxCell();
        DataGridViewTextBoxCell gridViewTextBoxCell2 = new DataGridViewTextBoxCell();
        DataGridViewComboBoxCell viewComboBoxCell = new DataGridViewComboBoxCell();
        foreach (string str in dictionary[index].ToArray())
        {
          gridViewTextBoxCell2.Value = (object) str;
          viewComboBoxCell.Items.Add((object) str);
        }
        viewComboBoxCell.FlatStyle = FlatStyle.Popup;
        viewComboBoxCell.Value = viewComboBoxCell.Items[0];
        if (Program.HoldKeyList.Contains(index))
          gridViewTextBoxCell1.Style.BackColor = Color.Yellow;
        gridViewTextBoxCell1.Value = (object) index;
        dataGridViewRow.Cells.Add((DataGridViewCell) gridViewTextBoxCell1);
        if (viewComboBoxCell.Items.Count > 1)
          dataGridViewRow.Cells.Add((DataGridViewCell) viewComboBoxCell);
        else
          dataGridViewRow.Cells.Add((DataGridViewCell) gridViewTextBoxCell2);
        dataGridViewRow.Height = 18;
        dataGridViewRowList.Add(dataGridViewRow);
      }
      this.DupeListAdd(dataGridViewRowList.ToArray());
    }

    public void DupeListAdd(DataGridViewRow[] list)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.DupeListCallback(this.DupeListAdd), (object) list);
      }
      else
      {
        this.dupeList.RowCount = 0;
        this.dupeList.Rows.AddRange(list);
      }
    }

    private void RestartWatch()
    {
      this.keepAlive = false;
      for (int index = 0; index < 5 && this.ItemLoading.IsAlive; ++index)
        Thread.Sleep(100);
      if (this.ItemLoading.IsAlive)
      {
        try
        {
          this.ItemLoading.Abort();
        }
        catch
        {
        }
      }
      this.CharLogMutex.WaitOne();
      this.descriptions.Clear();
      this.database.Clear();
      this.active.Clear();
      this.nodeCache.Clear();
      this.EventBuffer.Clear();
      this.EventSync.Dispose();
      this.watcher.Dispose();
      this.CharItems.Items.Clear();
      this.CharTree.Nodes.Clear();
      this.CharLogMutex.ReleaseMutex();
      this.ItemLoading = new Thread(new ThreadStart(this.WatchInit))
      {
        IsBackground = true
      };
      this.ItemLoading.Start();
    }

    public void WatchInit()
    {
      this.descriptions = new HashSet<string>();
      this.database = new Dictionary<string, ListViewItem>();
      this.active = new Dictionary<string, ListViewItem>();
      this.nodeCache = new Dictionary<string, TreeNode>();
      this.EventBuffer = new System.Collections.Generic.Queue<string>();
      this.EventSync = new Mutex();
      if (!Directory.Exists(Program.BOT_LIB + "\\mules\\"))
        return;
      try
      {
        this.ListDirectories(this.CharTree, Program.BOT_LIB + "\\mules\\");
      }
      catch
      {
      }
      this.watcher = new FileSystemWatcher(Program.BOT_LIB + "\\mules\\")
      {
        Filter = "*.txt",
        IncludeSubdirectories = true,
        InternalBufferSize = 64000
      };
      this.watcher.Changed += new FileSystemEventHandler(this.OnChanged);
      this.watcher.Created += new FileSystemEventHandler(this.OnChanged);
      this.watcher.Deleted += new FileSystemEventHandler(this.OnDeleted);
      this.watcher.EnableRaisingEvents = true;
      string str = (string) null;
      this.keepAlive = true;
      while (this.keepAlive)
      {
        Thread.Sleep(10);
        if (this.EventBuffer.Count > 0)
        {
          this.EventSync.WaitOne();
          if (this.EventBuffer.Count > 0)
            str = this.EventBuffer.Dequeue();
          this.EventSync.ReleaseMutex();
          if (str != null && str.Length > 0)
          {
            for (int index = 0; index < 10; ++index)
            {
              try
              {
                if (!Directory.Exists(str))
                {
                  if (System.IO.File.Exists(str))
                    break;
                }
                else
                  break;
              }
              catch
              {
                Thread.Sleep(100);
              }
            }
            TreeNode node = this.GenerateNode(str);
            if (node.Tag != null && node.Tag.GetType() == typeof (NodeTag))
              this.GenerateNodeItems(node);
          }
        }
      }
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
      this.EventSync.WaitOne();
      this.EventBuffer.Enqueue(string.Copy(e.FullPath));
      this.EventSync.ReleaseMutex();
    }

    private void OnDeleted(object source, FileSystemEventArgs e)
    {
      string fullPath = e.FullPath;
      if (!this.nodeCache.ContainsKey(fullPath))
        return;
      this.RemoveNode(this.nodeCache[fullPath]);
    }

    public void CharTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (this.CharTree.HitTest(e.Location).Location == TreeViewHitTestLocations.PlusMinus)
        return;
      this.SearchBox.Enabled = false;
      this.ItemLoading = new Thread((ThreadStart) (() => this.CharTree_ClickThread(e.Node)))
      {
        IsBackground = true
      };
      this.ItemLoading.Start();
    }

    public void CharTree_ClickThread(TreeNode e)
    {
      this.CharLogMutex.WaitOne();
      this.active.Clear();
      this.ShowNodeItems(e);
      this.ShowCharLog(this.active.Values.ToArray<ListViewItem>());
      this.CharLogMutex.ReleaseMutex();
    }

    private void ListDirectories(TreeView treeView, string path)
    {
      this.nodeCache.Clear();
      treeView.Nodes.Clear();
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      this.CharLogMutex.WaitOne();
      this.ToggleCharLog(false);
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
      {
        TreeNode directoryNodes = this.CreateDirectoryNodes(directory);
        this.AddNode(treeView, directoryNodes);
      }
      this.ToggleCharLog(true);
      this.CharLogMutex.ReleaseMutex();
    }

    private TreeNode CreateDirectoryNodes(DirectoryInfo directoryInfo)
    {
      TreeNode node1 = this.CreateNode(directoryInfo.FullName, false);
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
      {
        TreeNode directoryNodes = this.CreateDirectoryNodes(directory);
        directoryNodes.Name = directory.FullName;
        this.AddNode(node1, directoryNodes);
      }
      foreach (FileInfo file in directoryInfo.GetFiles())
      {
        if (file.FullName.EndsWith(".txt"))
        {
          TreeNode node2 = this.CreateNode(file.FullName, true);
          NodeTag nodeTag = new NodeTag(file.FullName, new List<ListViewItem>());
          node2.Tag = (object) nodeTag;
          this.AddNode(node1, node2);
          this.GenerateNodeItems(node2);
        }
      }
      return node1;
    }

    private TreeNode GenerateNode(string node)
    {
      bool isFile = false;
      if ((System.IO.File.GetAttributes(node) & FileAttributes.Directory) != FileAttributes.Directory)
        isFile = true;
      if (this.nodeCache.ContainsKey(node))
        return this.nodeCache[node];
      string[] strArray = node.Split(Path.DirectorySeparatorChar);
      Array.Reverse((Array) strArray);
      TreeNode leaf = this.CreateNode(node, isFile);
      if (isFile)
        leaf.Tag = (object) new NodeTag(node, new List<ListViewItem>());
      for (int index = 1; index < strArray.Length; ++index)
      {
        int length = node.IndexOf(strArray[index]);
        if (strArray[index].ToLower().Contains("mules"))
        {
          this.AddNode(this.CharTree, leaf);
          break;
        }
        if (!this.nodeCache.ContainsKey(node.Substring(0, length) + strArray[index]))
        {
          TreeNode node1 = this.CreateNode(node.Substring(0, length) + strArray[index], false);
          this.AddNode(node1, leaf);
          leaf = node1;
        }
        else
        {
          this.AddNode(this.nodeCache[node.Substring(0, length) + strArray[index]], leaf);
          break;
        }
      }
      return this.nodeCache[node];
    }

    public void ToggleCharLog(bool enable)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.ToggleCallback(this.ToggleCharLog), (object) enable);
      }
      else
      {
        this.CharItems.Enabled = enable;
        this.SearchBox.Enabled = enable;
        this.SearchButton.Enabled = enable;
        this.CharTree.Enabled = enable;
      }
    }

    public void AddNode(TreeNode root, TreeNode leaf)
    {
      if (this.InvokeRequired)
        this.BeginInvoke((Delegate) new Main.AddNodeCallback(this.AddNode), (object) root, (object) leaf);
      else
        root.Nodes.Add(leaf);
    }

    public void AddNode(TreeView root, TreeNode leaf)
    {
      if (this.InvokeRequired)
        this.BeginInvoke((Delegate) new Main.AddTreeCallback(this.AddNode), (object) root, (object) leaf);
      else
        root.Nodes.Add(leaf);
    }

    public void RemoveNode(TreeNode root)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.RemoveNodeCallback(this.RemoveNode), (object) root);
      }
      else
      {
        if (root == null)
          return;
        root.Remove();
        this.nodeCache.Remove(root.Name);
        foreach (TreeNode node in root.Nodes)
          this.RemoveNode(node);
      }
    }

    private TreeNode CreateNode(string name, bool isFile = false)
    {
      if (!this.nodeCache.ContainsKey(name))
      {
        string text = Path.GetFileName(name);
        if (isFile)
        {
          text = text.Substring(0, text.Length - 4);
          if (text.Contains("."))
            text = text.Substring(0, text.Length - 4) + " (" + text.Substring(text.Length - 3, 3).ToUpper() + ")";
        }
        TreeNode treeNode = new TreeNode(text)
        {
          Name = name
        };
        this.nodeCache.Add(treeNode.Name, treeNode);
      }
      return this.nodeCache[name];
    }

    private void GenerateNodeItems(TreeNode node)
    {
      if (node.Tag != null && node.Tag.GetType() == typeof (NodeTag))
      {
        NodeTag tag = node.Tag as NodeTag;
        tag.Items.Clear();
        string withoutExtension = Path.GetFileNameWithoutExtension(tag.Path);
        string str1 = !withoutExtension.Contains(".") ? (string) null : withoutExtension.Substring(withoutExtension.IndexOf(".") + 1, 3);
        string[] strArray = (string[]) null;
        for (int index = 0; index < 10; ++index)
        {
          try
          {
            strArray = System.IO.File.ReadAllLines(tag.Path);
            break;
          }
          catch
          {
            Thread.Sleep(100);
          }
        }
        if (strArray == null)
          return;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str2 = strArray[index];
          D2Item d2Item;
          try
          {
            d2Item = (D2Item) JsonConvert.DeserializeObject<D2Item>(str2);
          }
          catch (Exception ex)
          {
            string comment = "Item: " + str2;
            Program.LogCrash(ex, comment, false);
            continue;
          }
          Item obj1 = d2Item.ToItem();
          if (str1 != null)
          {
            Item obj2 = obj1;
            obj2.Header = obj2.Header + " / " + str1.ToUpper();
          }
          obj1.Path = node.Name;
          if (!this.database.ContainsKey(obj1.Description.ToLower()))
            this.database.Add(obj1.Description.ToLower(), obj1.ListViewItem("", ""));
          tag.Items.Add(this.database[obj1.Description.ToLower()]);
        }
      }
      foreach (TreeNode node1 in node.Nodes)
        this.GenerateNodeItems(node1);
    }

    private void ShowNodeItems(TreeNode node)
    {
      if (node.Tag != null && node.Tag.GetType() == typeof (NodeTag))
      {
        NodeTag tag1 = node.Tag as NodeTag;
        if (tag1.Items.Count == 0)
          this.GenerateNodeItems(node);
        foreach (ListViewItem listViewItem in tag1.Items)
        {
          Item tag2 = listViewItem.Tag as Item;
          if (!this.active.ContainsKey(tag2.Description.ToLower()))
            this.active.Add(tag2.Description.ToLower(), listViewItem);
        }
      }
      foreach (TreeNode node1 in node.Nodes)
        this.ShowNodeItems(node1);
    }

    private void QueryItem(object sender, EventArgs e)
    {
      this.SearchBox.Enabled = false;
      new Thread((ThreadStart) (() => this.QueryThread()))
      {
        IsBackground = true
      }.Start();
    }

    private void QueryThread()
    {
      this.CharLogMutex.WaitOne();
      if (!Main.IsValidRegex(this.SearchBox.Text))
      {
        this.ShowCharLog((ListViewItem[]) null);
        this.CharLogMutex.ReleaseMutex();
      }
      else
      {
        List<ListViewItem> listViewItemList = new List<ListViewItem>();
        Regex regex = new Regex(this.SearchBox.Text.ToLower());
        string[] array = this.active.Keys.ToArray<string>();
        try
        {
          foreach (string index in array)
          {
            if (!string.IsNullOrEmpty(index) && regex.IsMatch(index.ToLower().Replace("\n", ""), 0))
              listViewItemList.Add(this.active[index]);
          }
        }
        catch
        {
          int num = (int) MessageBox.Show("Invalid Regular Expression");
        }
        this.ShowCharLog(listViewItemList.ToArray());
        this.CharLogMutex.ReleaseMutex();
      }
    }

    private void SearchNodeItems(TreeNode node, Regex query, ConcurrentQueue<WebItem> result)
    {
      if (node.Tag != null && node.Tag.GetType() == typeof (NodeTag))
      {
        NodeTag tag1 = node.Tag as NodeTag;
        if (tag1.Items.Count == 0)
          this.GenerateNodeItems(node);
        Parallel.ForEach<ListViewItem>((IEnumerable<ListViewItem>) tag1.Items, (Action<ListViewItem>) (l =>
        {
          Item tag = l.Tag as Item;
          if (query != null && !this.IsMatch(query, tag.Description))
            return;
          result.Enqueue(tag.ToWebItem().GenerateImage());
        }));
      }
      foreach (TreeNode node1 in node.Nodes)
        this.SearchNodeItems(node1, query, result);
    }

    public bool IsMatch(Regex query, string desc)
    {
      return !string.IsNullOrEmpty(desc) && query.IsMatch(desc.ToLower().Replace("\n", ""), 0);
    }

    public void LoadList()
    {
      this.gameExe.set_FillsFreeSpace(true);
      this.objectProfileList.set_DragSource((IDragSource) new SimpleDragSource(true));
      this.objectProfileList.set_DropSink((IDropSink) new RearrangingDropSink(false));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      this.profileCol.set_ImageGetter(Main.\u003C\u003Ec.\u003C\u003E9__127_0 ?? (Main.\u003C\u003Ec.\u003C\u003E9__127_0 = new ImageGetterDelegate((object) Main.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CLoadList\u003Eb__127_0))));
    }

    public Main()
    {
      this.InitializeComponent();
      string str = "";
      if (Program.UPDATE)
        str = "  UPDATE AVAILABLE";
      this.Text = "D2Bot #  " + Program.VER.ToString() + str;
      this.charContainer.SplitterWidth = 1;
      this.keyWizardContainer.SplitterWidth = 1;
      this.systemFont.Checked = Settings.Default.System_Font;
      this.itemHeader.Checked = Settings.Default.Item_Header;
      this.startHidden.Checked = Settings.Default.Start_Hidden;
      this.serverToggle.Checked = Settings.Default.Start_Server;
      this.loadDelay.Checked = Settings.Default.Load_Delay;
      switch (Settings.Default.Wait_Time)
      {
        case 15:
          this.secondsToolStripMenuItem1.Checked = true;
          break;
        case 20:
          this.secondsToolStripMenuItem2.Checked = true;
          break;
        case 25:
          this.secondsToolStripMenuItem3.Checked = true;
          break;
        case 30:
          this.secondsToolStripMenuItem4.Checked = true;
          break;
        default:
          this.secondsToolStripMenuItem.Checked = true;
          break;
      }
      switch (Settings.Default.Delay_Time)
      {
        case 250:
          this.ms250.Checked = true;
          break;
        case 500:
          this.ms500.Checked = true;
          break;
        case 1000:
          this.ms1000.Checked = true;
          break;
        default:
          this.ms100.Checked = true;
          break;
      }
      this.SetUpTrayIcon();
      Image image1 = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("D2Bot.Resources.bgnd1.png"));
      Program.ItemBG.Add("bgnd1", image1);
      Image image2 = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("D2Bot.Resources.bgnd2.png"));
      Program.ItemBG.Add("bgnd2", image2);
      Image image3 = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("D2Bot.Resources.bgnd3.png"));
      Program.ItemBG.Add("bgnd3", image3);
      Image image4 = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("D2Bot.Resources.bgnd4.png"));
      Program.ItemBG.Add("bgnd4", image4);
      Program.D2Tool = new D2Tooltip((IContainer) null);
      this.LoadListView();
      MessageHelper.SendMessage(((Control) this.objectProfileList).Handle, 295, 1, 0);
      this.ItemLoading = new Thread(new ThreadStart(this.WatchInit))
      {
        IsBackground = true
      };
      this.ItemLoading.Start();
      this.WindowChecker = new Thread(new ThreadStart(this.CheckWindows))
      {
        IsBackground = true
      };
      this.WindowChecker.Start();
      this.WorkThread();
      Program.Handle = this.Handle;
      this.Timer = new System.Timers.Timer(1000.0);
      this.Timer.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
      this.Timer.Enabled = true;
    }

    private bool IsValidDay()
    {
      return DateTime.Now.ToString("MMdd").Equals("0401");
    }

    private void OnTimedEvent(object source, EventArgs e)
    {
      ++this.SEC_Count;
      if (this.WindowState != FormWindowState.Minimized)
      {
        this.UpdateProfiles();
        if (this.IsValidDay() && this.SEC_Count % 5 == 0)
          this.UpdateDraw(true);
      }
      if (!Program.GA || this.SEC_Count <= 600)
        return;
      this.SEC_Count = 0;
      try
      {
        Program.ReportError.trackPage("d2bot.sharp", "/", "Home");
        Program.ReportError.trackScreen("Home");
      }
      catch
      {
      }
    }

    private IList GetVisibleObjects()
    {
      int capacity = ((Control) this.objectProfileList).Height / this.objectProfileList.get_RowHeight() + 1;
      List<object> objectList = new List<object>(capacity);
      for (int index = 0; index < capacity; ++index)
        objectList.Add(this.objectProfileList.GetModelObject(this.objectProfileList.get_TopItemIndex() + index));
      return (IList) objectList;
    }

    public void LoadListView()
    {
      StyleHelper.DisableFlicker((Control) this.ItemLogger);
      StyleHelper.DisableFlicker((Control) this.CharTree);
      StyleHelper.DisableFlicker((Control) this.CharItems);
      StyleHelper.DisableFlicker((Control) this.KeyData);
    }

    public ProfileBase GetProfile(string name)
    {
      if (Program.ProfileList.ContainsKey(name.ToLower()))
        return Program.ProfileList[name.ToLower()];
      return (ProfileBase) null;
    }

    public D2Profile GetValidD2Profile(string name)
    {
      ProfileBase profile = this.GetProfile(name);
      if (profile != null && profile.Type == ProfileType.D2)
        return (D2Profile) profile;
      return (D2Profile) null;
    }

    public IRCProfile GetValidIRCProfile(string name)
    {
      ProfileBase profile = this.GetProfile(name);
      if (profile != null && profile.Type == ProfileType.IRC)
        return (IRCProfile) profile;
      return (IRCProfile) null;
    }

    public KeyList GetKeyList(string name)
    {
      if (name == null || Program.KeyLists == null)
        return (KeyList) null;
      if (!Program.KeyLists.ContainsKey(name.ToLower()))
        Program.KeyLists.TryAdd(name.ToLower(), new KeyList(name));
      return Program.KeyLists[name.ToLower()];
    }

    public Schedule GetSchedule(string name)
    {
      if (name == null || Program.Schedules == null)
        return (Schedule) null;
      if (!Program.Schedules.ContainsKey(name.ToLower()))
        Program.Schedules.TryAdd(name.ToLower(), new Schedule(name));
      return Program.Schedules[name.ToLower()];
    }

    public void ParseD2BotLog()
    {
      string[] array = System.IO.File.ReadAllLines(Application.StartupPath + "\\logs\\keyinfo.log");
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      for (int index = 0; index < array.Length; ++index)
      {
        int startIndex = array[index].IndexOf('<');
        array[index] = array[index].Substring(startIndex);
      }
      Array.Sort<string>(array);
      foreach (string input in array)
      {
        Match match1 = Regex.Match(input, "\\<(.*?)\\>");
        Match match2 = Regex.Match(input, "\\> (.*?)\\ ");
        int index = 0;
        while (index < listViewItemList.Count && (!(match2.Groups[1].Value == listViewItemList[index].SubItems[1].Text) || !(match1.Groups[1].Value == listViewItemList[index].SubItems[0].Text)))
          ++index;
        if (index == listViewItemList.Count)
          listViewItemList.Add(new ListViewItem(match1.Groups[1].Value)
          {
            SubItems = {
              match2.Groups[1].Value,
              "",
              "",
              ""
            }
          });
        int num1;
        if (input.Contains("use"))
        {
          if (listViewItemList[index].SubItems[2].Text != "")
          {
            int num2 = int.Parse(listViewItemList[index].SubItems[2].Text);
            ListViewItem.ListViewSubItem subItem = listViewItemList[index].SubItems[2];
            num1 = num2 + 1;
            string str = num1.ToString();
            subItem.Text = str;
          }
          else
          {
            ListViewItem.ListViewSubItem subItem = listViewItemList[index].SubItems[2];
            num1 = 1;
            string str = num1.ToString();
            subItem.Text = str;
          }
        }
        else if (input.Contains("realm"))
        {
          if (listViewItemList[index].SubItems[3].Text != "")
          {
            int num2 = int.Parse(listViewItemList[index].SubItems[3].Text);
            ListViewItem.ListViewSubItem subItem = listViewItemList[index].SubItems[3];
            num1 = num2 + 1;
            string str = num1.ToString();
            subItem.Text = str;
          }
          else
          {
            ListViewItem.ListViewSubItem subItem = listViewItemList[index].SubItems[3];
            num1 = 1;
            string str = num1.ToString();
            subItem.Text = str;
          }
        }
        else if (input.Contains("disabled"))
          listViewItemList[index].SubItems[4].Text = "DISABLED!";
      }
      this.AddKeyData(listViewItemList.ToArray());
    }

    public void AddKeyData(ListViewItem[] items)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new Main.AddKeyDataCallback(this.AddKeyData), (object) items);
      }
      else
      {
        this.KeyData.Items.Clear();
        this.KeyData.Items.AddRange(items);
        this.KeyData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        this.KeyData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
      }
    }

    public void ProfileToList(D2Profile p)
    {
      this.objectProfileList.RefreshObject((object) p);
    }

    private void LinkClicked(object sender, LinkClickedEventArgs e)
    {
      if (e.LinkText.StartsWith("http://i.imgur.com"))
      {
        Process.Start(e.LinkText);
      }
      else
      {
        if (!e.LinkText.Contains("#"))
          return;
        RichTextBoxEx richTextBoxEx = sender as RichTextBoxEx;
        if (richTextBoxEx.Tag == null || richTextBoxEx.Tag.GetType() != typeof (ToolTip))
          richTextBoxEx.Tag = (object) new ToolTip();
        ToolTip tag = richTextBoxEx.Tag as ToolTip;
        if (!string.IsNullOrEmpty(tag.GetToolTip((Control) this)))
        {
          if (!(tag.GetToolTip((Control) this) != e.LinkText.Split('#')[1]))
          {
            tag.ShowAlways = false;
            tag.Hide((IWin32Window) this);
            return;
          }
        }
        Point client = this.PointToClient(Cursor.Position);
        client.X += 65;
        client.Y += 20;
        tag.Hide((IWin32Window) this);
        tag.Show(e.LinkText.Split('#')[1].Replace("&$", Environment.NewLine), (IWin32Window) this, client, 2000);
      }
    }

    private void ConsoleBox_CursorChanged(object sender, EventArgs e)
    {
      RichTextBoxEx richTextBoxEx = sender as RichTextBoxEx;
      if (!(richTextBoxEx.Cursor == Cursors.Hand))
        return;
      ToolTip tag = richTextBoxEx.Tag as ToolTip;
      tag.ShowAlways = true;
      tag.Hide((IWin32Window) this);
    }

    private void ConsoleBox_MouseHover(object sender, MouseEventArgs e)
    {
      RichTextBoxEx richTextBoxEx = sender as RichTextBoxEx;
      if (!(richTextBoxEx.Cursor == Cursors.IBeam) || !(richTextBoxEx.Tag is ToolTip tag))
        return;
      tag.ShowAlways = false;
      tag.Hide((IWin32Window) this);
    }

    private void StartAll()
    {
      new Thread((ThreadStart) (() => this.StartAllThread()))
      {
        IsBackground = true
      }.Start();
    }

    private void StartAllThread()
    {
      bool flag = false;
      List<ProfileBase> profileBaseList = new List<ProfileBase>();
      int num = 0;
      if (this.loadDelay.Checked)
        num = Settings.Default.Delay_Time;
      try
      {
        flag = this.objectLock.WaitOne();
        foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.ProfileList.Values)
          profileBaseList.Add(profileBase);
      }
      catch
      {
      }
      finally
      {
        if (flag)
          this.objectLock.ReleaseMutex();
        for (int index = 0; index < profileBaseList.Count; ++index)
          profileBaseList[index].Load(num + index * num);
      }
    }

    private void StopAll()
    {
      new Thread((ThreadStart) (() => this.StopAllThread()))
      {
        IsBackground = true
      }.Start();
    }

    private void StopAllThread()
    {
      bool flag = false;
      try
      {
        flag = this.objectLock.WaitOne();
        foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.Runtime.Values)
        {
          while (profileBase.Status == Status.Busy)
            Thread.Sleep(10);
          profileBase.Stop();
        }
      }
      catch
      {
      }
      finally
      {
        if (flag)
          this.objectLock.ReleaseMutex();
      }
    }

    private void HideAll()
    {
      new Thread((ThreadStart) (() => this.HideAllThread()))
      {
        IsBackground = true
      }.Start();
    }

    private void HideAllThread()
    {
      try
      {
        foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.Runtime.Values)
        {
          if (profileBase is D2Profile d2Profile)
            d2Profile.HideWindow();
        }
      }
      catch
      {
      }
    }

    private void ShowAll()
    {
      new Thread((ThreadStart) (() => this.ShowAllThread()))
      {
        IsBackground = true
      }.Start();
    }

    private void ShowAllThread()
    {
      try
      {
        foreach (ProfileBase profileBase in (IEnumerable<ProfileBase>) Program.Runtime.Values)
        {
          if (profileBase is D2Profile d2Profile)
            d2Profile.ShowWindow();
        }
      }
      catch
      {
      }
    }

    public void VersionHandler(object sender, EventArgs e)
    {
      foreach (ToolStripMenuItem dropDownItem in (ArrangedElementCollection) this.versionMenuItem.DropDownItems)
        dropDownItem.Checked = dropDownItem.ToString() == sender.ToString();
      Settings.Default.D2_Version = sender.ToString();
      Settings.Default.Save();
    }

    private void MenuHandler(object sender, EventArgs e)
    {
      string s = sender.ToString();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 11205808:
          if (!(s == "Developer Mode") || !this.debugMode.Checked)
            break;
          int num = (int) MessageBox.Show("Developer mode is ON. Do not use unless you know what you are doing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case 529156820:
          if (!(s == "Show Item Header"))
            break;
          Settings.Default.Item_Header = this.itemHeader.Checked;
          Settings.Default.Save();
          break;
        case 807742496:
          if (!(s == "1000 ms"))
            break;
          this.ms100.Checked = false;
          this.ms250.Checked = false;
          this.ms500.Checked = false;
          this.ms2000.Checked = false;
          this.ms5000.Checked = false;
          Settings.Default.Delay_Time = 1000;
          Settings.Default.Save();
          break;
        case 838466321:
          if (!(s == "Start Hidden"))
            break;
          Settings.Default.Start_Hidden = this.startHidden.Checked;
          Settings.Default.Save();
          break;
        case 885933221:
          if (!(s == "Exit"))
            break;
          Application.Exit();
          break;
        case 1138198641:
          if (!(s == "10 Seconds"))
            break;
          this.secondsToolStripMenuItem1.Checked = false;
          this.secondsToolStripMenuItem2.Checked = false;
          this.secondsToolStripMenuItem3.Checked = false;
          this.secondsToolStripMenuItem4.Checked = false;
          Settings.Default.Wait_Time = 10;
          Settings.Default.Save();
          break;
        case 1226687018:
          if (!(s == "Load Delay"))
            break;
          Settings.Default.Load_Delay = this.loadDelay.Checked;
          Settings.Default.Save();
          break;
        case 1248323585:
          if (!(s == "2000 ms"))
            break;
          this.ms100.Checked = false;
          this.ms250.Checked = false;
          this.ms500.Checked = false;
          this.ms1000.Checked = false;
          this.ms5000.Checked = false;
          Settings.Default.Delay_Time = 2000;
          Settings.Default.Save();
          break;
        case 1296478972:
          if (!(s == "250 ms"))
            break;
          this.ms100.Checked = false;
          this.ms500.Checked = false;
          this.ms1000.Checked = false;
          this.ms2000.Checked = false;
          this.ms5000.Checked = false;
          Settings.Default.Delay_Time = 250;
          Settings.Default.Save();
          break;
        case 1445146119:
          if (!(s == "30 Seconds"))
            break;
          this.secondsToolStripMenuItem.Checked = false;
          this.secondsToolStripMenuItem1.Checked = false;
          this.secondsToolStripMenuItem2.Checked = false;
          this.secondsToolStripMenuItem3.Checked = false;
          Settings.Default.Wait_Time = 30;
          Settings.Default.Save();
          break;
        case 1827992440:
          if (!(s == "100 ms"))
            break;
          this.ms250.Checked = false;
          this.ms500.Checked = false;
          this.ms1000.Checked = false;
          this.ms2000.Checked = false;
          this.ms5000.Checked = false;
          Settings.Default.Delay_Time = 100;
          Settings.Default.Save();
          break;
        case 2049461964:
          if (!(s == "5000 ms"))
            break;
          this.ms100.Checked = false;
          this.ms250.Checked = false;
          this.ms500.Checked = false;
          this.ms1000.Checked = false;
          this.ms2000.Checked = false;
          Settings.Default.Delay_Time = 5000;
          Settings.Default.Save();
          break;
        case 2210502937:
          if (!(s == "Run API Server"))
            break;
          if (this.serverToggle.Checked)
          {
            Settings.Default.Start_Server = true;
            WebEvents.InitEvents();
            if (Program.WC != null)
            {
              Program.WS = new WebServer(Application.StartupPath + Program.WC.path, Program.WC.ip, Program.WC.port);
              Program.WS.Start();
            }
          }
          else
          {
            Settings.Default.Start_Server = false;
            if (Program.WC != null && Program.WS != null)
              Program.WS.Stop();
          }
          Settings.Default.Save();
          break;
        case 2614141314:
          if (!(s == "20 Seconds"))
            break;
          this.secondsToolStripMenuItem.Checked = false;
          this.secondsToolStripMenuItem1.Checked = false;
          this.secondsToolStripMenuItem3.Checked = false;
          this.secondsToolStripMenuItem4.Checked = false;
          Settings.Default.Wait_Time = 20;
          Settings.Default.Save();
          break;
        case 2721846031:
          if (!(s == "Show All"))
            break;
          this.ShowAll();
          break;
        case 2957479038:
          if (!(s == "15 Seconds"))
            break;
          this.secondsToolStripMenuItem.Checked = false;
          this.secondsToolStripMenuItem2.Checked = false;
          this.secondsToolStripMenuItem3.Checked = false;
          this.secondsToolStripMenuItem4.Checked = false;
          Settings.Default.Wait_Time = 15;
          Settings.Default.Save();
          break;
        case 3131884150:
          if (!(s == "Use System Font"))
            break;
          Settings.Default.System_Font = this.systemFont.Checked;
          Settings.Default.Save();
          break;
        case 3204846482:
          if (!(s == "Hide All"))
            break;
          this.HideAll();
          break;
        case 3358675452:
          if (!(s == "500 ms"))
            break;
          this.ms100.Checked = false;
          this.ms250.Checked = false;
          this.ms1000.Checked = false;
          this.ms2000.Checked = false;
          this.ms5000.Checked = false;
          Settings.Default.Delay_Time = 500;
          Settings.Default.Save();
          break;
        case 3372563069:
          if (!(s == "25 Seconds"))
            break;
          this.secondsToolStripMenuItem.Checked = false;
          this.secondsToolStripMenuItem1.Checked = false;
          this.secondsToolStripMenuItem2.Checked = false;
          this.secondsToolStripMenuItem4.Checked = false;
          Settings.Default.Wait_Time = 25;
          Settings.Default.Save();
          break;
        case 3432083940:
          if (!(s == "System Tray"))
            break;
          this.Main_ResizeEnd((object) null, (EventArgs) null);
          break;
        case 3475643868:
          if (!(s == "Start All"))
            break;
          this.StartAll();
          break;
        case 4203910954:
          if (!(s == "Stop All"))
            break;
          this.StopAll();
          break;
      }
    }

    protected override void OnShown(EventArgs e)
    {
      MessageHelper.SendMessage(this.Handle, 296, Win32.MakeParam(1, 1), 0);
      base.OnShown(e);
    }

    public void Main_Close(object sender, FormClosingEventArgs e)
    {
      if (Program.ProfileList != null)
      {
        this.objectLock.WaitOne();
        using (IEnumerator<ProfileBase> enumerator = Program.ProfileList.Values.GetEnumerator())
        {
label_7:
          while (enumerator.MoveNext())
          {
            ProfileBase current = enumerator.Current;
            current.Stop();
            if (current.Type != ProfileType.IRC)
            {
              D2Profile d2Profile = (D2Profile) current;
              while (true)
              {
                if (d2Profile.Client != null && d2Profile.Client.IsAlive)
                  Thread.Sleep(25);
                else
                  goto label_7;
              }
            }
          }
        }
        this.objectLock.ReleaseMutex();
      }
      Program.SaveAll();
      this.notifyIcon.Visible = false;
      this.notifyIcon.Dispose();
    }

    private void AboutDialog(object sender, EventArgs e)
    {
      new About().Show();
    }

    private void ObjectProfileList_ModelDropped(object sender, ModelDropEventArgs e)
    {
      Program.SaveProfiles();
    }

    private void RefreshCharViewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.RestartWatch();
    }

    private void CloseGameexeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Are you sure you want to kill all Diablo 2 instances?", "Kill Game.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.StopAllThread();
      Process[] processes = Process.GetProcesses();
      for (int index = 0; index < processes.Length; ++index)
      {
        try
        {
          if (processes[index].ProcessName.ToLower().Contains("game"))
          {
            if (processes[index].MainModule.FileVersionInfo.FileDescription.ToLower().Contains("diablo"))
              processes[index].Kill();
          }
        }
        catch
        {
        }
      }
    }

    private void ScheduleEdit_Click(object sender, EventArgs e)
    {
      new ListEditor((IListData) new ScheduleEditor()).Show();
    }

    private void KeysEdit_Click(object sender, EventArgs e)
    {
      new ListEditor((IListData) new KeylistEditor()).Show();
    }

    private void ImportProfilesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.InitialDirectory = Application.StartupPath;
      openFileDialog1.RestoreDirectory = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog() != DialogResult.OK)
        return;
      bool flag = false;
      if (MessageBox.Show("This will stop all running profiles and remove them from this list.", "Continue Loading?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
      {
        this.StopAll();
        int num = 0;
        while (Program.Runtime.Count > 0 && num < 500)
        {
          ++num;
          Thread.Sleep(10);
        }
        try
        {
          flag = this.objectLock.WaitOne();
          if (Program.ProfileList.Count > 0)
            this.objectProfileList.RemoveObjects((ICollection) Program.ProfileList.Values.ToList<ProfileBase>());
        }
        catch
        {
        }
        finally
        {
          if (flag)
            this.objectLock.ReleaseMutex();
        }
        Program.Runtime.Clear();
        Program.ProfileList.Clear();
      }
      if (!flag)
        return;
      Program.LoadProfile("\\" + Path.GetFileName(openFileDialog2.FileName));
      Program.SaveAll();
    }

    private void UpdateVisibleList()
    {
      int topItemIndex = this.objectProfileList.get_TopItemIndex();
      int num = Math.Min(((Control) this.objectProfileList).Height / this.objectProfileList.get_RowHeight() + 1, this.objectProfileList.GetItemCount() - topItemIndex);
      if (topItemIndex == this.UpdateIndex && num == this.UpdateTotal)
        return;
      this.UpdateTotal = num;
      this.UpdateIndex = topItemIndex;
      Program.Viewable.Clear();
      for (int index = 0; index < this.UpdateTotal && this.UpdateIndex + index < this.objectProfileList.GetItemCount(); ++index)
        Program.Viewable.Add(this.objectProfileList.GetModelObject(this.UpdateIndex + index) as ProfileBase);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Main));
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.importProfilesToolStripMenuItem = new ToolStripMenuItem();
      this.resetStatsToolStripMenuItem = new ToolStripMenuItem();
      this.enableScheduleToolStripMenuItem = new ToolStripMenuItem();
      this.disableScheduleToolStripMenuItem = new ToolStripMenuItem();
      this.muleProfileToolStripMenuItem = new ToolStripMenuItem();
      this.nextKeyToolStripMenuItem = new ToolStripMenuItem();
      this.releaseKeyToolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip3 = new ContextMenuStrip(this.components);
      this.toolStripMenuItem3 = new ToolStripMenuItem();
      this.importToolStripMenuItem = new ToolStripMenuItem();
      this.clearToolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip4 = new ContextMenuStrip(this.components);
      this.toolItemSaveImage = new ToolStripMenuItem();
      this.uploadImgurToolStripMenuItem = new ToolStripMenuItem();
      this.copyImageToolStripMenuItem = new ToolStripMenuItem();
      this.copyDescriptionToolStripMenuItem = new ToolStripMenuItem();
      this.createDupeToolStripMenuItem = new ToolStripMenuItem();
      this.removeToolStripMenuItem = new ToolStripMenuItem();
      this.clearItemsToolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip5 = new ContextMenuStrip(this.components);
      this.pauseKeyToolStripMenuItem = new ToolStripMenuItem();
      this.resumeKeyToolStripMenuItem = new ToolStripMenuItem();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.removeKeyToolStripMenuItem = new ToolStripMenuItem();
      this.removeFromListToolStripMenuItem = new ToolStripMenuItem();
      this.clearDetailsToolStripMenuItem = new ToolStripMenuItem();
      this.guiStart = new ToolStripButton();
      this.guiStop = new ToolStripButton();
      this.guiAdd = new ToolStripButton();
      this.guiEdit = new ToolStripButton();
      this.guiSave = new ToolStripButton();
      this.guiCopy = new ToolStripButton();
      this.guiShow = new ToolStripButton();
      this.guiHide = new ToolStripButton();
      this.guiAbout = new ToolStripButton();
      this.toolStrip1 = new ToolStrip();
      this.menuDropDown = new ToolStripDropDownButton();
      this.menuStart = new ToolStripMenuItem();
      this.menuStop = new ToolStripMenuItem();
      this.menuShow = new ToolStripMenuItem();
      this.menuHide = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.menuSave = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.menuExit = new ToolStripMenuItem();
      this.systemTrayToolStripMenuItem = new ToolStripMenuItem();
      this.settingsDropDown = new ToolStripDropDownButton();
      this.versionMenuItem = new ToolStripMenuItem();
      this.responseTimeToolStripMenuItem = new ToolStripMenuItem();
      this.secondsToolStripMenuItem = new ToolStripMenuItem();
      this.secondsToolStripMenuItem1 = new ToolStripMenuItem();
      this.secondsToolStripMenuItem2 = new ToolStripMenuItem();
      this.secondsToolStripMenuItem3 = new ToolStripMenuItem();
      this.secondsToolStripMenuItem4 = new ToolStripMenuItem();
      this.loadDelay = new ToolStripMenuItem();
      this.ms100 = new ToolStripMenuItem();
      this.ms250 = new ToolStripMenuItem();
      this.ms500 = new ToolStripMenuItem();
      this.ms1000 = new ToolStripMenuItem();
      this.ms2000 = new ToolStripMenuItem();
      this.ms5000 = new ToolStripMenuItem();
      this.startHidden = new ToolStripMenuItem();
      this.systemFont = new ToolStripMenuItem();
      this.itemHeader = new ToolStripMenuItem();
      this.refreshCharViewToolStripMenuItem = new ToolStripMenuItem();
      this.closeGameexeToolStripMenuItem = new ToolStripMenuItem();
      this.serverToggle = new ToolStripMenuItem();
      this.debugMode = new ToolStripMenuItem();
      this.guiRemove = new ToolStripButton();
      this.guiAddIRC = new ToolStripButton();
      this.keysEdit = new ToolStripButton();
      this.scheduleEdit = new ToolStripButton();
      this.mainContainer = new SplitContainer();
      this.pictureBox1 = new PictureBox();
      this.objectProfileList = new ObjectListView();
      this.profileCol = new OLVColumn();
      this.statusCol = new OLVColumn();
      this.runsCol = new OLVColumn();
      this.chickensCol = new OLVColumn();
      this.deathsCol = new OLVColumn();
      this.crashesCol = new OLVColumn();
      this.restartsCol = new OLVColumn();
      this.keyCol = new OLVColumn();
      this.gameExe = new OLVColumn();
      this.profileImages = new ImageList(this.components);
      this.PrintTab = new TabControl();
      this.Console = new TabPage();
      this.splitContainer1 = new SplitContainer();
      this.ItemLogger = new ListView();
      this.ItemDateProfile = new ColumnHeader();
      this.ItemLogColumn = new ColumnHeader();
      this.charView = new TabPage();
      this.charContainer = new SplitContainer();
      this.CharTree = new TreeView();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.charSearchBar = new TableLayoutPanel();
      this.SearchBox = new TextBox();
      this.SearchButton = new Button();
      this.CharItems = new ListView();
      this.CharItemColumn = new ColumnHeader();
      this.KeyAnalysis = new TabPage();
      this.keyWizardContainer = new SplitContainer();
      this.keyWizBorder = new Panel();
      this.dupeList = new DataGridView();
      this.keyDupe = new DataGridViewTextBoxColumn();
      this.profileDupe = new DataGridViewComboBoxColumn();
      this.KeyData = new ListView();
      this.KeyProfile = new ColumnHeader();
      this.KeyName = new ColumnHeader();
      this.KeyInUse = new ColumnHeader();
      this.KeyRD = new ColumnHeader();
      this.KeyDisabled = new ColumnHeader();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.ConsoleBox = new RichTextBoxEx();
      this.contextMenuStrip1.SuspendLayout();
      this.contextMenuStrip3.SuspendLayout();
      this.contextMenuStrip4.SuspendLayout();
      this.contextMenuStrip5.SuspendLayout();
      this.contextMenuStrip2.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.mainContainer.BeginInit();
      this.mainContainer.Panel1.SuspendLayout();
      this.mainContainer.Panel2.SuspendLayout();
      this.mainContainer.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      ((ISupportInitialize) this.objectProfileList).BeginInit();
      this.PrintTab.SuspendLayout();
      this.Console.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.charView.SuspendLayout();
      this.charContainer.BeginInit();
      this.charContainer.Panel1.SuspendLayout();
      this.charContainer.Panel2.SuspendLayout();
      this.charContainer.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.charSearchBar.SuspendLayout();
      this.KeyAnalysis.SuspendLayout();
      this.keyWizardContainer.BeginInit();
      this.keyWizardContainer.Panel1.SuspendLayout();
      this.keyWizardContainer.Panel2.SuspendLayout();
      this.keyWizardContainer.SuspendLayout();
      this.keyWizBorder.SuspendLayout();
      ((ISupportInitialize) this.dupeList).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.contextMenuStrip1.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.importProfilesToolStripMenuItem,
        (ToolStripItem) this.resetStatsToolStripMenuItem,
        (ToolStripItem) this.enableScheduleToolStripMenuItem,
        (ToolStripItem) this.disableScheduleToolStripMenuItem,
        (ToolStripItem) this.muleProfileToolStripMenuItem,
        (ToolStripItem) this.nextKeyToolStripMenuItem,
        (ToolStripItem) this.releaseKeyToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(176, 158);
      this.importProfilesToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("importProfilesToolStripMenuItem.Image");
      this.importProfilesToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.importProfilesToolStripMenuItem.Name = "importProfilesToolStripMenuItem";
      this.importProfilesToolStripMenuItem.Size = new Size(175, 22);
      this.importProfilesToolStripMenuItem.Text = "Import Profiles";
      this.importProfilesToolStripMenuItem.Click += new EventHandler(this.ImportProfilesToolStripMenuItem_Click);
      this.resetStatsToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("resetStatsToolStripMenuItem.Image");
      this.resetStatsToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.resetStatsToolStripMenuItem.Name = "resetStatsToolStripMenuItem";
      this.resetStatsToolStripMenuItem.ShowShortcutKeys = false;
      this.resetStatsToolStripMenuItem.Size = new Size(175, 22);
      this.resetStatsToolStripMenuItem.Text = "Reset Stats";
      this.resetStatsToolStripMenuItem.Click += new EventHandler(this.resetStatsToolStripMenuItem_Click);
      this.enableScheduleToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("enableScheduleToolStripMenuItem.Image");
      this.enableScheduleToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.enableScheduleToolStripMenuItem.Name = "enableScheduleToolStripMenuItem";
      this.enableScheduleToolStripMenuItem.Size = new Size(175, 22);
      this.enableScheduleToolStripMenuItem.Text = "Enable Schedule";
      this.enableScheduleToolStripMenuItem.Click += new EventHandler(this.enableScheduleToolStripMenuItem_Click);
      this.disableScheduleToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("disableScheduleToolStripMenuItem.Image");
      this.disableScheduleToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.disableScheduleToolStripMenuItem.Name = "disableScheduleToolStripMenuItem";
      this.disableScheduleToolStripMenuItem.Size = new Size(175, 22);
      this.disableScheduleToolStripMenuItem.Text = "Disable Schedule";
      this.disableScheduleToolStripMenuItem.Click += new EventHandler(this.disableScheduleToolStripMenuItem_Click);
      this.muleProfileToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("muleProfileToolStripMenuItem.Image");
      this.muleProfileToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.muleProfileToolStripMenuItem.Name = "muleProfileToolStripMenuItem";
      this.muleProfileToolStripMenuItem.Size = new Size(175, 22);
      this.muleProfileToolStripMenuItem.Text = "Mule Profile";
      this.muleProfileToolStripMenuItem.Click += new EventHandler(this.muleProfileToolStripMenuItem_Click);
      this.nextKeyToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("nextKeyToolStripMenuItem.Image");
      this.nextKeyToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.nextKeyToolStripMenuItem.Name = "nextKeyToolStripMenuItem";
      this.nextKeyToolStripMenuItem.Size = new Size(175, 22);
      this.nextKeyToolStripMenuItem.Text = "Next Key";
      this.nextKeyToolStripMenuItem.Click += new EventHandler(this.nextKeyToolStripMenuItem_Click);
      this.releaseKeyToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("releaseKeyToolStripMenuItem.Image");
      this.releaseKeyToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.releaseKeyToolStripMenuItem.Name = "releaseKeyToolStripMenuItem";
      this.releaseKeyToolStripMenuItem.Size = new Size(175, 22);
      this.releaseKeyToolStripMenuItem.Text = "Release Key";
      this.releaseKeyToolStripMenuItem.Click += new EventHandler(this.releaseKeyToolStripMenuItem_Click);
      this.contextMenuStrip3.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.contextMenuStrip3.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip3.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.toolStripMenuItem3,
        (ToolStripItem) this.importToolStripMenuItem,
        (ToolStripItem) this.clearToolStripMenuItem
      });
      this.contextMenuStrip3.Name = "contextMenuStrip3";
      this.contextMenuStrip3.Size = new Size(117, 70);
      this.toolStripMenuItem3.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.toolStripMenuItem3.Image = (Image) componentResourceManager.GetObject("toolStripMenuItem3.Image");
      this.toolStripMenuItem3.ImageScaling = ToolStripItemImageScaling.None;
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new Size(116, 22);
      this.toolStripMenuItem3.Text = "Export";
      this.toolStripMenuItem3.Click += new EventHandler(this.toolStripMenuItem3_Click);
      this.importToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.importToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("importToolStripMenuItem.Image");
      this.importToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.importToolStripMenuItem.Name = "importToolStripMenuItem";
      this.importToolStripMenuItem.Size = new Size(116, 22);
      this.importToolStripMenuItem.Text = "Import";
      this.importToolStripMenuItem.Click += new EventHandler(this.importToolStripMenuItem_Click);
      this.clearToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.clearToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("clearToolStripMenuItem.Image");
      this.clearToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
      this.clearToolStripMenuItem.Size = new Size(116, 22);
      this.clearToolStripMenuItem.Text = "Clear";
      this.clearToolStripMenuItem.Click += new EventHandler(this.ClearToolStripMenuItem_Click);
      this.contextMenuStrip4.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.contextMenuStrip4.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip4.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.toolItemSaveImage,
        (ToolStripItem) this.uploadImgurToolStripMenuItem,
        (ToolStripItem) this.copyImageToolStripMenuItem,
        (ToolStripItem) this.copyDescriptionToolStripMenuItem,
        (ToolStripItem) this.createDupeToolStripMenuItem,
        (ToolStripItem) this.removeToolStripMenuItem,
        (ToolStripItem) this.clearItemsToolStripMenuItem
      });
      this.contextMenuStrip4.Name = "contextMenuStrip4";
      this.contextMenuStrip4.Size = new Size(178, 158);
      this.toolItemSaveImage.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.toolItemSaveImage.Image = (Image) componentResourceManager.GetObject("toolItemSaveImage.Image");
      this.toolItemSaveImage.ImageScaling = ToolStripItemImageScaling.None;
      this.toolItemSaveImage.Name = "toolItemSaveImage";
      this.toolItemSaveImage.Size = new Size(177, 22);
      this.toolItemSaveImage.Text = "Save Image";
      this.toolItemSaveImage.Click += new EventHandler(this.ToolItemSaveImage_Click);
      this.uploadImgurToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.uploadImgurToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("uploadImgurToolStripMenuItem.Image");
      this.uploadImgurToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.uploadImgurToolStripMenuItem.Name = "uploadImgurToolStripMenuItem";
      this.uploadImgurToolStripMenuItem.Size = new Size(177, 22);
      this.uploadImgurToolStripMenuItem.Text = "Upload to Imgur";
      this.uploadImgurToolStripMenuItem.Click += new EventHandler(this.UploadImgurToolStripMenuItem_Click);
      this.copyImageToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.copyImageToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("copyImageToolStripMenuItem.Image");
      this.copyImageToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
      this.copyImageToolStripMenuItem.Size = new Size(177, 22);
      this.copyImageToolStripMenuItem.Text = "Copy Image";
      this.copyImageToolStripMenuItem.Click += new EventHandler(this.CopyImageToolStripMenuItem_Click);
      this.copyDescriptionToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.copyDescriptionToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("copyDescriptionToolStripMenuItem.Image");
      this.copyDescriptionToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.copyDescriptionToolStripMenuItem.Name = "copyDescriptionToolStripMenuItem";
      this.copyDescriptionToolStripMenuItem.Size = new Size(177, 22);
      this.copyDescriptionToolStripMenuItem.Text = "Copy Description";
      this.copyDescriptionToolStripMenuItem.Click += new EventHandler(this.CopyDescriptionToolStripMenuItem_Click);
      this.createDupeToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.createDupeToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("createDupeToolStripMenuItem.Image");
      this.createDupeToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.createDupeToolStripMenuItem.Name = "createDupeToolStripMenuItem";
      this.createDupeToolStripMenuItem.Size = new Size(177, 22);
      this.createDupeToolStripMenuItem.Text = "Create Dupe";
      this.createDupeToolStripMenuItem.Click += new EventHandler(this.CreateDupeToolStripMenuItem_Click);
      this.removeToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.removeToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("removeToolStripMenuItem.Image");
      this.removeToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
      this.removeToolStripMenuItem.Size = new Size(177, 22);
      this.removeToolStripMenuItem.Text = "Remove Item";
      this.removeToolStripMenuItem.Click += new EventHandler(this.RemoveToolStripMenuItem_Click);
      this.clearItemsToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.clearItemsToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("clearItemsToolStripMenuItem.Image");
      this.clearItemsToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.clearItemsToolStripMenuItem.Name = "clearItemsToolStripMenuItem";
      this.clearItemsToolStripMenuItem.Size = new Size(177, 22);
      this.clearItemsToolStripMenuItem.Text = "Clear Items";
      this.clearItemsToolStripMenuItem.Click += new EventHandler(this.ClearItemsToolStripMenuItem_Click);
      this.contextMenuStrip5.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.contextMenuStrip5.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip5.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.pauseKeyToolStripMenuItem,
        (ToolStripItem) this.resumeKeyToolStripMenuItem
      });
      this.contextMenuStrip5.Name = "contextMenuStrip5";
      this.contextMenuStrip5.Size = new Size(149, 48);
      this.pauseKeyToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pauseKeyToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("pauseKeyToolStripMenuItem.Image");
      this.pauseKeyToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.pauseKeyToolStripMenuItem.Name = "pauseKeyToolStripMenuItem";
      this.pauseKeyToolStripMenuItem.Size = new Size(148, 22);
      this.pauseKeyToolStripMenuItem.Text = "Pause Key";
      this.pauseKeyToolStripMenuItem.Click += new EventHandler(this.pauseKeyToolStripMenuItem_Click);
      this.resumeKeyToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.resumeKeyToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("resumeKeyToolStripMenuItem.Image");
      this.resumeKeyToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.resumeKeyToolStripMenuItem.Name = "resumeKeyToolStripMenuItem";
      this.resumeKeyToolStripMenuItem.Size = new Size(148, 22);
      this.resumeKeyToolStripMenuItem.Text = "Resume Key";
      this.resumeKeyToolStripMenuItem.Click += new EventHandler(this.resumeKeyToolStripMenuItem_Click);
      this.contextMenuStrip2.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.contextMenuStrip2.ImageScalingSize = new Size(20, 20);
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.removeKeyToolStripMenuItem,
        (ToolStripItem) this.removeFromListToolStripMenuItem,
        (ToolStripItem) this.clearDetailsToolStripMenuItem
      });
      this.contextMenuStrip2.Name = "contextMenuStrip2";
      this.contextMenuStrip2.Size = new Size(182, 70);
      this.removeKeyToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.removeKeyToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("removeKeyToolStripMenuItem.Image");
      this.removeKeyToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.removeKeyToolStripMenuItem.Name = "removeKeyToolStripMenuItem";
      this.removeKeyToolStripMenuItem.Size = new Size(181, 22);
      this.removeKeyToolStripMenuItem.Text = "Remove Key";
      this.removeKeyToolStripMenuItem.Click += new EventHandler(this.RemoveKeyToolStripMenuItem_Click);
      this.removeFromListToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.removeFromListToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("removeFromListToolStripMenuItem.Image");
      this.removeFromListToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.removeFromListToolStripMenuItem.Name = "removeFromListToolStripMenuItem";
      this.removeFromListToolStripMenuItem.Size = new Size(181, 22);
      this.removeFromListToolStripMenuItem.Text = "Remove From List";
      this.removeFromListToolStripMenuItem.Click += new EventHandler(this.RemoveFromListToolStripMenuItem_Click);
      this.clearDetailsToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.clearDetailsToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("clearDetailsToolStripMenuItem.Image");
      this.clearDetailsToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.clearDetailsToolStripMenuItem.Name = "clearDetailsToolStripMenuItem";
      this.clearDetailsToolStripMenuItem.Size = new Size(181, 22);
      this.clearDetailsToolStripMenuItem.Text = "Clear Wizard";
      this.clearDetailsToolStripMenuItem.Click += new EventHandler(this.ClearDetailsToolStripMenuItem_Click);
      this.guiStart.AccessibleName = "guiName";
      this.guiStart.AutoSize = false;
      this.guiStart.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiStart.Image = (Image) componentResourceManager.GetObject("guiStart.Image");
      this.guiStart.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiStart.ImageScaling = ToolStripItemImageScaling.None;
      this.guiStart.ImageTransparentColor = Color.Magenta;
      this.guiStart.Margin = new Padding(0);
      this.guiStart.Name = "guiStart";
      this.guiStart.Size = new Size(145, 36);
      this.guiStart.Text = "  Start";
      this.guiStart.ToolTipText = "Start Selected Profiles";
      this.guiStart.Click += new EventHandler(this.StartProfile);
      this.guiStop.AutoSize = false;
      this.guiStop.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiStop.Image = (Image) componentResourceManager.GetObject("guiStop.Image");
      this.guiStop.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiStop.ImageScaling = ToolStripItemImageScaling.None;
      this.guiStop.ImageTransparentColor = Color.Magenta;
      this.guiStop.Margin = new Padding(0);
      this.guiStop.Name = "guiStop";
      this.guiStop.Size = new Size(145, 36);
      this.guiStop.Text = "  Stop";
      this.guiStop.ToolTipText = "Stop Selected Profiles";
      this.guiStop.Click += new EventHandler(this.StopProfile);
      this.guiAdd.AutoSize = false;
      this.guiAdd.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiAdd.Image = (Image) componentResourceManager.GetObject("guiAdd.Image");
      this.guiAdd.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiAdd.ImageScaling = ToolStripItemImageScaling.None;
      this.guiAdd.ImageTransparentColor = Color.Magenta;
      this.guiAdd.Margin = new Padding(0);
      this.guiAdd.Name = "guiAdd";
      this.guiAdd.Size = new Size(145, 36);
      this.guiAdd.Text = "  Add";
      this.guiAdd.ToolTipText = "Add New Profile";
      this.guiAdd.Click += new EventHandler(this.Add);
      this.guiEdit.AutoSize = false;
      this.guiEdit.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiEdit.Image = (Image) componentResourceManager.GetObject("guiEdit.Image");
      this.guiEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiEdit.ImageScaling = ToolStripItemImageScaling.None;
      this.guiEdit.ImageTransparentColor = Color.Magenta;
      this.guiEdit.Margin = new Padding(0);
      this.guiEdit.Name = "guiEdit";
      this.guiEdit.Size = new Size(145, 36);
      this.guiEdit.Text = "  Edit";
      this.guiEdit.ToolTipText = "Edit Selected Profiles";
      this.guiEdit.Click += new EventHandler(this.Edit);
      this.guiSave.AutoSize = false;
      this.guiSave.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiSave.Image = (Image) componentResourceManager.GetObject("guiSave.Image");
      this.guiSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiSave.ImageScaling = ToolStripItemImageScaling.None;
      this.guiSave.ImageTransparentColor = Color.Magenta;
      this.guiSave.Margin = new Padding(0);
      this.guiSave.Name = "guiSave";
      this.guiSave.Size = new Size(145, 36);
      this.guiSave.Text = "  Save";
      this.guiSave.ToolTipText = "Save Runtime Info";
      this.guiSave.Click += new EventHandler(this.Save);
      this.guiCopy.AutoSize = false;
      this.guiCopy.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiCopy.Image = (Image) componentResourceManager.GetObject("guiCopy.Image");
      this.guiCopy.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiCopy.ImageScaling = ToolStripItemImageScaling.None;
      this.guiCopy.ImageTransparentColor = Color.Magenta;
      this.guiCopy.Margin = new Padding(0);
      this.guiCopy.Name = "guiCopy";
      this.guiCopy.Size = new Size(145, 36);
      this.guiCopy.Text = "  Clone";
      this.guiCopy.ToolTipText = "Clone Selected Profiles";
      this.guiCopy.Click += new EventHandler(this.Duplicate);
      this.guiShow.AutoSize = false;
      this.guiShow.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiShow.Image = (Image) componentResourceManager.GetObject("guiShow.Image");
      this.guiShow.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiShow.ImageScaling = ToolStripItemImageScaling.None;
      this.guiShow.ImageTransparentColor = Color.Magenta;
      this.guiShow.Margin = new Padding(0);
      this.guiShow.Name = "guiShow";
      this.guiShow.Size = new Size(145, 36);
      this.guiShow.Text = "  Show";
      this.guiShow.ToolTipText = "Show Selected Profiles";
      this.guiShow.Click += new EventHandler(this.ShowWindow);
      this.guiHide.AutoSize = false;
      this.guiHide.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiHide.Image = (Image) componentResourceManager.GetObject("guiHide.Image");
      this.guiHide.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiHide.ImageScaling = ToolStripItemImageScaling.None;
      this.guiHide.ImageTransparentColor = Color.Magenta;
      this.guiHide.Margin = new Padding(0);
      this.guiHide.Name = "guiHide";
      this.guiHide.Size = new Size(145, 36);
      this.guiHide.Text = "  Hide";
      this.guiHide.ToolTipText = "Hide Selected Profiles";
      this.guiHide.Click += new EventHandler(this.HideWindow);
      this.guiAbout.AutoSize = false;
      this.guiAbout.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiAbout.Image = (Image) componentResourceManager.GetObject("guiAbout.Image");
      this.guiAbout.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiAbout.ImageScaling = ToolStripItemImageScaling.None;
      this.guiAbout.ImageTransparentColor = Color.Magenta;
      this.guiAbout.Margin = new Padding(0);
      this.guiAbout.Name = "guiAbout";
      this.guiAbout.Size = new Size(145, 36);
      this.guiAbout.Text = "  About";
      this.guiAbout.ToolTipText = "About";
      this.guiAbout.Click += new EventHandler(this.AboutDialog);
      this.toolStrip1.BackColor = SystemColors.ButtonHighlight;
      this.toolStrip1.BackgroundImageLayout = ImageLayout.None;
      this.toolStrip1.CanOverflow = false;
      this.toolStrip1.Dock = DockStyle.Fill;
      this.toolStrip1.GripMargin = new Padding(0);
      this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
      this.toolStrip1.ImageScalingSize = new Size(48, 32);
      this.toolStrip1.Items.AddRange(new ToolStripItem[15]
      {
        (ToolStripItem) this.menuDropDown,
        (ToolStripItem) this.settingsDropDown,
        (ToolStripItem) this.guiStart,
        (ToolStripItem) this.guiStop,
        (ToolStripItem) this.guiShow,
        (ToolStripItem) this.guiHide,
        (ToolStripItem) this.guiAdd,
        (ToolStripItem) this.guiRemove,
        (ToolStripItem) this.guiEdit,
        (ToolStripItem) this.guiCopy,
        (ToolStripItem) this.guiAddIRC,
        (ToolStripItem) this.keysEdit,
        (ToolStripItem) this.scheduleEdit,
        (ToolStripItem) this.guiSave,
        (ToolStripItem) this.guiAbout
      });
      this.toolStrip1.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
      this.toolStrip1.Location = new Point(1, 1);
      this.toolStrip1.MinimumSize = new Size(0, 600);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Padding = new Padding(0);
      this.toolStrip1.RenderMode = ToolStripRenderMode.Professional;
      this.toolStrip1.Size = new Size(146, 600);
      this.toolStrip1.Stretch = true;
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      this.menuDropDown.AutoSize = false;
      this.menuDropDown.DropDownItems.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.menuStart,
        (ToolStripItem) this.menuStop,
        (ToolStripItem) this.menuShow,
        (ToolStripItem) this.menuHide,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.menuSave,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.menuExit,
        (ToolStripItem) this.systemTrayToolStripMenuItem
      });
      this.menuDropDown.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuDropDown.Image = (Image) componentResourceManager.GetObject("menuDropDown.Image");
      this.menuDropDown.ImageAlign = ContentAlignment.MiddleLeft;
      this.menuDropDown.ImageScaling = ToolStripItemImageScaling.None;
      this.menuDropDown.ImageTransparentColor = Color.Magenta;
      this.menuDropDown.Margin = new Padding(0);
      this.menuDropDown.Name = "menuDropDown";
      this.menuDropDown.Size = new Size(145, 36);
      this.menuDropDown.Text = "  Menu";
      this.menuStart.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuStart.Image = (Image) componentResourceManager.GetObject("menuStart.Image");
      this.menuStart.ImageScaling = ToolStripItemImageScaling.None;
      this.menuStart.ImageTransparentColor = Color.Magenta;
      this.menuStart.Name = "menuStart";
      this.menuStart.ShortcutKeys = Keys.A | Keys.Alt;
      this.menuStart.Size = new Size(171, 22);
      this.menuStart.Text = "Start All";
      this.menuStart.Click += new EventHandler(this.MenuHandler);
      this.menuStop.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuStop.Image = (Image) componentResourceManager.GetObject("menuStop.Image");
      this.menuStop.ImageScaling = ToolStripItemImageScaling.None;
      this.menuStop.ImageTransparentColor = Color.Magenta;
      this.menuStop.Name = "menuStop";
      this.menuStop.ShortcutKeys = Keys.S | Keys.Alt;
      this.menuStop.Size = new Size(171, 22);
      this.menuStop.Text = "Stop All";
      this.menuStop.Click += new EventHandler(this.MenuHandler);
      this.menuShow.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuShow.Image = (Image) componentResourceManager.GetObject("menuShow.Image");
      this.menuShow.ImageScaling = ToolStripItemImageScaling.None;
      this.menuShow.Name = "menuShow";
      this.menuShow.ShortcutKeys = Keys.Y | Keys.Control;
      this.menuShow.Size = new Size(171, 22);
      this.menuShow.Text = "Show All";
      this.menuShow.Click += new EventHandler(this.MenuHandler);
      this.menuHide.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuHide.Image = (Image) componentResourceManager.GetObject("menuHide.Image");
      this.menuHide.ImageScaling = ToolStripItemImageScaling.None;
      this.menuHide.Name = "menuHide";
      this.menuHide.ShortcutKeys = Keys.H | Keys.Control;
      this.menuHide.Size = new Size(171, 22);
      this.menuHide.Text = "Hide All";
      this.menuHide.Click += new EventHandler(this.MenuHandler);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(168, 6);
      this.menuSave.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuSave.Image = (Image) componentResourceManager.GetObject("menuSave.Image");
      this.menuSave.ImageScaling = ToolStripItemImageScaling.None;
      this.menuSave.ImageTransparentColor = Color.Magenta;
      this.menuSave.Name = "menuSave";
      this.menuSave.ShortcutKeys = Keys.S | Keys.Control;
      this.menuSave.Size = new Size(171, 22);
      this.menuSave.Text = "Save";
      this.menuSave.Click += new EventHandler(this.Save);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(168, 6);
      this.menuExit.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuExit.ImageScaling = ToolStripItemImageScaling.None;
      this.menuExit.Name = "menuExit";
      this.menuExit.ShortcutKeys = Keys.F4 | Keys.Alt;
      this.menuExit.Size = new Size(171, 22);
      this.menuExit.Text = "Exit";
      this.menuExit.Click += new EventHandler(this.MenuHandler);
      this.systemTrayToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.systemTrayToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.systemTrayToolStripMenuItem.Name = "systemTrayToolStripMenuItem";
      this.systemTrayToolStripMenuItem.Size = new Size(171, 22);
      this.systemTrayToolStripMenuItem.Text = "System Tray";
      this.systemTrayToolStripMenuItem.Click += new EventHandler(this.MenuHandler);
      this.settingsDropDown.AutoSize = false;
      this.settingsDropDown.DropDownItems.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.versionMenuItem,
        (ToolStripItem) this.responseTimeToolStripMenuItem,
        (ToolStripItem) this.loadDelay,
        (ToolStripItem) this.startHidden,
        (ToolStripItem) this.systemFont,
        (ToolStripItem) this.itemHeader,
        (ToolStripItem) this.refreshCharViewToolStripMenuItem,
        (ToolStripItem) this.closeGameexeToolStripMenuItem,
        (ToolStripItem) this.serverToggle,
        (ToolStripItem) this.debugMode
      });
      this.settingsDropDown.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.settingsDropDown.Image = (Image) componentResourceManager.GetObject("settingsDropDown.Image");
      this.settingsDropDown.ImageAlign = ContentAlignment.MiddleLeft;
      this.settingsDropDown.ImageScaling = ToolStripItemImageScaling.None;
      this.settingsDropDown.ImageTransparentColor = Color.Magenta;
      this.settingsDropDown.Margin = new Padding(0);
      this.settingsDropDown.Name = "settingsDropDown";
      this.settingsDropDown.Size = new Size(145, 36);
      this.settingsDropDown.Text = "  Settings";
      this.settingsDropDown.TextAlign = ContentAlignment.MiddleRight;
      this.versionMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.versionMenuItem.Image = (Image) componentResourceManager.GetObject("versionMenuItem.Image");
      this.versionMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.versionMenuItem.Name = "versionMenuItem";
      this.versionMenuItem.Size = new Size(185, 22);
      this.versionMenuItem.Text = "D2 Version";
      this.responseTimeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.secondsToolStripMenuItem,
        (ToolStripItem) this.secondsToolStripMenuItem1,
        (ToolStripItem) this.secondsToolStripMenuItem2,
        (ToolStripItem) this.secondsToolStripMenuItem3,
        (ToolStripItem) this.secondsToolStripMenuItem4
      });
      this.responseTimeToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.responseTimeToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("responseTimeToolStripMenuItem.Image");
      this.responseTimeToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.responseTimeToolStripMenuItem.Name = "responseTimeToolStripMenuItem";
      this.responseTimeToolStripMenuItem.Size = new Size(185, 22);
      this.responseTimeToolStripMenuItem.Text = "Response Time";
      this.secondsToolStripMenuItem.CheckOnClick = true;
      this.secondsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.secondsToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.secondsToolStripMenuItem.Name = "secondsToolStripMenuItem";
      this.secondsToolStripMenuItem.Size = new Size(143, 22);
      this.secondsToolStripMenuItem.Text = "10 Seconds";
      this.secondsToolStripMenuItem.TextImageRelation = TextImageRelation.Overlay;
      this.secondsToolStripMenuItem.Click += new EventHandler(this.MenuHandler);
      this.secondsToolStripMenuItem1.CheckOnClick = true;
      this.secondsToolStripMenuItem1.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.secondsToolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
      this.secondsToolStripMenuItem1.Name = "secondsToolStripMenuItem1";
      this.secondsToolStripMenuItem1.Size = new Size(143, 22);
      this.secondsToolStripMenuItem1.Text = "15 Seconds";
      this.secondsToolStripMenuItem1.Click += new EventHandler(this.MenuHandler);
      this.secondsToolStripMenuItem2.CheckOnClick = true;
      this.secondsToolStripMenuItem2.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.secondsToolStripMenuItem2.ImageScaling = ToolStripItemImageScaling.None;
      this.secondsToolStripMenuItem2.Name = "secondsToolStripMenuItem2";
      this.secondsToolStripMenuItem2.Size = new Size(143, 22);
      this.secondsToolStripMenuItem2.Text = "20 Seconds";
      this.secondsToolStripMenuItem2.Click += new EventHandler(this.MenuHandler);
      this.secondsToolStripMenuItem3.CheckOnClick = true;
      this.secondsToolStripMenuItem3.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.secondsToolStripMenuItem3.ImageScaling = ToolStripItemImageScaling.None;
      this.secondsToolStripMenuItem3.Name = "secondsToolStripMenuItem3";
      this.secondsToolStripMenuItem3.Size = new Size(143, 22);
      this.secondsToolStripMenuItem3.Text = "25 Seconds";
      this.secondsToolStripMenuItem3.Click += new EventHandler(this.MenuHandler);
      this.secondsToolStripMenuItem4.CheckOnClick = true;
      this.secondsToolStripMenuItem4.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.secondsToolStripMenuItem4.ImageScaling = ToolStripItemImageScaling.None;
      this.secondsToolStripMenuItem4.Name = "secondsToolStripMenuItem4";
      this.secondsToolStripMenuItem4.Size = new Size(143, 22);
      this.secondsToolStripMenuItem4.Text = "30 Seconds";
      this.secondsToolStripMenuItem4.Click += new EventHandler(this.MenuHandler);
      this.loadDelay.CheckOnClick = true;
      this.loadDelay.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.ms100,
        (ToolStripItem) this.ms250,
        (ToolStripItem) this.ms500,
        (ToolStripItem) this.ms1000,
        (ToolStripItem) this.ms2000,
        (ToolStripItem) this.ms5000
      });
      this.loadDelay.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.loadDelay.ImageScaling = ToolStripItemImageScaling.None;
      this.loadDelay.Name = "loadDelay";
      this.loadDelay.Size = new Size(185, 22);
      this.loadDelay.Text = "Load Delay";
      this.loadDelay.Click += new EventHandler(this.MenuHandler);
      this.ms100.CheckOnClick = true;
      this.ms100.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.ms100.ImageScaling = ToolStripItemImageScaling.None;
      this.ms100.Name = "ms100";
      this.ms100.Size = new Size(125, 22);
      this.ms100.Text = "100 ms";
      this.ms100.TextImageRelation = TextImageRelation.Overlay;
      this.ms100.Click += new EventHandler(this.MenuHandler);
      this.ms250.CheckOnClick = true;
      this.ms250.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.ms250.ImageScaling = ToolStripItemImageScaling.None;
      this.ms250.Name = "ms250";
      this.ms250.Size = new Size(125, 22);
      this.ms250.Text = "250 ms";
      this.ms250.TextImageRelation = TextImageRelation.Overlay;
      this.ms250.Click += new EventHandler(this.MenuHandler);
      this.ms500.CheckOnClick = true;
      this.ms500.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.ms500.ImageScaling = ToolStripItemImageScaling.None;
      this.ms500.Name = "ms500";
      this.ms500.Size = new Size(125, 22);
      this.ms500.Text = "500 ms";
      this.ms500.TextImageRelation = TextImageRelation.Overlay;
      this.ms500.Click += new EventHandler(this.MenuHandler);
      this.ms1000.CheckOnClick = true;
      this.ms1000.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.ms1000.ImageScaling = ToolStripItemImageScaling.None;
      this.ms1000.Name = "ms1000";
      this.ms1000.Size = new Size(125, 22);
      this.ms1000.Text = "1000 ms";
      this.ms1000.TextImageRelation = TextImageRelation.Overlay;
      this.ms1000.Click += new EventHandler(this.MenuHandler);
      this.ms2000.CheckOnClick = true;
      this.ms2000.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.ms2000.ImageScaling = ToolStripItemImageScaling.None;
      this.ms2000.Name = "ms2000";
      this.ms2000.Size = new Size(125, 22);
      this.ms2000.Text = "2000 ms";
      this.ms2000.TextImageRelation = TextImageRelation.Overlay;
      this.ms2000.Click += new EventHandler(this.MenuHandler);
      this.ms5000.CheckOnClick = true;
      this.ms5000.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.ms5000.ImageScaling = ToolStripItemImageScaling.None;
      this.ms5000.Name = "ms5000";
      this.ms5000.Size = new Size(125, 22);
      this.ms5000.Text = "5000 ms";
      this.ms5000.TextImageRelation = TextImageRelation.Overlay;
      this.ms5000.Click += new EventHandler(this.MenuHandler);
      this.startHidden.CheckOnClick = true;
      this.startHidden.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.startHidden.ImageScaling = ToolStripItemImageScaling.None;
      this.startHidden.Name = "startHidden";
      this.startHidden.Size = new Size(185, 22);
      this.startHidden.Text = "Start Hidden";
      this.startHidden.Click += new EventHandler(this.MenuHandler);
      this.systemFont.CheckOnClick = true;
      this.systemFont.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.systemFont.ImageScaling = ToolStripItemImageScaling.None;
      this.systemFont.Name = "systemFont";
      this.systemFont.Size = new Size(185, 22);
      this.systemFont.Text = "Use System Font";
      this.systemFont.Click += new EventHandler(this.MenuHandler);
      this.itemHeader.CheckOnClick = true;
      this.itemHeader.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.itemHeader.ImageScaling = ToolStripItemImageScaling.None;
      this.itemHeader.Name = "itemHeader";
      this.itemHeader.Size = new Size(185, 22);
      this.itemHeader.Text = "Show Item Header";
      this.itemHeader.Click += new EventHandler(this.MenuHandler);
      this.refreshCharViewToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.refreshCharViewToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("refreshCharViewToolStripMenuItem.Image");
      this.refreshCharViewToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.refreshCharViewToolStripMenuItem.Name = "refreshCharViewToolStripMenuItem";
      this.refreshCharViewToolStripMenuItem.Size = new Size(185, 22);
      this.refreshCharViewToolStripMenuItem.Text = "Refresh Char View";
      this.refreshCharViewToolStripMenuItem.Click += new EventHandler(this.RefreshCharViewToolStripMenuItem_Click);
      this.closeGameexeToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.closeGameexeToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
      this.closeGameexeToolStripMenuItem.Name = "closeGameexeToolStripMenuItem";
      this.closeGameexeToolStripMenuItem.Size = new Size(185, 22);
      this.closeGameexeToolStripMenuItem.Text = "Close Game.exe";
      this.closeGameexeToolStripMenuItem.Click += new EventHandler(this.CloseGameexeToolStripMenuItem_Click);
      this.serverToggle.CheckOnClick = true;
      this.serverToggle.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.serverToggle.ImageScaling = ToolStripItemImageScaling.None;
      this.serverToggle.Name = "serverToggle";
      this.serverToggle.Size = new Size(185, 22);
      this.serverToggle.Text = "Run API Server";
      this.serverToggle.Click += new EventHandler(this.MenuHandler);
      this.debugMode.CheckOnClick = true;
      this.debugMode.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.debugMode.ImageScaling = ToolStripItemImageScaling.None;
      this.debugMode.Name = "debugMode";
      this.debugMode.Size = new Size(185, 22);
      this.debugMode.Text = "Developer Mode";
      this.debugMode.Click += new EventHandler(this.MenuHandler);
      this.guiRemove.AutoSize = false;
      this.guiRemove.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiRemove.Image = (Image) componentResourceManager.GetObject("guiRemove.Image");
      this.guiRemove.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiRemove.ImageScaling = ToolStripItemImageScaling.None;
      this.guiRemove.ImageTransparentColor = Color.White;
      this.guiRemove.Margin = new Padding(0);
      this.guiRemove.Name = "guiRemove";
      this.guiRemove.Size = new Size(145, 36);
      this.guiRemove.Text = "  Delete";
      this.guiRemove.ToolTipText = "Delete Selected Profiles";
      this.guiRemove.Click += new EventHandler(this.Remove);
      this.guiAddIRC.AutoSize = false;
      this.guiAddIRC.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.guiAddIRC.Image = (Image) componentResourceManager.GetObject("guiAddIRC.Image");
      this.guiAddIRC.ImageAlign = ContentAlignment.MiddleLeft;
      this.guiAddIRC.ImageScaling = ToolStripItemImageScaling.None;
      this.guiAddIRC.ImageTransparentColor = Color.Magenta;
      this.guiAddIRC.Margin = new Padding(0);
      this.guiAddIRC.Name = "guiAddIRC";
      this.guiAddIRC.Size = new Size(145, 36);
      this.guiAddIRC.Text = "  Add IRC";
      this.guiAddIRC.ToolTipText = "Add IRC Profile";
      this.guiAddIRC.Click += new EventHandler(this.AddIRC);
      this.keysEdit.AutoSize = false;
      this.keysEdit.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.keysEdit.Image = (Image) componentResourceManager.GetObject("keysEdit.Image");
      this.keysEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.keysEdit.ImageScaling = ToolStripItemImageScaling.None;
      this.keysEdit.ImageTransparentColor = Color.Magenta;
      this.keysEdit.Margin = new Padding(0);
      this.keysEdit.Name = "keysEdit";
      this.keysEdit.Size = new Size(145, 36);
      this.keysEdit.Text = "  Keys";
      this.keysEdit.ToolTipText = "Create or Import Keylist";
      this.keysEdit.Click += new EventHandler(this.KeysEdit_Click);
      this.scheduleEdit.AutoSize = false;
      this.scheduleEdit.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.scheduleEdit.Image = (Image) componentResourceManager.GetObject("scheduleEdit.Image");
      this.scheduleEdit.ImageAlign = ContentAlignment.MiddleLeft;
      this.scheduleEdit.ImageScaling = ToolStripItemImageScaling.None;
      this.scheduleEdit.ImageTransparentColor = Color.Magenta;
      this.scheduleEdit.Margin = new Padding(0);
      this.scheduleEdit.Name = "scheduleEdit";
      this.scheduleEdit.Size = new Size(145, 36);
      this.scheduleEdit.Text = "  Schedules";
      this.scheduleEdit.ToolTipText = "Create or Import Schedule";
      this.scheduleEdit.Click += new EventHandler(this.ScheduleEdit_Click);
      this.mainContainer.Dock = DockStyle.Fill;
      this.mainContainer.Location = new Point(148, 1);
      this.mainContainer.Margin = new Padding(0);
      this.mainContainer.Name = "mainContainer";
      this.mainContainer.Orientation = Orientation.Horizontal;
      this.mainContainer.Panel1.Controls.Add((Control) this.pictureBox1);
      this.mainContainer.Panel1.Controls.Add((Control) this.objectProfileList);
      this.mainContainer.Panel2.Controls.Add((Control) this.PrintTab);
      this.mainContainer.Size = new Size(755, 559);
      this.mainContainer.SplitterDistance = 245;
      this.mainContainer.SplitterWidth = 5;
      this.mainContainer.TabIndex = 5;
      this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.InitialImage = (Image) componentResourceManager.GetObject("pictureBox1.InitialImage");
      this.pictureBox1.Location = new Point(0, 145);
      this.pictureBox1.Margin = new Padding(0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(430, 100);
      this.pictureBox1.TabIndex = 7;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Visible = false;
      ((ListView) this.objectProfileList).Activation = ItemActivation.OneClick;
      this.objectProfileList.get_AllColumns().Add(this.profileCol);
      this.objectProfileList.get_AllColumns().Add(this.statusCol);
      this.objectProfileList.get_AllColumns().Add(this.runsCol);
      this.objectProfileList.get_AllColumns().Add(this.chickensCol);
      this.objectProfileList.get_AllColumns().Add(this.deathsCol);
      this.objectProfileList.get_AllColumns().Add(this.crashesCol);
      this.objectProfileList.get_AllColumns().Add(this.restartsCol);
      this.objectProfileList.get_AllColumns().Add(this.keyCol);
      this.objectProfileList.get_AllColumns().Add(this.gameExe);
      this.objectProfileList.set_AlternateRowBackColor(SystemColors.Control);
      ((ListView) this.objectProfileList).AutoArrange = false;
      ((ListView) this.objectProfileList).BorderStyle = BorderStyle.None;
      ((Control) this.objectProfileList).CausesValidation = false;
      this.objectProfileList.set_CellEditUseWholeCell(false);
      this.objectProfileList.get_Columns().AddRange(new ColumnHeader[9]
      {
        (ColumnHeader) this.profileCol,
        (ColumnHeader) this.statusCol,
        (ColumnHeader) this.runsCol,
        (ColumnHeader) this.chickensCol,
        (ColumnHeader) this.deathsCol,
        (ColumnHeader) this.crashesCol,
        (ColumnHeader) this.restartsCol,
        (ColumnHeader) this.keyCol,
        (ColumnHeader) this.gameExe
      });
      ((Control) this.objectProfileList).ContextMenuStrip = this.contextMenuStrip1;
      this.objectProfileList.set_CopySelectionOnControlC(false);
      this.objectProfileList.set_CopySelectionOnControlCUsesDragSource(false);
      ((Control) this.objectProfileList).Cursor = Cursors.Default;
      ((Control) this.objectProfileList).Dock = DockStyle.Fill;
      this.objectProfileList.set_EmptyListMsg("");
      ((Control) this.objectProfileList).Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      ((Control) this.objectProfileList).ForeColor = SystemColors.WindowText;
      ((ListView) this.objectProfileList).FullRowSelect = true;
      ((ListView) this.objectProfileList).GridLines = true;
      this.objectProfileList.set_HasCollapsibleGroups(false);
      this.objectProfileList.set_HeaderMaximumHeight(24);
      ((ListView) this.objectProfileList).HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.objectProfileList.set_HeaderUsesThemes(true);
      this.objectProfileList.set_IsSearchOnSortColumn(false);
      ((Control) this.objectProfileList).Location = new Point(0, 0);
      ((Control) this.objectProfileList).Margin = new Padding(0);
      ((Control) this.objectProfileList).Name = "objectProfileList";
      this.objectProfileList.set_RowHeight(21);
      this.objectProfileList.set_SelectedBackColor(SystemColors.MenuHighlight);
      this.objectProfileList.set_ShowGroups(false);
      this.objectProfileList.set_ShowSortIndicators(false);
      ((Control) this.objectProfileList).Size = new Size(755, 245);
      this.objectProfileList.set_SmallImageList(this.profileImages);
      this.objectProfileList.set_SortGroupItemsByPrimaryColumn(false);
      ((Control) this.objectProfileList).TabIndex = 1;
      this.objectProfileList.set_UseAlternatingBackColors(true);
      ((ListView) this.objectProfileList).UseCompatibleStateImageBehavior = false;
      this.objectProfileList.set_UseHotControls(false);
      this.objectProfileList.set_UseOverlays(false);
      this.objectProfileList.set_View(View.Details);
      this.objectProfileList.add_ModelDropped(new EventHandler<ModelDropEventArgs>(this.ObjectProfileList_ModelDropped));
      ((Control) this.objectProfileList).DoubleClick += new EventHandler(this.Edit);
      this.profileCol.set_AspectName("Name");
      this.profileCol.set_AutoCompleteEditor(false);
      this.profileCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.profileCol.set_Groupable(false);
      this.profileCol.set_GroupWithItemCountFormat("");
      this.profileCol.set_GroupWithItemCountSingularFormat("");
      this.profileCol.set_Hideable(false);
      this.profileCol.set_ImageAspectName("");
      this.profileCol.set_MinimumWidth(60);
      this.profileCol.set_Searchable(false);
      this.profileCol.set_Sortable(false);
      ((ColumnHeader) this.profileCol).Text = "Profile";
      this.profileCol.set_UseFiltering(false);
      this.profileCol.set_Width(125);
      this.statusCol.set_AspectName("State");
      this.statusCol.set_AutoCompleteEditor(false);
      this.statusCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.statusCol.set_Groupable(false);
      this.statusCol.set_MinimumWidth(120);
      this.statusCol.set_Searchable(false);
      this.statusCol.set_Sortable(false);
      ((ColumnHeader) this.statusCol).Text = "Status";
      this.statusCol.set_UseFiltering(false);
      this.statusCol.set_Width(120);
      this.runsCol.set_AspectName("Runs");
      this.runsCol.set_AutoCompleteEditor(false);
      this.runsCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.runsCol.set_Groupable(false);
      this.runsCol.set_MaximumWidth(50);
      this.runsCol.set_MinimumWidth(50);
      this.runsCol.set_Searchable(false);
      ((ColumnHeader) this.runsCol).Text = "Runs";
      this.runsCol.set_UseFiltering(false);
      this.runsCol.set_Width(50);
      this.chickensCol.set_AspectName("Chickens");
      this.chickensCol.set_AutoCompleteEditor(false);
      this.chickensCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.chickensCol.set_Groupable(false);
      this.chickensCol.set_MaximumWidth(50);
      this.chickensCol.set_MinimumWidth(50);
      this.chickensCol.set_Searchable(false);
      ((ColumnHeader) this.chickensCol).Text = "Exits";
      this.chickensCol.set_UseFiltering(false);
      this.chickensCol.set_Width(50);
      this.deathsCol.set_AspectName("Deaths");
      this.deathsCol.set_AutoCompleteEditor(false);
      this.deathsCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.deathsCol.set_Groupable(false);
      this.deathsCol.set_MaximumWidth(55);
      this.deathsCol.set_MinimumWidth(55);
      this.deathsCol.set_Searchable(false);
      ((ColumnHeader) this.deathsCol).Text = "Deaths";
      this.deathsCol.set_UseFiltering(false);
      this.deathsCol.set_Width(55);
      this.crashesCol.set_AspectName("Crashes");
      this.crashesCol.set_AutoCompleteEditor(false);
      this.crashesCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.crashesCol.set_Groupable(false);
      this.crashesCol.set_MaximumWidth(60);
      this.crashesCol.set_MinimumWidth(60);
      this.crashesCol.set_Searchable(false);
      ((ColumnHeader) this.crashesCol).Text = "Crashes";
      this.crashesCol.set_UseFiltering(false);
      this.restartsCol.set_AspectName("Restarts");
      this.restartsCol.set_AutoCompleteEditor(false);
      this.restartsCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.restartsCol.set_Groupable(false);
      this.restartsCol.set_MaximumWidth(60);
      this.restartsCol.set_MinimumWidth(60);
      this.restartsCol.set_Searchable(false);
      ((ColumnHeader) this.restartsCol).Text = "Restarts";
      this.restartsCol.set_UseFiltering(false);
      this.keyCol.set_AspectName("Key");
      this.keyCol.set_AutoCompleteEditor(false);
      this.keyCol.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.keyCol.set_Groupable(false);
      this.keyCol.set_MaximumWidth(150);
      this.keyCol.set_MinimumWidth(80);
      this.keyCol.set_Searchable(false);
      ((ColumnHeader) this.keyCol).Text = "Key";
      this.keyCol.set_UseFiltering(false);
      this.keyCol.set_Width(80);
      this.gameExe.set_AspectName("GameExe");
      this.gameExe.set_AutoCompleteEditor(false);
      this.gameExe.set_AutoCompleteEditorMode(AutoCompleteMode.None);
      this.gameExe.set_Groupable(false);
      this.gameExe.set_Hideable(false);
      this.gameExe.set_MinimumWidth(110);
      this.gameExe.set_Searchable(false);
      this.gameExe.set_Sortable(false);
      ((ColumnHeader) this.gameExe).Text = "Target";
      this.gameExe.set_UseFiltering(false);
      this.gameExe.set_Width(120);
      this.profileImages.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("profileImages.ImageStream");
      this.profileImages.TransparentColor = Color.Transparent;
      this.profileImages.Images.SetKeyName(0, "bot.ico");
      this.profileImages.Images.SetKeyName(1, "Console.ico");
      this.profileImages.Images.SetKeyName(2, "greenbot.ico");
      this.profileImages.Images.SetKeyName(3, "Bot.ico");
      this.PrintTab.Controls.Add((Control) this.Console);
      this.PrintTab.Controls.Add((Control) this.charView);
      this.PrintTab.Controls.Add((Control) this.KeyAnalysis);
      this.PrintTab.Dock = DockStyle.Fill;
      this.PrintTab.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.PrintTab.ItemSize = new Size(90, 20);
      this.PrintTab.Location = new Point(0, 0);
      this.PrintTab.Margin = new Padding(0);
      this.PrintTab.Multiline = true;
      this.PrintTab.Name = "PrintTab";
      this.PrintTab.Padding = new Point(0, 0);
      this.PrintTab.SelectedIndex = 0;
      this.PrintTab.Size = new Size(755, 309);
      this.PrintTab.SizeMode = TabSizeMode.Fixed;
      this.PrintTab.TabIndex = 1;
      this.PrintTab.Selected += new TabControlEventHandler(this.PrintTab_Selected);
      this.Console.Controls.Add((Control) this.splitContainer1);
      this.Console.Location = new Point(4, 24);
      this.Console.Margin = new Padding(0);
      this.Console.Name = "Console";
      this.Console.Size = new Size(747, 281);
      this.Console.TabIndex = 0;
      this.Console.Text = "Console";
      this.Console.UseVisualStyleBackColor = true;
      this.splitContainer1.BackColor = SystemColors.Control;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Margin = new Padding(0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.ConsoleBox);
      this.splitContainer1.Panel2.Controls.Add((Control) this.ItemLogger);
      this.splitContainer1.Size = new Size(747, 281);
      this.splitContainer1.SplitterDistance = 425;
      this.splitContainer1.TabIndex = 7;
      this.ItemLogger.AutoArrange = false;
      this.ItemLogger.BorderStyle = BorderStyle.None;
      this.ItemLogger.Columns.AddRange(new ColumnHeader[2]
      {
        this.ItemDateProfile,
        this.ItemLogColumn
      });
      this.ItemLogger.ContextMenuStrip = this.contextMenuStrip4;
      this.ItemLogger.Dock = DockStyle.Fill;
      this.ItemLogger.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ItemLogger.FullRowSelect = true;
      this.ItemLogger.HeaderStyle = ColumnHeaderStyle.None;
      this.ItemLogger.LabelWrap = false;
      this.ItemLogger.Location = new Point(0, 0);
      this.ItemLogger.Margin = new Padding(0);
      this.ItemLogger.MultiSelect = false;
      this.ItemLogger.Name = "ItemLogger";
      this.ItemLogger.ShowGroups = false;
      this.ItemLogger.Size = new Size(318, 281);
      this.ItemLogger.TabIndex = 5;
      this.ItemLogger.UseCompatibleStateImageBehavior = false;
      this.ItemLogger.View = View.Details;
      this.ItemLogger.SelectedIndexChanged += new EventHandler(this.ItemLogger_SelectedIndexChanged);
      this.ItemLogger.MouseLeave += new EventHandler(this.ItemLogger_MouseLeave);
      this.ItemLogger.MouseMove += new MouseEventHandler(this.Mouse_Over);
      this.ItemDateProfile.Text = "";
      this.ItemDateProfile.Width = 25;
      this.ItemLogColumn.Text = "";
      this.ItemLogColumn.Width = 100;
      this.charView.Controls.Add((Control) this.charContainer);
      this.charView.Location = new Point(4, 24);
      this.charView.Margin = new Padding(0);
      this.charView.Name = "charView";
      this.charView.Size = new Size(747, 281);
      this.charView.TabIndex = 2;
      this.charView.Text = "Char Viewer";
      this.charView.UseVisualStyleBackColor = true;
      this.charContainer.Dock = DockStyle.Fill;
      this.charContainer.FixedPanel = FixedPanel.Panel1;
      this.charContainer.IsSplitterFixed = true;
      this.charContainer.Location = new Point(0, 0);
      this.charContainer.Margin = new Padding(0);
      this.charContainer.Name = "charContainer";
      this.charContainer.Panel1.Controls.Add((Control) this.CharTree);
      this.charContainer.Panel2.Controls.Add((Control) this.tableLayoutPanel2);
      this.charContainer.Size = new Size(747, 281);
      this.charContainer.SplitterDistance = 265;
      this.charContainer.SplitterWidth = 1;
      this.charContainer.TabIndex = 0;
      this.CharTree.BorderStyle = BorderStyle.FixedSingle;
      this.CharTree.Dock = DockStyle.Fill;
      this.CharTree.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.CharTree.Location = new Point(0, 0);
      this.CharTree.Margin = new Padding(0);
      this.CharTree.Name = "CharTree";
      this.CharTree.Size = new Size(265, 281);
      this.CharTree.TabIndex = 0;
      this.CharTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.CharTree_NodeMouseClick);
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.Controls.Add((Control) this.charSearchBar, 0, 1);
      this.tableLayoutPanel2.Controls.Add((Control) this.CharItems, 0, 0);
      this.tableLayoutPanel2.Dock = DockStyle.Fill;
      this.tableLayoutPanel2.Location = new Point(0, 0);
      this.tableLayoutPanel2.Margin = new Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 27f));
      this.tableLayoutPanel2.Size = new Size(481, 281);
      this.tableLayoutPanel2.TabIndex = 13;
      this.charSearchBar.ColumnCount = 2;
      this.charSearchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.charSearchBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 27f));
      this.charSearchBar.Controls.Add((Control) this.SearchBox, 0, 0);
      this.charSearchBar.Controls.Add((Control) this.SearchButton, 1, 0);
      this.charSearchBar.Dock = DockStyle.Fill;
      this.charSearchBar.Location = new Point(0, 254);
      this.charSearchBar.Margin = new Padding(0);
      this.charSearchBar.Name = "charSearchBar";
      this.charSearchBar.RowCount = 1;
      this.charSearchBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.charSearchBar.Size = new Size(481, 27);
      this.charSearchBar.TabIndex = 0;
      this.SearchBox.BorderStyle = BorderStyle.FixedSingle;
      this.SearchBox.Dock = DockStyle.Fill;
      this.SearchBox.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.SearchBox.Location = new Point(0, 0);
      this.SearchBox.Margin = new Padding(0, 0, 1, 0);
      this.SearchBox.Name = "SearchBox";
      this.SearchBox.RightToLeft = RightToLeft.No;
      this.SearchBox.Size = new Size(453, 27);
      this.SearchBox.TabIndex = 9;
      this.SearchBox.KeyPress += new KeyPressEventHandler(this.SearchEnter);
      this.SearchButton.BackColor = SystemColors.Window;
      this.SearchButton.BackgroundImage = (Image) componentResourceManager.GetObject("SearchButton.BackgroundImage");
      this.SearchButton.BackgroundImageLayout = ImageLayout.Center;
      this.SearchButton.Dock = DockStyle.Fill;
      this.SearchButton.FlatAppearance.BorderColor = Color.DimGray;
      this.SearchButton.FlatAppearance.MouseDownBackColor = SystemColors.ButtonFace;
      this.SearchButton.FlatAppearance.MouseOverBackColor = Color.White;
      this.SearchButton.FlatStyle = FlatStyle.Flat;
      this.SearchButton.ForeColor = SystemColors.ButtonHighlight;
      this.SearchButton.Location = new Point(454, 0);
      this.SearchButton.Margin = new Padding(0);
      this.SearchButton.MaximumSize = new Size(27, 27);
      this.SearchButton.MinimumSize = new Size(27, 27);
      this.SearchButton.Name = "SearchButton";
      this.SearchButton.Padding = new Padding(0, 0, 0, 2);
      this.SearchButton.Size = new Size(27, 27);
      this.SearchButton.TabIndex = 11;
      this.SearchButton.TextImageRelation = TextImageRelation.ImageAboveText;
      this.SearchButton.UseVisualStyleBackColor = false;
      this.SearchButton.Click += new EventHandler(this.QueryItem);
      this.CharItems.BackColor = SystemColors.Window;
      this.CharItems.BorderStyle = BorderStyle.FixedSingle;
      this.CharItems.Columns.AddRange(new ColumnHeader[1]
      {
        this.CharItemColumn
      });
      this.CharItems.ContextMenuStrip = this.contextMenuStrip4;
      this.CharItems.Dock = DockStyle.Fill;
      this.CharItems.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.CharItems.FullRowSelect = true;
      this.CharItems.HeaderStyle = ColumnHeaderStyle.None;
      this.CharItems.LabelWrap = false;
      this.CharItems.Location = new Point(0, 0);
      this.CharItems.Margin = new Padding(0, 0, 0, 1);
      this.CharItems.MultiSelect = false;
      this.CharItems.Name = "CharItems";
      this.CharItems.Size = new Size(481, 253);
      this.CharItems.TabIndex = 6;
      this.CharItems.UseCompatibleStateImageBehavior = false;
      this.CharItems.View = View.Details;
      this.CharItems.VirtualListSize = 25;
      this.CharItems.SelectedIndexChanged += new EventHandler(this.CharLogger_SelectedIndexChanged);
      this.CharItems.MouseLeave += new EventHandler(this.ItemLogger_MouseLeave);
      this.CharItems.MouseMove += new MouseEventHandler(this.Mouse_Over);
      this.CharItemColumn.Width = 300;
      this.KeyAnalysis.Controls.Add((Control) this.keyWizardContainer);
      this.KeyAnalysis.Location = new Point(4, 24);
      this.KeyAnalysis.Margin = new Padding(0);
      this.KeyAnalysis.Name = "KeyAnalysis";
      this.KeyAnalysis.Size = new Size(747, 281);
      this.KeyAnalysis.TabIndex = 3;
      this.KeyAnalysis.Text = "Key Wizard";
      this.KeyAnalysis.UseVisualStyleBackColor = true;
      this.keyWizardContainer.Dock = DockStyle.Fill;
      this.keyWizardContainer.Location = new Point(0, 0);
      this.keyWizardContainer.Name = "keyWizardContainer";
      this.keyWizardContainer.Panel1.BackColor = Color.Transparent;
      this.keyWizardContainer.Panel1.Controls.Add((Control) this.keyWizBorder);
      this.keyWizardContainer.Panel2.Controls.Add((Control) this.KeyData);
      this.keyWizardContainer.Size = new Size(747, 281);
      this.keyWizardContainer.SplitterDistance = 204;
      this.keyWizardContainer.SplitterWidth = 1;
      this.keyWizardContainer.TabIndex = 3;
      this.keyWizBorder.BorderStyle = BorderStyle.FixedSingle;
      this.keyWizBorder.Controls.Add((Control) this.dupeList);
      this.keyWizBorder.Dock = DockStyle.Fill;
      this.keyWizBorder.Location = new Point(0, 0);
      this.keyWizBorder.Margin = new Padding(0);
      this.keyWizBorder.Name = "keyWizBorder";
      this.keyWizBorder.Size = new Size(204, 281);
      this.keyWizBorder.TabIndex = 3;
      this.dupeList.AllowUserToAddRows = false;
      this.dupeList.AllowUserToDeleteRows = false;
      this.dupeList.AllowUserToResizeRows = false;
      this.dupeList.BackgroundColor = SystemColors.ButtonHighlight;
      this.dupeList.BorderStyle = BorderStyle.None;
      this.dupeList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      this.dupeList.Columns.AddRange((DataGridViewColumn) this.keyDupe, (DataGridViewColumn) this.profileDupe);
      this.dupeList.ContextMenuStrip = this.contextMenuStrip5;
      this.dupeList.Dock = DockStyle.Fill;
      this.dupeList.GridColor = SystemColors.ActiveBorder;
      this.dupeList.Location = new Point(0, 0);
      this.dupeList.Margin = new Padding(0);
      this.dupeList.Name = "dupeList";
      this.dupeList.RowHeadersVisible = false;
      this.dupeList.RowHeadersWidth = 10;
      this.dupeList.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      this.dupeList.RowTemplate.Height = 16;
      this.dupeList.RowTemplate.Resizable = DataGridViewTriState.True;
      this.dupeList.ScrollBars = ScrollBars.Vertical;
      this.dupeList.SelectionMode = DataGridViewSelectionMode.CellSelect;
      this.dupeList.ShowCellErrors = false;
      this.dupeList.ShowCellToolTips = false;
      this.dupeList.ShowEditingIcon = false;
      this.dupeList.ShowRowErrors = false;
      this.dupeList.Size = new Size(202, 279);
      this.dupeList.TabIndex = 2;
      this.keyDupe.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.keyDupe.FillWeight = 30f;
      this.keyDupe.HeaderText = "Keys";
      this.keyDupe.MinimumWidth = 60;
      this.keyDupe.Name = "keyDupe";
      this.keyDupe.ReadOnly = true;
      this.profileDupe.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.profileDupe.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
      this.profileDupe.FillWeight = 59.39086f;
      this.profileDupe.HeaderText = "List Name(s)";
      this.profileDupe.MinimumWidth = 100;
      this.profileDupe.Name = "profileDupe";
      this.KeyData.BorderStyle = BorderStyle.FixedSingle;
      this.KeyData.Columns.AddRange(new ColumnHeader[5]
      {
        this.KeyProfile,
        this.KeyName,
        this.KeyInUse,
        this.KeyRD,
        this.KeyDisabled
      });
      this.KeyData.ContextMenuStrip = this.contextMenuStrip2;
      this.KeyData.Dock = DockStyle.Fill;
      this.KeyData.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyData.FullRowSelect = true;
      this.KeyData.GridLines = true;
      this.KeyData.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.KeyData.HideSelection = false;
      this.KeyData.Location = new Point(0, 0);
      this.KeyData.Margin = new Padding(0);
      this.KeyData.Name = "KeyData";
      this.KeyData.Size = new Size(542, 281);
      this.KeyData.Sorting = SortOrder.Ascending;
      this.KeyData.TabIndex = 0;
      this.KeyData.UseCompatibleStateImageBehavior = false;
      this.KeyData.View = View.Details;
      this.KeyProfile.Text = "Profile";
      this.KeyProfile.Width = 100;
      this.KeyName.Text = "Key";
      this.KeyName.Width = 110;
      this.KeyInUse.Text = "In Use";
      this.KeyInUse.Width = 90;
      this.KeyRD.Text = "RDs";
      this.KeyRD.Width = 80;
      this.KeyDisabled.Text = "Disabled";
      this.KeyDisabled.Width = 100;
      this.tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.toolStrip1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.mainContainer, 1, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Margin = new Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(904, 561);
      this.tableLayoutPanel1.TabIndex = 6;
      this.ConsoleBox.AcceptsTab = true;
      this.ConsoleBox.BorderStyle = BorderStyle.None;
      this.ConsoleBox.ContextMenuStrip = this.contextMenuStrip3;
      this.ConsoleBox.DetectUrls = true;
      this.ConsoleBox.Dock = DockStyle.Fill;
      this.ConsoleBox.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ConsoleBox.Location = new Point(0, 0);
      this.ConsoleBox.Margin = new Padding(0);
      this.ConsoleBox.Name = "ConsoleBox";
      this.ConsoleBox.ScrollBars = RichTextBoxScrollBars.Vertical;
      this.ConsoleBox.Size = new Size(425, 281);
      this.ConsoleBox.TabIndex = 0;
      this.ConsoleBox.Text = "";
      this.ConsoleBox.LinkClicked += new LinkClickedEventHandler(this.LinkClicked);
      this.AutoScaleDimensions = new SizeF(8f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(904, 561);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.DoubleBuffered = true;
      this.Font = new Font("Segoe UI", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Location = new Point(200, 150);
      this.Margin = new Padding(4, 5, 4, 5);
      this.MinimumSize = new Size(920, 600);
      this.Name = nameof (Main);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "D2Bot #";
      this.FormClosing += new FormClosingEventHandler(this.Main_Close);
      this.Load += new EventHandler(this.Main_Load);
      this.contextMenuStrip1.ResumeLayout(false);
      this.contextMenuStrip3.ResumeLayout(false);
      this.contextMenuStrip4.ResumeLayout(false);
      this.contextMenuStrip5.ResumeLayout(false);
      this.contextMenuStrip2.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.mainContainer.Panel1.ResumeLayout(false);
      this.mainContainer.Panel2.ResumeLayout(false);
      this.mainContainer.EndInit();
      this.mainContainer.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      ((ISupportInitialize) this.objectProfileList).EndInit();
      this.PrintTab.ResumeLayout(false);
      this.Console.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.charView.ResumeLayout(false);
      this.charContainer.Panel1.ResumeLayout(false);
      this.charContainer.Panel2.ResumeLayout(false);
      this.charContainer.EndInit();
      this.charContainer.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.charSearchBar.ResumeLayout(false);
      this.charSearchBar.PerformLayout();
      this.KeyAnalysis.ResumeLayout(false);
      this.keyWizardContainer.Panel1.ResumeLayout(false);
      this.keyWizardContainer.Panel2.ResumeLayout(false);
      this.keyWizardContainer.EndInit();
      this.keyWizardContainer.ResumeLayout(false);
      this.keyWizBorder.ResumeLayout(false);
      ((ISupportInitialize) this.dupeList).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void ConsolePrintCallback(PrintMessage pm, ProfileBase profile);

    public delegate void ItemLogPrintCallback(D2Item d2item, D2Profile profile = null);

    public delegate void UpdateDrawCallback(bool show);

    public enum ProfileAction
    {
      None,
      Start,
      Stop,
      Show,
      Hide,
    }

    public delegate void ShowCharLogCallback(ListViewItem[] list);

    public delegate void DupeListCallback(DataGridViewRow[] list);

    public delegate void ToggleCallback(bool enable);

    public delegate void AddNodeCallback(TreeNode root, TreeNode leaf);

    public delegate void AddTreeCallback(TreeView root, TreeNode leaf);

    public delegate void RemoveNodeCallback(TreeNode root);

    public delegate void AddKeyDataCallback(ListViewItem[] items);
  }
}
