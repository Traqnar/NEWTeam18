using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static Grid;

public class Grid : MonoBehaviour
{
    public enum PieceType
    {
        EMPTY,
        NORMAL,
        COUNT,
    };
    [System.Serializable ]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    public int xDimension;
    public int yDimension;
    public float fillTime;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;

    private Dictionary<PieceType, GameObject> piecePrefabDict;
    private GamePiece[,] pieces;
    private GamePiece pressedPiece;
    private GamePiece enteredPiece;

    // Start is called before the first frame update
    void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type)) {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        for (int x = 0; x < xDimension; x++) {
            for (int y = 0; y < yDimension; y++)
            {
                GameObject background = Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }
        pieces = new GamePiece[xDimension, yDimension];

        for (int x = 0; x < xDimension; x++) {
            for (int y = 0; y < yDimension; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY);              
            }
        }
        StartCoroutine(Fill());
    }

    void Update()
    {

    }

    public IEnumerator Fill()
    {
        while (FillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }
    }
    public bool FillStep()
    {
        bool movedPiece = false;
        for (int y = yDimension - 2; y >= 0 ; y--)
        {
            for (int x = 0; x < xDimension; x++)
            {
                GamePiece piece = pieces[x, y];

                if (piece.IsMoveable())
                {
                    GamePiece pieceBelow = pieces[x, y + 1];
                    if(pieceBelow.Type == PieceType.EMPTY)
                    {
                        piece.MoveableComponent.Move(x, y + 1, fillTime);
                        pieces[x,y + 1] = piece;
                        SpawnNewPiece(x,y,PieceType.EMPTY);
                         
                        movedPiece= true;
                    }
                }

            }
        }
        for (int x = 0; x < xDimension; x++)
        {
            GamePiece pieceBelow = pieces[x, 0];
            if (pieceBelow.Type == PieceType.EMPTY)
            {
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL],GetWorldPosition(x,-1),Quaternion.identity);
                newPiece.transform.parent = transform;
                pieces[x,0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MoveableComponent.Move(x, 0, fillTime);
                pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors+1));
                movedPiece= true;

            }
        }
        return movedPiece;

    }


    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDimension / 2.0f + x, transform.position.y + yDimension / 2.0f - y);
    } 

    public GamePiece SpawnNewPiece(int x, int y , PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type],GetWorldPosition(x,y),Quaternion.identity);
        newPiece.transform.parent = transform;
        pieces[x,y] = newPiece.GetComponent<GamePiece>();
        pieces[x,y].Init(x, y,this,type);
         
        return pieces[x,y];
    }    
    public bool IsAdjacent(GamePiece piece1, GamePiece piece2)
    {
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1 || piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
       
    }
    public void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if(piece1.IsMoveable() && piece2.IsMoveable())
        {
            pieces[piece1.X, piece1.Y] = piece2;
            pieces[piece2.X, piece1.Y] = piece1;

            if (GetMatch(piece1, piece2.X, piece2.Y) != null || GetMatch(piece2, piece1.X, piece1.Y) != null)
            {
                int piece1X = piece1.X;
                int piece1Y = piece1.Y;
                
                piece1.MoveableComponent.Move(piece2.X, piece2.Y, fillTime);
                piece2.MoveableComponent.Move(piece1X, piece1Y, fillTime);

                ClearAllValidMatchs();
            }
            else{ 
                pieces[piece1.X, piece1.Y] = piece1;
                pieces[piece2.X,piece2.Y] = piece2;
            }
        } 
    }
    public void PressPiece(GamePiece piece)
    {
        pressedPiece = piece;
    }
    public void EnterPiece(GamePiece piece)
    {
        enteredPiece = piece;
    }
    public void ReleasePiece()
    {
        if (IsAdjacent(pressedPiece, enteredPiece))
        {
            SwapPieces(pressedPiece, enteredPiece);
        }
    }
    public List<GamePiece> GetMatch(GamePiece piece,int newX,int newY)
    {
        if(piece.IsColored())
        {
            ColorPiece.ColorType color = piece.ColorComponent.Color;
            List<GamePiece> horizontalPieces = new List<GamePiece>();
            List<GamePiece> verticalPieces = new List<GamePiece>();
            List<GamePiece> matchingPieces = new List<GamePiece>();
        
            //FIRST CHECK HORIZONTAL
            horizontalPieces.Add(piece);
            for (int dir = 0; dir < xDimension; dir++)
            {
                for (int xOffset = 0; xOffset < xDimension; xOffset++)
                {
                    int x;
                    if(dir == 0){
                        x = newX - xOffset;
                    }
                    else{
                        x = newX + xOffset;
                    }
                    if(x < 0 || x >= xDimension){
                        break;
                    }

                    if (pieces[x, newY].IsColored() && pieces[x, newY].ColorComponent.Color == color)
                    {
                        horizontalPieces.Add(pieces[x,newY]);
                    }
                    else{
                        break;
                    }
                }
            }

            if(horizontalPieces.Count >= 3){
                for (int i = 0; i < horizontalPieces.Count; i++)
                {
                    matchingPieces.Add(horizontalPieces[i]);
                }
            }

            //TRAVERSE VERTICALLY IF WE FOUND A MATCH
            if (horizontalPieces.Count >= 3){
                for (int i = 0; i < horizontalPieces.Count; i++){
                    for (int dir = 0; dir <=1; dir++){
                        for (int yOffset = 0; yOffset < yDimension; yOffset++){
                            int y;

                            if(dir == 0){
                                y = newY - yOffset;
                            }else { 
                                y= newY + yOffset;
                            }
                            if(y< 0 || y >= yDimension){
                                break;
                            }

                            if (pieces[horizontalPieces[i].X,y].IsColored() && pieces[horizontalPieces[i].X,y].ColorComponent.Color == color){
                                verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
                            }else {
                                break;
                            }
                        }
                    }

                    if(verticalPieces.Count < 2){
                        verticalPieces.Clear();
                    }
                    else
                    {
                        for(int j = 0 ; j < verticalPieces.Count; j++){
                            matchingPieces.Add(verticalPieces[j]);
                        }

                        break;
                    }
                }
            }

            if(matchingPieces.Count >= 3){
                return matchingPieces;
            }


            //DIDNT FIND ANYTHING GOING HORIZONTAL FIRST, NOW VERTICAL
            horizontalPieces.Clear();
            verticalPieces.Clear();
            verticalPieces.Add(piece);
            for (int dir = 0; dir < xDimension; dir++)
            {
                for (int yOffset = 0; yOffset < yDimension; yOffset++)
                {
                    int y;
                    if (dir == 0)
                    {
                        y = newX - yOffset;
                    }
                    else
                    {
                        y = newX + yOffset;
                    }
                    if (y < 0 || y >= yDimension)
                    {
                        break;
                    }

                    if (pieces[newX,y].IsColored() && pieces[newX,y].ColorComponent.Color == color)
                    {
                        verticalPieces.Add(pieces[newX,y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (verticalPieces.Count >= 3)
            {
                for (int i = 0; i < verticalPieces.Count; i++){
                    matchingPieces.Add(verticalPieces[i]);
                }
            }

            //TRAVERSE HORIZTONALY IF WE FOUND A MATCH
            if (verticalPieces.Count >= 3)
            {
                for (int i = 0; i < verticalPieces.Count; i++)
                {
                    for (int dir = 0; dir <= 1; dir++)
                    {
                        for (int xOffset = 0; xOffset < yDimension; xOffset++)
                        {
                            int x;

                            if (dir == 0)
                            {
                                x = newX - xOffset;
                            }
                            else
                            {
                                x = newX + xOffset;
                            }
                            if (x < 0 || x >= xDimension){
                                break;
                            }

                            if (pieces[x,verticalPieces[i].Y].IsColored() && pieces[x, verticalPieces[i].Y].ColorComponent.Color == color)
                            {
                                verticalPieces.Add(pieces[x,verticalPieces[i].Y ]);
                            }else{
                                break;
                            }
                        }
                    }

                    if (horizontalPieces.Count < 2){
                        horizontalPieces.Clear();
                    }else{
                        for (int j = 0; j < horizontalPieces.Count; j++){
                            matchingPieces.Add(horizontalPieces[j]);
                        }

                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3){
                return matchingPieces;
            }
        }

        return null;
    }  
    public bool ClearAllValidMatchs()
    {
        bool needsRefill = false;
        for (int y = 0; y < yDimension; y++)
        {
            for (int x = 0; x < xDimension; x++)
            {
                if (pieces[x, y].IsClearable())
                {
                    List<GamePiece> match = GetMatch(pieces[x, y], x, y);
                    if(match != null)
                    {
                        for (int i = 0; i < match.Count; i++)
                        {
                            if (ClearPiece(match[i].X, match[i].Y))
                            {
                                needsRefill = true;
                            }
                        }
                    }
                }
            }
        }
        return needsRefill;
    }

    public bool ClearPiece(int x,int y)
    {
        if (pieces[x,y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleared)
        {
            pieces[x,y].ClearableComponent.Clear();
            SpawnNewPiece(x, y, PieceType.EMPTY);

            return true;
        }
        return false;
    }
}

