using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Storm.Code
{
    /// <summary>
    /// C# 表达式计算
    /// 例 逻辑表达式解析
    ///  string strs = "(DateTime.Parse(\"2018-01-01\")>=DateTime.Parse(\"2018-01-01\")) && (1>2)";
    ///  ExpressionHelp ex = new ExpressionHelp(strs);
    ///  bool b = ex.Compute(true);
    /// </summary>
    public class ExpressionHelp
    {
        object instance;
        MethodInfo method;
        /// <summary>
        /// 表达试运算
        /// </summary>
        /// <param name="expression">表达试</param>
        public ExpressionHelp(string expression)
        {

            if (expression.IndexOf("return") < 0) expression = "return " + expression + ";";
            string className = "Expression";
            string methodName = "Compute";
            CompilerParameters p = new CompilerParameters();
            p.GenerateInMemory = true;
            CompilerResults cr = new CSharpCodeProvider().CompileAssemblyFromSource(p, string.
              Format("using System;sealed class {0}{{public bool {1}(bool x){{{2}}}}}",
              className, methodName, expression));
            if (cr.Errors.Count > 0)
            {
                string msg = "Expression(\"" + expression + "\"): \n";
                foreach (CompilerError err in cr.Errors) msg += err.ToString() + "\n";
                throw new Exception(msg);
            }
            instance = cr.CompiledAssembly.CreateInstance(className);
            method = instance.GetType().GetMethod(methodName);
        }
        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="x"></param>
        /// <returns>返回计算值</returns>
        public bool Compute(bool x)
        {
            return (bool)method.Invoke(instance, new object[] { x });
        }
    }
}
