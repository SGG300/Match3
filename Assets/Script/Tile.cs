using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileTypeEnum { Ball, Weight, Battery, Coach, Bussinesman, Shoes }

public class Tile : MonoBehaviour {

    private TileTypeEnum tyleType = TileTypeEnum.Ball;
    public TileTypeEnum TyleType
    {
        get
        {
            return tyleType;
        }
        set
        {
            tyleType = value;
            UpdateImage();
        }
    }

    public bool free = true;

    public SpriteRenderer tileImage;
    public List<Sprite> tileTypeSprites;

	void Start ()
    {
        tileImage = GetComponent<SpriteRenderer>();
        UpdateImage();
    }

    public void UpdateImage()
    {
        if(tileImage == null)
        {
            return;
        }
        tileImage.sprite = tileTypeSprites[(int)TyleType];
    }


}
