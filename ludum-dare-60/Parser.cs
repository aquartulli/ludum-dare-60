using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public sealed class Parser
{
    public static List<Instruction> ParseInstructions(string cardText)
    {
        var tokens = Tokenize(cardText);

        var instructions = new List<Instruction>();

        var index = 0;

        while (index < tokens.Count)
        {
            var instruction = ParseInstruction(tokens, ref index);

            instructions.Add(instruction);
        }
        return instructions;
    }

    private static Instruction ParseInstruction(List<string> tokens, ref int index)
    {
        if (!TryPeekToken(tokens, index, out var opCode)) 
        {
            throw new Exception("Expected opCode token");
        }
        return opCode switch
        {
            "MOV" => ParseMove(tokens, ref index),
            _ => throw new Exception($"Unknown opCode: {opCode}")
        };
    }

    private static Instruction ParseMove(List<string> tokens, ref int index) 
    {
        MatchToken(tokens, "MOV", ref index);

        var operand1 = new InstructionOperand(ParseRegisterName(tokens, ref index));
        var operand2 = new InstructionOperand(ParseRegisterName(tokens, ref index));

        return new Instruction(OpCode.Move, [operand1, operand2]);
    }

    private static RegisterName ParseRegisterName(List<string> tokens, ref int index) 
    {
        if (!TryPeekToken(tokens, index, out var token)) 
        {
            throw new Exception("Expected register name");
        }

        index++;

        if (!Enum.TryParse<RegisterName>(token, out var registerName))  
        {
            throw new Exception("Expected register name");
        }

        return registerName;
    }

    private static void MatchToken(List<string> tokens, string expected, ref int index) 
    {
        if (index < tokens.Count && tokens[index++] == expected)
        {
            return;
        }

        throw new Exception("Failed to match token");
    }

    private static bool TryPeekToken(List<string> tokens, int index, out string token)
    {
        if (index < tokens.Count)
        {  
            token = tokens[index];
            return true; 
        }

        token = null;
        return false;
    }

    private static List<string> Tokenize(string text) 
    {
        var tokens = new List<string>();
        var i = 0;
        while (TryGetNextToken(text, ref i, out var token)) 
        {
            tokens.Add(token);
        }
        return tokens;
    }

    private static bool TryGetNextToken(string text, ref int index, out string token) 
    {
        MoveCursorPastWhiteSpace(text, ref index);

        if (index >= text.Length) 
        {
            token = null;
            return false;
        }

        var sb = new StringBuilder();

        while (index < text.Length)
        {
            var c = text[index++];

            if (char.IsWhiteSpace(c))
            {
                token = sb.ToString();
                return true;
            }

            sb.Append(c);
        }

        token = sb.ToString();
        return true;
    }

    private static void MoveCursorPastWhiteSpace(string text, ref int index) 
    {
        while (index < text.Length && char.IsWhiteSpace(text[index]))
        { 
            index++; 
        }
    }
}