using System.ComponentModel;
using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using Data;


class FGClient : INotifyPropertyChanged
{
    private int maxFrame;
    private IData data;

    public int FramesPerSecond
    {
    get
        {
            return FramesPerSecond;
        }
        set
        {
            FramesPerSecond = value;
            zeroSpeed = false;
            //MaxSpeed, can be set as const or global and even can be set by the user
            if (value >= 80)
            {
                FramesPerSecond = 80;
            }
            if (value <= 1)
            {
                FramesPerSecond = 1;
                zeroSpeed = true;
                return;
            }
            sleepDaur = 1000 / FramesPerSecond;
        }
    }

    private bool zeroSpeed;
    public int CurrenFrame
    {
        get
        {
            return CurrenFrame;
        }
        set
        {
            if (value < -1)
            {
                value = -1;
            }
            if (value > maxFrame)
            {
                value = maxFrame;
            }
            if (CurrenFrame != value)
            {
                CurrenFrame = value;
                NotifyPropertyChanged("currenFrame");
            }
        }
    }
    private int sleepDaur;

    public bool Work
    {
        get
        {
            return Work;
        }
        set
        {
            Work = value;
            if (Work == false)
            {
                Pause = false;
            }
        }
    }
    public bool Pause
    {
        get
        {
            return Pause;
        }
        set
        {
            if (Pause != value)
            {
                Pause = value;
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;


    public FGClient(IData iData)
    {
        data = iData;
        Pause = false;
        CurrenFrame = -1;
        FramesPerSecond = 10;
        Work = true;
        zeroSpeed = false;
        maxFrame = data.getMaxIndex();
    }


    //This will update the listeners anytime we update an important stat, mostly currentFrame.
    public void NotifyPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    //Start is opening the client and send the info to the FG, the start
    //should be triggered in an independence thread/
    public void Start()
    {
        try
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("localHost", 5400);
            while (Work)
            {
                while (!zeroSpeed && !Pause)
                {
                    CurrenFrame++;
                    string frame = data.getChunk(CurrenFrame);
                    //sendings
                    byte[] bytes = Encoding.ASCII.GetBytes(frame);
                    udpClient.Send(bytes, bytes.Length);
                    Thread.Sleep(sleepDaur);
                }
                Thread.Sleep(200);
            }
            udpClient.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    //GetAttr is a bridge to the info inside the ts.
    public float GetAttr(string attr, int frame)
    {
        return data.getElement(attr, frame);
    }
    
    public void Close()
    {
        Work = false;
    }
}

