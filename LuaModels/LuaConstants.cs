using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.LoennScriptGenerator.LuaModels;

public static class LuaConstants
{
    public static readonly LuaIdentifier Local = new LuaIdentifier("local");
    public static readonly LuaIdentifier Function = new LuaIdentifier("function");
    public static readonly LuaIdentifier If = new LuaIdentifier("if");
    public static readonly LuaIdentifier ElseIf = new LuaIdentifier("elseif");
    public static readonly LuaIdentifier Else = new LuaIdentifier("else");
    public static readonly LuaIdentifier End = new LuaIdentifier("end");
    public static readonly LuaIdentifier While = new LuaIdentifier("while");
    public static readonly LuaIdentifier Repeat = new LuaIdentifier("repeat");
    public static readonly LuaIdentifier For = new LuaIdentifier("for");
    public static readonly LuaIdentifier In = new LuaIdentifier("in");
    public static readonly LuaIdentifier Do = new LuaIdentifier("do");
    public static readonly LuaIdentifier Then = new LuaIdentifier("then");
    public static readonly LuaIdentifier Return = new LuaIdentifier("return");
    public static readonly LuaIdentifier Not = new LuaIdentifier("not");
    public static readonly LuaIdentifier And = new LuaIdentifier("and");
    public static readonly LuaIdentifier Or = new LuaIdentifier("or");

    public static readonly LuaIdentifier Plus = new LuaIdentifier("+");
    public static readonly LuaIdentifier Minus = new LuaIdentifier("-");
    public static readonly LuaIdentifier Multiply = new LuaIdentifier("*");
    public static readonly LuaIdentifier Divide = new LuaIdentifier("/");
    public static readonly LuaIdentifier Equal = new LuaIdentifier("==");
    public static readonly LuaIdentifier NotEqual = new LuaIdentifier("~=");
    public static readonly LuaIdentifier LessThan = new LuaIdentifier("<");
    public static readonly LuaIdentifier GreaterThan = new LuaIdentifier(">");
    public static readonly LuaIdentifier LessThanOrEqual = new LuaIdentifier("<=");
    public static readonly LuaIdentifier GreaterThanOrEqual = new LuaIdentifier(">=");
    public static readonly LuaIdentifier Assignment = new LuaIdentifier("=");

    public static readonly LuaIdentifier Comma = new LuaIdentifier(",");
    public static readonly LuaIdentifier Semicolon = new LuaIdentifier(";");
    public static readonly LuaIdentifier Dot = new LuaIdentifier(".");
    public static readonly LuaIdentifier OpenParen = new LuaIdentifier("(");
    public static readonly LuaIdentifier CloseParen = new LuaIdentifier(")");
    public static readonly LuaIdentifier OpenBrace = new LuaIdentifier("{");
    public static readonly LuaIdentifier CloseBrace = new LuaIdentifier("}");
    public static readonly LuaIdentifier Quote = new LuaIdentifier("\"");
}