using System;
using Compiler.backend.compiler;
using Compiler.backend.interpreter;

namespace Compiler.backend
{
    public class BackendFactory
    {
        public static Backend CreateBackend(string operation)
        {
            if (operation.Equals("compile"))
            {
                return new CodeGenerator();
            }
            else if (operation.Equals("execute"))
            {
                return new Executor();
            }
            else
            {
                throw new Exception("Backend factory: Invalid operation '" + operation + "'");
            }
        }
    }
}