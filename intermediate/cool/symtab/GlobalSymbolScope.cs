﻿namespace Compiler.intermediate.cool.symtab
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
            IOType = new ClassSymbol("IO", this) {Parent = ObjectType };
            IntType = new ClassSymbol("Int", this) { Parent = ObjectType };
            StringType = new ClassSymbol("String", this) { Parent = ObjectType };
            BoolType = new ClassSymbol("Bool", this) { Parent = ObjectType };
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
            // ReSharper disable once InconsistentNaming
            var type_name = new MethodSymbol("type_name_", ObjectType) { RTType = StringType };
            var abort = new MethodSymbol("abort_", ObjectType) {RTType = ObjectType};
            var copy = new MethodSymbol("copy_", ObjectType) {RTType = SelfType};

            ObjectType.Enter(abort.SymName, abort);
            ObjectType.Enter(type_name.SymName, type_name);
            ObjectType.Enter(copy.SymName, copy);
        }

        private void InitIOType()
        {
            // ReSharper disable once InconsistentNaming
            var out_string = new MethodSymbol("out_string_", IOType);
            out_string.Formals.Add(StringType);
            out_string.RTType = SelfType;

            // ReSharper disable once InconsistentNaming
            var out_int = new MethodSymbol("out_int_", IOType);
            out_int.Formals.Add(IntType);
            out_int.RTType = SelfType;

            // ReSharper disable once InconsistentNaming
            var in_string = new MethodSymbol("in_string_", IOType) {RTType = StringType};

            // ReSharper disable once InconsistentNaming
            var in_int = new MethodSymbol("in_int_", IOType) {RTType = IntType};

            IOType.Enter(out_string.SymName, out_string);
            IOType.Enter(out_int.SymName, out_int);
            IOType.Enter(in_string.SymName, in_string);
            IOType.Enter(in_int.SymName, in_int);
        }

        private void InitString()
        {
            var length = new MethodSymbol("length_", StringType) { RTType = IntType };

            var concat = new MethodSymbol("concat_", StringType) { RTType = StringType };
            concat.Formals.Add(StringType);

            var substr = new MethodSymbol("substr_", StringType) { RTType = StringType };
            substr.Formals.Add(IntType);
            substr.Formals.Add(IntType);

            StringType.Enter(length.SymName, length);
            StringType.Enter(concat.SymName, concat);
            StringType.Enter(substr.SymName, substr);
        }
    }
}