// Decompiled with JetBrains decompiler
// Type: PInvoke.PageAccessProtectionFlags
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;

namespace PInvoke
{
  [Flags]
  public enum PageAccessProtectionFlags
  {
    NoAccess = 1,
    ReadOnly = 2,
    ReadWrite = 4,
    WriteCopy = 8,
    Execute = 16, // 0x00000010
    ExecuteRead = 32, // 0x00000020
    ExecuteReadWrite = 64, // 0x00000040
    ExecuteWriteCopy = 128, // 0x00000080
    Guard = 256, // 0x00000100
    NoCache = 512, // 0x00000200
    WriteCombine = 1024, // 0x00000400
  }
}
