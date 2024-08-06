using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class IniFile
{
    public string Path { get; }

    public IniFile(string iniPath)
    {
        if (string.IsNullOrWhiteSpace(iniPath))
        {
            throw new ArgumentException("INI file path cannot be null or empty.");
        }

        Path = iniPath;
    }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnString, int size, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileSection(string section, byte[] keyValue, int size, string filePath);

    public void WriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, Path);
    }

    public string ReadValue(string section, string key, string defaultValue = "")
    {
        var sb = new StringBuilder(255);
        GetPrivateProfileString(section, key, defaultValue, sb, 255, Path);
        return sb.ToString();
    }

    public bool KeyExists(string section, string key)
    {
        return !string.IsNullOrWhiteSpace(ReadValue(section, key));
    }

    public List<string> GetKeysInSection(string section)
    {
        byte[] buffer = new byte[8192];
        int length = GetPrivateProfileSection(section, buffer, buffer.Length, Path);
        var keys = Encoding.ASCII.GetString(buffer, 0, length - 1).Split(new char[] { '\0' });
        return new List<string>(keys);
    }

    public bool SectionExists(string section)
    {
        return GetKeysInSection(section).Count > 0;
    }
}