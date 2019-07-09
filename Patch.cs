// Decompiled with JetBrains decompiler
// Type: D2Bot.Patch
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using PInvoke;
using System;
using System.Diagnostics;

namespace D2Bot
{
  public class Patch
  {
    private Patch.Dll DLL;
    private int Offset;
    private byte[] OldCode;
    private byte[] NewCode;
    private bool Injected;

    public Patch(Patch.Dll dll, int offset, byte[] bytes)
    {
      this.DLL = dll;
      this.Offset = offset;
      this.OldCode = new byte[bytes.Length];
      this.NewCode = bytes;
      this.Injected = false;
    }

    public bool Install(Process p)
    {
      if (this.IsInstalled())
        return true;
      IntPtr dllOffset = Patch.GetDllOffset(p, this.DLL, this.Offset);
      Kernel32.ReadProcessMemory(p, dllOffset, ref this.OldCode);
      Kernel32.WriteProcessMemory(p, dllOffset, this.NewCode);
      this.Injected = true;
      return true;
    }

    public bool Remove(Process p)
    {
      if (!this.IsInstalled())
        return true;
      IntPtr dllOffset = Patch.GetDllOffset(p, this.DLL, this.Offset);
      Kernel32.WriteProcessMemory(p, dllOffset, this.OldCode);
      this.Injected = false;
      return true;
    }

    public bool IsInstalled()
    {
      return this.Injected;
    }

    public static IntPtr GetDllOffset(Process p, Patch.Dll dll, int offset)
    {
      string[] strArray = new string[15]
      {
        "D2CLIENT.dll",
        "D2COMMON.dll",
        "D2GFX.dll",
        "D2LANG.dll",
        "D2WIN.dll",
        "D2NET.dll",
        "D2GAME.dll",
        "D2LAUNCH.dll",
        "FOG.dll",
        "BNCLIENT.dll",
        "STORM.dll",
        "D2CMP.dll",
        "D2MULTI.dll",
        "D2MCPCLIENT.dll",
        "D2CMP.dll"
      };
      if (dll == Patch.Dll.GAME)
        return IntPtr.Add(p.MainModule.BaseAddress, offset);
      IntPtr moduleHandle = Kernel32.FindModuleHandle(p, strArray[(int) dll]);
      if (moduleHandle == IntPtr.Zero)
      {
        if (!Kernel32.LoadRemoteLibrary(p, (object) strArray[(int) dll]))
          return IntPtr.Zero;
        moduleHandle = Kernel32.FindModuleHandle(p, strArray[(int) dll]);
      }
      return IntPtr.Add(moduleHandle, offset);
    }

    public enum Dll
    {
      D2CLIENT,
      D2COMMON,
      D2GFX,
      D2LANG,
      D2WIN,
      D2NET,
      D2GAME,
      D2LAUNCH,
      FOG,
      BNCLIENT,
      STORM,
      D2CMP,
      D2MULTI,
      D2MCPCLIENT,
      GAME,
    }
  }
}
