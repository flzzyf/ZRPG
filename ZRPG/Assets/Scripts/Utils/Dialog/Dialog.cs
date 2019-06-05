using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Dialog")]
public class Dialog : ScriptableObject
{
    public Sentence[] sentencess = new Sentence[1];
}

[System.Serializable]
public class Sentence
{
    public string speaker;

    public Content[] contents;
}
[System.Serializable]
public class Content
{
    public Sprite portrait;
    [TextArea(3, 5)]
    public string text;
}