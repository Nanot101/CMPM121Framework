using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System;



public class RpnEvaluator
{
    private static readonly HashSet<string> Operators = new HashSet<string> { "+", "-", "*", "/", "%" };

    public int EvaluateRPN(string expression, Dictionary<string, int> variables)
    {
        Stack<string> stack = new Stack<string>();
        // Debug.Log($"Evaluating RPN: {expression}");

        foreach (string token in expression.Split(' '))
        {
            // Debug.Log($"Token: {token}, Stack: [{string.Join(", ", stack)}]");

            if (Operators.Contains(token))
            {
                if (stack.Count < 2)
                {
                    Debug.LogError("Invalid expression: not enough operands");
                    return 0;
                }

                string b = stack.Pop();
                string a = stack.Pop();
                int result = ApplyOperator(token, a, b, variables);
                stack.Push(result.ToString());
            }
            else
            {
                stack.Push(token);
            }
        }

        // Debug.Log($"Final Stack: [{string.Join(", ", stack)}]");

        if (stack.Count != 1)
        {
            Debug.LogError("Invalid expression: leftover tokens");
            return 0;
        }

        return int.Parse(stack.Pop());
    }

    private int ApplyOperator(string op, string val1, string val2, Dictionary<string, int> variables)
    {
        int a = ResolveValue(val1, variables);
        int b = ResolveValue(val2, variables);

        return op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => b != 0 ? a / b : throw new DivideByZeroException(),
            "%" => b != 0 ? a % b : throw new DivideByZeroException(),
            _ => throw new InvalidOperationException($"Unsupported operator: {op}")
        };
    }

    private int ResolveValue(string token, Dictionary<string, int> variables)
    {
        if (variables.ContainsKey(token))
        {
            return variables[token];
        }
        else if (int.TryParse(token, out int num))
        {
            return num;
        }
        else
        {
            throw new Exception($"Invalid token or undefined variable: {token}");
        }
    }
}
