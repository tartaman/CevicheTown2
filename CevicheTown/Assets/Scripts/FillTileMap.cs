using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FillTileMap : MonoBehaviour
{
    Tilemap tilemap;
    public TileBase[] tiles; // Array de tiles que deseas asignar
    public int width = 40;
    public int height = 40;
    private void Awake()
    {
        tilemap = GridBuildingSystem.instance.maintilemap;
        // Iterar sobre todas las posiciones dentro del rango
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Obtener la posición en forma de vector
                Vector3Int tilePosition = new Vector3Int(x, y, 0); // Asegúrate de ajustar el valor z según la capa de tu Tilemap

                // Asignar un tile aleatorio del array de tiles
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(tilePosition, randomTile);
            }
        }
    }

}
