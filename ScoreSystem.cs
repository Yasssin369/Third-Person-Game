using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    static public int _points;
    static  TMP_Text text;
    
    public static void Add(int points)
    {
        _points += points;
        text.SetText(_points.ToString());
    }
   
     void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

}
