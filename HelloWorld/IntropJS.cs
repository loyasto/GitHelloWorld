﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JScript;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Reflection;

namespace HelloWorld
{
    public class IntropJS
    {        
        public static JScriptCodeProvider MJScriptCodeProvider;
        public static CompilerParameters parameters = new CompilerParameters();

        static IntropJS()
        {
            MJScriptCodeProvider = new JScriptCodeProvider();
            parameters.GenerateInMemory = true;
        }

        public static bool ImportJSFile(string file)
        {
            if(File.Exists(file))
            {
                try
                {
                    var str = file;
                    var pa = new CompilerParameters
                    {
                        GenerateInMemory = true
                    };
                    var result = MJScriptCodeProvider.CompileAssemblyFromSource(parameters, str);
                    var ass = result.CompiledAssembly;
                    var s = ass.GetType();
                    var obj = s.InvokeMember("Test", BindingFlags.InvokeMethod, null, null, new object[] { "this ok"});
                    Console.WriteLine("OBJ::"+obj); 
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public static object RunJS_MS(string code)
        {
            //var msc = new MSScriptControl.ScriptControlClass();
            //msc.Language = "javascript";
            //msc.AddCode("test.js");
            //var res = msc.Eval(@"test(""sfsafafas"")");
            //Console.WriteLine(res);
            return null;
        }

        public static object RunJS_V8()
        {
            using(var engine = new V8ScriptEngine())
            {
                var f = "test.js";
                var code = File.ReadAllText(f);
                engine.Compile(code);
                engine.AddHostType("Console", typeof(Console));
                engine.Execute("Console.WriteLine('{0} is an interesting number.', Math.PI)");
                engine.AddHostType("Console", typeof(Console));
                engine.Evaluate("Console.debug('faasfafa')");
                var res = engine.Evaluate("test(fsfs)");
                Console.WriteLine(res);
            }
            return null;
        }

    }
}
