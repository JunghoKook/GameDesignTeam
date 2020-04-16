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

    int currTime = 0;
    int wait = 500;

    int color = 0;

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
        currTime = 0;
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
            goal.transform.position = new Vector3(col, 0, 0);
            goal.GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (level == 2)
        {
            currentMusic = GameObject.FindWithTag("music2").GetComponent<AudioSource>();
            currentMusic.Stop();
            int row = Random.Range(-5, 6);
            goal.transform.position = new Vector3(0, 0, row);
            goal.GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (level == 3) {
            currentMusic = GameObject.FindWithTag("music3").GetComponent<AudioSource>();
            currentMusic.Stop();
            int depth = Random.Range(-1, 2);
            goal.transform.position = new Vector3(5, depth, 0);
            goal.GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (level == 4)
        {
            currentMusic = GameObject.FindWithTag("music4").GetComponent<AudioSource>();
            currentMusic.Stop();
            int row = Random.Range(-5, 6);
            int col = Random.Range(-11, 12);
            int depth = Random.Range(-1, 2);
            goal.transform.position = new Vector3(col, depth, row);
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

    // changes the movement that is allowed
    void changeLight()
    {
        if (currTime >= wait * 2 / 3 && !beepColorSwitch.isPlaying)
        {
            beepColorSwitch.Play();
        }
        if (level == 1)
        {
            if (currTime % wait == 0)
            {
                color = Random.Range(0, 2);
                if (color == 0)
                    GetComponent<Renderer>().material.color = Color.green;
                else if (color == 1)
                    GetComponent<Renderer>().material.color = Color.red;
                currTime = 1;
                beepColorSwitch.Stop();
            }
            else
                currTime += 1;
        }

        else if (level == 2)
        {
            if (currTime % wait == 0)
            {
                color = Random.Range(0, 2);
                if (color == 0)
                    GetComponent<Renderer>().material.color = Color.blue;
                else if (color == 1)
                    GetComponent<Renderer>().material.color = Color.yellow;
                currTime = 1;
                beepColorSwitch.Stop();
            }
            else
                currTime += 1;
        }
        else if (level == 3)
        {
            if (currTime % wait == 0)
            {
                color = Random.Range(0, 4);
                if (color == 0)
                    GetComponent<Renderer>().material.color = Color.black;
                else if (color == 1)
                    GetComponent<Renderer>().material.color = Color.white;
                else if (color == 2)
                    GetComponent<Renderer>().material.color = Color.red;
                else if (color == 3)
                    GetComponent<Renderer>().material.color = Color.green;
                currTime = 1;
                beepColorSwitch.Stop();
            }
            else
                currTime += 1;
        }
        else if (level == 4) {
            if (currTime % wait == 0)
            {
                color = Random.Range(0, 6);
                if (color == 0)
                    GetComponent<Renderer>().material.color = Color.green;
                else if (color == 1)
                    GetComponent<Renderer>().material.color = Color.red;
                else if (color == 2)
                    GetComponent<Renderer>().material.color = Color.blue;
                else if (color == 3)
                    GetComponent<Renderer>().material.color = Color.yellow;
                else if (color == 4)
                    GetComponent<Renderer>().material.color = Color.black;
                else if (color == 5)
                    GetComponent<Renderer>().material.color = Color.white;
                currTime = 1;
                beepColorSwitch.Stop();
            }
            else
                currTime += 1;
        }
    }

    // moves the the cube that is the player
    bool move() {
        if (Input.GetKeyDown("up") && transform.position.z < 5)
        {
            transform.Translate(0, 0, 1);
            return GetComponent<Renderer>().material.color == Color.blue;
        }
        else if (Input.GetKeyDown("down") && transform.position.z > -5)
        {
            transform.Translate(0, 0, -1);
            return GetComponent<Renderer>().material.color == Color.yellow;
        }
        else if (Input.GetKeyDown("right") && transform.position.x < 11)
        {
            transform.Translate(1, 0, 0);
            return GetComponent<Renderer>().material.color == Color.red;
        }
        else if (Input.GetKeyDown("left") && transform.position.x > -11)
        {
            transform.Translate(-1, 0, 0);
            return GetComponent<Renderer>().material.color == Color.green;
        }
        else if (Input.GetKeyDown("w") && transform.position.y < 1)
        {
            transform.Translate(0, 1, 0);
            return GetComponent<Renderer>().material.color == Color.black;
        }
        else if (Input.GetKeyDown("s") && transform.position.y > -1)
        {
            transform.Translate(0, -1, 0);
            return GetComponent<Renderer>().material.color == Color.white;
        }
        return true;
    }

    // displays the level and the instrcutions for the level
    void OnGUI()
    {
        if (startScreen)
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(0, 0, 200, 200), "Press space to start the tutorial");
        }
        else
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(0, 0, 200, 200), "Level: " + level);
            if (state == 0)
            {
                if (currTime >= wait * 2 / 3)
                    GUI.Label(new Rect(0, 120, 200, 200), "Switching soon");
                //GetComponent<Renderer>().material.color = Color.white;
                if (level == 1)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Green = <-");
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "Red = ->");
                    //GetComponent<Renderer>().material.color = Color.red;
                        GUI.Label(new Rect(0, 20, 200, 200), "Get to the gray goal. Learn veritcal movement. Right arrow to move right during red color and left arrow for left for green color. Not doing this loses the game.");
                }
                else if (level == 2)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Black = w");
                    //GetComponent<Renderer>().material.color = Color.black;
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "White = s");
                    GUI.Label(new Rect(0, 20, 200, 200), "Get to the gray goal. Learn horizontal movement. Up arrow to move forward during blue color and down arrow for downward for yellow color. Not doing this loses the game.");
                }
                else if (level == 3)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Black = w");
                    //GetComponent<Renderer>().material.color = Color.black;
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "White = s");
                    if (color == 3)
                        GUI.Label(new Rect(0, 100, 200, 200), "Green = <-");
                    else if (color == 2)
                        GUI.Label(new Rect(0, 100, 200, 200), "Red = ->");
                    GUI.Label(new Rect(0, 20, 200, 200), "Get to the gray goal. Learn depth. w key to move up during black color and s key for down for white color. Not doing this loses the game.");
                }
                else if (level == 4)
                {
                    if (color == 0)
                        GUI.Label(new Rect(0, 100, 200, 200), "Green = <-");
                    else if (color == 1)
                        GUI.Label(new Rect(0, 100, 200, 200), "Red = ->");
                    //GetComponent<Renderer>().material.color = Color.red;
                    else if (color == 2)
                        GUI.Label(new Rect(0, 100, 200, 200), "Blue = ^");
                    //GetComponent<Renderer>().material.color = Color.blue;
                    else if (color == 3)
                        GUI.Label(new Rect(0, 100, 200, 200), "Yellow = V");
                    //GetComponent<Renderer>().material.color = Color.yellow;
                    else if (color == 4)
                        GUI.Label(new Rect(0, 100, 200, 200), "Black = w");
                    //GetComponent<Renderer>().material.color = Color.black;
                    else if (color == 5)
                        GUI.Label(new Rect(0, 100, 200, 200), "White = s");
                    GUI.Label(new Rect(0, 20, 200, 200), "Use everything you have learned so far to get to the gray goal. Succeeding will send you back to level one. Any of the previous failures will lose the game.");
                }
            }
            else if (state == 1)
            {
                GUI.Label(new Rect(0, 20, 200, 200), "You Win. Press space to restart.");
            }
            else if (state == 2) {
                GUI.Label(new Rect(0, 20, 200, 200), "You Lose. Press space to restart.");
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
                level = 1;
                resetLevel();
            }
        }
        else
        {
            changeLight();
            if (!move())
                state = 2;
                //resetLevel();
            if (getGoal())
            {
                if (level == 4)
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
