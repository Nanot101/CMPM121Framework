using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System;
using org.matheval;

public class Util : MonoBehaviour
{
    string[] variables = new string[""];
    string[] operators = new string["+", "-", "*", "/", "%"];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int applyOperator(string op, string val1, string val2)
    {
        var firstVal;
        var secondVal;
        if (variables.Contains(val1))
        {
            firstVal = val1;
        }
        else
        {
            firstVal = int.Parse(val1);
        }
        if (variables.Contains(val2)) {
            secondVal = val2;
        }
        else
        {
            secondVal = int.Parse(val2);
        }
        Expression finalEq = new Expression(firstVal + " " + op + " " + secondVal);
        return finalVal = finalEq.Eval<int>();
        /*
        if (op == "+")
        {
            int finalVal = (firstVal + secondVal);
        }
        else if (op == "-")
        {
            int finalVal = firstVal - secondVal;
        }
        else if (op == "*")
        {
            int finalVal = firstVal * secondVal;
        }
        else if (op == "/")
        {
            int finalVal = firstVal / secondVal;
        }
        else
        {
            int finalVal = firstVal % secondVal;
        }
        */
    }
            int RPNEvaluator(string expression)
            {
                Stack<string> stack = new Stack<string>();
                foreach (string token in expression.Split(" "))
                {
                    if (variables.Contains(token))
                    {
                        stack.Push(token);
                    }
                    else if (operators.Contains(token))
                    {
                        string a = stack.Pop();
                        string b = stack.Pop();
                        stack.Push(applyOperator(token, b, a));
                    }
                }
            }


    }
}
