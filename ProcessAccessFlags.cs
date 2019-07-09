// Decompiled with JetBrains decompiler
// Type: PInvoke.ProcessAccessFlags
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;

namespace PInvoke
{
  [Flags]
  public enum ProcessAccessFlags : uint
  {
    All = 2035711, // 0x001F0FFF
    Terminate = 1,
    CreateThread = 2,
    VMOperation = 8,
    VMRead = 16, // 0x00000010
    VMWrite = 32, // 0x00000020
    DupHandle = 64, // 0x00000040
    SetInformation = 512, // 0x00000200
    QueryInformation = 1024, // 0x00000400
    Synchronize = 1048576, // 0x00100000
  }
}
