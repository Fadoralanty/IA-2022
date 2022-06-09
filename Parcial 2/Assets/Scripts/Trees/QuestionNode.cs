using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : INode
{
    //(int,string,bool)=>()
    //Action<int,string,bool> action =  = delegate { };;

    // () => bool
    //Func<bool>

    //// () => bool
    //public delegate bool MyDelegate();
    //MyDelegate _question;
    //Func<bool> _question = delegate { return true; };

    Func<bool> _question;
    INode _nT;
    INode _nF;
    public QuestionNode(Func<bool> newQuestion, INode nT, INode nF)
    {
        //_question = Example;
        //_question += Example2;
        //_question -= Example;
        //_question();
        _question = newQuestion;
        _nF = nF;
        _nT = nT;
    }

    // () => ()
    public void Execute()
    {
        if (_question())
        {
            _nT.Execute();
        }
        else
        {
            _nF.Execute();
        }
    }
}
