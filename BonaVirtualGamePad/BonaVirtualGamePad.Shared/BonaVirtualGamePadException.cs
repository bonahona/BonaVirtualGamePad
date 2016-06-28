using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class BonaVirtualGamePadException : Exception
    {
        public BonaVirtualGamePadException(String message) : base(message){ }
        public BonaVirtualGamePadException(String message, Exception innerException) : base(message, innerException) { }
    }
}
