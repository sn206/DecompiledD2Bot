// Decompiled with JetBrains decompiler
// Type: PInvoke.CreateThreadFlags
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;

namespace PInvoke
{
  [Flags]
  public enum CreateThreadFlags
  {
    RunImmediately = 0,
    CreateSuspended = 4,
    StackSizeParamIsAReservation = 65536, // 0x00010000
    UseNtCreateThreadEx = 8388608, // 0x00800000
  }
}
