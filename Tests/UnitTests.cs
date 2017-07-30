using CheckDebugFlags;
using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        public class FakeAssembly : Assembly
        {
        }

        [TestMethod]
        public void NullDebugAttributeGivesCorrectDefaults()
        {
            var assembly = new Mock<FakeAssembly>();
            assembly.Setup(
                x => x.GetCustomAttributes(
                    It.IsAny<Type>(), 
                    It.IsAny<bool>()))
                .Returns(new DebuggableAttribute[] { });
            var assemblyFlags 
                = new PrivateObject(
                    typeof(AssemblyFlags), 
                    new object[] { assembly.Object });

            assemblyFlags.Invoke("LoadInformation");

            Assert.IsFalse((bool)assemblyFlags.GetProperty("JitTrackingEnabled"));
            Assert.IsTrue((bool)assemblyFlags.GetProperty("JitOptimized"));
            Assert.IsFalse((bool)assemblyFlags.GetProperty("EditAndContinueEnabled"));
        }
    }
}
