// Decompiled with JetBrains decompiler
// Type: PInvoke.AllocationType
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;

namespace PInvoke
{
  [Flags]
  public enum AllocationType
  {
    WriteMatchFlagReset = 1,
    Commit = 4096, // 0x00001000
    Reserve = 8192, // 0x00002000
    CommitOrReserve = Reserve | Commit, // 0x00003000
    Decommit = 16384, // 0x00004000
    Release = 32768, // 0x00008000
    Free = 65536, // 0x00010000
    Public = 131072, // 0x00020000
    Mapped = 262144, // 0x00040000
    Reset = 524288, // 0x00080000
    TopDown = 1048576, // 0x00100000
    WriteWatch = 2097152, // 0x00200000
    Physical = 4194304, // 0x00400000
    SecImage = 16777216, // 0x01000000
    Image = SecImage, // 0x01000000
  }
}
