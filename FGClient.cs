using System.ComponentModel;
using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;


class FGClient : INotifyPropertyChanged
{
    private IData data;
    private int _maxFrame;
    private int _currentFrame;
    private bool zeroSpeed;
    private bool _pause;
    private bool _work;
    private int sleepDaur;
    private int _framesPerSecond;
    public event  PropertyChangedEventHandler PropertyChanged;

    public int MaxFrame
    {
        get
        {
            return _maxFrame;
        }
        set
        {
            _maxFrame = value;
        }
    }
    public int FramesPerSecond
    {
    get
        {
            return _framesPerSecond;
        }
        set
        {
            _framesPerSecond = value;
            zeroSpeed = false;
            //MaxSpeed, can be set as const or global and even can be set by the user
            if (value >= 80)
            {
                _framesPerSecond = 80;
            }
            if (value < 1)
            {
                _framesPerSecond = 0;
                zeroSpeed = true;
                return;
            }
            sleepDaur = 1000 / _framesPerSecond;
        }
    }

    public int CurrentFrame
    {
        get
        {
            return _currentFrame;
        }
        set
        {
            if (value < -1)
            {
                value = -1;
            }
            if (value > MaxFrame)
            {
                value = MaxFrame;
            }
            if (CurrentFrame != value)
            {
                _currentFrame = value;
                NotifyPropertyChanged("currentFrame");
            }
        }
    }

    public bool Pause
    {
        get
        {
            return _pause;
        }
        set
        {
            _pause = value;
        }
    }

    public bool Work
    {
        get
        {
            return _work;
        }
        set
        {
            _work = value;
            if (_work == false)
            {
                Pause = true
                    ;
            }
        }
    }


    public FGClient(IData iData)
    {
        data = iData;
        Pause = false;
        CurrentFrame = -1;
        //default speed is 10 frames per second
        FramesPerSecond = 10;
        Work = true;
        zeroSpeed = false;
        MaxFrame = data.getMaxIndex();
    }


    //This will update the listeners anytime we update an important stat, mostly currentFrame.
    public void NotifyPropertyChanged(string propName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }



    /**
     * Thats the udp client, using udp client over tcp will allow us to login multiple time
     * to the same FG flight. Tcp client is in the buttom of this file under as a comment
     */
    private void sendUdpLine(UdpClient udpClient, int index)
    {
        string frame = data.getChunk(index);
        //sendings
        byte[] bytes = Encoding.ASCII.GetBytes(frame);
        udpClient.Send(bytes, bytes.Length, "localHost", 5400);
    }


    public void Start()
    {
        try
        {
            UdpClient udpClient = new UdpClient();
            while (Work)
            {
                while (!zeroSpeed && !Pause)
                {
                    CurrentFrame++;
                    sendUdpLine(udpClient, CurrentFrame);
                    Thread.Sleep(sleepDaur);
                }
                //prevent busy waiting
                Thread.Sleep(200);
            }
            sendUdpLine(udpClient, 1);
            Thread.Sleep(200);
            udpClient.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }


    public float GetElement(string element, int frame)
    {
        return data.getElement(element, frame);
    }
    
    public void Close()
    {
        Work = false;
    }
}




/**
*tcp client
    private void sendTcpLine(NetworkStream stream, int index)
    {
        string frame = data.getChunk(index);
        //sendings
        byte[] bytes = Encoding.ASCII.GetBytes(frame);
        // Send the message to the connected TcpServer.
        stream.Write(bytes, 0, frame.Length);
        stream.Flush();
    }

    //Start is opening the client and send the info to the FG, the start
    //should be triggered in an independence thread/
    public void Start1()
    {
        try
        {
            TcpClient tcpClient = new TcpClient("localHost", 5400);
            // Get a client stream for reading and writing.
            NetworkStream stream = tcpClient.GetStream();
            Console.WriteLine("Connected");
            while (Work)
            {
                while (!zeroSpeed && !Pause)
                {
                    CurrentFrame++;
                    sendTcpLine(stream, CurrentFrame);
                    Thread.Sleep(sleepDaur);
                }
                //prevent busy waiting
                Thread.Sleep(200);
            }
            //send the 1 line withought updating CurrentFrame so that in the end of the flight the
            //engines will be quiet
            sendTcpLine(stream, 1);
            //Thread.Sleep(200);
            stream.Close();
            tcpClient.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
*/