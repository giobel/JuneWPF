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

        public RequestId Take()
        {
            return (RequestId)Interlocked.Exchange(ref this.m_request, 0);
        }

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
            /// <summary>
            /// "Delete" request
            /// </summary>
            Delete = 1,

            TextNote = 2,

            UpdateNote = 3
           
        }
    }
}
