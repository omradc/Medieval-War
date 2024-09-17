namespace Assets.Scripts.Abstracts.Movements
{
    internal interface IMove
    {
        void MoveCommand();
        void HorizontalLineFormation(bool horizontalLineFormation, float distance);
        void VerticalLineFormation(bool verticalLineFormation, float distance);
        void RectangleFormation(bool horizontalRectangleFormation, float distance);
        void RightTriangleFormation(bool rightTriangleFormation, float distance);
        void LeftTriangleFormation(bool leftTriangleFormation, float distance);
        void UpTriangleFormation(bool upTriangleFormation, float distance);
        void DownTriangleFormation(bool downTriangleFormation, float distance);

    }
}
