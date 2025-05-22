using System;
using System.Collections.Generic;
using UnityEngine;

public class RpnEvaluator
{
    private static readonly HashSet<string> Operators = new HashSet<string> { "+", "-", "*", "/", "%" };

    // --- RPN Evaluation Methods ---
    public float EvaluateRPN(string expression, Dictionary<string, float> variables)
    {
        Stack<string> stack = new Stack<string>();

        foreach (string token in expression.Split(' '))
        {
            if (Operators.Contains(token))
            {
                if (stack.Count < 2)
                {
                    Debug.LogError("Invalid expression: not enough operands");
                    return 0;
                }

                string b = stack.Pop();
                string a = stack.Pop();
                float result = ApplyOperator(token, a, b, variables);
                stack.Push(result.ToString());
            }
            else
            {
                stack.Push(token);
            }
        }

        if (stack.Count != 1)
        {
            Debug.LogError("Invalid expression: leftover tokens");
            return 0;
        }

        return float.Parse(stack.Pop());
    }

    private float ApplyOperator(string op, string val1, string val2, Dictionary<string, float> variables)
    {
        float a = ResolveValue(val1, variables);
        float b = ResolveValue(val2, variables);

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

    private float ResolveValue(string token, Dictionary<string, float> variables)
    {
        if (variables.ContainsKey(token))
        {
            return variables[token];
        }
        else if (float.TryParse(token, out float num))
        {
            return num;
        }
        else
        {
            throw new Exception($"Invalid token or undefined variable: {token}");
        }
    }

    // --- Safe Evaluation Methods ---
    public int SafeEvaluateInt(string expression, Dictionary<string, float> variables, int fallback = 0)
    {
        if (string.IsNullOrEmpty(expression)) {
            Debug.LogWarning("expression is null or empty. Defaulting to " + fallback);
            return fallback;
        }
        try
        {
            float result = EvaluateRPN(expression, variables);
            return Mathf.RoundToInt(result);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to evaluate RPN expression (int): {expression} - {ex.Message}");
            return fallback;
        }
    }

    public float SafeEvaluateFloat(string expression, Dictionary<string, float> variables, float fallback = 0f)
    {
        if (string.IsNullOrEmpty(expression)) {
            Debug.LogWarning("Expression is null or empty. Defaulting to " + fallback);
            return fallback;
        }

        // Check if the expression is a simple numeric value
        if (float.TryParse(expression, out float result))
        {
            return result;
        }

        try
        {
            return EvaluateRPN(expression, variables);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to evaluate RPN expression (float): {expression} - {ex.Message}");
            return fallback;
        }
    }

    public float SafeParseFloat(string value, float fallback = 0f)
    {
        if (string.IsNullOrEmpty(value)) return fallback;
        if (float.TryParse(value, out float result))
        {
            return result;
        }
        Debug.LogWarning($"Failed to parse float: {value}");
        return fallback;
    }
}
