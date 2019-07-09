// Decompiled with JetBrains decompiler
// Type: PInvoke.User32
// Assembly: D2Bot, Version=19.3.3.100, Culture=neutral, PublicKeyToken=null
// MVID: 9640A237-EFDF-452F-9BB7-661440486ADD
// Assembly location: C:\Users\sjbeck86\Downloads\d2bot-with-kolbot-master\d2bot-with-kolbot-master\D2Bot.exe

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace PInvoke
{
  public static class User32
  {
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll")]
    public static extern IntPtr SetFocus(IntPtr hWnd);

    public static string GetClassNameFromProcess(Process p)
    {
      StringBuilder lpClassName = new StringBuilder(100);
      User32.GetClassName(p.MainWindowHandle, lpClassName, lpClassName.Capacity);
      return lpClassName.ToString();
    }
  }
}
