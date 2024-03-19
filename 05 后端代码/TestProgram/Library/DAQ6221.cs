/**************************
 **Writer： Simon
 **Date：   2013-11-25
 **Modifier：
 **Modification date：
 **Checker：
 **Check date：
 **Description：
 **      Class of DAQ6259(NI);
 **************************/

using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;
using System.Threading;
using NationalInstruments.DAQmx;

namespace Library
{

    public class DAQ_PCI6221
    {
        //Counter
        private Task counterTask;
        public CICountEdgesActiveEdge edgeCounter;
        public CICountEdgesCountDirection directionCounter;
        private CounterReader CounterReader;
        private uint counterValue;

        //frequency
        private Task FreqTask;
        private CounterReader FreqReader;
        public int FreqNumberOfSamples = 0;
        private double[] measureFrequency = { 0 };
        private CIFrequencyStartingEdge edgeFreq;
        private AsyncCallback FreqCallBack;


        // Analog Input
        private AnalogMultiChannelReader analogInReader;
        private Task aiTask;
        private double[] analigAiVolt;
        //Analog Outout
        private Task aoTask = new Task();
        //private Task runningAoTask;        

        //Digital Input
        private Task diTask = new Task();
        private Task doTask = new Task();

        string[] allStrItems;

        //angle ebcoder Task
        private Task encoderTask;
        public CIChannel EncoderChan;
        private Task runningEncoderTask;
        private CounterReader encoderInReader;
        public int encoderNumberOfSamples = 5000;
        private double[] measureEncoder;
        public bool measureEncoderFlag = true;

        public DAQ_PCI6221()
        {
            allStrItems = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.All, PhysicalChannelAccess.External);
            //counter
            edgeCounter = CICountEdgesActiveEdge.Rising;
            directionCounter = CICountEdgesCountDirection.Up;
            //freq
            edgeFreq = CIFrequencyStartingEdge.Rising;
        }

