using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class Dialogue_Variables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    public Dialogue_Variables(string globalsFilePath)
    {
        // compile the story
        string inkFileContents = File.ReadAllText(globalsFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
        Story globalVariablesStory = compiler.Compile();

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach(string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + value);
        }
    }

    public void StartListening(Story story) 
    {
        // its important that VariablesToStory is before assigning the listener!
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string PartyStatus, Ink.Runtime.Object value)
    {
        //only maintain variables that were initialized from the globals ink file

        if(variables.ContainsKey(PartyStatus))
        {
            variables.Remove(PartyStatus);
            variables.Add(PartyStatus, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
