﻿using System;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;
using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationTypeSystemTests
    {
        [Fact]
        public void Should_correctly_load_type_From_assembly()
        {
            var search = new Mock<IAnyWhereConfigurationFileSearch>();

            var assemblyDirectory = Directory.GetCurrentDirectory();
            var assemblyPath = Path.Combine(assemblyDirectory, "CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Adapters.dll");

            var assembly = new Mock<IAnyWhereConfigurationFile>();
            assembly
               .Setup(instance => instance.Directory)
               .Returns(assemblyDirectory)
               .Verifiable();
            assembly
               .Setup(instance => instance.Path)
               .Returns(assemblyPath)
               .Verifiable();

            var system = new AnyWhereConfigurationTypeSystem(search.Object);

            var result = system.Get(
                assembly.Object, 
                "CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Adapters.AnyWhereVerificationConfigurationSourceAdapter");

            Assert.NotNull(result);

            var obj = result.CreateInstance();

            Assert.NotNull(obj);
            Assert.IsAssignableFrom(typeof(IAnyWhereConfigurationAdapter), obj);

            assembly.VerifyAll();
            search.VerifyAll();
        }
    }
}
