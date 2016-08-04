namespace Compiler.intermediate.cool.symtab
{
    public class GlobalSymbolScope : SymbolScope
    {
        public ClassSymbol ObjectType { get; }
        public ClassSymbol IOType { get; }
        public ClassSymbol IntType { get; }
        public ClassSymbol StringType { get; }
        public ClassSymbol BoolType { get; }
        public ClassSymbol SelfType { get; }

        public static GlobalSymbolScope Instance = new GlobalSymbolScope();

        private GlobalSymbolScope() : base("Global", null)
        {
            ObjectType = new ClassSymbol("Object", this);
            IOType = new ClassSymbol("IO", this);
            IntType = new ClassSymbol("Int", this);
            StringType = new ClassSymbol("String", this);
            BoolType = new ClassSymbol("Bool", this);
            SelfType = new ClassSymbol("SELF_TYPE", this);

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
            var abort = new MethodSymbol("abort", ObjectType);
            abort.TypeSignature.Add(ObjectType);
            // ReSharper disable once InconsistentNaming
            var type_name = new MethodSymbol("type_name", ObjectType);
            type_name.TypeSignature.Add(StringType);
            var copy = new MethodSymbol("copy", ObjectType);
            copy.TypeSignature.Add(SelfType);

            ObjectType.Enter(abort.SymName, abort);
            ObjectType.Enter(type_name.SymName, type_name);
            ObjectType.Enter(copy.SymName, copy);
        }

        private void InitIOType()
        {
            // ReSharper disable once InconsistentNaming
            var out_string = new MethodSymbol("out_string", IOType);
            out_string.TypeSignature.Add(StringType);
            out_string.TypeSignature.Add(SelfType);

            // ReSharper disable once InconsistentNaming
            var out_int = new MethodSymbol("out_int", IOType);
            out_int.TypeSignature.Add(IntType);
            out_int.TypeSignature.Add(SelfType);

            // ReSharper disable once InconsistentNaming
            var in_string = new MethodSymbol("in_string", IOType);
            in_string.TypeSignature.Add(StringType);

            // ReSharper disable once InconsistentNaming
            var in_int = new MethodSymbol("in_int", IOType);
            in_int.TypeSignature.Add(IntType);

            IOType.Enter(out_string.SymName, out_string);
            IOType.Enter(out_int.SymName, out_int);
            IOType.Enter(in_string.SymName, in_string);
            IOType.Enter(in_int.SymName, in_int);
        }

        private void InitString()
        {
            var length = new MethodSymbol("length", StringType);
            length.TypeSignature.Add(IntType);

            var concat = new MethodSymbol("concat", StringType);
            concat.TypeSignature.Add(StringType);
            concat.TypeSignature.Add(StringType);

            var substr = new MethodSymbol("substr", StringType);
            substr.TypeSignature.Add(IntType);
            substr.TypeSignature.Add(IntType);
            substr.TypeSignature.Add(StringType);

            StringType.Enter(length.SymName, length);
            StringType.Enter(concat.SymName, concat);
            StringType.Enter(substr.SymName, substr);
        }
    }
}