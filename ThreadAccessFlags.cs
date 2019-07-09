// Decompiled with JetBrains decompiler
// Type: PInvoke.ThreadAccessFlags
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

namespace PInvoke
{
  public enum ThreadAccessFlags : uint
  {
    Terminate = 1,
    SuspendResume = 2,
    GetContext = 8,
    SetContext = 16, // 0x00000010
    SetInformation = 32, // 0x00000020
    QueryInformation = 64, // 0x00000040
    SetThreadToken = 128, // 0x00000080
    Impersonate = 256, // 0x00000100
    DirectImpersonate = 512, // 0x00000200
    SetLimitedInformation = 1024, // 0x00000400
    QueryLimitedInformation = 2048, // 0x00000800
  }
}
