
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using NationalInstruments.DAQmx;


namespace Library
{

    public class NI_DAQ
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
        private AsyncCallback AICallBack;
        AnalogSingleChannelReader analogInReader;

        public double ai_freq = 0;
        public int ai_freq_status = 0;


        // Analog Input
        private AnalogMultiChannelReader analogInReader1;
        private Task aiTask1;
        private double[] analigAiVolt1;

        private AnalogMultiChannelReader analogInReader2;
        private Task aiTask2;
        private double[] analigAiVolt2;
        public double[,] ai_multi_data;

        //Analog Outout
        private Task aoTask1 = new Task();
        private Task aoTask2 = new Task();
        AnalogSingleChannelWriter aoWriter1 = null;
        AnalogSingleChannelWriter aoWriter2 = null;
        //private Task runningAoTask;        

        //Digital Input&Output
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

        public void AllTask_Dispose()
        {
            aiTask1.Dispose();
            aiTask1 = null;

            aoTask1.Dispose();
            aoTask1 = null;

            doTask.Dispose();
            doTask = null;

            encoderTask.Dispose();
            encoderTask = null;

            runningEncoderTask.Dispose();
            runningEncoderTask = null;

            FreqTask.Dispose();
            FreqTask = null;

            counterTask.Dispose();
            counterTask = null;
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
        public string[] AOChannel
        {
            get { return DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External); }
        }
        public string[] DOChannel
        {
            get { return DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External); }
        }
        #endregion

        #region AI
        //采样率为每秒1000个点，两点间隔为1ms
        public bool AI_Create_Task1_Volt_RSE(string[] aiChannel, double minRangeValue, double maxRangeValue)
        {
            aiTask1 = new Task();
            try
            {
                for (int i = 0; i < aiChannel.Length; i++)
                {
                    if (aiChannel[i] == null || !aiChannel[i].Contains("ai"))
                        continue;
                    if (!juageChannel(aiChannel[i], DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External)))
                    {
                        aiTask1.Dispose();
                        throw new DaqException("AI_Channel err,pls check.");
                    }

                    aiTask1.AIChannels.CreateVoltageChannel(aiChannel[i], "", (AITerminalConfiguration)10083, minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                    //aiTask1.Timing.ConfigureSampleClock("", 1000, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                }
                //Verify the Task
                aiTask1.Control(TaskAction.Verify);
                analogInReader1 = new AnalogMultiChannelReader(aiTask1.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aiTask1.Dispose();
                throw new Exception(exception.Message);
            }

            return true;
        }

        //采样率为每秒1000个点，两点间隔为1ms
        public bool AI_Create_Task1_Wave_RSE(string[] aiChannel, double minRangeValue, double maxRangeValue)
        {
            aiTask1 = new Task();
            try
            {
                for (int i = 0; i < aiChannel.Length; i++)
                {
                    if (aiChannel[i] == null || !aiChannel[i].Contains("ai"))
                        continue;
                    if (!juageChannel(aiChannel[i], DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External)))
                    {
                        aiTask1.Dispose();
                        throw new DaqException("AI_Channel err,pls check.");
                    }

                    aiTask1.AIChannels.CreateVoltageChannel(aiChannel[i], "", (AITerminalConfiguration)10083, minRangeValue, maxRangeValue, AIVoltageUnits.Volts);
                    aiTask1.Timing.ConfigureSampleClock("", 1000, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                }
                //Verify the Task
                aiTask1.Control(TaskAction.Verify);
                analogInReader1 = new AnalogMultiChannelReader(aiTask1.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aiTask1.Dispose();
                throw new Exception(exception.Message);
            }

            return true;
        }

        public void AI_Start_Task1()
        {
            aiTask1.Start();
        }

        public double[] AI_Task1_Read_Single()
        {
            analigAiVolt1 = analogInReader1.ReadSingleSample();
            return analigAiVolt1;
        }

        public double[,] AI_Task1_Read_Multi(int sampleNumPerCH)
        {
            ai_multi_data = analogInReader1.ReadMultiSample(sampleNumPerCH);
            return ai_multi_data;
        }

        public void AI_Stop_Task1()
        {
            aiTask1.Stop();
        }

        public void AI_Dispose_Task1()
        {
            try
            {
                aiTask1.Stop();
                aiTask1.Dispose();
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }

        //private Task runningTask;
        //采样率为每秒1000个点，两点间隔为1ms
        public void AI_Create_Task2_Freq(string aiChannel, double minRangeFreq, double maxRangeFreq)
        {
            try
            {
                aiTask2 = new Task();

                AIChannel myAIChannel;
                myAIChannel = aiTask2.AIChannels.CreateFrequencyVoltageChannel(aiChannel, "", minRangeFreq, maxRangeFreq, 0, 0.2, AIFrequencyUnits.Hertz);

                aiTask2.Timing.ConfigureSampleClock("", 1000, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);

                //myAIChannel.LowpassCutoffFrequency = 1;
                //AICallBack = new AsyncCallback(AnalogInCallback_Freq);
                //runningTask = aiTask2;

                analogInReader = new AnalogSingleChannelReader(aiTask2.Stream);

                //// Use SynchronizeCallbacks to specify that the object 
                //// marshals callbacks across threads appropriately.
                //analogInReader.SynchronizeCallbacks = true;
                //analogInReader.BeginReadWaveform(numOfSamples, AICallBack, aiTask2);

            }
            catch (DaqException exception)
            {
                aiTask2.Dispose();
                throw new Exception(exception.Message);
            }
        }

        public double AI_Task2_Read_Freq(int sampleNum)
        {
            try
            {
                int iterations;
                NationalInstruments.AnalogWaveform<double> data = analogInReader.ReadWaveform(sampleNum);
                if (data.Samples.Count < 10)
                    iterations = data.Samples.Count;
                else
                    iterations = 10;

                for (int i = 0; i < iterations; i++)
                {
                    ai_freq += data.Samples[i].Value;
                }
                ai_freq = ai_freq / iterations;
                return ai_freq;
            }
            catch (DaqException exception)
            {
                aiTask2.Dispose();
                throw new Exception(exception.Message);
            }
        }

        //private void AnalogInCallback_Freq(IAsyncResult ar)
        //{
        //    if (runningTask != null && runningTask == ar.AsyncState)
        //    {
        //        try
        //        {
        //            int iterations;

        //            // Retrieve data
        //            NationalInstruments.AnalogWaveform<double> data = analogInReader.EndReadWaveform(ar);

        //            if (data.Samples.Count < 10)
        //                iterations = data.Samples.Count;
        //            else
        //                iterations = 10;

        //            for (int i = 0; i < iterations; i++)
        //            {
        //                ai_freq += data.Samples[i].Value;
        //            }
        //            ai_freq = ai_freq / iterations;
        //            ai_freq_status = 1; 
        //        }
        //        catch (DaqException exception)
        //        {
        //            aiTask2.Dispose();
        //            throw new Exception(exception.Message);
        //        }
        //    }
        //}

        public void AI_Start_Task2()
        {
            aiTask2.Start();
        }

        public void AI_Stop_Task2()
        {
            aiTask2.Stop();
        }

        public void AI_Dispose_Task2()
        {
            try
            {
                aiTask2.Stop();
                aiTask2.Dispose();
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }

        #endregion

        #region AO
        /// <summary>
        /// 建立AO volt任务
        /// </summary>
        /// <param name="ch">Dev1/ao0...</param>
        public void AO_Create_Task1(string ch)
        {
            aoTask1 = new Task();
            try
            {
                aoTask1.AOChannels.CreateVoltageChannel(ch, "aoChannel", -10, 10, AOVoltageUnits.Volts);
                aoWriter1 = new AnalogSingleChannelWriter(aoTask1.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aoTask1.Dispose();
                throw new Exception(exception.Message);
            }
        }

        public void AO_Task1_Write_Single(double value)
        {
            //aoTask1 = new Task();
            try
            {
                aoWriter1.WriteSingleSample(true, value);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aoTask1.Stop();
                aoTask1.Dispose();
                throw new Exception(exception.Message);
            }
        }

        public void AO_Start_Task1()
        {
            aoTask1.Start();

        }

        public void AO_Stop_Task1()
        {
            aoTask1.Stop();
        }

        public void AO_Dispose_Task1()
        {
            if (aoTask1 != null)
            {
                aoTask1.Dispose();
            }
        }


        /// <summary>
        /// 建立AO wave任务
        /// </summary>
        /// <param name="ch">Dev1/ao0...</param>
        /// <param name="rate">每秒发送多少个点</param>
        /// <param name="dataLen">一次发送的点数</param>
        public void AO_Create_Task2(string ch)
        {
            aoTask2 = new Task();
            try
            {
                aoTask2.AOChannels.CreateVoltageChannel(ch, "aoChannel", -10, 10, AOVoltageUnits.Volts);

                // Verify the task before doing the waveform calculations
                aoTask2.Control(TaskAction.Verify);

                aoWriter2 = new AnalogSingleChannelWriter(aoTask2.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aoTask2.Dispose();
                throw new Exception("DAQ Create AO Task FAIL:" + exception.Message);
            }
        }

        public void AO_Task2_Config(double rate, int dataLen)
        {
            // Configure the sample clock with the calculated rate and 
            // specified clock source
            aoTask2.Timing.ConfigureSampleClock("",
               rate,
               SampleClockActiveEdge.Rising,
               SampleQuantityMode.FiniteSamples,
               dataLen);
        }

        public void AO_Task2_Write_Data2Buffer(double[] data)
        {
            try
            {
                // Write data to buffer 
                aoWriter2.WriteMultiSample(false, data);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aoTask2.Stop();
                aoTask2.Dispose();
                throw new Exception(exception.Message);
            }
        }

        public void AO_Start_Task2()
        {
            aoTask2.Start();

        }

        public void AO_Stop_Task2()
        {
            aoTask2.Stop();
        }

        public void AO_Dispose_Task2()
        {
            if (aoTask2 != null)
            {
                aoTask2.Dispose();
            }
        }
        #endregion

        #region DIO
        DigitalSingleChannelWriter DO_writer = null;
        DigitalSingleChannelReader DO_reader = null;
        public void DO_Create_Task(string ch)
        {
            doTask = new Task();
            try
            {
                doTask.DOChannels.CreateChannel(ch, "port0", ChannelLineGrouping.OneChannelForAllLines);
                DO_writer = new DigitalSingleChannelWriter(doTask.Stream);
                DO_reader = new DigitalSingleChannelReader(doTask.Stream);
            }
            catch (DaqException exception)
            {
                //disable the timer and dispose of the task
                aoTask1.Dispose();
                throw new Exception(exception.Message);
            }
        }

        public void DO_Start_Task()
        {
            doTask.Start();
        }

        public void DO_Write(uint value)
        {
            DO_writer.WriteSingleSamplePort(true, value);
        }

        public uint DO_Read()
        {
            uint value = DO_reader.ReadSingleSamplePortUInt32();
            return value;
        }

        public void DO_Stop_Task()
        {
            doTask.Stop();
        }

        public void DO_Dispose_Task()
        {
            doTask.Dispose();
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
                AICallBack = new AsyncCallback(readFrequencyCallBack);
                //FreqReader.BeginReadSingleSampleDouble(FreqCallBack, null);

                FreqReader.BeginReadMultiSampleDouble(FreqNumberOfSamples, AICallBack, FreqTask);
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
