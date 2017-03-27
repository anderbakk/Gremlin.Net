﻿using Gremlin.Net.Process.Traversal;
using Xunit;

namespace Gremlin.Net.Process.UnitTest.Traversal
{
    public class BytecodeTests
    {
        [Fact]
        public void ShouldUseBingings()
        {
            var bytecode = new Bytecode();
            var bindings = new Bindings();

            bytecode.AddStep("hasLabel", bindings.Of("label", "testLabel"));

            var arg = bytecode.StepInstructions[0].Arguments[0];
            var binding = arg as Binding;
            Assert.Equal(new Binding("label", "testLabel"), binding);
        }
    }
}