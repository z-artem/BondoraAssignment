using System;
using System.Collections.Generic;
using System.Text;

namespace SharedComponents
{
    public interface IRPCClient
    {
        string SendMessage(string message);
    }
}
