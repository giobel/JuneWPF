using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JuneWPF.Model
{
    public class Request
    {
        private int m_request = 0;

        /// <summary>
        ///   Take - The Idling handler calls this to obtain the latest request. 
        /// </summary>
        /// <remarks>
        ///   This is not a getter! It takes the request and replaces it
        ///   with 'None' to indicate that the request has been "passed on".
        /// </remarks>
        /// 
        public RequestId Take()
        {
            return (RequestId)Interlocked.Exchange(ref this.m_request, 0);
        }

        /// <summary>
        ///   Make - The Dialog calls this when the user presses a command button there. 
        /// </summary>
        /// <remarks>
        ///   It replaces any older request previously made.
        /// </remarks>
        /// 
        public void Make(RequestId request)
        {
            Interlocked.Exchange(ref this.m_request, (int)request);
        }

        public enum RequestId : int
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,

            TextNote = 1,

            UpdateNote = 2,

            OpenView = 3,

            BackToSheet = 4
           
        }
    }
}
