using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using static Xilium.CefGlue.ConsoleColors;

namespace Xilium.CefGlue;

class DevTools
{
    public static void WaitForAttach()
    {
        if (Debugger.IsAttached)
        {
            return;
        }

        Console.WriteLine("[Debug] Attach debugger and use 'Set next statement'");

        while (true)
        {
            Thread.Sleep(100);

            if (Debugger.IsAttached)
            {
                break;
            }
        }
    }

    public static void SoftBreakpoint(string method, object? obj)
    {
        var methodMessage = $" [Breakpoint] at {method} ".PadRight(Console.BufferWidth, ' ');
        Console.WriteLine($"{Background.Red}{Foreground.Black}{methodMessage}{Reset}");

        while (true)
        {
            Console.Write($"{Background.White}{Foreground.Black} Continue {Reset} F5 {Background.White}{Foreground.Black} Stacktrace {Reset} S");
            Console.Write($"\t{Background.White}{Foreground.Black} Local value {Reset} L");
            Console.WriteLine();
            Console.Write($"{Background.White}{Foreground.Black} Input {Reset}: ");
            var key = Console.ReadKey();
            Console.WriteLine(key.Key);

            if (key.Key == ConsoleKey.F5)
            {
                // 继续
                break;
            }
            else if (key.Key == ConsoleKey.S)
            {
                // 堆栈。
                Console.WriteLine(new StackTrace(1, true));
            }
            else if (key.Key == ConsoleKey.L)
            {
                // 局部变量。
                Console.WriteLine(ViewObject(obj));
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }
    }

    private static string ViewObject(object obj)
    {
        if (obj is null)
        {
            return $"{Background.Red}{Foreground.White} null {Reset}";
        }

        var builder = new StringBuilder();
        var type = obj.GetType();
        builder.AppendLine().AppendLine($"{Foreground.BrightBlue}{type.FullName}{Foreground.White}:{Reset}");

        builder.AppendLine($"{Background.BrightBlack}{Foreground.Black}public properties >>{Reset}");
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(x => x.Name))
        {
            var value = FormatObjectValue(() => property.GetValue(obj), () => property.PropertyType);
            builder.AppendLine($"{Foreground.White} - {property.Name}{Reset} = {value}");
        }

        builder.AppendLine($"{Background.BrightBlack}{Foreground.Black}non public properties >>{Reset}");
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).OrderBy(x => x.Name))
        {
            var value = FormatObjectValue(() => property.GetValue(obj), () => property.PropertyType);
            builder.AppendLine($"{Foreground.White} - {property.Name}{Reset} = {value}");
        }

        builder.AppendLine($"{Background.BrightBlack}{Foreground.Black}public static properties >>{Reset}");
        foreach (var property in type.GetProperties(BindingFlags.Static | BindingFlags.Public).OrderBy(x => x.Name))
        {
            var value = FormatObjectValue(() => property.GetValue(obj), () => property.PropertyType);
            builder.AppendLine($"{Foreground.White} - {property.Name}{Reset} = {value}");
        }

        builder.AppendLine($"{Background.BrightBlack}{Foreground.Black}non public static properties >>{Reset}");
        foreach (var property in type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic).OrderBy(x => x.Name))
        {
            var value = FormatObjectValue(() => property.GetValue(obj), () => property.PropertyType);
            builder.AppendLine($"{Foreground.White} - {property.Name}{Reset} = {value}");
        }

        builder.AppendLine($"{Background.BrightBlack}{Foreground.Black}fields >>{Reset}");
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).OrderBy(x => x.Name))
        {
            var value = FormatObjectValue(() => field.GetValue(obj), () => field.FieldType);
            builder.AppendLine($"{Foreground.White} - {field.Name}{Reset} = {value}");
        }

        builder.AppendLine("————————————————");
        return builder.ToString();
    }

    private static string FormatObjectValue(Func<object> valueGetter, Func<Type> valueTypeGetter)
    {
        string? valueString = null;
        try
        {
            var value = valueGetter();
            var valueType = valueTypeGetter();
            valueString = value is null
                ? $"{Background.Red}{Foreground.Black}null{Reset} {Foreground.BrightBlack}({valueType}){Reset}"
                : $"{Foreground.BrightYellow}{value}{Reset} {Foreground.BrightBlack}({valueType}){Reset}";
        }
        catch (Exception ex)
        {
            valueString = $"{Foreground.Red}{ex.Message}{Reset}";
        }
        return valueString;
    }
}

/// <summary>
/// 包含控制台输出颜色的字符串常量。
/// </summary>
internal static class ConsoleColors
{
    public const string Reset = "\u001b[0m";

    public static class Foreground
    {
        #region 4-bit colors

        public const string Black = "\u001b[30m";
        public const string Red = "\u001b[31m";
        public const string Green = "\u001b[32m";
        public const string Yellow = "\u001b[33m";
        public const string Blue = "\u001b[34m";
        public const string Magenta = "\u001b[35m";
        public const string Cyan = "\u001b[36m";
        public const string White = "\u001b[37m";
        public const string BrightBlack = "\u001b[90m";
        public const string BrightRed = "\u001b[91m";
        public const string BrightGreen = "\u001b[92m";
        public const string BrightYellow = "\u001b[93m";
        public const string BrightBlue = "\u001b[94m";
        public const string BrightMagenta = "\u001b[95m";
        public const string BrightCyan = "\u001b[96m";
        public const string BrightWhite = "\u001b[97m";

        #endregion
    }

    public static class Background
    {
        #region 4-bit colors

        public const string Black = "\u001b[40m";
        public const string Red = "\u001b[41m";
        public const string Green = "\u001b[42m";
        public const string Yellow = "\u001b[43m";
        public const string Blue = "\u001b[44m";
        public const string Magenta = "\u001b[45m";
        public const string Cyan = "\u001b[46m";
        public const string White = "\u001b[47m";
        public const string BrightBlack = "\u001b[100m";
        public const string BrightRed = "\u001b[101m";
        public const string BrightGreen = "\u001b[102m";
        public const string BrightYellow = "\u001b[103m";
        public const string BrightBlue = "\u001b[104m";
        public const string BrightMagenta = "\u001b[105m";
        public const string BrightCyan = "\u001b[106m";
        public const string BrightWhite = "\u001b[107m";

        #endregion
    }

    public static class Decoration
    {
        public const string Bold = "\u001b[1m";
        public const string Dim = "\u001b[2m";
        public const string Italic = "\u001b[3m";
        public const string Underline = "\u001b[4m";
        public const string Blink = "\u001b[5m";
        public const string Reverse = "\u001b[7m";
        public const string Hidden = "\u001b[8m";
        public const string Strikethrough = "\u001b[9m";
    }
}
