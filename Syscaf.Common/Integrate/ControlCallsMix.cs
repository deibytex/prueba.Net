using Syscaf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;


namespace SyscafWebApi.App_Data
{
    public class ControlCallsMix 
    {
        public  int TotalCalls;        
        public Stopwatch _stopwatch;
        public ControlCallsMix() {
            _stopwatch = new Stopwatch();
            TotalCalls = 0;
            
        }

        public void Start()
        {
            _stopwatch.Start();
            TotalCalls++;
        }
        public void Stop()
        {
            _stopwatch.Stop();

        }

        public Stopwatch getInstancia() {
            return _stopwatch;
        }

        public double GetDiffTime()
        {
            return  Constants.SecondMinute - _stopwatch.Elapsed.TotalSeconds;

        }
        public double ValidateCalls()
        {
            if(Constants.CallsMin == TotalCalls)
            {
                TotalCalls = 0;
                double diff = Constants.SecondMinute - _stopwatch.Elapsed.TotalSeconds;

                // determina la diferencia, si aun falta tiempo para completarse el minuto pone a dormir el servicio
                if (diff > 0)
                    Thread.Sleep(((int)diff + 1) * 1000);

                _stopwatch.Reset();                
            }
            return Constants.SecondMinute - _stopwatch.Elapsed.TotalSeconds;

        }

        // verifica si las llamadas se pueden realizar 
        public bool isValidate() {
            return (TotalCalls < Constants.CallsMin && _stopwatch.Elapsed.TotalSeconds < Constants.SecondMinute);
        }

    }
}