using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game : MonoBehaviour
{
    // move from the start to the goal
    // first level: Move left during the green light, right during the red light.
    // second level: Move foward during the blue light, downward during the yellow light
    // third level: move up during the the black light, down during the white light
    // fourth level: adds moving horizontal and veritical wall blocks

    // z from -5 to 5
    // x from -11 to 11

    //string[] colors;

    int level = 1;
    GameObject goal;
    bool musicSet = false;

    float wait = 1.0f;
    bool switching_soon = false;
    bool switch_ready = true;

    int color = 0;
    int color_next = 0;

    bool moveAllowed;

    // 0 = game running
    // 1 = win
    // 2 = loss
    int state = 0;

    AudioSource currentMusic;
    AudioSource beepColorSwitch;

    bool startScreen = true;

    // resets the level
    void resetLevel() {
        setGoal();
        transform.position = new Vector3(0, 0, 0);
    }

    // sets the goal
    void setGoal() {
        if (musicSet)
            currentMusic.Stop();
        if (level == 1)
        {
            musicSet = true;
            currentMusic = GameObject.FindWithTag("music1").GetComponent<AudioSource>();
            currentMusic.Stop();
            int col = Random.Range(-11, 12);
            if (col == 0)
            {
                col = -12;
            }
            goal.transform.position = new Vector3(col, 0, 0);
            goal.GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (level == 2)
        {
            currentMusic = GameObject.FindWithTag("music2").GetComponent<AudioSource>();
            currentMusic.Stop();
            int row = Random.Range(-5, 6);
            if (row == 0)
            {
                row = -6;
            }
            goal.transform.position = new Vector3(0, 0, row);
            goal.GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (level == 3)
        {
            currentMusic = GameObject.FindWithTag("music3").GetComponent<AudioSource>();
            currentMusic.Stop();
            int row = Random.Range(-5, 6);
            int col = Random.Range(-11, 12);
            if (col == 0)
            {
                col = -12;
            }
            if (row == 0)
            {
                row = -6;
            }
            goal.transform.position = new Vector3(col, 0, row);
            goal.GetComponent<Renderer>().material.color = Color.gray;
        }
        currentMusic.Play();
    }

    // returns whether the goal was reached
    bool getGoal() {
        return (transform.position.x == goal.transform.position.x) && (transform.position.y == goal.transform.position.y) && (transform.position.z == goal.transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("goal");
        beepColorSwitch = GameObject.FindWithTag("beepColorSwitch").GetComponent<AudioSource>();
        beepColorSwitch.Stop();
        beepColorSwitch.loop = true;
        resetLevel();
    }
    
    public void ChangeLight()
    {
        if (!switch_ready) return;
        StartCoroutine(IChangeLight());
    }

    IEnumerator IChangeLight()
    {
        SwitchColor();
        switching_soon = false;
        switch_ready = false;
        yield return new WaitForSeconds(wait * 2.0f / 3.0f);
        switching_soon = true;
        beepColorSwitch.Play();
        yield return new WaitForSeconds(wait * 1.0f / 3.0f);
        switching_soon = false;
        beepColorSwitch.Stop();
        switch_ready = true;
    }

    // Set differentColor as true to guarantee a different color
    void GenerateNextColor(bool differentColor = false)
    {
        int numColors = 0;
        if (level == 1)
        {
            numColors = 2;
        }
        else if (level == 2)
        {
            numColors = 2;
        }
        else if (level == 3)
        {
            numColors = 4;
        }
        int cnew = Random.Range(0, numColors);
        if (differentColor)
        {
            while (cnew == color_next)
            {
                cnew = Random.Range(0, numColors);
            }
        }
        color_next = cnew;
    }

    void SwitchColor()
    {
        color = color_next;
        GenerateNextColor();
        if (level == 1)
        {
            if (color == 0)
                GetComponent<Renderer>().material.color = Color.green;
            else if (color == 1)
                GetComponent<Renderer>().material.color = Color.red;
        }
        else if (level == 2)
        {
            if (color == 0)
                GetComponent<Renderer>().material.color = Color.blue;
            else if (color == 1)
                GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (level == 3)
        {
            if (color == 0)
                GetComponent<Renderer>().material.color = Color.green;
            else if (color == 1)
                GetComponent<Renderer>().material.color = Color.red;
            else if (color == 2)
                GetComponent<Renderer>().material.color = Color.blue;
            else if (color == 3)
                GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    bool matchColor()
    {
        Color targetColor = Color.clear;
        if (Input.GetKeyDown("1"))
        {
            targetColor = Color.green;
        }
        else if (Input.GetKeyDown("2"))
        {
            targetColor = Color.red;
        }
        else if (Input.GetKeyDown("3"))
        {
            targetColor = Color.blue;
        }
        else if (Input.GetKeyDown("4"))
        {
            targetColor = Color.yellow;
        }
        else
        {
            return true;
        }

        if (GetComponent<Renderer>().material.color == targetColor)
        {
            moveAllowed = true;
            return true;
        }
        return false;
    }
    // moves the the cube that is the player
    bool move() {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GenerateNextColor(true);
        }

        if (!matchColor())
        {
            return false;
        }

        if (moveAllowed)
        {
            moveAllowed = false;
            if (Input.GetKeyDown("up") && transform.position.z < 5)
            {
                transform.Translate(0, 0, 1);
            }
            else if (Input.GetKeyDown("down") && transform.position.z > -5)
            {
                transform.Translate(0, 0, -1);
            }
            else if (Input.GetKeyDown("right") && transform.position.x < 11)
            {
                transform.Translate(1, 0, 0);
            }
            else if (Input.GetKeyDown("left") && transform.position.x > -11)
            {
                transform.Translate(-1, 0, 0);
            }
            else
            {
                moveAllowed = true;
            }
        }
        return true;
    }

    // displays the level and the instrcutions for the level
    void OnGUI()
    {
        GUIStyle font = new GUIStyle();
        font.fontSize = 15;
        if (startScreen)
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(0, 0, 200, 200), "Press space to start the tutorial", font);
        }
        else
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(0, 0, 200, 200), "Level: " + level);
            if (state == 0)
            {
                if (switching_soon)
                    GUI.Label(new Rect(0, 120, 200, 200), "Switching soon", font);
                //GetComponent<Renderer>().material.color = Color.white;
                if (level == 1)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Green = 1", font);
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "Red = 2", font);
                }
                else if (level == 2)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Blue = 3", font);
                    //GetComponent<Renderer>().material.color = Color.blue;
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "Yellow = 4", font);
                }
                else if (level == 3)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Green = 1", font);
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "Red = 2"), font;
                    //GetComponent<Renderer>().material.color = Color.red;
                    else if (color == 2)
                        GUI.Label(new Rect(0, 100, 200, 200), "Blue = 3", font);
                    //GetComponent<Renderer>().material.color = Color.blue;
                    else if (color == 3)
                        GUI.Label(new Rect(0, 100, 200, 200), "Yellow = 4", font);
                    //GetComponent<Renderer>().material.color = Color.yellow;
                }
                GUI.Label(new Rect(0, 20, 200, 200), "Get to the gray goal. \nPress the matching key for each color. \nIf the correct key is pressed, you can move once in any direction with arrow keys. \nIf not, the level is reset.", font);
            }
            else if (state == 1)
            {
                GUI.Label(new Rect(0, 20, 200, 200), "You Win. Press space to restart.", font);
            }
            else if (state == 2) {
                GUI.Label(new Rect(0, 20, 200, 200), "You Lose. Press space to restart.", font);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (startScreen)
        {
            currentMusic.loop = true;
            if (Input.GetKeyDown("space"))
            {
                GameObject.FindWithTag("MainCamera").transform.position = new Vector3(0, 10, 0);
                startScreen = false;
            }
        }
        else if (state == 1 || state == 2)
        {
            if (Input.GetKeyDown("space"))
            {
                state = 0;
                resetLevel();
            }
        }
        else
        {
            ChangeLight();
            if (!move())
                state = 2;
                //resetLevel();
            if (getGoal())
            {
                if (level == 3)
                {
                    state = 1;
                    //level = 1;
                    //resetLevel();
                }
                else
                {
                    level += 1;
                    resetLevel();
                }
            }
        }
    }
}
