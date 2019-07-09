// Decompiled with JetBrains decompiler
// Type: PInvoke.Kernel32
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PInvoke
{
  public static class Kernel32
  {
    [DllImport("kernel32.dll")]
    public static extern Kernel32.ErrorModes SetErrorMode(Kernel32.ErrorModes uMode);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CreateProcess(
      string Application,
      string CommandLine,
      IntPtr ProcessAttributes,
      IntPtr ThreadAttributes,
      bool InheritHandles,
      uint CreationFlags,
      IntPtr Environment,
      string CurrentDirectory,
      ref Kernel32.StartupInfo StartupInfo,
      out Kernel32.ProcessInformation ProcessInformation);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint ResumeThread(IntPtr hThread);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool VirtualProtectEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      uint dwSize,
      PageAccessProtectionFlags flags,
      out PageAccessProtectionFlags oldFlags);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(
      IntPtr hProcess,
      IntPtr baseAddress,
      [Out] byte[] buffer,
      uint size,
      out uint numBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(
      ProcessAccessFlags dwDesiredAccess,
      [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
      uint dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateRemoteThread(
      IntPtr hProcess,
      IntPtr lpThreadAttributes,
      uint dwStackSize,
      IntPtr lpStartAddress,
      IntPtr lpParameter,
      uint dwCreationFlags,
      IntPtr lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr VirtualAllocEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      uint dwSize,
      AllocationType flAllocationType,
      PageAccessProtectionFlags flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool VirtualFreeEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      int dwSize,
      AllocationType dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WriteProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      byte[] lpBuffer,
      uint nSize,
      out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibraryEx(
      string lpFileName,
      IntPtr hFile,
      LoadLibraryFlags dwFlags);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibrary(string library);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FreeLibrary(IntPtr hHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenThread(
      ThreadAccessFlags flags,
      bool bInheritHandle,
      uint threadId);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    public static bool LoadRemoteLibrary(Process p, object module)
    {
      IntPtr address = Kernel32.WriteObject(p, module);
      int num = Kernel32.CallRemoteFunction(p, "kernel32.dll", "LoadLibraryW", address) ? 1 : 0;
      Kernel32.FreeObject(p, address);
      return num != 0;
    }

    public static bool UnloadRemoteLibrary(Process p, string module)
    {
      IntPtr moduleHandle = Kernel32.FindModuleHandle(p, module);
      IntPtr address = Kernel32.WriteObject(p, (object) moduleHandle);
      int num = Kernel32.CallRemoteFunction(p, "kernel32.dll", "FreeLibrary", address) ? 1 : 0;
      Kernel32.FreeObject(p, address);
      return num != 0;
    }

    public static bool CallRemoteFunction(Process p, string module, string function, IntPtr param)
    {
      return Kernel32.CallRemoteFunction(p.Id, module, function, param);
    }

    public static bool CallRemoteFunction(int pid, string module, string function, IntPtr param)
    {
      IntPtr hModule = Kernel32.LoadLibraryEx(module, LoadLibraryFlags.LoadAsDataFile);
      IntPtr procAddress = Kernel32.GetProcAddress(hModule, function);
      if (hModule == IntPtr.Zero || procAddress == IntPtr.Zero)
        return false;
      IntPtr remoteThread = Kernel32.CreateRemoteThread(pid, procAddress, param, CreateThreadFlags.RunImmediately);
      if (remoteThread != IntPtr.Zero)
      {
        int num = (int) Kernel32.WaitForSingleObject(remoteThread, uint.MaxValue);
      }
      return remoteThread != IntPtr.Zero;
    }

    public static bool ReadProcessMemory(Process p, IntPtr address, ref byte[] buffer)
    {
      IntPtr processHandle = Kernel32.GetProcessHandle(p, ProcessAccessFlags.VMRead);
      PageAccessProtectionFlags oldFlags1;
      if (!Kernel32.VirtualProtect(new IntPtr(p.Id), address, (uint) buffer.Length, PageAccessProtectionFlags.ExecuteReadWrite, out oldFlags1))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      uint numBytesRead;
      if (!Kernel32.ReadProcessMemory(processHandle, address, buffer, (uint) buffer.Length, out numBytesRead))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      PageAccessProtectionFlags oldFlags2;
      if (!Kernel32.VirtualProtect(new IntPtr(p.Id), address, (uint) buffer.Length, oldFlags1, out oldFlags2))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      Kernel32.CloseProcessHandle(processHandle);
      return (long) numBytesRead == (long) buffer.Length;
    }

    public static IntPtr WriteObject(Process p, object data)
    {
      byte[] rawBytes = Kernel32.GetRawBytes(data);
      IntPtr address = Kernel32.VirtualAlloc(p, IntPtr.Zero, (uint) rawBytes.Length, AllocationType.CommitOrReserve, PageAccessProtectionFlags.ReadWrite);
      Kernel32.WriteProcessMemory(p, address, rawBytes);
      return address;
    }

    public static void FreeObject(Process p, IntPtr address)
    {
      Kernel32.VirtualFree(p, address);
    }

    public static IntPtr FindModuleHandle(Process p, string module)
    {
      foreach (ProcessModule module1 in (ReadOnlyCollectionBase) p.Modules)
      {
        if (Path.GetFileName(module1.FileName).ToLowerInvariant() == Path.GetFileName(module).ToLowerInvariant())
          return module1.BaseAddress;
      }
      return IntPtr.Zero;
    }

    public static IntPtr FindOffset(string moduleName, string function)
    {
      IntPtr num = Kernel32.LoadLibraryEx(moduleName, LoadLibraryFlags.LoadAsDataFile);
      if (!(num != IntPtr.Zero))
        return IntPtr.Zero;
      IntPtr procAddress = Kernel32.GetProcAddress(num, function);
      Kernel32.FreeLibrary(num);
      return (IntPtr) (procAddress.ToInt32() - num.ToInt32());
    }

    [DebuggerHidden]
    public static bool VirtualProtect(
      IntPtr pid,
      IntPtr address,
      uint size,
      PageAccessProtectionFlags flags,
      out PageAccessProtectionFlags oldFlags)
    {
      IntPtr processHandle = Kernel32.GetProcessHandle(pid, ProcessAccessFlags.VMOperation);
      int num = Kernel32.VirtualProtectEx(processHandle, address, size, flags, out oldFlags) ? 1 : 0;
      Kernel32.CloseHandle(processHandle);
      return num != 0;
    }

    [DebuggerHidden]
    public static IntPtr GetProcessHandle(IntPtr pid, ProcessAccessFlags flags)
    {
      IntPtr num = Kernel32.OpenProcess(flags, false, (uint) (int) pid);
      if (!(num == IntPtr.Zero))
        return num;
      throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DebuggerHidden]
    public static IntPtr LoadLibraryEx(string module, LoadLibraryFlags flags)
    {
      return Kernel32.LoadLibraryEx(module, IntPtr.Zero, flags);
    }

    [DebuggerHidden]
    public static IntPtr GetProcessHandle(Process p, ProcessAccessFlags flags)
    {
      IntPtr num = Kernel32.OpenProcess(flags, false, (uint) p.Id);
      if (!(num == IntPtr.Zero))
        return num;
      throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DebuggerHidden]
    public static void CloseProcessHandle(IntPtr handle)
    {
      if (!Kernel32.CloseHandle(handle))
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DebuggerHidden]
    public static IntPtr VirtualAlloc(
      Process p,
      IntPtr address,
      uint size,
      AllocationType type,
      PageAccessProtectionFlags flags)
    {
      IntPtr processHandle = Kernel32.GetProcessHandle(p, ProcessAccessFlags.VMOperation);
      IntPtr num = Kernel32.VirtualAllocEx(processHandle, address, size, type, flags);
      if (num == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error());
      Kernel32.CloseProcessHandle(processHandle);
      return num;
    }

    [DebuggerHidden]
    public static void VirtualFree(Process p, IntPtr address)
    {
      IntPtr processHandle = Kernel32.GetProcessHandle(p, ProcessAccessFlags.VMOperation);
      int num = Kernel32.VirtualFreeEx(processHandle, address, 0, AllocationType.Release) ? 1 : 0;
      Kernel32.CloseProcessHandle(processHandle);
      if (num == 0)
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DebuggerHidden]
    public static int WriteProcessMemory(Process p, IntPtr address, byte[] buffer)
    {
      IntPtr processHandle = Kernel32.GetProcessHandle(p, ProcessAccessFlags.VMOperation | ProcessAccessFlags.VMWrite);
      int lpNumberOfBytesWritten;
      if (!Kernel32.WriteProcessMemory(processHandle, address, buffer, (uint) buffer.Length, out lpNumberOfBytesWritten))
        throw new Win32Exception(Marshal.GetLastWin32Error());
      Kernel32.CloseProcessHandle(processHandle);
      return lpNumberOfBytesWritten;
    }

    [DebuggerHidden]
    public static IntPtr CreateRemoteThread(
      Process p,
      IntPtr address,
      IntPtr param,
      CreateThreadFlags flags)
    {
      return Kernel32.CreateRemoteThread(p.Id, address, param, flags);
    }

    [DebuggerHidden]
    public static IntPtr CreateRemoteThread(
      int pid,
      IntPtr address,
      IntPtr param,
      CreateThreadFlags flags)
    {
      IntPtr processHandle = Kernel32.GetProcessHandle(new IntPtr(pid), ProcessAccessFlags.CreateThread | ProcessAccessFlags.VMOperation | ProcessAccessFlags.VMRead | ProcessAccessFlags.VMWrite | ProcessAccessFlags.QueryInformation);
      IntPtr remoteThread = Kernel32.CreateRemoteThread(processHandle, IntPtr.Zero, 0U, address, param, (uint) flags, IntPtr.Zero);
      if (remoteThread == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error());
      Kernel32.CloseProcessHandle(processHandle);
      return remoteThread;
    }

    [DebuggerHidden]
    public static void SuspendThread(int tid)
    {
      IntPtr num1 = Kernel32.OpenThread(ThreadAccessFlags.SuspendResume, false, (uint) tid);
      int num2 = (int) Kernel32.SuspendThread(num1);
      Kernel32.CloseHandle(num1);
    }

    [DebuggerHidden]
    public static void ResumeThread(int tid)
    {
      IntPtr num1 = Kernel32.OpenThread(ThreadAccessFlags.SuspendResume, false, (uint) tid);
      int num2 = (int) Kernel32.ResumeThread(num1);
      Kernel32.CloseHandle(num1);
    }

    [DebuggerHidden]
    public static byte[] GetRawBytes(object anything)
    {
      if (anything.GetType().Equals(typeof (string)))
        return Encoding.Unicode.GetBytes((string) anything);
      int length = Marshal.SizeOf(anything);
      IntPtr num = Marshal.AllocHGlobal(length);
      Marshal.StructureToPtr(anything, num, false);
      byte[] destination = new byte[length];
      Marshal.Copy(num, destination, 0, length);
      Marshal.FreeHGlobal(num);
      return destination;
    }

    public static uint StartProcess(
      string directory,
      string application,
      ProcessCreationFlags flags,
      params string[] parameters)
    {
      Kernel32.StartupInfo StartupInfo = new Kernel32.StartupInfo();
      Kernel32.ProcessInformation ProcessInformation = new Kernel32.ProcessInformation();
      if (Kernel32.CreateProcess(application, application + string.Concat(parameters), IntPtr.Zero, IntPtr.Zero, false, (uint) flags, IntPtr.Zero, directory, ref StartupInfo, out ProcessInformation))
        return ProcessInformation.ProcessId;
      return uint.MaxValue;
    }

    public static Process StartSuspended(Process process, ProcessStartInfo startInfo)
    {
      return Process.GetProcessById((int) Kernel32.StartProcess(startInfo.WorkingDirectory, startInfo.FileName, ProcessCreationFlags.Suspended, startInfo.Arguments));
    }

    public static void Suspend(Process process)
    {
      foreach (ProcessThread thread in (ReadOnlyCollectionBase) process.Threads)
      {
        int num = (int) Kernel32.SuspendThread(new IntPtr(thread.Id));
      }
    }

    public static void Resume(Process process)
    {
      foreach (ProcessThread thread in (ReadOnlyCollectionBase) process.Threads)
        Kernel32.ResumeThread(thread.Id);
    }

    public static void Suspend(ProcessThread thread)
    {
      int num = (int) Kernel32.SuspendThread(new IntPtr(thread.Id));
    }

    public static void Resume(ProcessThread thread)
    {
      int num = (int) Kernel32.ResumeThread(new IntPtr(thread.Id));
    }

    private struct ProcessInformation
    {
      public IntPtr Process;
      public IntPtr Thread;
      public uint ProcessId;
      public uint ThreadId;
    }

    private struct StartupInfo
    {
      public uint CB;
      public string Reserved;
      public string Desktop;
      public string Title;
      public uint X;
      public uint Y;
      public uint XSize;
      public uint YSize;
      public uint XCountChars;
      public uint YCountChars;
      public uint FillAttribute;
      public uint Flags;
      public short ShowWindow;
      public short Reserved2;
      public IntPtr lpReserved2;
      public IntPtr StdInput;
      public IntPtr StdOutput;
      public IntPtr StdError;
    }

    [Flags]
    public enum ErrorModes : uint
    {
      SYSTEM_DEFAULT = 0,
      SEM_FAILCRITICALERRORS = 1,
      SEM_NOALIGNMENTFAULTEXCEPT = 4,
      SEM_NOGPFAULTERRORBOX = 2,
      SEM_NOOPENFILEERRORBOX = 32768, // 0x00008000
    }
  }
}
