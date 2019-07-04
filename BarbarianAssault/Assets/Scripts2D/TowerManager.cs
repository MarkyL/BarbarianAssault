using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Singleton<TowerManager>
{
    public TowerBtn towerBtnPressed{get; set;}
	public List<GameObject> TowerList = new List<GameObject>();
	public List<Collider2D> BuildList = new List<Collider2D>();

	private SpriteRenderer spriteRenderer;
	private Collider2D buildTile;

    void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		buildTile = GetComponent<Collider2D>();
	}

    void Update()
    {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            //If something was hit, the RaycastHit2D.collider will not be null.
            //Debug.Log("hit.collider.tag = " + hit.collider.tag);
            if (hit.collider.tag == "BuildSite")
            {
                // hit.collider.tag = "BuildSiteFull";
                buildTile = hit.collider;
                Debug.Log("buildTile.name = " + buildTile.name);
                buildTile.tag = "BuildSiteFull" + towerBtnPressed.TowerPrice;
                RegisterBuildSite(buildTile);
                placeTower(hit);
            } else if (hit.collider.tag.Contains("BuildSiteFull"))
            {
                // might be an attempt to upgrade a tower!
                buildTile = hit.collider;
                int oldTowerPrice = int.Parse(buildTile.tag.Substring(13));
                Debug.Log("oldTowerPrice = " + oldTowerPrice);
                if (towerBtnPressed.TowerPrice > oldTowerPrice)
                {
                    Debug.Log("user attempts to upgrade a tower!");
                    for (int i = 0; i< TowerList.Count; i++)
                    {
                        GameObject currentTower = TowerList[i];
                        if (currentTower.transform.position == hit.transform.position)
                        {
                            Destroy(currentTower);
                            TowerList.RemoveAt(i);
                            Debug.Log("removing a tower");
                        }
                    }
                    buildTile.tag = "BuildSiteFull" + towerBtnPressed.TowerPrice;
                    RegisterBuildSite(buildTile);
                    placeTower(hit);
                }
                
                Rigidbody2D body = buildTile.attachedRigidbody;
                //Debug.Log("body = " + body);
            }
        }
        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag) {
        // site.collider.tag = "BuildSiteFull";
        Debug.Log("RegisterBuildSite"); 

        BuildList.Add(buildTag);
	}

	public void RenameTagsBuildSites() {
		foreach (Collider2D buildTag in BuildList) {
			buildTag.tag = "BuildSite";
		}
		BuildList.Clear();
	}

	public void RegisterTower(GameObject tower) {
  		TowerList.Add(tower);
	}

	public void DestroyAllTowers() {
        Debug.Log("DestroyAllTowers, TowerList = " + TowerList);

        foreach (GameObject tower in TowerList) {
			Destroy(tower.gameObject);
		}
		TowerList.Clear();
	}

  	public void placeTower(RaycastHit2D hit) {
        //Debug.Log("placeTower");
	  if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null) {
            //Debug.Log("inside placeTower if");
            GameObject newTower = Instantiate (towerBtnPressed.TowerObject);
			newTower.transform.position = hit.transform.position;
			RegisterTower(newTower);
			buyTower(towerBtnPressed.TowerPrice);
            GameManaging.Instance.AudioSource.PlayOneShot(SoundManager.Instance.BuildTower);
			disableDragSprite();
	  }
  	}

	public void selectedTower(TowerBtn towerBtn) {
		if (towerBtn.TowerPrice <= GameManaging.Instance.TotalMoney){
			towerBtnPressed = towerBtn;
			enableDragSprite(towerBtn.DragSprite);
		}
	}

	public void buyTower(int price) {
        GameManaging.Instance.subtractMoney(price);
	}

	private void followMouse() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector2(transform.position.x, transform.position.y);
	}

	public void enableDragSprite(Sprite sprite) {
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;
	}

	public void disableDragSprite() {
		spriteRenderer.enabled = false;
		towerBtnPressed = null;
	}
}
