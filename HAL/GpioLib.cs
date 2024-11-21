using System;
using System.Runtime.InteropServices;
using System.Threading;

public class GpioLib
{
    // Import libgpiod functions using P/Invoke

    // Enum to represent the line value (Active or Inactive)
    public enum GpiodLineValue
    {
        Active = 1,
        Inactive = 0
    }

    // Struct definitions for required types in libgpiod
    [StructLayout(LayoutKind.Sequential)]
    public struct GpiodLineRequest
    {
        // Placeholder for the actual structure contents
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GpiodChip
    {
        // Placeholder for the actual structure contents
    }

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr gpiod_chip_open([MarshalAs(UnmanagedType.LPStr)] string path);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_chip_close(IntPtr chip);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr gpiod_line_settings_new();

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_line_settings_free(IntPtr settings);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_line_settings_set_direction(IntPtr settings, int direction);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_line_settings_set_output_value(IntPtr settings, GpiodLineValue value);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr gpiod_line_config_new();

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_line_config_free(IntPtr config);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern int gpiod_line_config_add_line_settings(IntPtr config, ref uint offset, int count, IntPtr settings);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr gpiod_chip_request_lines(IntPtr chip, IntPtr reqConfig, IntPtr lineConfig);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_line_request_set_value(IntPtr request, uint offset, GpiodLineValue value);

    [DllImport("libgpiod.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void gpiod_line_request_release(IntPtr request);

    // Request the output line from the GPIO chip
    public static IntPtr RequestOutputLine(string chipPath, uint offset, GpiodLineValue value, string consumer)
    {
        IntPtr chip = gpiod_chip_open(chipPath);
        if (chip == IntPtr.Zero)
            return IntPtr.Zero;

        IntPtr settings = gpiod_line_settings_new();
        if (settings == IntPtr.Zero)
            return IntPtr.Zero;

        gpiod_line_settings_set_direction(settings, 1); // 1: OUTPUT
        gpiod_line_settings_set_output_value(settings, value);

        IntPtr lineConfig = gpiod_line_config_new();
        if (lineConfig == IntPtr.Zero)
            return IntPtr.Zero;

        int ret = gpiod_line_config_add_line_settings(lineConfig, ref offset, 1, settings);
        if (ret != 0)
            return IntPtr.Zero;

        IntPtr reqConfig = IntPtr.Zero;
        if (!string.IsNullOrEmpty(consumer))
        {
            reqConfig = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr))); // Allocate memory for request config
        }

        IntPtr request = gpiod_chip_request_lines(chip, reqConfig, lineConfig);

        gpiod_line_settings_free(settings);
        gpiod_line_config_free(lineConfig);
        gpiod_chip_close(chip);

        return request;
    }

    // Toggle the line value (active or inactive)
    public static GpiodLineValue ToggleLineValue(GpiodLineValue value)
    {
        return value == GpiodLineValue.Active ? GpiodLineValue.Inactive : GpiodLineValue.Active;
    }

    // Convert line value to string (for display purposes)
    public static string ValueStr(GpiodLineValue value)
    {
        return value == GpiodLineValue.Active ? "Active" : "Inactive";
    }
}