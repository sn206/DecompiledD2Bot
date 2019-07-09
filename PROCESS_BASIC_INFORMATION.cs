// Decompiled with JetBrains decompiler
// Type: PInvoke.PROCESS_BASIC_INFORMATION
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

namespace PInvoke
{
  public struct PROCESS_BASIC_INFORMATION
  {
    public int ExitStatus;
    public int PebBaseAddress;
    public int AffinityMask;
    public int BasePriority;
    public uint UniqueProcessId;
    public uint InheritedFromUniqueProcessId;
  }
}
