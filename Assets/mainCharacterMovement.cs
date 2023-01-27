using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class mainCharacterMovement : MonoBehaviour
{

    public float speed;
    public float size;
    public static int numberOfHearths=4;
    public static float health=4;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public List<ParticleCollisionEvent> collisionEvents;

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        string data = (string)bf.Deserialize(file);
        file.Close();

        Debug.Log(data);
    }
    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

      
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, "134234");
        file.Close();
    }
    // Start is called before the first frame update
    void Start()
    {
        var localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * size, localScale.y * size, localScale.z);
        SaveFile();
        LoadFile();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        this.transform.Translate(new Vector3(horizontal, vertical, 0f));
        if(!(horizontal + this.transform.position.x > -8.8))
        {
            this.transform.Translate(new Vector3(-horizontal, vertical, 0f));
        }
         if (!(this.transform.position.y > -5))
        {
            transform.position = new Vector3(this.transform.position.x, 4.8f, 0f); 
        } 
         if (!(vertical + this.transform.position.y < 5))
        {
            transform.position = new Vector3(this.transform.position.x, -4.8f, 0f);
        }
       

        if (Input.GetKey("z"))
        {
            this.GetComponent<bulletHellSpawner>().isShooting = true;
        }
        else
        {
            this.GetComponent<bulletHellSpawner>().isShooting = false;
        }

        if (health > numberOfHearths)
        {
            health = numberOfHearths;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numberOfHearths)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void DecrementHp()
    {
        health -= 1f;
        if (health == 0)
        {
            // SceneManager.LoadScene("MainMenuScene");  
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        DecrementHp();
    }
}
