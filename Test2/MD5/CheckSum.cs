namespace CheckSum;

using System.Security.Cryptography;
using System.Text;

public static class CheckSum
{
    private const int multiplier = 13;

    public static long checkSum(string path)
    {
        if (File.Exists(path))
        {
            return getSum(getMD5File(path));
        }
        else if (Directory.Exists(path))
        {
            return getSum(getMD5Dir(path));
        }
        else
        {
            throw new ArgumentException("No such file or directory");
        }
    }

    private static byte[] getMD5File(string path)
    {
        var fileContent = File.ReadAllBytes(path);
        var hashData = MD5.HashData(fileContent);
        return hashData;
    }

    private static byte[] getMD5Dir(string path)
    {
        var fileName = path[(path.LastIndexOf('/') + 1)..];
        var hashData = MD5.HashData(Encoding.UTF8.GetBytes(fileName));
        var files = Directory.GetFiles(path);
        var directories = Directory.GetDirectories(path);
        var sumArray = new List<byte>();
        sumArray.AddRange(hashData);
        foreach (var f in files)
        {
            sumArray.AddRange(getMD5File(f));
        }
        foreach (var d in directories)
        {
            sumArray.AddRange(getMD5Dir(d));
        }
        return sumArray.ToArray();
    }

    private static long getSum(byte[] array)
    {
        long result = 0;
        for (int i = 0; i < array.Length; ++i)
        {
            result += array[i] * multiplier ^ i;
        }
        return result;
    }

    public async static Task<long> checkSumAsync(string path)
    {
        if (File.Exists(path))
        {
            return getSum(await getMD5FileAsync(path));
        }
        else if (Directory.Exists(path))
        {
            return getSum(await getMD5DirAsync(path));
        }
        else
        {
            throw new ArgumentException("No such file or directory");
        }
    }

    private async static Task<byte[]> getMD5FileAsync(string path)
    {
        var fileContent = await File.ReadAllBytesAsync(path);
        var hashData = MD5.HashData(fileContent);
        return hashData;
    }

    private async static Task<byte[]> getMD5DirAsync(string path)
    {
        var fileName = path[(path.LastIndexOf('/') + 1)..];
        var hashData = MD5.HashData(Encoding.UTF8.GetBytes(fileName));
        var files = Directory.GetFiles(path);
        var directories = Directory.GetDirectories(path);
        var sumArray = new List<byte>();
        sumArray.AddRange(hashData);
        foreach (var f in files)
        {
            sumArray.AddRange(await Task.Run(() => getMD5FileAsync(f)));
        }
        foreach (var d in directories)
        {
            sumArray.AddRange(await Task.Run(() => getMD5DirAsync(d)));
        }
        return sumArray.ToArray();
    }
}