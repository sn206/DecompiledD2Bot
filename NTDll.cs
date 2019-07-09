// Decompiled with JetBrains decompiler
// Type: PInvoke.NTDll
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PInvoke
{
  public static class NTDll
  {
    public static IntPtr CreateRemoteThread(IntPtr address, IntPtr param, IntPtr handle)
    {
      NTDll.NtCreateThreadExBuffer outlpvBytesBuffer = new NTDll.NtCreateThreadExBuffer();
      outlpvBytesBuffer.Size = Marshal.SizeOf((object) outlpvBytesBuffer);
      outlpvBytesBuffer.Unknown1 = 65539UL;
      outlpvBytesBuffer.Unknown2 = 8UL;
      outlpvBytesBuffer.Unknown3 = Marshal.AllocHGlobal(4);
      outlpvBytesBuffer.Unknown4 = 0UL;
      outlpvBytesBuffer.Unknown5 = 65540UL;
      outlpvBytesBuffer.Unknown6 = 4UL;
      outlpvBytesBuffer.Unknown7 = Marshal.AllocHGlobal(4);
      outlpvBytesBuffer.Unknown8 = 0UL;
      IntPtr outhThread = IntPtr.Zero;
      NTDll.NtCreateThreadEx(out outhThread, 2097151, IntPtr.Zero, handle, address, param, false, 0UL, 0UL, 0UL, out outlpvBytesBuffer);
      if (outhThread == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error());
      return outhThread;
    }

    [DllImport("ntdll.dll")]
    private static extern int NtQueryInformationProcess(
      IntPtr hProcess,
      int processInformationClass,
      ref PROCESS_BASIC_INFORMATION processBasicInformation,
      uint processInformationLength,
      out uint returnLength);

    public static bool ProcessIsChildOf(Process parent, Process child)
    {
      PROCESS_BASIC_INFORMATION processBasicInformation = new PROCESS_BASIC_INFORMATION();
      try
      {
        uint returnLength;
        NTDll.NtQueryInformationProcess(child.Handle, 0, ref processBasicInformation, (uint) Marshal.SizeOf((object) processBasicInformation), out returnLength);
        if ((long) processBasicInformation.InheritedFromUniqueProcessId == (long) parent.Id)
          return true;
      }
      catch
      {
        return false;
      }
      return false;
    }

    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern IntPtr NtCreateThreadEx(
      out IntPtr outhThread,
      int inlpvDesiredAccess,
      IntPtr lpObjectAttributes,
      IntPtr inhProcessHandle,
      IntPtr lpStartAddress,
      IntPtr lpParameter,
      bool inCreateSuspended,
      ulong inStackZeroBits,
      ulong inSizeOfStackCommit,
      ulong inSizeOfStackReserve,
      [MarshalAs(UnmanagedType.Struct)] out NTDll.NtCreateThreadExBuffer outlpvBytesBuffer);

    private struct NtCreateThreadExBuffer
    {
      public int Size;
      public ulong Unknown1;
      public ulong Unknown2;
      public IntPtr Unknown3;
      public ulong Unknown4;
      public ulong Unknown5;
      public ulong Unknown6;
      public IntPtr Unknown7;
      public ulong Unknown8;
    }
  }
}
