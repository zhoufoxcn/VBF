﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VBF.Compilers;

namespace VBF.MiniSharp
{
    class ProgramEntry
    {
        static void Main(string[] args)
        {
            string source = @"
static class 程序入口
{
    //中文注释
    public static void Main(string[] args)
    {
        //hello world
        Fac o;
        o = new Fac();
        System.Console.WriteLine(o.ComputeFac(123));
    }
}

class Fac
{
    public int ComputeFac(int num)
    {
        int num_aux;
        if (num < 1)
            num_aux = 1;
        else
            num_aux = num * (this.ComputeFac(num - 1));
        return num_aux;
    }
}
";
            CompilationErrorManager errorManager = new CompilationErrorManager();
            MiniSharpParser p = new MiniSharpParser(errorManager);
            var ast = p.Parse(source);

            if (errorManager.Errors.Count != 0)
            {
                return;
            }

            TypeDeclResolver resolver1 = new TypeDeclResolver(errorManager);
            resolver1.DefineErrors();
            resolver1.Visit(ast);

            MemberDeclResolver resolver2 = new MemberDeclResolver(errorManager, resolver1.Types);
            resolver2.DefineErrors();
            resolver2.Visit(ast);

            MethodBodyResolver resolver3 = new MethodBodyResolver(errorManager, resolver1.Types);
            resolver3.DefineErrors();
            resolver3.Visit(ast);

            ;
        }
    }
}
