using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Framework.Server
{
    public class StopWatch
    {
        private DateTime _dt;

        public void Start()
        {
            //
            _dt = DateTime.Now;
        }

        public void Stop()
        {
            TimeSpan span = DateTime.Now.Subtract(_dt);
            TotalMilliseconds = (int)span.TotalMilliseconds;
            ExecuteTime = String.Format("{0:00}:{1:00}.{2:000}", span.Minutes, span.Seconds, span.Milliseconds);
        }

        /// <summary>
        /// 실행시간을 분초(00:00.000) 형태로 가져옵니다.
        /// </summary>
        public string ExecuteTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 실행시간을 밀리초 형태로 가져옵니다.
        /// </summary>
        public int TotalMilliseconds
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return "ExecuteTime=" + ExecuteTime;
        }
    }
}
