// D2BSLoader.Main
using D2BSLoader;
using PInvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

public class Main : Form
{
	private delegate void StatusCallback(string status, Color color);

	private delegate int LoadAction(int pid);

	private IContainer components;

	private ListBox Processes;

	private Label Status;

	private CheckBox Autoload;

	private static string D2Path = string.Empty;

	private static string D2Exe = string.Empty;

	private static string D2Args = string.Empty;

	private static string D2BSDLL = string.Empty;

	private static int LoadDelay = 1000;

	private static bool useNtCreateThreadEx = false;

	private static Dictionary<string, LoadAction> actions = new Dictionary<string, LoadAction>
	{
		{
			"inject",
			Inject
		},
		{
			"kill",
			Kill
		},
		{
			"start",
			Start
		},
		{
			"save",
			Save
		}
	};

	private BindingList<ProcessWrapper> processes = new BindingList<ProcessWrapper>();

	public bool Autoclosed
	{
		get;
		set;
	}

	private static bool IsInDebug
	{
		get;
		set;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(D2BSLoader.Main));
		Processes = new System.Windows.Forms.ListBox();
		Status = new System.Windows.Forms.Label();
		Autoload = new System.Windows.Forms.CheckBox();
		System.Windows.Forms.Label label = new System.Windows.Forms.Label();
		System.Windows.Forms.Button button = new System.Windows.Forms.Button();
		System.Windows.Forms.Button button2 = new System.Windows.Forms.Button();
		System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
		System.Windows.Forms.Button button3 = new System.Windows.Forms.Button();
		SuspendLayout();
		label.AutoSize = true;
		label.Location = new System.Drawing.Point(12, 9);
		label.Name = "label1";
		label.Size = new System.Drawing.Size(141, 13);
		label.TabIndex = 5;
		label.Text = "Available Diablo II Instances";
		button.Location = new System.Drawing.Point(12, 152);
		button.Name = "Load";
		button.Size = new System.Drawing.Size(75, 23);
		button.TabIndex = 2;
		button.Text = "Load";
		button.UseVisualStyleBackColor = true;
		button.Click += new System.EventHandler(Load_Click);
		button2.Location = new System.Drawing.Point(109, 152);
		button2.Name = "StartNew";
		button2.Size = new System.Drawing.Size(75, 23);
		button2.TabIndex = 3;
		button2.Text = "Start New";
		button2.UseVisualStyleBackColor = true;
		button2.Click += new System.EventHandler(StartNew_Click);
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(12, 123);
		label2.Name = "label2";
		label2.Size = new System.Drawing.Size(87, 13);
		label2.TabIndex = 6;
		label2.Text = "Last Load Status";
		button3.Location = new System.Drawing.Point(154, 1);
		button3.Name = "Options";
		button3.Size = new System.Drawing.Size(56, 21);
		button3.TabIndex = 4;
		button3.Text = "Options";
		button3.UseVisualStyleBackColor = true;
		button3.Click += new System.EventHandler(Options_Click);
		Processes.FormattingEnabled = true;
		Processes.Location = new System.Drawing.Point(12, 25);
		Processes.Name = "Processes";
		Processes.Size = new System.Drawing.Size(198, 95);
		Processes.TabIndex = 0;
		Status.AutoSize = true;
		Status.ForeColor = System.Drawing.SystemColors.InactiveCaption;
		Status.Location = new System.Drawing.Point(28, 136);
		Status.Name = "Status";
		Status.Size = new System.Drawing.Size(33, 13);
		Status.TabIndex = 7;
		Status.Text = "None";
		Autoload.AutoSize = true;
		Autoload.Location = new System.Drawing.Point(142, 123);
		Autoload.Name = "Autoload";
		Autoload.Size = new System.Drawing.Size(68, 17);
		Autoload.TabIndex = 1;
		Autoload.Text = "Autoload";
		Autoload.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(222, 187);
		base.Controls.Add(button3);
		base.Controls.Add(Autoload);
		base.Controls.Add(Status);
		base.Controls.Add(label2);
		base.Controls.Add(button2);
		base.Controls.Add(button);
		base.Controls.Add(label);
		base.Controls.Add(Processes);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.Name = "Main";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		Text = "D2BS Loader";
		base.Load += new System.EventHandler(Main_Load);
		ResumeLayout(performLayout: false);
		PerformLayout();
	}

	public Main(string[] args)
	{
		if (args.Length > 0)
		{
			ProcessCmdArgs(args);
			return;
		}
		Splash splash = new Splash(this);
		splash.FormClosed += delegate
		{
			LoadSettings(showError: true);
			if (string.IsNullOrEmpty(D2Path) || string.IsNullOrEmpty(D2Exe) || !File.Exists(Path.Combine(D2Path, D2Exe)) || string.IsNullOrEmpty(D2BSDLL) || !File.Exists(Path.Combine(Application.StartupPath, D2BSDLL)))
			{
				Options_Click(null, null);
			}
		};
		splash.Show();
		InitializeComponent();
		processes.RaiseListChangedEvents = true;
		Processes.DataSource = processes;
		Processes.DisplayMember = "ProcessName";
		Thread t = new Thread(ListUpdateThread);
		base.Shown += delegate
		{
			t.Start();
			Process.EnterDebugMode();
			IsInDebug = true;
		};
		base.FormClosing += delegate
		{
			t.Abort();
			Process.LeaveDebugMode();
			IsInDebug = false;
		};
	}

	private void ProcessCmdArgs(string[] args)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = "cGuard.dll";
		int pid = -1;
		for (int i = 0; i < args.Length; i++)
		{
			switch (args[i])
			{
			case "--pid":
				pid = Convert.ToInt32(args[i + 1]);
				i++;
				break;
			case "--dll":
				text5 = args[i + 1];
				i++;
				break;
			case "--path":
				text2 = args[i + 1];
				i++;
				break;
			case "--exe":
				text3 = args[i + 1];
				i++;
				break;
			case "--params":
			{
				string[] array = new string[args.Length - i - 1];
				Array.Copy(args, i + 1, array, 0, args.Length - i - 1);
				text4 = " " + string.Join(" ", array);
				i = args.Length;
				break;
			}
			default:
				text = args[i].Substring(2);
				break;
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			D2Path = text2;
		}
		if (!string.IsNullOrEmpty(text3))
		{
			D2Exe = text3;
		}
		if (!string.IsNullOrEmpty(text5))
		{
			D2BSDLL = text5;
		}
		D2Args = string.Join(" ", new string[2]
		{
			D2Args,
			text4
		});
		if (text == "start" && (string.IsNullOrEmpty(D2Path) || string.IsNullOrEmpty(D2Exe)))
		{
			if (string.IsNullOrEmpty(D2Path) || string.IsNullOrEmpty(D2Exe))
			{
				LoadSettings(showError: false);
			}
			if (string.IsNullOrEmpty(D2Path))
			{
				Autoclosed = true;
				Console.WriteLine("-1");
				Close();
				return;
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			Autoclosed = true;
			int value = actions[text](pid);
			if (text == "start")
			{
				Console.WriteLine(value);
			}
			Close();
		}
	}

	private void ListUpdateThread()
	{
		while (true)
		{
			foreach (ProcessWrapper process in processes)
			{
				process.Process.Refresh();
			}
			Process[] array = Process.GetProcesses();
			Process p;
			for (int i = 0; i < array.Length; i++)
			{
				p = array[i];
				if (!BindingListExtensions.Exists(processes, (ProcessWrapper x) => p.Id == x.Process.Id) && IsD2Window(p))
				{
					ProcessWrapper pw = new ProcessWrapper(p);
					processes.Add(pw);
					p.Exited += delegate
					{
						processes.Remove(pw);
					};
					if (GetAutoload())
					{
						p.WaitForInputIdle();
						Attach(pw);
					}
				}
			}
			Processes.Invoke((MethodInvoker)delegate
			{
				((CurrencyManager)Processes.BindingContext[Processes.DataSource]).Refresh();
			});
			Thread.Sleep(400);
		}
	}

	private static bool IsD2Window(Process p)
	{
		if (p == null)
		{
			return false;
		}
		string lCClassName = GetLCClassName(p);
		string text = "";
		try
		{
			text = Path.GetFileName(p.MainModule.FileName).ToLowerInvariant();
		}
		catch
		{
		}
		if (!string.IsNullOrEmpty(lCClassName) && lCClassName == "diablo ii")
		{
			if (!(text == "game.exe") && !text.Contains("d2loader"))
			{
				return text.Contains("d2launcher");
			}
			return true;
		}
		return false;
	}

	public static void SaveSettings(string path, string exe, string args, string dll)
	{
		string exePath = Path.Combine(Application.StartupPath, "D2BS.exe");
		Configuration configuration = ConfigurationManager.OpenExeConfiguration(exePath);
		configuration.AppSettings.Settings.Remove("D2Path");
		configuration.AppSettings.Settings.Remove("D2Exe");
		configuration.AppSettings.Settings.Remove("D2Args");
		configuration.AppSettings.Settings.Remove("D2BSDLL");
		configuration.AppSettings.Settings.Add("D2Path", path);
		configuration.AppSettings.Settings.Add("D2Exe", exe);
		configuration.AppSettings.Settings.Add("D2Args", args);
		configuration.AppSettings.Settings.Add("D2BSDLL", dll);
		configuration.Save(ConfigurationSaveMode.Full);
	}

	public static void LoadSettings(bool showError)
	{
		try
		{
			string exePath = Path.Combine(Application.StartupPath, "D2BS.exe");
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(exePath);
			D2Path = configuration.AppSettings.Settings["D2Path"].Value;
			D2Exe = configuration.AppSettings.Settings["D2Exe"].Value;
			D2Args = configuration.AppSettings.Settings["D2Args"].Value;
			D2BSDLL = configuration.AppSettings.Settings["D2BSDLL"].Value;
			try
			{
				useNtCreateThreadEx = Convert.ToBoolean(configuration.AppSettings.Settings["UseVistaCreateMethod"].Value);
			}
			catch
			{
				useNtCreateThreadEx = false;
			}
			try
			{
				LoadDelay = Convert.ToInt32(configuration.AppSettings.Settings["LoadDelay"].Value);
			}
			catch
			{
				LoadDelay = 1000;
			}
		}
		catch
		{
			if (showError)
			{
				MessageBox.Show("Configuration not found.", "D2BS");
			}
		}
	}

	public static void SetSettings(string path, string exe, string args, string dll)
	{
		if (!string.IsNullOrEmpty(path))
		{
			D2Path = path;
		}
		if (!string.IsNullOrEmpty(exe))
		{
			D2Exe = exe;
		}
		if (!string.IsNullOrEmpty(args))
		{
			D2Args = args;
		}
		if (!string.IsNullOrEmpty(dll))
		{
			D2BSDLL = dll;
		}
	}

	public static void GetSettings(out string path, out string exe, out string args, out string dll)
	{
		try
		{
			string exePath = Path.Combine(Application.StartupPath, "D2BS.exe");
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(exePath);
			path = configuration.AppSettings.Settings["D2Path"].Value;
			exe = configuration.AppSettings.Settings["D2Exe"].Value;
			args = configuration.AppSettings.Settings["D2Args"].Value;
			dll = configuration.AppSettings.Settings["D2BSDLL"].Value;
		}
		catch
		{
			path = (exe = (args = (dll = null)));
		}
	}

	private bool GetAutoload()
	{
		bool autoload = false;
		if (Autoload.InvokeRequired)
		{
			Autoload.Invoke((MethodInvoker)delegate
			{
				autoload = Autoload.Checked;
			});
		}
		else
		{
			autoload = Autoload.Checked;
		}
		return autoload;
	}

	private void SetStatus(string status, Color color)
	{
		if (Status != null)
		{
			if (Status.InvokeRequired)
			{
				StatusCallback method = SetStatus;
				Status.Invoke(method, status, color);
			}
			else
			{
				Status.ForeColor = color;
				Status.Text = status;
			}
		}
	}

	private static string GetLCClassName(Process p)
	{
		return User32.GetClassNameFromProcess(p).ToLowerInvariant();
	}

	private static Process GetProcessById(int pid)
	{
		try
		{
			return Process.GetProcessById(pid);
		}
		catch (ArgumentException)
		{
			return null;
		}
	}

	private static int Start(int pid)
	{
		int num = Start();
		Inject(num);
		return num;
	}

	public static int Inject(int pid)
	{
		Process processById = GetProcessById(pid);
		if (IsD2Window(processById))
		{
			Attach(processById);
			return pid;
		}
		return -1;
	}

	public static int Kill(int pid)
	{
		Process processById = GetProcessById(pid);
		if (IsD2Window(processById))
		{
			processById.Kill();
			return pid;
		}
		return -1;
	}

	private static int Save(int pid)
	{
		SaveSettings(D2Path, D2Exe, D2Args, D2BSDLL);
		return -1;
	}

	private static bool Attach(Process p)
	{
		if (!IsInDebug)
		{
			Process.EnterDebugMode();
		}
		string text = Path.Combine(Application.StartupPath, "nspr4.dll");
		string text2 = Path.Combine(Application.StartupPath, "mozjs.dll");
		string text3 = Path.Combine(Application.StartupPath, D2BSDLL);
		if (File.Exists(text) && File.Exists(text3) && File.Exists(text2) && Kernel32.LoadRemoteLibrary(p, text) && Kernel32.LoadRemoteLibrary(p, text2))
		{
			return Kernel32.LoadRemoteLibrary(p, text3);
		}
		return false;
	}

	private void Attach(ProcessWrapper pw)
	{
		if (pw.Loaded)
		{
			SetStatus("Already loaded!", Color.Blue);
		}
		else if (Attach(pw.Process))
		{
			SetStatus("Success!", Color.Green);
			pw.Loaded = true;
		}
		else
		{
			SetStatus("Failed!", Color.Red);
		}
	}

	public static int Start(string path, string exe, string param, string dll)
	{
		D2Path = path;
		D2Exe = exe;
		D2Args = param;
		D2BSDLL = dll;
		return Start();
	}

	public static int Start(params string[] args)
	{
		if (string.IsNullOrEmpty(D2Path) || string.IsNullOrEmpty(D2Exe) || string.IsNullOrEmpty(D2BSDLL))
		{
			LoadSettings(showError: false);
		}
		D2Args += string.Join(" ", args);
		return Start();
	}

	private static int Start()
	{
		if (!File.Exists(Path.Combine(D2Path, D2Exe)) || !File.Exists(Path.Combine(Application.StartupPath, D2BSDLL)))
		{
			return -1;
		}
		ProcessStartInfo processStartInfo = new ProcessStartInfo(Path.Combine(D2Path, D2Exe), " " + D2Args);
		processStartInfo.UseShellExecute = false;
		processStartInfo.WorkingDirectory = D2Path;
		Process.EnterDebugMode();
		Process process = new Process();
		process.StartInfo = processStartInfo;
		process = Kernel32.StartSuspended(process, processStartInfo);
		byte[] buffer = new byte[2];
		Kernel32.LoadRemoteLibrary(process, Path.Combine(D2Path, "D2Gfx.dll"));
		Kernel32.WriteProcessMemory(process, (IntPtr)1873327792, new byte[2]
		{
			235,
			69
		});
		IntPtr address = (IntPtr)1873327792;
		Kernel32.Resume(process);
		process.WaitForInputIdle();
		Kernel32.Suspend(process);
		Kernel32.WriteProcessMemory(process, address, buffer);
		Kernel32.Resume(process);
		Thread.Sleep(LoadDelay);
		Process[] childProcesses = ProcessExtensions.GetChildProcesses(process);
		if (childProcesses.Length > 0)
		{
			Process[] array = childProcesses;
			foreach (Process process2 in array)
			{
				if (IsD2Window(process2))
				{
					process2.WaitForInputIdle();
					return process2.Id;
				}
			}
		}
		return process.Id;
	}

	private void WriteMultiWindow(Process p)
	{
	}

	private void Load_Click(object sender, EventArgs e)
	{
		if (Processes.SelectedIndex == -1)
		{
			SetStatus("No process selected!", Color.Blue);
		}
		else
		{
			Attach(Processes.SelectedItem as ProcessWrapper);
		}
	}

	private void StartNew_Click(object sender, EventArgs e)
	{
		Start();
	}

	private void Options_Click(object sender, EventArgs e)
	{
		Options options = new Options(D2Path, D2Exe, D2Args, D2BSDLL);
		options.ShowDialog();
		LoadSettings(showError: false);
	}

	private void Main_Load(object sender, EventArgs e)
	{
	}
}
