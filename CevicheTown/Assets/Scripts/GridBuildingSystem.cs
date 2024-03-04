using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GridBuildingSystem : MonoBehaviour
{
    //necesito una instancia de este script para acceder a sus métodos
    public static GridBuildingSystem instance;
    //ambos tilemaps
    public GridLayout gridLayout;
    public Tilemap temptilemap;
    public Tilemap maintilemap;
    //Diccionario de tipos de tiles, tenemos un tipo y una base(Sprite)
    private static Dictionary<tileTypes, TileBase> tileBases = new Dictionary<tileTypes, TileBase>();
    //Un Building temporal junto con su posicion anterior y su area anterior
    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;
    //Los sprites de las tilebases
    [SerializeField] TileBase available;
    [SerializeField] TileBase Right;
    [SerializeField] TileBase Wrong;
    //Materiales para la transparencia y color
    [SerializeField]
    Material Transparency;
    [SerializeField]
    Material TransparencyRight;
    [SerializeField]
    Material TransparencyWrong;
    //Boton de la tienda
    [SerializeField]
    UnityEngine.UI.Button boton;
    // Objeto de previsualización del edificio
    public GameObject previewBuilding;
    public bool isPlacing = false;
    [SerializeField]
    public BoundsInt Currentrange;
    [SerializeField]
    public List<Building> placedBuildings;
    [SerializeField]
    public List<ResourceScript> Enviroment;
    BoundsInt range;
    #region Unity Method
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        placedBuildings = new List<Building>();
        tileBases.Add(tileTypes.Empty, null);
        tileBases.Add(tileTypes.White, available);
        tileBases.Add(tileTypes.Green, Right);
        tileBases.Add(tileTypes.Red, Wrong);
        
        // Crear el objeto de previsualización del edificio
        if (temp) { 
            previewBuilding = Instantiate(temp.gameObject);
            previewBuilding.SetActive(false); // Ocultar el objeto de previsualización inicialmente
            
        }
        //Crear una copia del mainTilemap
        Instantiate(maintilemap, maintilemap.transform);
    }

    private void Update()
    {
        if(!tileBases.ContainsKey(tileTypes.Accepted) && temp) 
        {
            tileBases.Add(tileTypes.Accepted, temp.AcceptedTile);
        } else if (tileBases.ContainsKey(tileTypes.Accepted))
        {
            tileBases[tileTypes.Accepted] = temp.AcceptedTile;
        }
        if (temp)
        {
            CalculateRange();
            UpdatePreviewBuilding(); // Actualizar la previsualización del edificio si existe
        }
        if (Input.GetMouseButtonDown(0))//SI da click
        {
            
            if (EventSystem.current.IsPointerOverGameObject(0))//Si ese click fue sobre algun gameobject
            {
                return;
            }
            if (temp)//Si existe el objeto temporal
            {
                if (!temp.Placed)//SI ese objeto en cuestion aun no está colocado
                {
                    //Obtenemos la posicion del click
                    Vector2 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //Convertimos la posicion del click a posicion en celdas
                    Vector3 cellPos = gridLayout.LocalToCell(touchpos);
                    //Si dio click en una celda que no sea parte de la anterior
                    if (prevPos != cellPos)
                    {
                        //A partir de aqui pidanme que les explique yo pq escrito noc
                        //Solo esto que pues, lo interpola ya que si no quedaria desfasado
                        temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                        prevPos = cellPos;
                        FollowBuilding();
                        if (!temp.CanBeplaced())
                        {
                            temp.GetComponentInChildren<SpriteRenderer>().material = TransparencyWrong;
                        }
                        if (temp.CanBeplaced())
                        {
                            //Movemos nuestro edificio temporal a la grid
                            temp.Place();
                            temp.currRange = range;
                            Destroy(previewBuilding);
                            temp.SetSortingOrder();
                            temp = null;
                            boton.interactable = true;
                            ClearArea();
                            isPlacing = false;
                            
                        }
                        else
                        {
                            ClearArea();
                            ResetRangeTiles(Currentrange);
                            Destroy(temp.gameObject);
                            Destroy(previewBuilding);
                            temp = null;
                            boton.interactable = true;
                            ClearArea();
                        }
                    }
                }
            } else
            {
                isPlacing = false;
            }
        }
    }
    #endregion

    #region Tilemap Management
    //Obtenemos los tiles de un tilemap segun el area que le pasemos para despues pintarlos o hacer lo que sea con ellos
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            //Obtenemos la posicion en el area y despues llenamos el arreglo con la tile en cada posicion
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }
    //Cambia los tiles de un área a otro especificandole que área, que tipo de tile y que tilemap vamos a modificar
    public static void SetTilesBlock(BoundsInt area, tileTypes type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }
    //Llena las tiles de una base (un arreglo de tiles) de un color especifico
    private static void FillTiles(TileBase[] arr, tileTypes type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }
    #endregion

    #region Building placement

    //Es la funcion que usan los prefabs en la tienda que son botones.
    //Hace una instancia del prefab que se le da y se lo pasa a preview building, se pone transparente y empieza la previsualizacion
    public void InitializeWithBuilding(GameObject building)
    {
        
        isPlacing = true;
        temp = Instantiate(building, new Vector3(10000,10000,10000 ), Quaternion.identity).GetComponent<Building>();
        previewBuilding = Instantiate(temp.gameObject);
        SpriteRenderer spriteRenderer = previewBuilding.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.material = Transparency;
        previewBuilding.SetActive(true); // Mostrar el objeto de previsualización del edificio
        FollowBuilding(); // Actualizar la previsualización inicial
        boton.interactable = false;
    }
    public void InitializeWithBuildingR(GameObject building)
    {
        if (isPlacing)
        {
            temp = Instantiate(building, new Vector3(10000, 10000, 10000), Quaternion.identity).GetComponent<Building>();
            previewBuilding = Instantiate(temp.gameObject);
            SpriteRenderer spriteRenderer = previewBuilding.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = Transparency;
            previewBuilding.SetActive(true); // Mostrar el objeto de previsualización del edificio
            FollowBuilding(); // Actualizar la previsualización inicial
            boton.interactable = false;
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, tileTypes.Empty);
        temptilemap.SetTilesBlock(prevArea, toClear);

    }

    //Basicament checamos las tiles debajo del edificio y las ponemos verdes o rojas si está correcto o no, respectivamente
    private void FollowBuilding()
    {
        //Limpiamos el area (Las casillas donde antes estaba el edificio
        ClearArea();
        //Tomamos la posicion del objeto que acabamos de mover y sacamos el area (Este método se llama en update)
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;
        
        //Obtenemos un arreglo lleno de las tiles de la base
        TileBase[] BaseArray = GetTilesBlock(buildingArea, maintilemap);
        int size = BaseArray.Length;
        //Recorremos las tiles una por una y vamos checando si todas están disponibles
        TileBase[] tileArray = new TileBase[size];
        for (int i = 0; i < BaseArray.Length; i++)
        {
            if (BaseArray[i] == temp.AcceptedTile)
            {
                tileArray[i] = tileBases[tileTypes.Green];
            }
            else
            {
                FillTiles(tileArray, tileTypes.Red);
                break;
            }
        }
        
        //Ponemos las tiles del temp tilemap rojas o verdes respectivamente para una previsualizacion
        temptilemap.SetTilesBlock(buildingArea, tileArray);
        //Sabemos ahora que el area donde construiriamos tambien es la anterior para sobreescribirla
        prevArea = buildingArea;
    }
    //Aqui deberia llamarse CanTakeArea pero se me fue una t
    public bool CantTakeArea(BoundsInt area)
    {
        //Obtenemos el area en el mainTilemap, suponemos que ya le dimos el area que queremos llenar
        TileBase[] baseArray = GetTilesBlock(area, maintilemap);
        //Recorremos las tiles
        foreach (var b in baseArray)
        {
            if (b != temp.AcceptedTile)
            {
                UnityEngine.Debug.Log("Cant place here");
                return false;
            }
        }
        //Si las terminamos de recorrer y si se pudo entonces regresamos true
        return true;
    }
    //Pos con el area que le diste llenar los tilemaps
    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, tileTypes.Empty, temptilemap);
        SetTilesBlock(area, tileTypes.Green, maintilemap);
    }
    #endregion
    //Necesito este bool para chear si el edificio transparente se puede colocar
    public bool canplace;
    private void UpdatePreviewBuilding()
    {
        ClearArea();
        // Actualizar la posición del objeto de previsualización del edificio
        Vector2 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cellPos = gridLayout.LocalToCell(touchpos);
        previewBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
        BoundsInt buildingArea = new BoundsInt(gridLayout.WorldToCell(previewBuilding.transform.position), temp.area.size);
        TileBase[] BaseArray = GetTilesBlock(buildingArea, maintilemap);
        //Aqui sigo la misma logica que en FollowBuilding
        int size = BaseArray.Length;

        TileBase[] tileArray = new TileBase[size];
        canplace = false;
        for (int i = 0; i < BaseArray.Length; i++)
        {
            if (BaseArray[i] == temp.AcceptedTile)
            {
                tileArray[i] = tileBases[tileTypes.Green];
                canplace = true;
            }
            else
            {
                FillTiles(tileArray, tileTypes.Red);
                canplace = false;
                break;
            }
        }
        //El rango es rango veces el tamaño
        range = prevArea;
        range.xMin -= previewBuilding.GetComponent<Building>().Range;
        range.yMin -= previewBuilding.GetComponent<Building>().Range;
        range.xMax += previewBuilding.GetComponent<Building>().Range;
        range.yMax += previewBuilding.GetComponent<Building>().Range;
        TileBase[] RangeArray = GetTilesBlock(range, maintilemap);
        ResetRangeTiles(range);
        int rangesize = RangeArray.Length;
        TileBase[] tilerangeArray = new TileBase[rangesize];
        for (int i = 0; i < RangeArray.Length; i++)
        {
            tilerangeArray[i] = tileBases[tileTypes.White];
        }
        temptilemap.SetTilesBlock(range, tilerangeArray);
        if (prevArea != buildingArea)
        {
            UnityEngine.Debug.Log("Changed mousepos");
            ResetRangeTiles(range);
        }
        //Checo si se puede poner el edificio temporal y si sí entonces le pongo color verde en este caso el material
        //Si no le pongo material rojo
        if (canplace)
        {
            SpriteRenderer spriteRenderer = previewBuilding.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = TransparencyRight;
        } else
        {
            SpriteRenderer spriteRenderer = previewBuilding.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = TransparencyWrong;
        }
        //Hago con el temp pues lo mismo con el de follow pero como este está en update pues se van actualizando las tiles
        temptilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
        previewBuilding.GetComponent<Building>().SetSortingOrder();

    }
    private void CalculateRange()
    {
        BoundsInt buildingArea = new BoundsInt(gridLayout.WorldToCell(temp.transform.position), temp.area.size);
        Currentrange = buildingArea;
        Currentrange.xMin -= previewBuilding.GetComponent<Building>().Range;
        Currentrange.yMin -= previewBuilding.GetComponent<Building>().Range;
        Currentrange.xMax += previewBuilding.GetComponent<Building>().Range;
        Currentrange.yMax += previewBuilding.GetComponent<Building>().Range;
    }
    private void ResetRangeTiles(BoundsInt range)
    {
        // Crea un arreglo de tiles vacío del tamaño del área
        TileBase[] emptyTiles = new TileBase[range.size.x * range.size.y * range.size.z];

        // Llena el arreglo con tiles del tipo Empty
        FillTiles(emptyTiles, tileTypes.Empty);

        // Establece los tiles en el área especificada como tiles vacíos
        temptilemap.SetTilesBlock(range, emptyTiles);
    }
}

public enum tileTypes
{
    Empty,
    White,
    Green,
    Red,
    Accepted
}
