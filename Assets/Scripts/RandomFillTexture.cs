using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

// might come back to this later - currently not using.  
public class RandomFillTexture : MonoBehaviour
{
    public List<SpriteShape> _spriteShapes;
    private SpriteShapeController _spriteShapeController;

    void Start()
    {
        _spriteShapeController = GetComponent<SpriteShapeController>();

        if (_spriteShapes.Count > 0)
        {
            int randomIndex = Random.Range(0, _spriteShapes.Count);
            _spriteShapeController.spriteShape = _spriteShapes[randomIndex];
        }
    }
}
