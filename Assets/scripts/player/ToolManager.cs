
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class ToolManager : MonoBehaviour
{
    // Values set in inspector
    public GameObject toolToPlace;
    
    // -- audio --
    public AudioSource scrubAudio;
    [SerializeField] private AudioClip scrubFwd;
    [SerializeField] private AudioClip scrubBwd;

    public GameObject gunkRadar;
    public Transform radarActiveTransform;
    public Transform radarRestingTransform;
    private bool animateRadar = false;
    private Transform radarTarget;

    public Camera gameCamera;

    public float reach = 4.0f;
    public float rotationSpeed = 20.0f; 
    
    // gunk stuff
    public Shader drawShader;
    private Material _drawMaterial;
    private RenderTexture _activeSplatmap;
    //public Texture drawTexture;
    
    //public GunkManager gunkManager;
    private GunkSurface _activeGunkSurface;

    [Range(1, 500)]
    public float brushSize;
    [Range(0, 1f)]
    public float brushStrength;

    // the origin position, by the players hand
    public Transform restingPosition;

    public scr_fpscontroller charController;

    // ---- Values for internal use ----

    private Quaternion _lookRotation;
    private Vector3 _fwd;
    private LayerMask hittable_layer;
    
    
    // -- audio stuff --
    public bool isScrubbing = false;
    private bool playSound = false;
    private float randomPitch;

    private Vector2 oldMouseAxis;
    
    
    RaycastHit hitInfo;
    private static readonly int Coordinate = Shader.PropertyToID("_Coordinate");
    private static readonly int Strength = Shader.PropertyToID("_Strength");
    private static readonly int Size = Shader.PropertyToID("_Size");

    [SerializeField]
    private GunkTrackCounter gunkCounter;

    public ParticleSystem scrubParticles;

    [SerializeField]
    private ParticleSystem foamParticles;


    // Start is called before the first frame update
    void Start()
    {
        //gunkRadar.SetActive(false);
        //ToggleRadarVisibility();
        radarTarget = radarRestingTransform;
        

        // -- gunk initialazitanion
      
        _drawMaterial = new Material(drawShader);
        
        

        _activeSplatmap = FindObjectOfType<GunkSurface>().splatmap;
        // //_drawMaterial.SetVector(_Color, Color.red);
        //
        // _baseMaterial = scrubSurface.GetComponent<MeshRenderer>().material;
        //
        // randomGunk(_splatmap);
        // _baseMaterial.SetTexture(Splat, _splatmap);
        
        // environ == floors, ground, etc -- not dirt
        var environ_layer = 6;
        // gunk == stuff you clean up with a tool
        var gunk_layer = 7;

        var environ_layermask = 1 << environ_layer;
        var gunk_layermask = 1 << gunk_layer;

        var final_layermask = environ_layermask | gunk_layermask;
        hittable_layer = final_layermask;
    }

    void ResetToolPosition()
    {
        toolToPlace.transform.position = restingPosition.position;
        toolToPlace.transform.rotation = restingPosition.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        isScrubbing = Math.Abs(Mathf.Sign(mouseAxis.x) - Mathf.Sign(oldMouseAxis.x)) > 0.1 ||
                      Math.Abs(Mathf.Sign(mouseAxis.y) - Mathf.Sign(oldMouseAxis.y)) > 0.1; 
        //new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //isScrubbing = Mathf.Sign(mouseAxis.x) != Mathf.Sign(oldMouseAxis.x) ||
        //              Mathf.Sign(mouseAxis.y) != Mathf.Sign(oldMouseAxis.y);
        
        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            
         //   Debug.Log("x: " + mouseAxis.x);
         //   Debug.Log("y: " + mouseAxis.y);
            
            
            //gameCamera.GetComponentInParent<scr_fpscontroller>().canMove = false;
            charController.canMove = false;

            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);

            _fwd = gameCamera.transform.forward;
            
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            if (Physics.Raycast(ray, out hitInfo, reach, hittable_layer))
            {
                if (isScrubbing)
                {
                    scrubParticles.Play();
                    foamParticles.Play();
                    
                    if (playSound == false)
                    {

                        scrubAudio.clip = scrubAudio.clip == scrubFwd ? scrubBwd : scrubFwd;
                        StartCoroutine("playScrubSound");
                    }
                }
                
                
                //lastMouseCoordinate = Input.mousePosition;
                
                toolToPlace.transform.position = hitInfo.point;
                
                var proj = _fwd - (Vector3.Dot(_fwd, hitInfo.normal)) * hitInfo.normal;

                _lookRotation = Quaternion.LookRotation(-proj, -hitInfo.normal);

                var _curRotation = toolToPlace.transform.rotation;

                toolToPlace.transform.rotation = Quaternion.Slerp(_curRotation, _lookRotation, Time.deltaTime * rotationSpeed);
                
                var gunkySurface = GunkManager.GetGunkSurface(hitInfo.transform);

                if (gunkySurface)
                {
                    _activeGunkSurface = hitInfo.transform.GetComponent<GunkSurface>();;
                    _activeSplatmap = _activeGunkSurface.splatmap;
                    gunkCounter.SetSurface(_activeGunkSurface);
                }
                else
                {
                    goto NoGunk;
                }
                
                
                _drawMaterial.SetVector(Coordinate, new Vector4(hitInfo.textureCoord.x, hitInfo.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat(Strength, brushStrength);
                _drawMaterial.SetFloat(Size, brushSize*50/(_activeSplatmap.width*_activeGunkSurface.splatScale));
               // _drawMaterial.SetTexture("_MainTex", drawTexture);
                
                RenderTexture temp = RenderTexture.GetTemporary(_activeSplatmap.width, _activeSplatmap.height, 0, RenderTextureFormat.ARGBFloat);
                
                Graphics.Blit(_activeSplatmap, temp);
                Graphics.Blit(temp, _activeSplatmap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
                // if (hitInfo.transform.CompareTag(gunkTag) && Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Mouse Y") != 0)
                // {
                //     hitInfo.transform.localScale -= cleanRate;
                //     
                //     if (hitInfo.transform.localScale.sqrMagnitude < smallEnough) 
                //     {
                //         Destroy(hitInfo.transform.gameObject);
                //     }
                // }
                NoGunk: ;
            }
            else
            {
                toolToPlace.transform.position = ray.GetPoint(reach);
                scrubParticles.Stop();
            }
            
        } else if (Input.GetMouseButtonUp(0))
        {
            ResetToolPosition();
            charController.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            scrubParticles.Stop();
            foamParticles.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _activeGunkSurface.GunkItUp();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {

            ToggleRadar();
            animateRadar = true;
            //toolToPlace.gameObject.SetActive(!toolToPlace.transform.gameObject.activeSelf);
        }
        
        oldMouseAxis = mouseAxis;

        if (!animateRadar) return;
        gunkRadar.transform.position = Vector3.Slerp(gunkRadar.transform.position,
            radarTarget.position, Time.deltaTime * rotationSpeed);
        gunkRadar.transform.localScale = Vector3.Slerp(gunkRadar.transform.localScale,
            radarTarget.localScale, Time.deltaTime * rotationSpeed);
        if (gunkRadar.transform.position == radarTarget.transform.position)
        {
            animateRadar = false;
        }

        
    }

    private IEnumerator playScrubSound()
    {
        playSound = true;
        randomPitch = Random.Range(0.9f, 1.3f);
        scrubAudio.pitch = randomPitch;
        
        scrubAudio.PlayOneShot(scrubAudio.clip);
        yield return new WaitForSeconds(scrubAudio.clip.length - 0.02f);
        playSound = false;
    }

    void ToggleRadar()
    {
        if (radarTarget == radarActiveTransform)
        {
            radarTarget = radarRestingTransform;
        }
        else if (radarTarget == radarRestingTransform)
        {
            radarTarget = radarActiveTransform;
        }
    }

    public void ToggleRadarVisibility()
    {
        foreach (MeshRenderer rendr in gunkRadar.GetComponentsInChildren<MeshRenderer>())
        {
            rendr.enabled = !rendr.enabled;
        }
    }
}
