using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private int towerNum;


    public GameObject towerPrefab;
    public GameObject discPrefab;
    public List<Tower> towers = new List<Tower>();
    public List<GameObject> discs = new List<GameObject>();
    public List<GameObject> towersObjects = new List<GameObject>();
    public int discNum;
    private int count;
    public Text text;
    public Text canText;

    public Tower selectedTower;
    private Dictionary<GameObject, Tower> dictionary = new Dictionary<GameObject, Tower>();


    public static TowerManager instance;

    void Start()
    {
        instance = this;
        canText.enabled = false;


        for (int i = 0; i < towerNum; i++)
        {
            towers.Add(new Tower(i + 1));
            GameObject towerObject = Instantiate(towerPrefab, new Vector3(i * 5, 0, 0), Quaternion.identity);
            towersObjects.Add(towerObject);
            
            // Attach the TowerComponent script to the tower GameObject and store the Tower reference
            TowerComponent towerComponent = towerObject.AddComponent<TowerComponent>();
            towerComponent.tower = towers[i];

            if (i == 0)
            {
                for (int j = 0; j < discNum; j++)
{
    TowerDisc disc = new TowerDisc(j + 1, i, j * i + j);
    towers[i].addDisc(disc);
    GameObject currentDiscObject = Instantiate(discPrefab,
        new Vector3(i * 5, (disc.level * 0.254f) + 0.25f, 0),
        Quaternion.identity);
    float scale = 1 - j / 2f / (float)discNum;
    currentDiscObject.transform.localScale = new Vector3(scale, 1, scale);

    // Acesse o objeto filho dentro do prefab
    Transform childObject = currentDiscObject.transform.Find("Disco1");

    // Verifique se o objeto filho existe
    if (childObject != null)
    {
        // Obtenha o componente Renderer do objeto filho
        Renderer renderer = childObject.GetComponent<Renderer>();

        // Verifique se o objeto filho tem um componente Renderer
        if (renderer != null)
        {
            // Altere a cor do objeto filho
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
        else
        {
            Debug.LogError("O objeto filho não tem um componente Renderer.");
        }
    }
    else
    {
        Debug.LogError("O objeto filho não foi encontrado no prefab.");
    }

    discs.Add(currentDiscObject);
}
            }
        }
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            selectTower();
        }
        

        text.text = "Count: " + count;
    }


    public void selectTower()
    {
        Tower currentTower = getClickedTower();

        if (currentTower == null)
        {
            
            Debug.Log("entrou no 1");
            selectedTower = null;
            return;
        }

        if (selectedTower == null)
        {
            selectedTower = currentTower;
            Debug.Log("entrou no 2");

            return;
        }

        if (currentTower == selectedTower)
        {
            selectedTower = null;
            Debug.Log("entrou no 3");

            return;
        }

        Debug.Log(" Boraa aaa");
        moveDisc(selectedTower, currentTower);
        
        selectedTower = null;
    }


    public Tower getClickedTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ray from camera to mouse position
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object has the tag "Tower"
                if (hit.collider.gameObject.CompareTag("Tower1"))
                {
                    Debug.Log("1 TORRE");
                    return towers[0];
                    
                    // return hit.collider.gameObject.GameObject().GetComponent<TowerComponent>().tower;
                }if (hit.collider.gameObject.CompareTag("Tower2"))
                {
                    
                    Debug.Log("2 TORRE");
                    return towers[1];
                    
                    // return hit.collider.gameObject.GameObject().GetComponent<TowerComponent>().tower;
                }if (hit.collider.gameObject.CompareTag("Tower3"))
                {
                    Debug.Log("3 TORRE");
                    return towers[2];
                    
                    // return hit.collider.gameObject.GameObject().GetComponent<TowerComponent>().tower;
                }
            }
        }

        // Return null if no tower was clicked
        return null;
    }

    public Tower GetTowerFromGameObject(GameObject gameObject)
    {
        if (dictionary.TryGetValue(gameObject, out Tower tower))
        {
            Debug.Log("Ta retornando aqui");
            return tower;
        }
        else
        {
            Debug.Log("N'AO TERONA NADA");
            return null;
        }
    }
    
    

    public bool checkMoveDisc(Tower discTower, Tower targetTower)
    {
        if (targetTower.towerDiscs.Count == 0)
        {
            Debug.Log("TA ZERADO");
            return true;
        }

        if (discTower.towerDiscs.Peek().level > targetTower.towerDiscs.Peek().level)
        {
            
            Debug.Log("PASSOU DE BOA");
            return true;
        }
        
        else
        {
            return false;
        }
    }

    public void moveDisc(Tower discTower, Tower targetTower)
    {
        if (!checkMoveDisc(discTower, targetTower))
        {
            canText.enabled = true;
            return;
        }
        else
        {
            canText.enabled = false;
        }

        GameObject disc = discs[discTower.towerDiscs.Peek().level - 1];

        if (targetTower.towerDiscs.Count == 0)
        {
            
            disc.transform.position = new Vector3((targetTower.position-1) * 5, (1 * 0.254f) + 0.25f,
                disc.transform.position.z);
            targetTower.towerDiscs.Push(discTower.towerDiscs.Pop());
        }
        else
        {
            // if (targetTower.towerDiscs.Peek().level > discTower.towerDiscs.Peek().level)
            // {
                disc.transform.position = new Vector3((targetTower.position-1) * 5,
                    ((targetTower.towerDiscs.Count + 1) * 0.254f) + 0.25f, disc.transform.position.z);
                targetTower.towerDiscs.Push(discTower.towerDiscs.Pop());
            // }
        }

        count++;
    }


    public void debugCallObejct()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ray from camera to mouse position
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Get the name and tag of the clicked object
                string objectName = hit.collider.gameObject.name;
                string objectTag = hit.collider.gameObject.tag;

                GameObject clickedObject = hit.collider.gameObject;

                Debug.Log("Clicked on object: " + objectName + " with tag: " + objectTag);
            }
        }
    }


    public void SelectTowerRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ray from camera to mouse position
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object has the tag "Tower"
                if (hit.collider.gameObject.CompareTag("Tower"))
                {
                    // Get the clicked object
                    GameObject clickedObject = hit.collider.gameObject;

                    // Create a LineRenderer object
                    LineRenderer lineRenderer = clickedObject.AddComponent<LineRenderer>();

                    // Set the properties of the LineRenderer
                    lineRenderer.startWidth = 0.05f;
                    lineRenderer.endWidth = 0.05f;
                    lineRenderer.positionCount = 5;

                    // Calculate the points of the circle
                    for (int i = 0; i < 5; i++)
                    {
                        float angle = i * Mathf.PI / 2;
                        Vector3 point = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                        lineRenderer.SetPosition(i, clickedObject.transform.position + point);
                    }

                    Debug.Log("Clicked on object: " + clickedObject.name + " with tag: " + clickedObject.tag);
                }
            }
        }
    }
}