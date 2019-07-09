// Decompiled with JetBrains decompiler
// Type: PInvoke.LoadLibraryFlags
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;

namespace PInvoke
{
  [Flags]
  public enum LoadLibraryFlags : uint
  {
    LoadAsDataFile = 2,
    DontResolveReferences = 1,
    LoadWithAlteredSeachPath = 8,
    IgnoreCodeAuthzLevel = 16, // 0x00000010
    LoadAsExclusiveDataFile = 64, // 0x00000040
  }
}
