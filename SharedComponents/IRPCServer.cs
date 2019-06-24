using System;
using System.Collections.Generic;
using System.Text;

namespace SharedComponents
{
    public interface IRPCServer
    {
        void RegisterReceiverCallback(Func<string, string> callback);
    }
}
