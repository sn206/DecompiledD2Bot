// Decompiled with JetBrains decompiler
// Type: D2Bot.PatchData
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

namespace D2Bot
{
  public class PatchData
  {
    public string Name { get; set; }

    public string Version { get; set; }

    public Patch.Dll Module { get; set; }

    public int Offset { get; set; }

    public byte[] Data { get; set; }
  }
}
