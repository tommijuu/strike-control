using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    public bool isFullscreen;
    public bool isFirstPlay;

    public string playerName;

    public float masterVolume;
    public float musicVolume;
    public float voiceActingVolume;
    public float soundEffectsVolume;

    public Texture2D cursorArrow;

    private void Awake()
    {
        isFirstPlay = true;
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
