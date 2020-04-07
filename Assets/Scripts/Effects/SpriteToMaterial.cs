using UnityEngine;

public class SpriteToMaterial : MonoBehaviour
{
    // ======================================================================================================= Variables
    public SpriteRenderer _sprite;
    public Material _mat;
    
    // =========================================================================================================== Start
    void Start()
    {
        _mat = _sprite.material;
    }

    // ========================================================================================================== Update
    void Update()
    {
        if(_sprite.sprite==null) return;
        print("changing");
        Texture tex = _sprite.sprite.texture;
        _mat.SetTexture("_BaseMap", tex);
        _mat.SetTexture("_EmissionMap",  tex);
    }
}
