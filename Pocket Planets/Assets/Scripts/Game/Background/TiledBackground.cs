//using UnityEngine;

//public class TiledBackground : MonoBehaviour
//{
//    [SerializeField] Sprite backgroundSprite;

//    private float textureWidth;
//    private float textureHeight;

//    private Vector3 scaleHelper = new Vector3(1.0f, 1.0f, 1.0f);

//    private float prevWidth = 0.0f;
//    private float prevHeight = 0.0f;

//    private void Start()
//    {
//        textureWidth = backgroundSprite.textureRect.width;
//        textureHeight = backgroundSprite.textureRect.height;

//        //SetDimensions();
//        UpdateDimensions();
//    }

//    //private void Update()
//    //{
//    //if (prevWidth != Screen.width || prevHeight != Screen.height)
//    //{
//    //    UpdateDimensions();
//    //}
//    //}

//    private void OnEnable()
//    {
//        Managers.EventManager.OnPanCamera += PanBackground;
//    }

//    private void OnDisable()
//    {
//        Managers.EventManager.OnPanCamera -= PanBackground;
//    }

//    private void UpdateDimensions()
//    {
//        var newWidth = Mathf.Ceil(Screen.width / (textureWidth * Managers.DisplayManager.Instance.Scale));
//        var newHeight = Mathf.Ceil(Screen.height / (textureHeight * Managers.DisplayManager.Instance.Scale));

//        scaleHelper.x = newWidth;
//        scaleHelper.y = newHeight;
//        GetComponent<Renderer>().material.mainTextureScale = scaleHelper;

//        scaleHelper.x *= textureWidth;
//        scaleHelper.y *= textureHeight;
//        transform.localScale = scaleHelper;
//    }

//    private void PanBackground(Vector3 dragDistance)
//    {
//        Vector2 pan = Vector2.zero;
//        pan.x = dragDistance.x;
//        pan.y = dragDistance.y;
//        Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
//        GetComponent<Renderer>().material.mainTextureOffset = offset + pan;
//    }

//    //private void SetDimensions()
//    //{
//    //    var newWidth = Mathf.Ceil(Managers.WorldBoundsManager.Instance.HorizontalRadius * 2.0f / textureWidth);
//    //    var newHeight = Mathf.Ceil(Managers.WorldBoundsManager.Instance.VerticalRadius * 2.0f / textureHeight);

//    //    scaleHelper.x = newWidth;
//    //    scaleHelper.y = newHeight;
//    //    GetComponent<Renderer>().material.mainTextureScale = scaleHelper;

//    //    scaleHelper.x *= textureWidth;
//    //    scaleHelper.y *= textureHeight;
//    //    transform.localScale = scaleHelper;
//    //}
//}
