using timeseries.dll



interface IData
{
    int getChunkSize();
    int getMaxIndex();
    string getChunk(int num);
    float getElement(string attr, int frame);
}


class TSDataAdapter : IData
{
    private TimeSeries timeseries;

    public TSData(TimeSeries ts)
    {
        timeseries = ts;
    }

    int getChunkSize()
    {
        return timeseries.getRowSize();
    }
    int getMaxIndex()
    {
        return timeseries.getMaxFrame();
    }
    string getChunk(int num)
    {
        string tmp = new string(timeseries.getFrame(int num));
        return tmp;
    }
    float getElement(string attr, int frame)
    {
        return timeseries.getAttr(string attr, int frame);
    }

}