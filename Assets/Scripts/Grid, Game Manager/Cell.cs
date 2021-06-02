using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Material goalMaterial;
    [SerializeField] private Material slideMaterial;
    [SerializeField] private Material freeMaterial;
    [SerializeField] private Material staticMaterial;
    [SerializeField] private Material bounceMaterial;
    [SerializeField] private Material teleportMaterial;
    public CellProperty property;
    private int materialIndex;
    private Renderer render;

    [HideInInspector] public Cell twinCell;

    [SerializeField] private LocalEvent onCellFlick;


    private void Awake()
    {
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        SetProperty(property);
    }

    public void SetProperty(CellProperty newProp)
    {
        switch (newProp)
        {
            case CellProperty.Static:
                render.material = staticMaterial;
                break;
            case CellProperty.Bounce:
                render.material = bounceMaterial;
                break;
            case CellProperty.Free:
                render.material = freeMaterial;
                break;
            case CellProperty.Slide:
                render.material = slideMaterial;
                break;
            case CellProperty.Goal:
                render.material = goalMaterial;
                break;
            case CellProperty.Teleport:
                render.material = teleportMaterial;
                break;
            default:
                break;
        }
        property = newProp;
    }

    public void SwitchProperty(Vector3 rotation)
    {
        if (rotation != Vector3.zero && (property == CellProperty.Slide|| property == CellProperty.Free))
        {
            Vector3 newRot = rotation.x != 0 ? new Vector3(0, 0, rotation.x) : new Vector3(rotation.z, 0, 0);
            LeanTween.rotateAround(gameObject, newRot, 180, 0.5f)
                .setEaseOutBack()
                .setOnComplete(() => {
                    onCellFlick.Raise(transform.position + Vector3.up) ;

                    if (property == CellProperty.Free)
                    {
                        SetProperty(CellProperty.Slide);
                    }
                    else if (property == CellProperty.Slide)
                    {
                        SetProperty(CellProperty.Free);
                    }
                    else
                    {
                        return;
                    }
                });
        }
        else
        {
            return;
        }
    }

    public void Bounce()
    {
        LeanTween.scale(gameObject, new Vector3(4, 1, 4), 0.5f).setEasePunch();
    }

    public void SetTwinCells(Cell twin)
    {
        twinCell = twin;
        twin.twinCell = this;
    }
}

public enum CellProperty
{
    Free, Slide, Goal, Static, Bounce, Teleport
}
