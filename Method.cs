using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PSRServer
{
    class Method
    {
        MethodInfo methodInfo;
        ParameterInfo[] paramInfo;

        public MethodInfo ClassMethodInfo
        {
            get => methodInfo;
            set => methodInfo = value;
        }

        public ParameterInfo[] ParamInfo
        {
            get => paramInfo;
            set => paramInfo = value;
        }

    }
}

