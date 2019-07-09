// Decompiled with JetBrains decompiler
// Type: PInvoke.ProcessCreationFlags
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;

namespace PInvoke
{
  [Flags]
  public enum ProcessCreationFlags : uint
  {
    BreakawayFromJob = 16777216, // 0x01000000
    DefaultErrorMode = 67108864, // 0x04000000
    NewConsole = 16, // 0x00000010
    NewProcessGroup = 512, // 0x00000200
    NoWindow = 134217728, // 0x08000000
    ProtectedProcess = 262144, // 0x00040000
    PreserveCodeAuthzLevel = 33554432, // 0x02000000
    SeparateWowVdm = 2048, // 0x00000800
    SharedWowVdm = 4096, // 0x00001000
    Suspended = 4,
    UnicodeEnvironment = 1024, // 0x00000400
    DebugOnlyThisProcess = 2,
    DebugProcess = 1,
    DetachedProcess = 8,
    ExtendedStartupInfo = 524288, // 0x00080000
    InheritParentAffinity = 65536, // 0x00010000
  }
}
