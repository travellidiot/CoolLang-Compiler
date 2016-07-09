namespace Compiler.intermediate.cool.symtab
{
    public class GlobalSymbolScope : SymbolScope
    {
        public ClassSymbolScope ObjectType { get; }
        public ClassSymbolScope IOType { get; }
        public ClassSymbolScope IntType { get; }
        public ClassSymbolScope StringType { get; }
        public ClassSymbolScope BoolType { get; }
        public ClassSymbolScope SelfType { get; }

        public static GlobalSymbolScope Instance = new GlobalSymbolScope();

        //public void CompleteSymbolType()
        //{
        //    var queue = new Queue<SymbolScope>();
        //    queue.Enqueue(this);

        //    while (queue.Count > 0)
        //    {
        //        var scope = queue.Dequeue();
        //        foreach (var entry in scope.Symbols)
        //        {
        //            var symbol = entry.Value as VariableSymbol;
        //            if (symbol != null)
        //            {
        //                var typeName = symbol.SymType.TypeName;
        //                if (typeName == ClassSymbolScope.Undefined)
        //                {
        //                    var typeSymbol = LookupForType(typeName);
        //                    if (typeSymbol != null)
        //                        symbol.SymType = typeSymbol;
        //                    else
        //                    {
        //                        foreach (var program in Symbols)
        //                        {
        //                            typeSymbol = ((SymbolScope) program.Value).LookupForType(typeName);
        //                            if (typeSymbol != null)
        //                            {
        //                                symbol.SymType = typeSymbol;
        //                                break;
        //                            }
        //                        }

        //                        if (typeSymbol == null)
        //                        {
        //                            // Fix me: Type not found
        //                        }
        //                    }
        //                }
        //                // else: everything is fine, just continue
        //            }
        //            else
        //            {
        //                var sScope = entry.Value as SymbolScope;
        //                if (sScope != null)
        //                {
        //                    queue.Enqueue(sScope);
        //                }
        //                else
        //                {
        //                    // Fix me: symbol type error
        //                }
        //            }
        //        }
        //    }
        //}

        private GlobalSymbolScope() : base("Global", null)
        {
            ObjectType = new ClassSymbolScope("Object", this);
            IOType = new ClassSymbolScope("IO", this);
            IntType = new ClassSymbolScope("Int", this);
            StringType = new ClassSymbolScope("String", this);
            BoolType = new ClassSymbolScope("Bool", this);
            SelfType = new ClassSymbolScope("SELF_TYPE", this);

            InitObjectType();
            InitIOType();
            InitString();

            Enter(ObjectType.SymName, ObjectType);
            Enter(IOType.SymName, IOType);
            Enter(IntType.SymName, IntType);
            Enter(StringType.SymName, StringType);
            Enter(BoolType.SymName, BoolType);
            Enter(SelfType.SymName, SelfType);
        }

        private void InitObjectType()
        {
            var abort = new MethodSymbolScope("abort", ObjectType);
            abort.TypeSignature.Add(ObjectType);
            var type_name = new MethodSymbolScope("type_name", ObjectType);
            type_name.TypeSignature.Add(StringType);
            var copy = new MethodSymbolScope("copy", ObjectType);
            copy.TypeSignature.Add(SelfType);

            ObjectType.Enter(abort.SymName, abort);
            ObjectType.Enter(type_name.SymName, type_name);
            ObjectType.Enter(copy.SymName, copy);
        }

        private void InitIOType()
        {
            var out_string = new MethodSymbolScope("out_string", IOType);
            out_string.TypeSignature.Add(StringType);
            out_string.TypeSignature.Add(SelfType);

            var out_int = new MethodSymbolScope("out_int", IOType);
            out_int.TypeSignature.Add(IntType);
            out_int.TypeSignature.Add(SelfType);

            var in_string = new MethodSymbolScope("in_string", IOType);
            in_string.TypeSignature.Add(StringType);

            var in_int = new MethodSymbolScope("in_int", IOType);
            in_int.TypeSignature.Add(IntType);

            IOType.Enter(out_string.SymName, out_string);
            IOType.Enter(out_int.SymName, out_int);
            IOType.Enter(in_string.SymName, in_string);
            IOType.Enter(in_int.SymName, in_int);
        }

        private void InitString()
        {
            var length = new MethodSymbolScope("length", StringType);
            length.TypeSignature.Add(IntType);

            var concat = new MethodSymbolScope("concat", StringType);
            concat.TypeSignature.Add(StringType);
            concat.TypeSignature.Add(StringType);

            var substr = new MethodSymbolScope("substr", StringType);
            substr.TypeSignature.Add(IntType);
            substr.TypeSignature.Add(IntType);
            substr.TypeSignature.Add(StringType);

            StringType.Enter(length.SymName, length);
            StringType.Enter(concat.SymName, concat);
            StringType.Enter(substr.SymName, substr);
        }
    }
}