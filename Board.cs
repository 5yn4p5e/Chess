using System;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    public class MovesEventArgs : EventArgs
    {
        public int hor, ver, horPiece, verPiece, horExtra, verExtra, horPieceExtra, verPieceExtra, army;
        public int[,] matr = new int[8, 8];
        public bool res = false, extra = false, castling = false;
        public Graphics g;
        public Piece pi, piExtra;

        public MovesEventArgs() : base() { }
    }

    public delegate void UserMove(object sender, MovesEventArgs ev);

    //public class StatusEventArgs : EventArgs
    //{
    //    public int mode, army;

    //    public StatusEventArgs() : base() { }
    //}

    //public delegate void StatusChanger(object sender, StatusEventArgs ev);

    public class NotationEventArgs : EventArgs
    {
        public string str;

        public NotationEventArgs() : base() { }
    }

    public delegate void NotationChanger(object sender, NotationEventArgs ev);

    [Serializable]

    public class Board
    {
        Cell[,] cells, cellsMatr;
        int[,] matr, matrCopy;
        [NonSerialized] MovesEventArgs movesArgs = new MovesEventArgs();
        //[NonSerialized] StatusEventArgs statArgs = new StatusEventArgs();
        public static event UserMove UserClickEvent;
        public static event UserMove GetPieceEvent;
        public static event UserMove AddPieceEvent;
        public static event UserMove EndMoveEvent;
        //public static event StatusChanger ChangeStatusEvent;
        public static event NotationChanger ChangeNotationEvent;

        public Board()
        {
            matr = new int[8, 8];
            matrCopy = new int[8, 8];
            cells = new Cell[8, 8];
            cellsMatr = new Cell[8, 8];
            for (int i = 7; i > -1; i--)
            {
                for (int j = 7; j > -1; j--)
                {
                    switch (j)
                    {
                        case 0:
                            matr[i, j] = 1;
                            matrCopy[i, j] = 1;
                            switch (i)
                            {
                                case 0:
                                    cells[i, j] = new Cell(72, 157, 60, new Rook(1));
                                    break;
                                case 1:
                                    cells[i, j] = new Cell(255, 253, 157, new Knight(1));
                                    break;
                                case 2:
                                    cells[i, j] = new Cell(72, 157, 60, new Bishop(1));
                                    break;
                                case 3:
                                    cells[i, j] = new Cell(255, 253, 157, new Queen(1));
                                    break;
                                case 4:
                                    cells[i, j] = new Cell(72, 157, 60, new King(1));
                                    break;
                                case 5:
                                    cells[i, j] = new Cell(255, 253, 157, new Bishop(1));
                                    break;
                                case 6:
                                    cells[i, j] = new Cell(72, 157, 60, new Knight(1));
                                    break;
                                case 7:
                                    cells[i, j] = new Cell(255, 253, 157, new Rook(1));
                                    break;
                            }
                            break;
                        case 1:
                            matr[i, j] = 1;
                            matrCopy[i, j] = 1;
                            if (i % 2 == 0)
                                cells[i, j] = new Cell(255, 253, 157, new Pawn(1));
                            else
                                cells[i, j] = new Cell(72, 157, 60, new Pawn(1));
                            break;
                        case 6:
                            matr[i, j] = 0;
                            matrCopy[i, j] = 0;
                            if (i % 2 != 0)
                                cells[i, j] = new Cell(255, 253, 157, new Pawn(0));
                            else
                                cells[i, j] = new Cell(72, 157, 60, new Pawn(0));
                            break;
                        case 7:
                            matr[i, j] = 0;
                            matrCopy[i, j] = 0;
                            switch (i)
                            {
                                case 0:
                                    cells[i, j] = new Cell(255, 253, 157, new Rook(0));
                                    break;
                                case 1:
                                    cells[i, j] = new Cell(72, 157, 60, new Knight(0));
                                    break;
                                case 2:
                                    cells[i, j] = new Cell(255, 253, 157, new Bishop(0));
                                    break;
                                case 3:
                                    cells[i, j] = new Cell(72, 157, 60, new Queen(0));
                                    break;
                                case 4:
                                    cells[i, j] = new Cell(255, 253, 157, new King(0));
                                    break;
                                case 5:
                                    cells[i, j] = new Cell(72, 157, 60, new Bishop(0));
                                    break;
                                case 6:
                                    cells[i, j] = new Cell(255, 253, 157, new Knight(0));
                                    break;
                                case 7:
                                    cells[i, j] = new Cell(72, 157, 60, new Rook(0));
                                    break;
                            }
                            break;
                        default:
                            matr[i, j] = -1;
                            matrCopy[i, j] = -1;
                            if ((i + j) % 2 == 0)
                                cells[i, j] = new Cell(72, 157, 60);
                            else
                                cells[i, j] = new Cell(255, 253, 157);
                            break;
                    }
                    cells[i, j].SetXY(x + margin + i * Cell.side, 625 - j * Cell.side);
                    cells[i, j].SetHorVer(i, j);
                }
            }
        }

        public const int x = 70, y = 50, side = 700, margin = 50;
        int horPiece, verPiece;
        //переключатели: игрок делает ход в два этапа; фигуры разных цветов ходят поочерёдно, хочет игрок переворачивать доску или нет
        bool selectOrMove = false, flipBoard = false, promotion = false;
        int blackOrWhite = 1, movesCounter = 0, promHor = -1;

        int kingHor = 0, kingVer = 0;
        int lastMoveHor, lastMoveVer;
        int[,] matrForCheck = new int[8, 8], matrForNoMoves = new int[8, 8];
        bool castling = true;
        Bitmap pic = new Bitmap(@"..\..\assets\board.png");
        string notation;

        public void Draw(Graphics g)
        {
            g.DrawImage(pic, x, y);
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                    cells[i, j].Draw(g);
        }

        public void Delete()
        {
            
        }

        void Flip(int a, int b)
        {
            pic.RotateFlip((RotateFlipType)2);
            for (int i = a - 1; i > -1; i--)
                for (int j = b - 1; j > -1; j--)
                {
                    if (blackOrWhite == 0)
                    {
                        cells[i, j].SetXY(x + margin + (7 - i) * Cell.side, 625 - (7 - j) * Cell.side);
                    }
                    else
                    {
                        cells[i, j].SetXY(x + margin + i * Cell.side, 625 - j * Cell.side);
                    }

                }
        }

        void GetCoord(ref int mouseX, ref int mouseY)
        {
            if (blackOrWhite == 0 && flipBoard)
            {
                mouseX = 7 - (mouseX - x - margin) / 75;
                mouseY = (mouseY - y - margin) / 75;
            }
            else
            {
                mouseX = (mouseX - x - margin) / 75;
                mouseY = 7 - (mouseY - y - margin) / 75;
            }
        }

        int CheckCheck(int[,] matrForCheck, Cell[,] cellsMatr, ref int kingHor, ref int kingVer)
        {
            int[,] matrForCheckCopy = new int[8, 8];
            Array.Copy(matrForCheck, matrForCheckCopy, 64);
            Piece king = cellsMatr[kingHor, kingVer].Pi;
            Checker pi = new Checker(blackOrWhite);
            if (king is King && king.Army == blackOrWhite)
                goto NextStep;
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                {
                    king = cellsMatr[i, j].Pi;
                    kingHor = i;
                    kingVer = j;
                    if (king is King && king.Army == blackOrWhite)
                        goto NextStep;
                }
            NextStep:
            for (int i = 0; i < 4; i++)
            {
                pi.Check(matrForCheck, 8, 8, kingHor, kingVer, i);
                for (int j = 7; j > -1; j--)
                    for (int k = 7; k > -1; k--)
                    {
                        if (matrForCheck[j, k] != 2)
                            continue;
                        switch (i)
                        {
                            case 0:
                                if (cellsMatr[j, k].Pi is Knight)
                                    goto Check;
                                break;
                            case 1:
                                if (cellsMatr[j, k].Pi is Rook || cellsMatr[j, k].Pi is Queen)
                                    goto Check;
                                break;
                            case 2:
                                if (cellsMatr[j, k].Pi is Bishop || cellsMatr[j, k].Pi is Queen)
                                    goto Check;
                                break;
                            case 3:
                                if (cellsMatr[j, k].Pi is Pawn)
                                    goto Check;
                                break;
                        }
                    }
                Array.Copy(matrForCheckCopy, matrForCheck, 64);
            }
            return 0;
            Check:
            Array.Copy(matrForCheckCopy, matrForCheck, 64);
            return 1;
        }

        void MovesArgsFilling(int mouseX, int mouseY, Graphics g)
        {
            movesArgs.hor = mouseX;
            movesArgs.ver = mouseY;
            movesArgs.army = blackOrWhite;
            movesArgs.horPiece = horPiece;
            movesArgs.verPiece = verPiece;
            movesArgs.castling = castling;
            Array.Copy(matr, movesArgs.matr, 64);
            movesArgs.res = false;
            movesArgs.extra = false;
            movesArgs.g = g;
        }

        void AnalyzeMatr()
        {
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                {
                    if (movesArgs.matr[i, j] < 2)
                        continue;
                    else
                    {
                        matrForCheck[i, j] = blackOrWhite;
                        matrForCheck[horPiece, verPiece] = -1;
                        if (cells[horPiece, verPiece].Pi is King)
                            CheckKingMoves(i, j);
                        else if (cells[horPiece, verPiece].Pi is Pawn && movesArgs.matr[i, j] == 4 && (i != lastMoveHor || verPiece != lastMoveVer))
                        {
                            movesArgs.matr[i, j] = matr[i, j];
                            goto RecPos;
                        }
                        if (CheckCheck(matrForCheck, cellsMatr, ref kingHor, ref kingVer) != 0)
                        {
                            movesArgs.matr[i, j] = matr[i, j];
                            goto RecPos;
                        }
                        RecPos:
                        RecoverPosition();
                    }
                }
        }

        void KingsMeeting(int i, int j)
        {
            int[,] posMoves = new int[8, 2]
                                        { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 } };
            for (int k = 0; k < 8; k++)
            {
                int c = i + posMoves[k, 0], d = j + posMoves[k, 1];
                if (c >= 0 && c < 8 && d >= 0 && d < 8 && cells[c, d].Pi is King && matrCopy[c, d] == 1 - blackOrWhite)
                {
                    movesArgs.matr[i, j] = matr[i, j];
                    RecoverPosition();
                    break;
                }
            }
        }

        void CheckCastling(int i, int j)
        {
            if (movesArgs.matr[i, j] == 3)
            {
                if (!castling)
                {
                    movesArgs.matr[i, j] = -1;
                    RecoverPosition();
                    return;
                }
                if (i == 6)
                    kingHor = 5;
                else
                    kingHor = 3;
                kingVer = j;
                cellsMatr[horPiece, verPiece] = new Cell(0, 0, 0);
                cellsMatr[kingHor, j] = new Cell(0, 0, 0, new King(blackOrWhite));
                if (CheckCheck(matrForCheck, cellsMatr, ref kingHor, ref kingVer) != 0)
                {
                    movesArgs.matr[i, j] = -1;
                    movesArgs.matr[kingHor, j] = -1;
                    RecoverPosition();
                }
            }
        }

        void CheckKingMoves(int i, int j)
        {
            KingsMeeting(i, j);
            CheckCastling(i, j);
            kingHor = i;
            kingVer = j;
            cellsMatr[horPiece, verPiece] = new Cell(0, 0, 0);
            cellsMatr[i, j] = new Cell(0, 0, 0, new King(blackOrWhite));
        }

        void DrawPosMoves()
        {
            Brush br = new SolidBrush(Color.FromArgb(36, 189, 200));
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                {
                    if (movesArgs.matr[i, j] < 2)
                        continue;
                    else
                    {
                        if (matrCopy[i, j] == -1)
                            cells[i, j].DrawE(movesArgs.g, br);
                        else if (matrCopy[i, j] == 1 - blackOrWhite)
                            cells[i, j].DrawT(movesArgs.g, br);
                    }
                }
            br.Dispose();
        }

        void HighlightPiece()
        {
            Brush br = new SolidBrush(Color.FromArgb(36, 189, 200));
            cells[horPiece, verPiece].DrawR(movesArgs.g, br);
            br.Dispose();
        }

        void CopySubtotals()
        {
            Array.Copy(movesArgs.matr, matr, 64);
            Array.Copy(movesArgs.matr, matrForCheck, 64);
            Array.Copy(movesArgs.matr, matrForNoMoves, 64);
        }

        void DeleteMovesHighlighting()
        {
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                {
                    if (movesArgs.matr[i, j] >= 2 || (i == movesArgs.horPiece && j == movesArgs.verPiece))
                        cells[i, j].Draw(movesArgs.g);
                }
        }

        void DrawPosPieces()
        {
            promHor = movesArgs.hor;
            cells[movesArgs.hor, 7 * blackOrWhite].Highlight(movesArgs.g, new Queen(blackOrWhite));
            cells[movesArgs.hor, 1 + 5 * blackOrWhite].Highlight(movesArgs.g, new Knight(blackOrWhite));
            cells[movesArgs.hor, 2 + 3 * blackOrWhite].Highlight(movesArgs.g, new Rook(blackOrWhite));
            cells[movesArgs.hor, 3 + blackOrWhite].Highlight(movesArgs.g, new Bishop(blackOrWhite));
        }

        bool ProcessingSpecMove()
        {
            if (movesArgs.matr[movesArgs.hor, movesArgs.ver] > 2)
            {
                movesArgs.res = false;
                movesArgs.extra = true;
                if (movesArgs.matr[movesArgs.hor, movesArgs.ver] == 3)
                {
                    if (movesArgs.hor == 2)
                    {
                        movesArgs.horExtra = 3;
                        movesArgs.horPieceExtra = 0;
                    }
                    else
                    {
                        movesArgs.horExtra = 5;
                        movesArgs.horPieceExtra = 7;
                    }
                    movesArgs.verExtra = movesArgs.ver;
                    movesArgs.verPieceExtra = movesArgs.ver;
                    movesArgs.matr[movesArgs.horExtra, movesArgs.verExtra] = 2;
                }
                else if (movesArgs.matr[movesArgs.hor, movesArgs.ver] == 4)
                {
                    movesArgs.horPieceExtra = movesArgs.hor;
                    movesArgs.verPieceExtra = movesArgs.verPiece;
                }
                //else if (movesArgs.matr[movesArgs.hor, movesArgs.ver] == 5)
                //{
                //    promotion = true;
                //    DrawPosPieces();
                //}
                GetPieceEvent(this, movesArgs);
                if (movesArgs.matr[movesArgs.hor, movesArgs.ver] > 3)
                {
                    matrCopy[movesArgs.horPieceExtra, movesArgs.verPieceExtra] = -1;
                    return false;
                }
                else if (!movesArgs.res)
                    return true;
                else
                {
                    movesArgs.res = false;
                    AddPieceEvent(this, movesArgs);
                    if (!movesArgs.res)
                        return true;
                    else
                    {
                        matrCopy[movesArgs.horExtra, movesArgs.verExtra] = blackOrWhite;
                        matrCopy[movesArgs.horPieceExtra, movesArgs.verPieceExtra] = -1;
                    }
                }
            }
            return false;
        }

        void EditNotation(ref string str)
        {
            if (blackOrWhite == 1)
                str = Convert.ToString(movesCounter) + ". ";
            if (movesArgs.matr[movesArgs.hor, movesArgs.ver] == 2)
            {
                ConvertPiece(ref str);
                ConvertHorPiece(ref str);
                str += Convert.ToString(movesArgs.verPiece + 1);
                if (matrCopy[movesArgs.hor, movesArgs.ver] == 1 - blackOrWhite)
                    str += "x";
                else
                    str += "-";
                ConvertHor(ref str);
                str += Convert.ToString(movesArgs.ver + 1);
            }
            else if (movesArgs.matr[movesArgs.hor, movesArgs.ver] == 3)
            {
                if (movesArgs.hor == 2)
                    str += "O-O-O";
                else
                    str += "O-O";
            }
            else if (movesArgs.matr[movesArgs.hor, movesArgs.ver] == 4)
            {
                ConvertHorPiece(ref str);
                str += Convert.ToString(movesArgs.verPiece + 1) + "x";
                ConvertHor(ref str);
                str += Convert.ToString(movesArgs.ver + 1);
            }
            else if (promotion)
            {
                ConvertHorPiece(ref str);
                str += Convert.ToString(movesArgs.verPiece + 1);
                if (matrCopy[movesArgs.hor, 7] == 1 - blackOrWhite)
                    str += "x";
                else
                    str += "-";
                ConvertHor(ref str);
                str += Convert.ToString(movesArgs.ver * blackOrWhite + 1) + "=";
                ConvertNewPiece(ref str);
            }
        }

        void ConvertPiece(ref string str)
        {
            if (movesArgs.pi is Rook)
            {
                str += "R";
            }
            else if (movesArgs.pi is Knight)
            {
                str += "N";
            }
            else if (movesArgs.pi is Bishop)
            {
                str += "B";
            }
            else if (movesArgs.pi is King)
            {
                str += "K";
            }
            else if (movesArgs.pi is Queen)
            {
                str += "Q";
            }
        }

        void ConvertHorPiece(ref string str)
        {
            if (movesArgs.horPiece == 0)
            {
                str += "a";
            }
            else if (movesArgs.horPiece == 1)
            {
                str += "b";
            }
            else if (movesArgs.horPiece == 2)
            {
                str += "c";
            }
            else if (movesArgs.horPiece == 3)
            {
                str += "d";
            }
            else if (movesArgs.horPiece == 4)
            {
                str += "e";
            }
            else if (movesArgs.horPiece == 5)
            {
                str += "f";
            }
            else if (movesArgs.horPiece == 6)
            {
                str += "g";
            }
            else if (movesArgs.horPiece == 7)
            {
                str += "h";
            }
        }

        void ConvertHor(ref string str)
        {
            if (movesArgs.hor == 0)
            {
                str += "a";
            }
            else if (movesArgs.hor == 1)
            {
                str += "b";
            }
            else if (movesArgs.hor == 2)
            {
                str += "c";
            }
            else if (movesArgs.hor == 3)
            {
                str += "d";
            }
            else if (movesArgs.hor == 4)
            {
                str += "e";
            }
            else if (movesArgs.hor == 5)
            {
                str += "f";
            }
            else if (movesArgs.hor == 6)
            {
                str += "g";
            }
            else if (movesArgs.hor == 7)
            {
                str += "h";
            }
        }

        void ConvertNewPiece(ref string str)
        {
            if (movesArgs.ver == 7 * blackOrWhite)
                str += "Q";
            else if (movesArgs.ver == 1 + 5 * blackOrWhite)
                str += "N";
            else if (movesArgs.ver == 2 + 3 * blackOrWhite)
                str += "R";
            else if (movesArgs.ver == 3 + blackOrWhite)
                str += "B";
        }

        void GetNewPiece(ref Piece p)
        {
            if (movesArgs.ver == 7 * blackOrWhite)
                p = new Queen(blackOrWhite);
            else if (movesArgs.ver == 1 + 5 * blackOrWhite)
                p = new Knight(blackOrWhite);
            else if (movesArgs.ver == 2 + 3 * blackOrWhite)
                p = new Rook(blackOrWhite);
            else if (movesArgs.ver == 3 + blackOrWhite)
                p = new Queen(blackOrWhite);
        }

        void SetParams()
        {
            if (promotion)
                matrCopy[movesArgs.hor, 7 * blackOrWhite] = blackOrWhite;
            else matrCopy[movesArgs.hor, movesArgs.ver] = blackOrWhite;
            promotion = false;
            matrCopy[movesArgs.horPiece, movesArgs.verPiece] = -1;
            blackOrWhite = 1 - blackOrWhite;
            lastMoveHor = movesArgs.hor;
            lastMoveVer = movesArgs.ver;
        }

        void DrawPosition()
        {
            if (flipBoard)
            {
                Flip(8, 8);
                Draw(movesArgs.g);
            }
            else
            {
                cells[kingHor, kingVer].Draw(movesArgs.g);
                cells[movesArgs.hor, movesArgs.ver].Draw(movesArgs.g);
                cells[movesArgs.horPiece, movesArgs.verPiece].Draw(movesArgs.g);
                if (movesArgs.extra)
                {
                    cells[movesArgs.horExtra, movesArgs.verExtra].Draw(movesArgs.g);
                    cells[movesArgs.horPieceExtra, movesArgs.verPieceExtra].Draw(movesArgs.g);
                }
                if (promotion)
                {
                    cells[promHor, 7 * blackOrWhite].Draw(movesArgs.g);
                    cells[promHor, 1 + 5 * blackOrWhite].Draw(movesArgs.g);
                    cells[promHor, 2 + 3 * blackOrWhite].Draw(movesArgs.g);
                    cells[promHor, 3 + blackOrWhite].Draw(movesArgs.g);
                }
            }
        }

        void CopyResults()
        {
            Array.Copy(matrCopy, matr, 64);
            Array.Copy(matrCopy, matrForCheck, 64);
            Array.Copy(matrCopy, matrForNoMoves, 64);
            Array.Copy(cells, cellsMatr, 64);
        }

        void AnalyzeNewPosition(ref string str)
        {
            int noMoves = CheckNoMoves(matrForNoMoves, cellsMatr, kingHor, kingVer);
            Array.Copy(cells, cellsMatr, 64);
            int check = CheckCheck(matrForCheck, cellsMatr, ref kingHor, ref kingVer);
            //statArgs.mode = 0;
            //statArgs.army = blackOrWhite;
            //bool statEvSubs = false;
            /*if (ChangeStatusEvent != null)
            {
                statEvSubs = true;
                ChangeStatusEvent(this, statArgs);
            }*/
            if (noMoves == 0 && check == 1)
            {
                Brush br = new SolidBrush(Color.FromArgb(237, 28, 36));
                cells[kingHor, kingVer].DrawT(movesArgs.g, br);
                br.Dispose();
                /*if (statEvSubs)
                {
                    statArgs.mode = 1;
                    ChangeStatusEvent(this, statArgs);
                }*/
                str += "+";
            }
            /*else if (noMoves == 1 && check == 0)
            {
                if (statEvSubs)
                {
                    statArgs.mode = 2;
                    ChangeStatusEvent(this, statArgs);
                }
            }*/
            else if (noMoves == 1 && check == 1)
            {
                Brush br = new SolidBrush(Color.FromArgb(237, 28, 36));
                cells[kingHor, kingVer].DrawT(movesArgs.g, br);
                br.Dispose();
                /*if (statEvSubs)
                {
                    statArgs.mode = 4;
                    ChangeStatusEvent(this, statArgs);
                }*/
                str += "#";
            }
            if (1 - blackOrWhite == 0)
                str += "\r\n";
            else
                str += "           ";// 11 spaces
            notation += str;
            NotationEventArgs notaEvArgs = new NotationEventArgs { str = str };
            ChangeNotationEvent?.Invoke(this, notaEvArgs);
            castling = true;
        }

        int CheckNoMoves(int[,] matrForNoMoves, Cell[,] cellsMatr, int kingHor, int kingVer)
        {
            int[,] matrForNoMovesCopy = new int[8, 8], matrCopy = new int[8, 8], matrForCheck = new int[8, 8];
            Array.Copy(cells, cellsMatr, 64);
            Array.Copy(matrForNoMoves, matrCopy, 64);
            Array.Copy(matrForNoMoves, matrForNoMovesCopy, 64);
            Array.Copy(matrForNoMoves, matrForCheck, 64);
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                {
                    if (cellsMatr[i, j].Pi == null)
                        continue;
                    if (cellsMatr[i, j].Pi.Army == blackOrWhite)
                    {
                        cellsMatr[i, j].Pi.Move(matrCopy, 8, 8);
                        for (int k = 7; k > -1; k--)
                            for (int m = 7; m > -1; m--)
                            {
                                if (matrCopy[k, m] >= 2)
                                {
                                    matrForCheck[k, m] = blackOrWhite;
                                    matrForCheck[i, j] = -1;
                                    if (cellsMatr[i, j].Pi is King)
                                    {
                                        int[,] posMoves = new int[8, 2]
                                        { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 } };
                                        for (int n = 0; n < 8; n++)
                                        {
                                            int c = k + posMoves[n, 0], d = m + posMoves[n, 1];
                                            if (c >= 0 && c < 8 && d >= 0 && d < 8 && cellsMatr[c, d].Pi is King && matrForNoMovesCopy[c, d] == 1 - blackOrWhite)
                                            {
                                                matrCopy[k, m] = matrForNoMoves[k, m];
                                                goto RecPos;
                                            }
                                        }
                                        kingHor = k;
                                        kingVer = m;
                                        cellsMatr[i, j] = new Cell(0, 0, 0);
                                        cellsMatr[k, m] = new Cell(0, 0, 0, new King(blackOrWhite));
                                    }
                                    if (CheckCheck(matrForCheck, cellsMatr, ref kingHor, ref kingVer) == 1)
                                    {
                                        matrCopy[k, m] = matrForNoMoves[k, m];
                                        goto RecPos;
                                    }
                                    Array.Copy(matrForNoMoves, matrForCheck, 64);
                                    Array.Copy(cells, cellsMatr, 64);
                                    Array.Copy(matrCopy, matrForNoMoves, 64);
                                    return 0;
                                }
                                RecPos:
                                Array.Copy(matrForNoMoves, matrForCheck, 64);
                                Array.Copy(cells, cellsMatr, 64);
                            }
                        Array.Copy(matrCopy, matrForNoMoves, 64);
                        Array.Copy(matrCopy, matrForCheck, 64);
                        Array.Copy(matrCopy, matrForNoMovesCopy, 64);
                    }
                }
            return 1;
        }

        void RecoverPosition()
        {
            Array.Copy(cells, cellsMatr, 64);
            Array.Copy(matr, matrForCheck, 64);
        }

        void CheckForHighlighting()
        {
            if (CheckCheck(matrForCheck, cellsMatr, ref kingHor, ref kingVer) == 1)
            {
                Brush br = new SolidBrush(Color.FromArgb(237, 28, 36));
                cells[kingHor, kingVer].DrawT(movesArgs.g, br);
                br.Dispose();
            }
        }

        bool CheckPiece(int hor, int ver)
        {
            if (movesArgs.matr[promHor, 7 * blackOrWhite] == 5 && hor == promHor && (ver == 7 * blackOrWhite || ver == 1 + 5 * blackOrWhite || ver == 2 + 3 * blackOrWhite || ver == 3 + blackOrWhite))
                return true;
            else return false;
        }

        public void Play(int mouseX, int mouseY, Graphics g)
        {
            Array.Copy(matr, matrForCheck, 64);
            Array.Copy(matr, matrForNoMoves, 64);
            Array.Copy(cells, cellsMatr, 64);
            GetCoord(ref mouseX, ref mouseY);
            if (CheckCheck(matrForCheck, cellsMatr, ref kingHor, ref kingVer) == 1)
                castling = false;
            // ЗАПУСК ТАЙМЕРА
            NewPiece:
            MovesArgsFilling(mouseX, mouseY, g);
            if (!selectOrMove)
            {
                if (promotion)
                {
                    if (!CheckPiece(mouseX, mouseY))
                        goto Failed;
                    else
                    {
                        Piece p = null;
                        GetNewPiece(ref p);
                        cells[movesArgs.horPiece, movesArgs.verPiece].Pi = null;
                        cells[mouseX, 7 * blackOrWhite].Pi = p;
                        if (blackOrWhite == 1)
                            movesCounter++;
                        goto Success;
                    }
                }
                if (UserClickEvent != null)
                {
                    UserClickEvent(this, movesArgs);
                    if (movesArgs.res)
                    {
                        selectOrMove = true;
                        horPiece = mouseX;
                        verPiece = mouseY;
                        AnalyzeMatr();
                        DrawPosMoves();
                        HighlightPiece();
                        CopySubtotals();
                    }
                }
                return;
            }
            else
            {
                DeleteMovesHighlighting();
                if (GetPieceEvent != null && AddPieceEvent != null && EndMoveEvent != null)
                {
                    selectOrMove = false;
                    if (matr[movesArgs.hor, movesArgs.ver] == 5)
                    {
                        promotion = true;
                        DrawPosPieces();
                        return;
                    }
                    GetPieceEvent(this, movesArgs);
                    if (movesArgs.res)
                    {
                        movesArgs.res = false;
                        AddPieceEvent(this, movesArgs);
                        if (movesArgs.res)
                        {
                            movesArgs.res = false;
                            EndMoveEvent(this, movesArgs);
                            if (movesArgs.res)
                            {
                                // ОСТАНОВКА ТАЙМЕРА
                                if (ProcessingSpecMove())
                                    goto Failed;
                                if (blackOrWhite == 1)
                                    movesCounter++;
                                goto Success;
                            }
                        }
                    } 
                }
            }
            Failed:
            DrawPosition();
            CopyResults();
            CheckForHighlighting();
            promotion = false;
            goto NewPiece;
            Success:
            string str = "";
            EditNotation(ref str);
            DrawPosition();
            SetParams();
            CopyResults();
            AnalyzeNewPosition(ref str);
            return;
        }

        public void NotationToForm(ref string str)
        {
            str = notation;
        }

        public void RecoverySubscription()
        {
            for (int i = 7; i > -1; i--)
                for (int j = 7; j > -1; j--)
                {
                    cells[i, j].DeleteSubscriptions();
                    cellsMatr[i, j].DeleteSubscriptions();
                    cells[i, j].InstallSubscriptions();
                }
            movesArgs = new MovesEventArgs();
            //selectOrMove = false;
            //statArgs = new StatusEventArgs();
        }
    }
}
