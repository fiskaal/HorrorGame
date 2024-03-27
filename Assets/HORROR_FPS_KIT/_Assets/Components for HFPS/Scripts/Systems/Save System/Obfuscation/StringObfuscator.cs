using System;
using System.Text;

public static class StringObfuscator
{
    const int MAX = byte.MaxValue + 1;
    
    public static byte[] Convert(string str) {
        
        if (string.IsNullOrEmpty(str))
            return null;
        
        byte[] text = Encoding.UTF8.GetBytes(str);
        byte[] result = new byte[text.Length + 1];
        
        result[0] = (byte)(0x7F + new Random().Next(0x7F));
        
        for (long i = 1; i < result.Length; i++)
            result[i] = (byte)((text[i - 1] + result[0]) % MAX);
        
        return result;
        
    }
    
    public static string Parse(byte[] bin) {
        
        byte[] text = new byte[bin.Length - 1];
        
        int key = bin[0];
        
        for (long i = 0; i < text.Length; i++) {
            text[i] = (byte)((bin[i + 1] - key) % MAX);
        }
        
        return Encoding.UTF8.GetString(text);
        
    }
}