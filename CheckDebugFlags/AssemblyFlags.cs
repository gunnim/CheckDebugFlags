using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CheckDebugFlags
{
    class AssemblyFlags
    {
        public AssemblyFlags(Assembly assembly)
        {
            Assembly = assembly;
            LoadInformation();
        }

        public Assembly Assembly { get; private set; }

        public bool EditAndContinueEnabled { get; private set; }

        public bool JitOptimized { get; private set; }

        public bool JitTrackingEnabled { get; private set; }

        private void LoadInformation()
        {
            DebuggableAttribute debugAttribute = Assembly.GetCustomAttributes<DebuggableAttribute>().FirstOrDefault();

            if (debugAttribute != null)
            {
                JitTrackingEnabled = debugAttribute.IsJITTrackingEnabled;
                JitOptimized = !debugAttribute.IsJITOptimizerDisabled;
                EditAndContinueEnabled = (debugAttribute.DebuggingFlags & DebuggableAttribute.DebuggingModes.EnableEditAndContinue) != DebuggableAttribute.DebuggingModes.None;
            }
            else
            {
                // No DebuggableAttribute means IsJITTrackingEnabled=false, IsJITOptimizerDisabled=false, EnableEditAndContinue=false
                JitTrackingEnabled = false;
                JitOptimized = true;
                EditAndContinueEnabled = false;
            }
        }
    }
}
