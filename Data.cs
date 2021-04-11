using System;
using System.Runtime.InteropServices;


public interface IData
{
    public int getChunkSize();
    public int getMaxIndex();
    public string getChunk(int num);
    public float getElement(string attr, int frame);

    public void destroy();
}


public class TSDataAdapter : IData
{
    //note that size_t is not compatible with the DLL transition, and therefore the return value is int.
    [DllImport("DllForAnomalyDetector")]
    public static extern int GetRowSize(IntPtr ts);


    //the return value is stringWrapper! get it as IntPtr and cast it to string (example below)
    [DllImport("DllForAnomalyDetector", CallingConvention = CallingConvention.Cdecl)]
    //[return: MarshalAs(UnmanagedType.LPStr)]
    public static extern IntPtr GetFrame(IntPtr ts, int frame);

    [DllImport("DllForAnomalyDetector")]
    public static extern char GetCharByIndex(IntPtr str, int x);

    [DllImport("DllForAnomalyDetector")]
    public static extern int GetMaxFrame(IntPtr ts);

    [DllImport("DllForAnomalyDetector.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float GetAttr(IntPtr ts, char[] attr, int frame);


    //the return value is string! get it as IntPtr and cast it to string (example below)
    [DllImport("DllForAnomalyDetector", CallingConvention = CallingConvention.Cdecl)]
    //[return: MarshalAs(UnmanagedType.LPStr)]
    public static extern int GetFrameLength(IntPtr ts, int frame);

    //destructor
    [DllImport("DllForAnomalyDetector")]
    public static extern void Destroy(IntPtr ts);



    private IntPtr timeseries;

    public TSDataAdapter(IntPtr ts)
    {
        timeseries = ts;
    }

    int IData.getChunkSize()
    {
        return GetRowSize(timeseries);
    }
    int IData.getMaxIndex()
    {
        return GetMaxFrame(timeseries);
    }
    string IData.getChunk(int num)
    {
        IntPtr dllstr = GetFrame(timeseries, num);
        //cast it to string using Marshal
        int i;
        char c;
        string frame = "";
        int str_len = GetFrameLength(timeseries, num);
        for (i = 0; i < str_len; i++)
        {
            c = GetCharByIndex(dllstr, i);
            frame += c.ToString();
        }
        frame += "\n";
        return frame;
    }
    float IData.getElement(string attr, int frame)
    {
        try
        {
            return GetAttr(timeseries, attr.ToCharArray(), frame);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return 0;
    }

    public void destroy()
    {
        if (timeseries != null)
        {
            Destroy(timeseries);
        }
    }
}