        #region 获得通道
        public string[] AllChannel
        {
            get { return DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.All, PhysicalChannelAccess.External); }
        }
        public string[] AIChannel
        {
            get { return DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External); }
        }
        public string[] CIChannel
        {
            get { return DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External); }
        }
        public string[] COChannel
        {
            get { return DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CO, PhysicalChannelAccess.External); }
        }
        #endregion

        #region 差分模拟电压测试
        public bool createAiVolt(string diChannel, double minRangeValue, double maxRangeValue)
        {
            aiTask = new Task();
            try
            {
                if (!juageChannel(diChannel, DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External)))
                {
                    aiTask.Dispose();
                    throw new DaqException("diChannel err,pls check.");
                }
                //差分的话 AITerminalConfiguration 设为2；
                aiTask.AIChannels.CreateVoltageChannel(diChannel, "", (AITerminalConfiguration)(-1), minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                //Verify the Task
                aiTask.Control(TaskAction.Verify);
                analogInReader = new AnalogMultiChannelReader(aiTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aiTask.Dispose();
                throw new Exception(exception.Message);
            }
            return false;
        }

        public bool createAiVolt(string diChannel, double minRangeValue, double maxRangeValue, int TestKind)
        {
            aiTask = new Task();
            try
            {
                if (!juageChannel(diChannel, DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External)))
                {
                    aiTask.Dispose();
                    throw new DaqException("diChannel err,pls check.");
                }
                //差分的话 AITerminalConfiguration 设为2；
                aiTask.AIChannels.CreateVoltageChannel(diChannel, "", (AITerminalConfiguration)(TestKind), minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                //Verify the Task
                aiTask.Control(TaskAction.Verify);
                analogInReader = new AnalogMultiChannelReader(aiTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aiTask.Dispose();
                throw new Exception(exception.Message);
            }
            return false;
        }

        public bool createAiVolt(string[] diChannel, double minRangeValue, double maxRangeValue)
        {
            aiTask = new Task();
            try
            {
                for (int i = 0; i < diChannel.Length; i++)
                {
                    if (diChannel[i] == null || !diChannel[i].Contains("ai"))
                        continue;
                    if (!juageChannel(diChannel[i], DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External)))
                    {
                        aiTask.Dispose();
                        throw new DaqException("diChannel err,pls check.");
                    }
                    aiTask.AIChannels.CreateVoltageChannel(diChannel[i], "", (AITerminalConfiguration)(-1), minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                }
                //Verify the Task
                aiTask.Control(TaskAction.Verify);
                analogInReader = new AnalogMultiChannelReader(aiTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aiTask.Dispose();
                throw new Exception(exception.Message);
            }

            return true;
        }

        public bool createAiVolts(string[] diChannel, double minRangeValue, double maxRangeValue, int TestKind)
        {
            aiTask = new Task();
            try
            {
                for (int i = 0; i < diChannel.Length; i++)
                {
                    if (diChannel[i] == null || !diChannel[i].Contains("ai"))
                        continue;
                    if (!juageChannel(diChannel[i], DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External)))
                    {
                        aiTask.Dispose();
                        throw new DaqException("diChannel err,pls check.");
                    }
                    if (diChannel[i] != "Dev1/ai1")
                    {
                        aiTask.AIChannels.CreateVoltageChannel(diChannel[i], "", (AITerminalConfiguration)(TestKind), minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                    }
                    else
                    {
                        aiTask.AIChannels.CreateVoltageChannel(diChannel[i], "", (AITerminalConfiguration)10083, minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                    }
                }
                //Verify the Task
                aiTask.Control(TaskAction.Verify);
                analogInReader = new AnalogMultiChannelReader(aiTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aiTask.Dispose();
                throw new Exception(exception.Message);
            }

            return true;
        }

        public void aiTaskStart()
        {
            aiTask.Start();
        }

        public void aiTaskStop()
        {
            aiTask.Stop();
        }

        public double[] readAiVolt()
        {
            analigAiVolt = analogInReader.ReadSingleSample();
            return analigAiVolt;
        }

        public void releaseAiVolt()
        {
            try
            {
                aiTask.Stop();
                aiTask.Dispose();
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }
        #endregion

        #region 计数器测量
        public bool createCounter(string counterChannel, string counterDirection, string counterEdge, int counterInitial, ref string errMsg)
        {
            counterTask = new Task();
            try
            {
                switch (counterDirection)
                {
                    case "Count Up":
                        directionCounter = CICountEdgesCountDirection.Up;
                        break;
                    case "Count Down":
                        directionCounter = CICountEdgesCountDirection.Down;
                        break;
                    case "Externally Controlled":
                        directionCounter = CICountEdgesCountDirection.ExternallyControlled;
                        break;
                }
                switch (counterEdge)
                {
                    case "Count Rising":
                        edgeCounter = CICountEdgesActiveEdge.Rising;
                        break;
                    case "Count Falling":
                        edgeCounter = CICountEdgesActiveEdge.Falling;
                        break;
                }

                if (!juageChannel(counterChannel, DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External)))
                {
                    counterTask.Dispose();
                    throw new DaqException("counterChannel err,pls check.");
                }
                counterTask.CIChannels.CreateCountEdgesChannel(counterChannel, "Count Edges", edgeCounter, Convert.ToInt64(counterInitial), directionCounter);
                CounterReader = new CounterReader(counterTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                counterTask.Dispose();
                errMsg = exception.Message;
                return false;
            }
            return true;
        }

        public bool createCounter(string[] counterChannel, string counterDirection, string counterEdge, int counterInitial)
        {
            counterTask = new Task();
            try
            {
                switch (counterDirection)
                {
                    case "Count Up":
                        directionCounter = CICountEdgesCountDirection.Up;
                        break;
                    case "Count Down":
                        directionCounter = CICountEdgesCountDirection.Down;
                        break;
                    case "Externally Controlled":
                        directionCounter = CICountEdgesCountDirection.ExternallyControlled;
                        break;
                }
                switch (counterEdge)
                {
                    case "Count Rising":
                        edgeCounter = CICountEdgesActiveEdge.Rising;
                        break;
                    case "Count Falling":
                        edgeCounter = CICountEdgesActiveEdge.Falling;
                        break;
                }
                for (int i = 0; i < counterChannel.Length; i++)
                {
                    if (counterChannel[i] == null || !counterChannel[i].Contains("ctr"))
                        continue;
                    if (!juageChannel(counterChannel[i], DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External)))
                    {
                        counterTask.Dispose();
                        throw new DaqException("counterChannel err,pls check.");
                    }
                    counterTask.CIChannels.CreateCountEdgesChannel(counterChannel[i], "Count Edges", edgeCounter, Convert.ToInt64(counterInitial), directionCounter);
                }
                CounterReader = new CounterReader(counterTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                counterTask.Dispose();
                throw new Exception(exception.Message);
            }
            return true;
        }

        public void startCounter()
        {
            counterTask.Start();
        }
        public void stopCounter()
        {
            counterTask.Stop();
        }
        public uint readCounter()
        {
            counterValue = CounterReader.ReadSingleSampleUInt32();
            return counterValue;
        }
        public void releaseCounter()
        {
            counterTask.Dispose();
        }
        #endregion

        #region 增量编码器测量角度
        public bool createEncoder(string counterChannel, int counterInitial, ref string errMsg)
        {
            CIEncoderDecodingType _encoderType;
            CIEncoderZIndexPhase _encoderPhase;
            bool zIndexEnable = false;//Z Index Enabled
            double zIndexValue = 0;//Z Index Value:
            int pulsePerRev = 2048;//pulsePerRev
            //Sample Clock Source:
            try
            {
                encoderTask = new Task();
                measureEncoderFlag = true;
                _encoderType = CIEncoderDecodingType.X4;
                _encoderPhase = CIEncoderZIndexPhase.AHighBHigh;


                EncoderChan = encoderTask.CIChannels.CreateAngularEncoderChannel(counterChannel,
                    "", _encoderType, zIndexEnable, zIndexValue, _encoderPhase, pulsePerRev,
                    0.0, CIAngularEncoderUnits.Degrees);
                encoderTask.Timing.ConfigureSampleClock("/Dev1/100kHzTimebase", 100000,//Rate
                            SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                EncoderChan.EncoderAInputDigitalFilterEnable = true;
                EncoderChan.EncoderBInputDigitalFilterEnable = true;
                EncoderChan.EncoderAInputDigitalFilterMinimumPulseWidth = 0.000006425;
                EncoderChan.EncoderBInputDigitalFilterMinimumPulseWidth = 0.000006425;
                EncoderChan.AngularEncoderInitialAngle = 0.0;
                encoderTask.Stream.Timeout = -1;
                runningEncoderTask = encoderTask;

                runningEncoderTask.Control(TaskAction.Verify);

                encoderInReader = new CounterReader(runningEncoderTask.Stream);

            }
            catch (DaqException ex)
            {
                encoderTask.Dispose();
                throw new Exception(ex.Message);
            }


            return true;
        }
        public void encodeStart()
        {
            runningEncoderTask.Start();
        }
        public void encodeStop()
        {
            runningEncoderTask.Stop();
        }

        public double[] readEncode()
        {
            try
            {
                measureEncoder = encoderInReader.ReadMultiSampleDouble(800);
                return measureEncoder;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public void releaseEncode()
        {
            runningEncoderTask.Stop();
            measureEncoderFlag = false;
            runningEncoderTask = null;
            encoderTask.Dispose();
        }
        #endregion

        #region 频率测试
        public bool createDigFreqLowFrequency(string frequencyChannel, string edgeFrequency, double minRangeValue, double maxRangeValue, ref string errMsg)
        {
            FreqTask = new Task();
            switch (edgeFrequency)
            {
                case "Freq Rising":
                    edgeFreq = CIFrequencyStartingEdge.Rising;
                    break;
                case "Freq Falling":
                    edgeFreq = CIFrequencyStartingEdge.Falling;
                    break;

            }
            if (!juageChannel(frequencyChannel, DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External)))
            {
                FreqTask.Dispose();
                errMsg = "frequencyChannel err,pls check.";
                return false;
            }
            try
            {
                FreqTask.CIChannels.CreateFrequencyChannel(frequencyChannel, "Measure Dig Freq Low Frequency",
                     Convert.ToDouble(minRangeValue),
                      Convert.ToDouble(maxRangeValue), edgeFreq,
                     CIFrequencyMeasurementMethod.LowFrequencyOneCounter, 0.001,
                     4, CIFrequencyUnits.Hertz);
                FreqReader = new CounterReader(FreqTask.Stream);

                FreqReader.SynchronizeCallbacks = true;
                FreqCallBack = new AsyncCallback(readFrequencyCallBack);
                //FreqReader.BeginReadSingleSampleDouble(FreqCallBack, null);

                FreqReader.BeginReadMultiSampleDouble(FreqNumberOfSamples, FreqCallBack, FreqTask);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                FreqTask.Dispose();
                errMsg = exception.Message;
                return false;
            }


            return true;
        }
        private void readFrequencyCallBack(IAsyncResult ar)
        {
            double[] tmpValue = new double[FreqNumberOfSamples];
            try
            {
                tmpValue = FreqReader.EndReadMultiSampleDouble(ar);
                measureFrequency = tmpValue;
            }
            catch (DaqException exception)
            {
                throw new Exception(exception.Message);
            }
        }
        public double[] MeasureFreqValue
        {
            get { return measureFrequency; }
            set { measureFrequency = value; }
        }
        public void releaseFreq()
        {
            FreqTask.Dispose();
        }
        #endregion

        public void releaseDAQ_PCI6259()
        {
            aiTask = null;
            aoTask = null;
            encoderTask = null;
            runningEncoderTask = null;
            FreqTask = null;
            counterTask = null;
        }

        private bool juageChannel(string tmpStr, string[] items)
        {
            if (tmpStr == null)
                return false;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Equals(tmpStr))
                    return true;
                else
                    continue;
            }
            return false;
        }
    }
}

